using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using AgentsWatch.Cli;
using AgentsWatch.Core.Contracts;
using AgentsWatch.Core.Runs;
using AgentsWatch.Git;
using Xunit;

namespace AgentsWatch.Tests;

/// <summary>
/// AW-FIX-EXECUTOR-001 regression tests over the real start -> finish -> receipt flow.
/// Deliberately written only against CLI/JSON surfaces that already existed before the fix,
/// so they compile on the unmodified code and fail there (RED) on behavior, not on compilation.
/// Principle under test: declared identity != verified identity; matching claims != proof.
/// </summary>
public sealed class ExecutorIdentityFlowTests
{
    private const string LogPath = "evidence/impl-run.md";
    private const string FixtureName = "v1-receipt-pre-executor-identity.json";

    internal static string V2Log(string? agent = null, string? provider = null, string? model = null, string? client = null,
        string? format = "v2", string extraHeader = "", string body = "")
    {
        var b = new StringBuilder();
        b.Append("# T Evidence\n\n");
        if (format is not null)
        {
            b.Append("Evidence format: ").Append(format).Append('\n');
        }

        b.Append("Prompt ID: T\nQueue: user-assigned\n");
        if (agent is not null) b.Append("Agent/tool: ").Append(agent).Append('\n');
        if (provider is not null) b.Append("Model provider: ").Append(provider).Append('\n');
        if (model is not null) b.Append("Model name/id: ").Append(model).Append('\n');
        if (client is not null) b.Append("Client/IDE: ").Append(client).Append('\n');
        b.Append("Run mode: known-fix\n");
        b.Append(extraHeader);
        b.Append("\n## Outcome\n- narrative only\n");
        b.Append(body);
        return b.ToString();
    }

    private static async Task<(TempGitRepo Repo, string RunId)> StartAsync(ExecutorMetadata? declared, Action<TempGitRepo>? beforeStart = null)
    {
        var repo = new TempGitRepo();
        beforeStart?.Invoke(repo);
        var created = new ContractService(new FileRunContractStore(repo.Root)).Create(new ContractDraft("T", null, "intent", ["feature works"], ["**"], [],
            PermissionMode.WorkspaceWrite, RunMode.Implementation, [new ValidationRequirement("t", "dotnet test")], ["stop"], ["evidence"], null));
        Assert.True(created.Created, created.Error);
        var git = new GitRepositoryStateReader(new GitCommandRunner());
        var r = await new RunStartService(new FileRunContractStore(repo.Root), new FileActiveRunStore(repo.Root), git).StartAsync(repo.Root, "t", declared);
        Assert.True(r.Started, r.Error);
        return (repo, r.Baseline!.RunId);
    }

    private static async Task<(int Exit, string Out, string Err)> FinishCli(TempGitRepo repo, string runId, params string[] extra)
    {
        var o = new StringWriter();
        var e = new StringWriter();
        var exit = await FinishCommand.RunAsync(repo.Root, [runId, .. extra], o, e);
        return (exit, o.ToString(), e.ToString());
    }

    private static string ReceiptText(TempGitRepo repo, string runId) => File.ReadAllText(new FileRunReceiptStore(repo.Root).PathFor(runId));

    private static JsonElement Receipt(TempGitRepo repo, string runId) => JsonDocument.Parse(ReceiptText(repo, runId)).RootElement.Clone();

    private static string Status(JsonElement r) =>
        r.TryGetProperty("executorIdentity", out var id) && id.TryGetProperty("status", out var s) ? s.GetString()! : "<absent>";

    private static JsonElement Identity(JsonElement r)
    {
        Assert.True(r.TryGetProperty("executorIdentity", out var id), "receipt has no executorIdentity assessment");
        return id;
    }

    private static string? Observed(JsonElement r, string field) =>
        r.TryGetProperty("observedExecutorEvidence", out var o) && o.TryGetProperty(field, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : null;

    private static string[] StringArray(JsonElement e, string name) =>
        e.TryGetProperty(name, out var a) && a.ValueKind == JsonValueKind.Array ? a.EnumerateArray().Select(x => x.GetString()!).ToArray() : [];

    private static JsonElement[] Findings(JsonElement r, string prefix) =>
        r.GetProperty("findings").EnumerateArray().Where(f => f.GetProperty("ruleId").GetString()!.StartsWith(prefix, StringComparison.Ordinal)).ToArray();

    private static string[] Lines(string text) =>
        text.Split('\n').Select(l => l.TrimEnd('\r').Trim()).Select(l => l.StartsWith("- ", StringComparison.Ordinal) ? l[2..] : l).ToArray();

    private static string Show(TempGitRepo repo, string runId)
    {
        var o = new StringWriter();
        var e = new StringWriter();
        Assert.Equal(0, ReceiptCommand.Show(repo.Root, ["show", runId], o, e));
        Assert.DoesNotContain("digest mismatch", e.ToString());
        return o.ToString();
    }

    private static string Sha256File(string path) => "sha256:" + Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();

    private static bool ActiveRunExists(TempGitRepo repo, string runId) => new FileActiveRunStore(repo.Root).ActiveRunIds().Contains(runId);

    private static bool ReceiptExists(TempGitRepo repo, string runId) => new FileRunReceiptStore(repo.Root).Exists(runId);

    // 1. Claude declared + Codex implementation log -> CONFLICT (explicit finding, explicit display).
    [Fact]
    public async Task T01_ClaudeDeclared_CodexLog_IsConflict_WithFindingAndDisplay()
    {
        var (repo, run) = await StartAsync(new ExecutorMetadata(Agent: "Claude"));
        using var cleanup = repo;
        repo.Write("a.txt", "edit");
        repo.Write(LogPath, V2Log("Codex", "OpenAI", "gpt-6", "ChatGPT Work"));
        var logSha = Sha256File(Path.Combine(repo.Root, LogPath));

        var (exit, output, err) = await FinishCli(repo, run, "--executor-log", LogPath);
        Assert.True(exit == 0, err);

        var r = Receipt(repo, run);
        var id = Identity(r);
        Assert.Equal("conflict", Status(r));
        Assert.False(id.GetProperty("authorshipVerified").GetBoolean());
        Assert.Contains("agent", StringArray(id, "conflictingFields"));
        Assert.Equal("Codex", Observed(r, "agentTool"));
        Assert.Equal("OpenAI", Observed(r, "modelProvider"));
        Assert.Equal("gpt-6", Observed(r, "modelName"));
        Assert.Equal("ChatGPT Work", Observed(r, "clientIde"));
        Assert.Equal(LogPath, Observed(r, "sourcePath"));
        Assert.Equal(logSha, Observed(r, "sourceSha256"));
        Assert.Equal("v2", Observed(r, "format"));

        var conflict = Assert.Single(Findings(r, "AWI"));
        Assert.Equal("AWI001", conflict.GetProperty("ruleId").GetString());
        Assert.Equal("executor-identity-conflict", conflict.GetProperty("category").GetString());
        Assert.DoesNotContain(Findings(r, "AWR002"), f => f.GetProperty("message").GetString()!.Contains("executor", StringComparison.OrdinalIgnoreCase));

        foreach (var text in new[] { output, Show(repo, run) })
        {
            var lines = Lines(text);
            Assert.Contains("Executor declared: Claude", lines);
            Assert.Contains("Additional source: Codex / OpenAI / gpt-6", lines);
            Assert.Contains("Identity status: CONFLICT", lines);
            Assert.Contains("Authorship verified: NO", lines);
            Assert.Contains("Neither source independently proves who authored the changes.", text.Replace("\r", "").Replace("\n", " "));
        }
    }

    // 1b. The exact incident shape (declared Claude / owner-run-manual vs Codex / OpenAI / gpt-6 / ChatGPT Work).
    [Fact]
    public async Task T01b_IncidentShape_IsConflict_NotContaminated()
    {
        var (repo, run) = await StartAsync(new ExecutorMetadata(Agent: "Claude", Tool: "owner-run-manual"));
        using var cleanup = repo;
        repo.Write(LogPath, V2Log("Codex", "OpenAI", "gpt-6", "ChatGPT Work"));
        var (exit, _, err) = await FinishCli(repo, run, "--executor-log", LogPath);
        Assert.True(exit == 0, err);
        var r = Receipt(repo, run);
        Assert.Equal("conflict", Status(r));
        Assert.Equal(["agent"], StringArray(Identity(r), "conflictingFields"));
        Assert.DoesNotContain("contaminated", ReceiptText(repo, run), StringComparison.OrdinalIgnoreCase);
    }

    // 2. Claude + Claude -> CONSISTENT_UNVERIFIED, never VERIFIED.
    [Fact]
    public async Task T02_ClaudeAndClaude_IsConsistentUnverified_NeverVerified()
    {
        var (repo, run) = await StartAsync(new ExecutorMetadata(Agent: "Claude"));
        using var cleanup = repo;
        repo.Write(LogPath, V2Log("Claude", "Anthropic", null, "Claude Code"));
        var (exit, output, err) = await FinishCli(repo, run, "--executor-log", LogPath);
        Assert.True(exit == 0, err);
        var r = Receipt(repo, run);
        Assert.Equal("consistent-unverified", Status(r));
        Assert.NotEqual("verified", Status(r));
        Assert.False(Identity(r).GetProperty("authorshipVerified").GetBoolean());
        Assert.Empty(Findings(r, "AWI"));
        foreach (var text in new[] { output, Show(repo, run) })
        {
            Assert.Contains("Identity status: CONSISTENT_UNVERIFIED", Lines(text));
            Assert.Contains("Authorship verified: NO", Lines(text));
            Assert.Contains("not verification", text);
        }
    }

    // 3. Declaration only -> DECLARED_ONLY.
    [Fact]
    public async Task T03_DeclarationOnly_IsDeclaredOnly()
    {
        var (repo, run) = await StartAsync(new ExecutorMetadata(Agent: "Claude"));
        using var cleanup = repo;
        var (exit, output, err) = await FinishCli(repo, run);
        Assert.True(exit == 0, err);
        var r = Receipt(repo, run);
        Assert.Equal("declared-only", Status(r));
        Assert.False(r.TryGetProperty("observedExecutorEvidence", out _));
        Assert.Contains("Identity status: DECLARED_ONLY", Lines(output));
        Assert.Contains("Additional source: none", Lines(output));
        Assert.Contains("Authorship verified: NO", Lines(Show(repo, run)));
    }

    // 4. No valid identity data -> UNKNOWN (no declaration, no log; no declaration + malformed log; declaration + non-v2 log).
    [Fact]
    public async Task T04_NoValidIdentityData_IsUnknown()
    {
        var (repo1, run1) = await StartAsync(null);
        using (repo1)
        {
            var (exit, output, err) = await FinishCli(repo1, run1);
            Assert.True(exit == 0, err);
            Assert.Equal("unknown", Status(Receipt(repo1, run1)));
            Assert.Contains("Executor declared: unknown", Lines(output));
            Assert.Contains("Identity status: UNKNOWN", Lines(output));
            Assert.Contains("Authorship verified: NO", Lines(output));
        }

        var (repo2, run2) = await StartAsync(null);
        using (repo2)
        {
            repo2.Write(LogPath, "this is not a run log\n");
            var (exit, _, err) = await FinishCli(repo2, run2, "--executor-log", LogPath);
            Assert.True(exit == 0, err);
            Assert.Equal("unknown", Status(Receipt(repo2, run2)));
        }

        var (repo3, run3) = await StartAsync(new ExecutorMetadata(Agent: "Claude"));
        using (repo3)
        {
            repo3.Write(LogPath, V2Log("Codex", format: "v1"));
            var (exit, _, err) = await FinishCli(repo3, run3, "--executor-log", LogPath);
            Assert.True(exit == 0, err);
            var r = Receipt(repo3, run3);
            Assert.Equal("unknown", Status(r));
            Assert.Contains("Evidence format: v2", Identity(r).GetProperty("reason").GetString());
            Assert.Empty(Findings(r, "AWI"));
        }
    }

    // 5. Incomplete log -> no false attribution (placeholders are absent; narrative body is never parsed).
    [Fact]
    public async Task T05_IncompleteLog_DoesNotAttribute()
    {
        var (repo, run) = await StartAsync(new ExecutorMetadata(Agent: "Claude"));
        using var cleanup = repo;
        repo.Write(LogPath, V2Log("unknown-not-exposed", null, null, "ChatGPT Work", body: "Agent/tool: Codex\nModel name/id: gpt-6\n"));
        var (exit, _, err) = await FinishCli(repo, run, "--executor-log", LogPath);
        Assert.True(exit == 0, err);
        var r = Receipt(repo, run);
        Assert.Equal("declared-only", Status(r));
        Assert.Empty(StringArray(Identity(r), "conflictingFields"));
        Assert.Empty(StringArray(Identity(r), "comparedFields"));
        Assert.Null(Observed(r, "agentTool"));
        Assert.Null(Observed(r, "modelName"));
        Assert.Equal("ChatGPT Work", Observed(r, "clientIde"));
        Assert.Empty(Findings(r, "AWI"));
    }

    // 6. Case and whitespace -> deterministic normalization, no false conflict.
    [Theory]
    [InlineData("  claude ", "CLAUDE", "GPT-6", " gpt-6 ")]
    [InlineData("Claude   Code", "claude code", null, null)]
    [InlineData("Claude\u00A0Code", "CLAUDE CODE", "Opus\t4", "opus 4")]
    public async Task T06_CaseAndWhitespace_NormalizeWithoutFalseConflict(string declaredAgent, string logAgent, string? declaredModel, string? logModel)
    {
        var (repo, run) = await StartAsync(new ExecutorMetadata(Agent: declaredAgent, Model: declaredModel));
        using var cleanup = repo;
        repo.Write(LogPath, V2Log(logAgent, null, logModel));
        var (exit, _, err) = await FinishCli(repo, run, "--executor-log", LogPath);
        Assert.True(exit == 0, err);
        var r = Receipt(repo, run);
        Assert.Equal("consistent-unverified", Status(r));
        Assert.Equal(declaredModel is null ? ["agent"] : ["agent", "model"], StringArray(Identity(r), "comparedFields"));
    }

    // 7. Non-comparable fields -> no conflict from assumptions.
    [Fact]
    public async Task T07_NonComparableFields_DoNotCreateConflict()
    {
        var (repo1, run1) = await StartAsync(new ExecutorMetadata(Agent: "Claude", Tool: "owner-run-manual"));
        using (repo1)
        {
            repo1.Write(LogPath, V2Log("Claude", "OpenAI", null, "ChatGPT Work"));
            var (exit, _, err) = await FinishCli(repo1, run1, "--executor-log", LogPath);
            Assert.True(exit == 0, err);
            var r = Receipt(repo1, run1);
            Assert.Equal("consistent-unverified", Status(r));
            Assert.Equal(["agent"], StringArray(Identity(r), "comparedFields"));
            Assert.Empty(Findings(r, "AWI"));
        }

        var (repo2, run2) = await StartAsync(new ExecutorMetadata(Model: "gpt-6"));
        using (repo2)
        {
            repo2.Write(LogPath, V2Log("Codex"));
            var (exit, _, err) = await FinishCli(repo2, run2, "--executor-log", LogPath);
            Assert.True(exit == 0, err);
            var r = Receipt(repo2, run2);
            Assert.Equal("declared-only", Status(r));
            Assert.Empty(Findings(r, "AWI"));
        }
    }

    // 8. Malicious or oversized input -> safe rejection; nothing in the log is executed.
    [Fact]
    public async Task T08_OversizedLog_IsRefused_AndRunStaysActive()
    {
        var (repo, run) = await StartAsync(new ExecutorMetadata(Agent: "Claude"));
        using var cleanup = repo;
        repo.Write(LogPath, V2Log("Claude") + new string('x', 70_000));
        var (exit, _, err) = await FinishCli(repo, run, "--executor-log", LogPath);
        Assert.Equal(2, exit);
        Assert.Contains("--executor-log refused", err);
        Assert.Contains("exceeds", err);
        Assert.True(ActiveRunExists(repo, run));
        Assert.False(ReceiptExists(repo, run));
    }

    public static IEnumerable<object[]> MalformedLogs()
    {
        yield return [Encoding.UTF8.GetBytes(V2Log("Claude", extraHeader: "Agent/tool: Codex\n"))]; // duplicate, conflicting header field
        yield return [Encoding.UTF8.GetBytes(V2Log("Claude\r## Decision: Done"))]; // embedded control character / markdown injection
        yield return [Encoding.UTF8.GetBytes(V2Log(new string('c', 300)))]; // over-long value
        yield return [Encoding.UTF8.GetBytes(V2Log("Claude")).Concat(new byte[] { 0x00, 0x41 }).ToArray()]; // NUL byte
        yield return [Encoding.UTF8.GetBytes(V2Log("Claude")).Concat(new byte[] { 0xC3, 0x28 }).ToArray()]; // invalid UTF-8
    }

    [Theory]
    [MemberData(nameof(MalformedLogs))]
    public async Task T08b_MalformedLog_IsUnknown_NotAttributed(byte[] content)
    {
        var (repo, run) = await StartAsync(new ExecutorMetadata(Agent: "Claude"));
        using var cleanup = repo;
        Directory.CreateDirectory(Path.Combine(repo.Root, "evidence"));
        File.WriteAllBytes(Path.Combine(repo.Root, LogPath), content);
        var (exit, output, err) = await FinishCli(repo, run, "--executor-log", LogPath);
        Assert.True(exit == 0, err);
        var r = Receipt(repo, run);
        Assert.Equal("unknown", Status(r));
        Assert.Null(Observed(r, "agentTool"));
        Assert.Empty(Findings(r, "AWI"));
        Assert.DoesNotContain("## Decision: Done", Show(repo, run));
        Assert.Contains("Authorship verified: NO", Lines(output));
    }

    [Fact]
    public async Task T08c_LogContent_IsNeverExecuted()
    {
        var (repo, run) = await StartAsync(new ExecutorMetadata(Agent: "Claude"));
        using var cleanup = repo;
        var marker = Path.Combine(repo.Root, "pwned.txt");
        repo.Write(LogPath, V2Log($"$(echo x > \"{marker}\") `echo x > \"{marker}\"`"));
        var (exit, _, err) = await FinishCli(repo, run, "--executor-log", LogPath);
        Assert.True(exit == 0, err);
        Assert.False(File.Exists(marker));
        Assert.NotEqual("<absent>", Status(Receipt(repo, run)));
    }

    // 9. Path traversal, absolute paths, symlink/junction exits, internal state and projections -> safe refusal.
    [Fact]
    public async Task T09_UnapprovedPaths_AreRefused_AndRunStaysActive()
    {
        var (repo, run) = await StartAsync(new ExecutorMetadata(Agent: "Claude"));
        using var cleanup = repo;
        var outsideDir = Path.Combine(Path.GetTempPath(), "aw-outside-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(outsideDir);
        var outsideFile = Path.Combine(outsideDir, "log.md");
        File.WriteAllText(outsideFile, V2Log("Codex"));
        var siblingName = "aw-outside-" + Guid.NewGuid().ToString("N") + ".md";
        var sibling = Path.Combine(Path.GetDirectoryName(repo.Root)!, siblingName);
        File.WriteAllText(sibling, V2Log("Codex"));
        var linkDir = Path.Combine(repo.Root, "linkdir");
        var linkFile = Path.Combine(repo.Root, "link.md");
        try
        {
            repo.Write(LogPath, V2Log("Codex"));
            repo.Write("notes/projection.md", "# AgentsWatch Run Receipt - other\n\n> Generated from `.agentwatch/runs/other.json` (canonical). Do not edit; edits here are not evidence.\n\nAgent/tool: Codex\n");
            repo.Write(".ai/handoffs/other.md", "# Handoff - T (other)\n");

            async Task Refused(string arg, string reasonFragment)
            {
                var (exit, output, err) = await FinishCli(repo, run, "--executor-log", arg);
                Assert.True(exit == 2, $"{arg}: exit {exit}: {err}");
                Assert.Contains("--executor-log refused", err);
                Assert.Contains(reasonFragment, err);
                Assert.DoesNotContain(outsideDir, err + output);
                Assert.True(ActiveRunExists(repo, run), arg);
                Assert.False(ReceiptExists(repo, run), arg);
            }

            await Refused("../" + siblingName, "escapes the workspace");
            await Refused("evidence/../../" + siblingName, "escapes the workspace");
            await Refused(Path.Combine(repo.Root, LogPath), "workspace-relative");
            await Refused(outsideFile, "workspace-relative");
            await Refused(".agentwatch/contracts/t.json", "internal state");
            await Refused(".ai/handoffs/other.md", "projection");
            await Refused("notes/projection.md", "projection");
            await Refused("evidence/missing.md", "not found");
            await Refused("evidence", "regular file");

            Assert.True(TryLinkDirectory(linkDir, outsideDir), "could not create a directory symlink or junction for the escape test");
            await Refused("linkdir/log.md", "symbolic link");
            if (TryLinkFile(linkFile, outsideFile))
            {
                await Refused("link.md", "symbolic link");
            }

            var (okExit, _, okErr) = await FinishCli(repo, run, "--executor-log", LogPath);
            Assert.True(okExit == 0, okErr);
            Assert.Equal("conflict", Status(Receipt(repo, run)));
        }
        finally
        {
            RemoveLink(linkDir);
            if (File.Exists(linkFile) || new FileInfo(linkFile).LinkTarget is not null)
            {
                File.Delete(linkFile);
            }

            File.Delete(sibling);
            Directory.Delete(outsideDir, true);
        }
    }

    // 10. Historical v1 receipt (written by pre-fix fe1be51 CLI on Windows) -> reads; stored digest matches its producing (CRLF) form;
    //     strict VerifyDigest only where the newline is CRLF (cross-OS: AW-FIX-DIGEST-001); re-serializes to the same text after
    //     line-ending normalization (NOT an original-bytes comparison); show/handoff leave the file bytes unchanged.
    [Fact]
    public void T10_HistoricalV1Receipt_ReadsAndKeepsOriginalDigest()
    {
        var fixture = Path.Combine(AppContext.BaseDirectory, "Fixtures", FixtureName);
        var original = File.ReadAllText(fixture);
        var receipt = RunReceiptJson.Deserialize(original);
        // The fixture was produced on Windows by the pre-fix CLI. RunReceiptJson.ComputeDigest hashes indented JSON whose
        // newline follows Environment.NewLine (pre-existing behavior, unchanged by this fix), so its digest is bound to CRLF.
        // Platform-independent check: the stored digest still matches the unchanged content in its producing (CRLF) form.
        var producingForm = RunReceiptJson.Serialize(receipt with { EvidenceDigest = null }).Replace("\r\n", "\n").Replace("\n", "\r\n");
        Assert.Equal(receipt.EvidenceDigest, "sha256:" + Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(producingForm))).ToLowerInvariant());
        var producerNewline = Environment.NewLine == "\r\n";
        if (producerNewline)
        {
            Assert.True(RunReceiptJson.VerifyDigest(receipt));
            RunReceiptJson.EnsureStoredDigest(receipt);
        }

        Assert.Equal("Claude", receipt.ExecutorMetadata!.Agent);
        // Same text after CRLF->LF normalization and trailing-newline trim. This is semantic/layout equality, not byte identity.
        Assert.Equal(original.Replace("\r\n", "\n").TrimEnd('\n'), RunReceiptJson.Serialize(receipt).Replace("\r\n", "\n"));
        Assert.False(JsonDocument.Parse(original).RootElement.TryGetProperty("executorIdentity", out _));

        using var repo = new TempGitRepo();
        var path = new FileRunReceiptStore(repo.Root).PathFor(receipt.RunId);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.Copy(fixture, path);
        var before = Sha256File(path);
        var showOut = new StringWriter();
        var showErr = new StringWriter();
        Assert.Equal(0, ReceiptCommand.Show(repo.Root, ["show", receipt.RunId], showOut, showErr));
        if (producerNewline)
        {
            Assert.DoesNotContain("digest mismatch", showErr.ToString());
        }

        var shown = showOut.ToString();
        Assert.Contains("Executor declared: Claude / owner-run-manual", Lines(shown));
        Assert.Contains("Identity status: UNKNOWN", Lines(shown));
        Assert.Contains("Authorship verified: NO", Lines(shown));
        Assert.Contains("not assessed", shown);
        Assert.Equal(0, ReceiptCommand.Handoff(repo.Root, [receipt.RunId], new StringWriter(), new StringWriter()));
        Assert.Contains("Authorship verified: NO", Lines(File.ReadAllText(Path.Combine(repo.Root, ".ai", "handoffs", receipt.RunId + ".md"))));
        Assert.Equal(before, Sha256File(path));
    }

    // 11. New receipt -> stable JSON round-trip and digest; identity is covered by the digest and survives evidence re-checks.
    [Fact]
    public async Task T11_NewReceipt_RoundTripsAndDigestCoversIdentity()
    {
        var (repo, run) = await StartAsync(new ExecutorMetadata(Agent: "Claude"));
        using var cleanup = repo;
        repo.Write(LogPath, V2Log("Codex", "OpenAI", "gpt-6"));
        var (exit, _, err) = await FinishCli(repo, run, "--executor-log", LogPath);
        Assert.True(exit == 0, err);

        var text = ReceiptText(repo, run).Replace("\r\n", "\n").TrimEnd('\n');
        var receipt = RunReceiptJson.Deserialize(text);
        Assert.Equal(text, RunReceiptJson.Serialize(receipt).Replace("\r\n", "\n"));
        Assert.True(RunReceiptJson.VerifyDigest(receipt));
        Assert.Equal("conflict", Status(JsonDocument.Parse(text).RootElement));

        var tampered = text.Replace("\"status\": \"conflict\"", "\"status\": \"consistent-unverified\"");
        Assert.NotEqual(text, tampered);
        Assert.False(RunReceiptJson.VerifyDigest(RunReceiptJson.Deserialize(tampered)));

        var check = new EvidenceService(repo.Root, new FileRunReceiptStore(repo.Root), new FileRunContractStore(repo.Root)).Check(run);
        Assert.True(check.Ok, check.Error);
        var after = Receipt(repo, run);
        Assert.Equal("conflict", Status(after));
        Assert.Single(Findings(after, "AWI001"));
        Assert.True(RunReceiptJson.VerifyDigest(new FileRunReceiptStore(repo.Root).Read(run)));
    }

    // 12. CLI, Markdown and handoff show the status and leak no secrets or absolute paths.
    [Fact]
    public async Task T12_CliMarkdownHandoff_ShowStatus_WithoutLeaks()
    {
        var (repo, run) = await StartAsync(new ExecutorMetadata(Agent: "Claude"));
        using var cleanup = repo;
        repo.Write(LogPath, V2Log("Codex token=AWSENTINEL-TOKEN-leak123", "OpenAI", "gpt-6 Bearer abc.def.ghi"));
        var (exit, output, err) = await FinishCli(repo, run, "--executor-log", LogPath);
        Assert.True(exit == 0, err);
        Assert.Equal(0, ReceiptCommand.Handoff(repo.Root, [run], new StringWriter(), new StringWriter()));

        var surfaces = new Dictionary<string, string>
        {
            ["finish stdout"] = output,
            ["finish stderr"] = err,
            ["receipt json"] = ReceiptText(repo, run),
            ["receipt show"] = Show(repo, run),
            ["report md"] = File.ReadAllText(Path.Combine(repo.Root, ".ai", "runs", run + ".md")),
            ["handoff md"] = File.ReadAllText(Path.Combine(repo.Root, ".ai", "handoffs", run + ".md"))
        };
        foreach (var (name, text) in surfaces)
        {
            Assert.False(text.Contains("AWSENTINEL-TOKEN-leak123"), name + " leaked the sentinel token");
            Assert.False(text.Contains("abc.def.ghi"), name + " leaked the bearer value");
            Assert.False(text.Contains(repo.Root, StringComparison.OrdinalIgnoreCase), name + " leaked the absolute workspace path");
        }

        foreach (var name in new[] { "finish stdout", "receipt show", "report md", "handoff md" })
        {
            Assert.Contains("Identity status: CONFLICT", Lines(surfaces[name]));
            Assert.Contains("Authorship verified: NO", Lines(surfaces[name]));
        }
    }

    // 13. Git delta attribution -> unchanged by the identity assessment.
    [Fact]
    public async Task T13_GitDeltaAttribution_IsUnchanged()
    {
        static void Pre(TempGitRepo r)
        {
            r.Write("a.txt", "pre-existing");
            r.Write(LogPath, V2Log("Codex", "OpenAI", "gpt-6"));
        }

        static void During(TempGitRepo r)
        {
            r.Write("a.txt", "pre-existing + more");
            r.Write("c.txt", "new");
        }

        var (withLog, runA) = await StartAsync(new ExecutorMetadata(Agent: "Claude"), Pre);
        var (without, runB) = await StartAsync(new ExecutorMetadata(Agent: "Claude"), Pre);
        using (withLog)
        using (without)
        {
            During(withLog);
            During(without);
            var a = await FinishCli(withLog, runA, "--executor-log", LogPath);
            var b = await FinishCli(without, runB);
            Assert.True(a.Exit == 0, a.Err);
            Assert.True(b.Exit == 0, b.Err);

            static string DeltaShape(JsonElement r)
            {
                var d = r.GetProperty("runDelta");
                return string.Join("\n", new[] { "runIntervalChanges", "preExistingUnchangedChanges", "preExistingChangedFurther", "attributionAmbiguities" }
                    .SelectMany(k => d.GetProperty(k).EnumerateArray().Select(c =>
                        $"{k}|{c.GetProperty("path").GetString()}|{c.GetProperty("status").GetString()}|{c.GetProperty("classification").GetString()}|{c.GetProperty("classificationReason").GetString()}")));
            }

            static string AttributionFindings(JsonElement r) => string.Join("\n", r.GetProperty("findings").EnumerateArray()
                .Where(f => f.GetProperty("ruleId").GetString()!.StartsWith("AWR", StringComparison.Ordinal) || f.GetProperty("category").GetString() == "attribution-ambiguous")
                .Select(f => f.GetProperty("ruleId").GetString() + "|" + f.GetProperty("message").GetString()));

            var ra = Receipt(withLog, runA);
            var rb = Receipt(without, runB);
            Assert.Equal("conflict", Status(ra));
            Assert.Equal("declared-only", Status(rb));
            Assert.Equal(DeltaShape(rb), DeltaShape(ra));
            Assert.Equal(AttributionFindings(rb), AttributionFindings(ra));
            Assert.Equal(rb.GetProperty("attributionPolicy").GetString(), ra.GetProperty("attributionPolicy").GetString());
            foreach (var line in new[] { "Run-interval changes: 1", "Pre-existing changed further: 1", "Note: interval changes are observations, not proof of executor authorship." })
            {
                Assert.Contains(line, Lines(a.Out));
                Assert.Contains(line, Lines(b.Out));
            }
        }
    }

    // 14. Neither CONFLICT nor CONSISTENT_UNVERIFIED confirms Done, authorship or review independence.
    [Fact]
    public async Task T14_IdentityStatus_NeverConfirmsDoneAuthorshipOrIndependence()
    {
        var (none, runN) = await StartAsync(new ExecutorMetadata(Agent: "Claude"));
        var (conflict, runC) = await StartAsync(new ExecutorMetadata(Agent: "Claude"));
        var (consistent, runS) = await StartAsync(new ExecutorMetadata(Agent: "Claude"));
        using (none)
        using (conflict)
        using (consistent)
        {
            conflict.Write(LogPath, V2Log("Codex"));
            consistent.Write(LogPath, V2Log("Claude"));
            none.Write(LogPath, V2Log("Claude"));
            Assert.True((await FinishCli(none, runN)).Exit == 0);
            Assert.True((await FinishCli(conflict, runC, "--executor-log", LogPath)).Exit == 0);
            Assert.True((await FinishCli(consistent, runS, "--executor-log", LogPath)).Exit == 0);

            var rn = Receipt(none, runN);
            var rc = Receipt(conflict, runC);
            var rs = Receipt(consistent, runS);
            Assert.Equal("conflict", Status(rc));
            Assert.Equal("consistent-unverified", Status(rs));

            static string Decision(JsonElement r) => r.GetProperty("decision").GetProperty("status").GetString()!;
            static string Reasons(JsonElement r) => string.Join("\n", r.GetProperty("decision").GetProperty("reasons").EnumerateArray().Select(x => x.GetString()));
            Assert.NotEqual("done", Decision(rc));
            Assert.NotEqual("done", Decision(rs));
            Assert.Equal(Decision(rn), Decision(rc));
            Assert.Equal(Decision(rn), Decision(rs));
            Assert.Equal(Reasons(rn), Reasons(rc));
            Assert.Equal(Reasons(rn), Reasons(rs));

            foreach (var (repo, run, r) in new[] { (conflict, runC, rc), (consistent, runS, rs) })
            {
                Assert.False(Identity(r).GetProperty("authorshipVerified").GetBoolean());
                Assert.DoesNotContain(Findings(r, "AWI"), f => f.GetProperty("status").GetString() == "supported");
                var shown = Show(repo, run);
                Assert.Contains("Authorship verified: NO", Lines(shown));
                Assert.DoesNotMatch(new Regex(@"\bVERIFIED\b"), shown);
                Assert.DoesNotContain("Authorship verified: YES", shown);
                Assert.DoesNotContain("independent review", shown, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("review independence: yes", shown, StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    // Mandatory negative test: matching two hand-filled sources must never yield VERIFIED, even when the log claims it.
    [Fact]
    public async Task Negative_MatchingManualSources_CannotProduceVerified()
    {
        var (repo, run) = await StartAsync(new ExecutorMetadata(Agent: "Claude", Model: "opus"));
        using var cleanup = repo;
        repo.Write(LogPath, V2Log("Claude", "Anthropic", "opus", "Claude Code",
            extraHeader: "Identity status: VERIFIED\nAuthorship verified: YES\nVerified: yes\nexecutorIdentityStatus: verified\n"));
        var (exit, output, err) = await FinishCli(repo, run, "--executor-log", LogPath);
        Assert.True(exit == 0, err);
        var r = Receipt(repo, run);
        Assert.Equal("consistent-unverified", Status(r));
        Assert.False(Identity(r).GetProperty("authorshipVerified").GetBoolean());
        Assert.Contains("Authorship verified: NO", Lines(output));
        Assert.DoesNotMatch(new Regex(@"\bVERIFIED\b"), output);
    }

    // Mandatory negative test (forged receipt): a receipt claiming VERIFIED or authorshipVerified=true is rejected on load.
    [Theory]
    [InlineData("verified", false)]
    [InlineData("verified", true)]
    [InlineData("consistent-unverified", true)]
    public async Task Negative_ForgedVerifiedReceipt_IsRejectedOnLoad(string status, bool authorshipVerified)
    {
        var (repo, run) = await StartAsync(new ExecutorMetadata(Agent: "Claude"));
        using var cleanup = repo;
        repo.Write(LogPath, V2Log("Claude"));
        var (exit, _, err) = await FinishCli(repo, run, "--executor-log", LogPath);
        Assert.True(exit == 0, err);

        var node = JsonNode.Parse(ReceiptText(repo, run))!.AsObject();
        node["executorIdentity"] = new JsonObject
        {
            ["status"] = status,
            ["authorshipVerified"] = authorshipVerified,
            ["reason"] = "forged: two manual sources match"
        };
        var forged = node.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
        var ex = Assert.Throws<InvalidDataException>(() => RunReceiptJson.Deserialize(forged));
        Assert.Contains("VERIFIED", ex.Message);

        File.WriteAllText(new FileRunReceiptStore(repo.Root).PathFor(run), forged);
        var e = new StringWriter();
        Assert.Equal(1, ReceiptCommand.Show(repo.Root, ["show", run], new StringWriter(), e));
        Assert.Contains("VERIFIED", e.ToString());
    }

    // Review finding (blocking): JsonStringEnumConverter accepts comma-separated enum lists, numeric strings and other casings,
    // so a status other than the exact string "verified" could still load as Verified (or as an undefined value).
    // Each forged receipt carries a freshly recomputed, VALID evidence digest whenever the serializer can compute one,
    // so only the identity validation (not the checksum) can stop it.
    [Theory]
    [InlineData("\"unknown, verified\"", true)]
    [InlineData("\"declared-only,verified\"", false)]
    [InlineData("\"Verified\"", true)]
    [InlineData("\"VERIFIED\"", true)]
    [InlineData("\"vErIfIeD\"", true)]
    [InlineData("\"4\"", true)]
    [InlineData("\"declared-only, consistent-unverified\"", true)]
    [InlineData("\"conflict,verified\"", false)]
    [InlineData("\"7\"", false)]
    [InlineData("4", false)]
    [InlineData("7", false)]
    public async Task Negative_ForgedIdentityStatusVariant_WithRecomputedDigest_IsRejected(string rawStatusJson, bool digestComputable)
    {
        var (repo, run) = await StartAsync(new ExecutorMetadata(Agent: "Claude"));
        using var cleanup = repo;
        repo.Write(LogPath, V2Log("Codex", "OpenAI", "gpt-6"));
        var (exit, _, err) = await FinishCli(repo, run, "--executor-log", LogPath);
        Assert.True(exit == 0, err);

        var node = JsonNode.Parse(ReceiptText(repo, run))!.AsObject();
        node["executorIdentity"]!["status"] = JsonNode.Parse(rawStatusJson);
        node.Remove("evidenceDigest");

        // An attacker can recompute the checksum with the public serializer whenever the forged value round-trips.
        string? digest = null;
        try
        {
            var raw = JsonSerializer.Deserialize<RunReceipt>(node.ToJsonString(), RunContractJson.Options)!;
            digest = RunReceiptJson.ComputeDigest(raw);
        }
        catch (JsonException)
        {
        }

        Assert.Equal(digestComputable, digest is not null);
        node["evidenceDigest"] = digest ?? "sha256:0000000000000000000000000000000000000000000000000000000000000000";
        var forged = node.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
        if (digestComputable)
        {
            Assert.True(RunReceiptJson.VerifyDigest(JsonSerializer.Deserialize<RunReceipt>(forged, RunContractJson.Options)!),
                "the forged receipt's digest must be valid so that only identity validation can reject it");
        }

        var ex = Assert.Throws<InvalidDataException>(() => RunReceiptJson.Deserialize(forged));
        Assert.Contains("executorIdentity.status", ex.Message);

        File.WriteAllText(new FileRunReceiptStore(repo.Root).PathFor(run), forged);
        var e = new StringWriter();
        Assert.NotEqual(0, ReceiptCommand.Show(repo.Root, ["show", run], new StringWriter(), e));
        Assert.Contains("executorIdentity.status", e.ToString());
        Assert.False(new EvidenceService(repo.Root, new FileRunReceiptStore(repo.Root), new FileRunContractStore(repo.Root)).Check(run).Ok);
    }

    // Review finding m2: duplicate keys. System.Text.Json binds the LAST duplicate (and JsonElement.TryGetProperty
    // also matches the last one), so a shadowed earlier "verified" is invisible to validation and to the digest,
    // yet a first-wins reader would see it. The receipt's digest stays valid here (it covers the bound values only),
    // so only an explicit duplicate-key check can reject it.
    [Theory]
    [InlineData("status-shadowed-verified")]
    [InlineData("status-repeated-same")]
    [InlineData("authorship-shadowed-true")]
    [InlineData("identity-object-duplicated")]
    public async Task Negative_DuplicateExecutorIdentityKeys_AreRejected(string variant)
    {
        var (repo, run) = await StartAsync(new ExecutorMetadata(Agent: "Claude"));
        using var cleanup = repo;
        repo.Write(LogPath, V2Log("Codex", "OpenAI", "gpt-6"));
        var (exit, _, err) = await FinishCli(repo, run, "--executor-log", LogPath);
        Assert.True(exit == 0, err);

        var text = ReceiptText(repo, run);
        var status = Receipt(repo, run).GetProperty("executorIdentity").GetProperty("status").GetString();
        const string open = "\"executorIdentity\": {";
        Assert.Contains(open, text);
        var forged = variant switch
        {
            "status-shadowed-verified" => text.Replace(open, open + "\n    \"status\": \"verified\","),
            "status-repeated-same" => text.Replace(open, open + "\n    \"status\": \"" + status + "\","),
            "authorship-shadowed-true" => text.Replace(open, open + "\n    \"authorshipVerified\": true,"),
            "identity-object-duplicated" => text.Replace(open, "\"executorIdentity\": { \"status\": \"verified\", \"authorshipVerified\": true },\n  " + open),
            _ => throw new ArgumentOutOfRangeException(nameof(variant))
        };
        Assert.NotEqual(text, forged);

        // Precondition: the forged file still carries a valid digest over the bound (last-wins) content.
        Assert.True(RunReceiptJson.VerifyDigest(JsonSerializer.Deserialize<RunReceipt>(forged, RunContractJson.Options)!),
            "the duplicate must be invisible to the digest so that only duplicate-key validation can reject it");

        var ex = Assert.Throws<InvalidDataException>(() => RunReceiptJson.Deserialize(forged));
        Assert.Contains("duplicate", ex.Message);
        Assert.Contains("executorIdentity", ex.Message);

        File.WriteAllText(new FileRunReceiptStore(repo.Root).PathFor(run), forged);
        var e = new StringWriter();
        Assert.Equal(1, ReceiptCommand.Show(repo.Root, ["show", run], new StringWriter(), e));
        Assert.Contains("duplicate", e.ToString());
        Assert.False(new EvidenceService(repo.Root, new FileRunReceiptStore(repo.Root), new FileRunContractStore(repo.Root)).Check(run).Ok);
    }

    // Review finding: a FIFO in the workspace passes File.Exists and the 0-byte length check, and opening it for read
    // blocks until a writer appears, so `finish --executor-log <fifo>` would hang. Unix-only: Windows has no
    // filesystem FIFOs (named pipes live under \\.\pipe\, and device names such as NUL resolve outside the
    // workspace and are refused by the path check). Reported as Skipped on Windows, not as passed.
    [UnixOnlyFact]
    public void Negative_ExecutorLog_Fifo_IsRefusedWithoutBlocking()
    {
        using var ws = new TempDir();
        Directory.CreateDirectory(Path.Combine(ws.Path, "evidence"));
        var fifo = Path.Combine(ws.Path, "evidence", "pipe.md");
        Assert.Equal(0, PosixNative.mkfifo(fifo, Convert.ToUInt32("600", 8)));

        var read = Task.Run(() => ExecutorLogReader.Read(ws.Path, "evidence/pipe.md"));
        try
        {
            Assert.True(read.Wait(TimeSpan.FromSeconds(5)), "ExecutorLogReader.Read blocked on a FIFO");
            Assert.True(read.Result.Refused);
            Assert.Contains("not a regular file", read.Result.RefusalReason);
            Assert.DoesNotContain("pipe.md", read.Result.RefusalReason);
        }
        finally
        {
            if (!read.IsCompleted)
            {
                // Unblock the reader so the test process does not leak a thread stuck in open(2).
                using var w = new FileStream(fifo, FileMode.Open, FileAccess.Write);
                read.Wait(TimeSpan.FromSeconds(5));
            }
        }
    }

    // LIMITATION (documented, not fixed): a hard link is the same file under a second name. From inside the workspace a
    // hard link to a file outside it is indistinguishable from an ordinary regular file with the public .NET APIs on both
    // OSes, so it is read. This characterization test pins that behavior on both OSes so the limitation stays visible;
    // see docs/EXECUTOR_IDENTITY.md ("Limits").
    [Fact]
    public void Limitation_ExecutorLog_HardLinkToOutsideFile_IsReadAsRegularFile()
    {
        using var ws = new TempDir();
        using var outside = new TempDir();
        var target = Path.Combine(outside.Path, "outside-log.md");
        File.WriteAllText(target, V2Log("Codex", "OpenAI", "gpt-6"));
        Directory.CreateDirectory(Path.Combine(ws.Path, "evidence"));
        var link = Path.Combine(ws.Path, "evidence", "hard.md");
        Assert.True(PosixNative.TryCreateHardLink(link, target), "could not create a hard link (same volume required)");

        var result = ExecutorLogReader.Read(ws.Path, "evidence/hard.md");
        Assert.False(result.Refused, result.RefusalReason);
        Assert.NotNull(result.Evidence);
    }

    private static bool TryLinkDirectory(string link, string target)
    {
        try
        {
            Directory.CreateSymbolicLink(link, target);
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or PlatformNotSupportedException)
        {
        }

        if (!OperatingSystem.IsWindows())
        {
            return false;
        }

        // Junctions do not need the symlink privilege on Windows and are reparse points like symlinks.
        using var p = Process.Start(new ProcessStartInfo("cmd.exe", $"/c mklink /J \"{link}\" \"{target}\"")
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        })!;
        p.WaitForExit();
        return p.ExitCode == 0 && Directory.Exists(link);
    }

    private static bool TryLinkFile(string link, string target)
    {
        try
        {
            File.CreateSymbolicLink(link, target);
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or PlatformNotSupportedException)
        {
            return false;
        }
    }

    private static void RemoveLink(string link)
    {
        var info = new DirectoryInfo(link);
        if (info.Exists || info.LinkTarget is not null)
        {
            info.Delete(); // removes the link itself, never the target's contents
        }
    }
}
