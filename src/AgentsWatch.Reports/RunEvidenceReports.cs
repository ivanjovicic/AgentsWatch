using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AgentsWatch.Core;

namespace AgentsWatch.Reports;

public sealed class RunManifestStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public async Task<string> CreateAsync(
        string repositoryRoot,
        RunManifest manifest,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(repositoryRoot);
        ArgumentNullException.ThrowIfNull(manifest);
        ValidateManifest(manifest);

        var path = GetJsonPath(repositoryRoot, manifest.TaskId);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        if (File.Exists(path))
        {
            throw new IOException($"Run manifest already exists: {path}");
        }

        var json = JsonSerializer.Serialize(manifest, JsonOptions);
        await AtomicTextFile.WriteNewAsync(path, json, cancellationToken);
        return path;
    }

    public async Task SaveAsync(
        string repositoryRoot,
        RunManifest manifest,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(repositoryRoot);
        ArgumentNullException.ThrowIfNull(manifest);
        ValidateManifest(manifest);

        var path = GetJsonPath(repositoryRoot, manifest.TaskId);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        await AtomicTextFile.WriteReplaceAsync(
            path,
            JsonSerializer.Serialize(manifest, JsonOptions),
            cancellationToken);
    }

    public async Task<RunManifest> LoadAsync(
        string repositoryRoot,
        string taskId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(repositoryRoot);
        RunId.Validate(taskId);

        var path = GetJsonPath(repositoryRoot, taskId);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Run '{taskId}' does not exist.", path);
        }

        var json = await File.ReadAllTextAsync(path, cancellationToken);
        var manifest = JsonSerializer.Deserialize<RunManifest>(json, JsonOptions)
            ?? throw new InvalidDataException($"Run manifest '{path}' is empty or invalid.");

        ValidateManifest(manifest);
        if (!string.Equals(manifest.TaskId, taskId, StringComparison.Ordinal))
        {
            throw new InvalidDataException(
                $"Run manifest identity mismatch: requested '{taskId}', file contains '{manifest.TaskId}'.");
        }

        return manifest;
    }

    public async Task<IReadOnlyList<RunManifest>> LoadAllAsync(
        string repositoryRoot,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(repositoryRoot);
        var directory = GetManifestDirectory(repositoryRoot);
        if (!Directory.Exists(directory))
        {
            return [];
        }

        var manifests = new List<RunManifest>();
        foreach (var path in Directory.EnumerateFiles(directory, "*.json", SearchOption.TopDirectoryOnly)
                     .OrderBy(static path => path, StringComparer.Ordinal))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var taskId = Path.GetFileNameWithoutExtension(path);
            try
            {
                manifests.Add(await LoadAsync(repositoryRoot, taskId, cancellationToken));
            }
            catch (ArgumentException ex)
            {
                throw new InvalidDataException($"Invalid run manifest filename '{path}'.", ex);
            }
        }

        return manifests
            .OrderBy(static manifest => manifest.StartedAt)
            .ThenBy(static manifest => manifest.TaskId, StringComparer.Ordinal)
            .ToArray();
    }

    public async Task<RunManifest?> FindLatestAsync(
        string repositoryRoot,
        CancellationToken cancellationToken = default)
    {
        var manifests = await LoadAllAsync(repositoryRoot, cancellationToken);
        return manifests
            .OrderByDescending(static manifest => manifest.StartedAt)
            .ThenByDescending(static manifest => manifest.TaskId, StringComparer.Ordinal)
            .FirstOrDefault();
    }

    public async Task<IReadOnlyList<RunManifest>> FindActiveAsync(
        string repositoryRoot,
        CancellationToken cancellationToken = default)
    {
        var manifests = await LoadAllAsync(repositoryRoot, cancellationToken);
        return manifests
            .Where(static manifest => manifest.Status == RunLifecycleStatus.InProgress)
            .OrderBy(static manifest => manifest.StartedAt)
            .ToArray();
    }

    public string GetJsonPath(string repositoryRoot, string taskId)
    {
        RunId.Validate(taskId);
        return Path.Combine(GetManifestDirectory(repositoryRoot), taskId + ".json");
    }

    public string GetMarkdownPath(string repositoryRoot, string taskId)
    {
        RunId.Validate(taskId);
        return Path.Combine(repositoryRoot, ".ai", "runs", taskId + ".md");
    }

    private static string GetManifestDirectory(string repositoryRoot) =>
        Path.Combine(repositoryRoot, ".agentwatch", "runs");

    private static void ValidateManifest(RunManifest manifest)
    {
        RunId.Validate(manifest.TaskId);
        if (!string.Equals(
                manifest.SchemaVersion,
                RunManifest.CurrentSchemaVersion,
                StringComparison.Ordinal))
        {
            throw new InvalidDataException(
                $"Unsupported run manifest schema '{manifest.SchemaVersion}'. Expected '{RunManifest.CurrentSchemaVersion}'.");
        }

        if (string.IsNullOrWhiteSpace(manifest.Title)
            || string.IsNullOrWhiteSpace(manifest.StartBranch)
            || manifest.AllowedPaths is null
            || manifest.ChangedFiles is null
            || manifest.OutOfScopeFiles is null
            || manifest.Warnings is null)
        {
            throw new InvalidDataException("Run manifest is missing required fields.");
        }

        ValidateObjectId(manifest.StartCommitSha, "start");
        if (manifest.Status == RunLifecycleStatus.Finished)
        {
            if (manifest.FinishedAt is null
                || string.IsNullOrWhiteSpace(manifest.EndBranch)
                || string.IsNullOrWhiteSpace(manifest.EndCommitSha))
            {
                throw new InvalidDataException("Finished run manifest is missing end evidence.");
            }

            ValidateObjectId(manifest.EndCommitSha, "end");
        }
    }

    private static void ValidateObjectId(string value, string label)
    {
        try
        {
            GitObjectId.Validate(value);
        }
        catch (ArgumentException ex)
        {
            throw new InvalidDataException($"Run manifest contains an invalid {label} Git object ID.", ex);
        }
    }
}

public sealed class MarkdownRunEvidenceFormatter
{
    public string Format(RunManifest manifest)
    {
        ArgumentNullException.ThrowIfNull(manifest);

        var builder = new StringBuilder();
        builder.AppendLine($"# AgentsWatch Run Evidence — {manifest.TaskId}");
        builder.AppendLine();
        builder.AppendLine($"- Title: {manifest.Title}");
        builder.AppendLine($"- Status: {manifest.Status}");
        builder.AppendLine($"- Validation: {manifest.ValidationStatus}");
        builder.AppendLine($"- Started: {manifest.StartedAt:O}");
        builder.AppendLine($"- Finished: {(manifest.FinishedAt is null ? "not finished" : manifest.FinishedAt.Value.ToString("O"))}");
        builder.AppendLine($"- Start branch: `{manifest.StartBranch}`");
        builder.AppendLine($"- Start commit: `{manifest.StartCommitSha}`");
        builder.AppendLine($"- End branch: `{manifest.EndBranch ?? "not captured"}`");
        builder.AppendLine($"- End commit: `{manifest.EndCommitSha ?? "not captured"}`");
        builder.AppendLine();

        builder.AppendLine("## Allowed scope");
        builder.AppendLine();
        AppendList(builder, manifest.AllowedPaths, "No scope restriction was declared.");
        builder.AppendLine();

        builder.AppendLine("## Changed files");
        builder.AppendLine();
        if (manifest.ChangedFiles.Count == 0)
        {
            builder.AppendLine("No changed files recorded.");
        }
        else
        {
            foreach (var file in manifest.ChangedFiles.OrderBy(static file => file.Path, StringComparer.Ordinal))
            {
                builder.AppendLine($"- `{file.Path}` — {file.Status}");
            }
        }

        builder.AppendLine();
        builder.AppendLine("## Outside declared scope");
        builder.AppendLine();
        if (manifest.AllowedPaths.Count == 0)
        {
            builder.AppendLine("Not evaluated because no scope restriction was declared.");
        }
        else if (manifest.OutOfScopeFiles.Count == 0)
        {
            builder.AppendLine("No out-of-scope files recorded.");
        }
        else
        {
            foreach (var file in manifest.OutOfScopeFiles.OrderBy(static file => file.Path, StringComparer.Ordinal))
            {
                builder.AppendLine($"- `{file.Path}` — {file.Status}");
            }
        }

        builder.AppendLine();
        builder.AppendLine("## Warnings");
        builder.AppendLine();
        AppendList(builder, manifest.Warnings, "No warnings recorded.");
        builder.AppendLine();
        builder.AppendLine("## Evidence boundary");
        builder.AppendLine();
        builder.AppendLine("This report records Git state and declared scope only.");
        builder.AppendLine("Validation is `NotRun`; no build, test, CI, UI, database, runtime, or agent-claim evidence was captured by this slice.");

        return builder.ToString();
    }

    public async Task<string> WriteAsync(
        string path,
        RunManifest manifest,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        await AtomicTextFile.WriteReplaceAsync(path, Format(manifest), cancellationToken);
        return path;
    }

    private static void AppendList(StringBuilder builder, IReadOnlyList<string> values, string emptyMessage)
    {
        if (values.Count == 0)
        {
            builder.AppendLine(emptyMessage);
            return;
        }

        foreach (var value in values)
        {
            builder.AppendLine($"- `{value}`");
        }
    }
}

internal static class AtomicTextFile
{
    private static readonly UTF8Encoding Utf8NoBom = new(encoderShouldEmitUTF8Identifier: false);

    public static async Task WriteNewAsync(
        string path,
        string content,
        CancellationToken cancellationToken)
    {
        var temporaryPath = CreateTemporaryPath(path);
        try
        {
            await File.WriteAllTextAsync(temporaryPath, content, Utf8NoBom, cancellationToken);
            File.Move(temporaryPath, path, overwrite: false);
        }
        finally
        {
            DeleteIfPresent(temporaryPath);
        }
    }

    public static async Task WriteReplaceAsync(
        string path,
        string content,
        CancellationToken cancellationToken)
    {
        var temporaryPath = CreateTemporaryPath(path);
        try
        {
            await File.WriteAllTextAsync(temporaryPath, content, Utf8NoBom, cancellationToken);
            File.Move(temporaryPath, path, overwrite: true);
        }
        finally
        {
            DeleteIfPresent(temporaryPath);
        }
    }

    private static string CreateTemporaryPath(string path) =>
        path + ".tmp-" + Guid.NewGuid().ToString("N");

    private static void DeleteIfPresent(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
