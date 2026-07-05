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
        RunId.Validate(manifest.TaskId);

        var path = GetJsonPath(repositoryRoot, manifest.TaskId);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        var json = JsonSerializer.Serialize(manifest, JsonOptions);
        await using var stream = new FileStream(
            path,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 4096,
            useAsync: true);
        await using var writer = new StreamWriter(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        await writer.WriteAsync(json.AsMemory(), cancellationToken);
        await writer.FlushAsync(cancellationToken);
        return path;
    }

    public async Task SaveAsync(
        string repositoryRoot,
        RunManifest manifest,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(repositoryRoot);
        ArgumentNullException.ThrowIfNull(manifest);
        RunId.Validate(manifest.TaskId);

        var path = GetJsonPath(repositoryRoot, manifest.TaskId);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        var temporaryPath = path + ".tmp-" + Guid.NewGuid().ToString("N");
        try
        {
            var json = JsonSerializer.Serialize(manifest, JsonOptions);
            await File.WriteAllTextAsync(
                temporaryPath,
                json,
                new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
                cancellationToken);
            File.Move(temporaryPath, path, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }
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
        var manifest = JsonSerializer.Deserialize<RunManifest>(json, JsonOptions);
        return manifest ?? throw new InvalidDataException($"Run manifest '{path}' is empty or invalid.");
    }

    public string GetJsonPath(string repositoryRoot, string taskId)
    {
        RunId.Validate(taskId);
        return Path.Combine(repositoryRoot, ".ai", "runs", taskId + ".json");
    }

    public string GetMarkdownPath(string repositoryRoot, string taskId)
    {
        RunId.Validate(taskId);
        return Path.Combine(repositoryRoot, ".ai", "runs", taskId + ".md");
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
            foreach (var file in manifest.ChangedFiles)
            {
                builder.AppendLine($"- `{file.Path}` — {file.Status}");
            }
        }

        builder.AppendLine();
        builder.AppendLine("## Outside declared scope");
        builder.AppendLine();
        if (manifest.OutOfScopeFiles.Count == 0)
        {
            builder.AppendLine("No out-of-scope files recorded.");
        }
        else
        {
            foreach (var file in manifest.OutOfScopeFiles)
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
        builder.AppendLine("This report records Git state and declared scope only. It does not yet prove which build, test, UI, database, or runtime validation commands were executed.");

        return builder.ToString();
    }

    public async Task<string> WriteAsync(
        string path,
        RunManifest manifest,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        await File.WriteAllTextAsync(
            path,
            Format(manifest),
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
            cancellationToken);
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
