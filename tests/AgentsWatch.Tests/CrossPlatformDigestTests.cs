using System.Security.Cryptography;
using System.Text;
using AgentsWatch.Cli;
using AgentsWatch.Core.Contracts;
using AgentsWatch.Core.Runs;
using AgentsWatch.Git;
using AgentsWatch.Reports;
using Xunit;

namespace AgentsWatch.Tests;

/// <summary>
/// AW-FIX-DIGEST-001. Receipt and contract digests must not depend on the OS that computed them.
/// The fixtures are real, synthetic-only receipts/contracts produced by the UNCHANGED fe1be51 CLI:
/// "windows-crlf" on Windows (digests over CRLF-indented JSON) and "linux-lf" on Linux (LF).
/// They are byte-exact (see .gitattributes) and are never rewritten by these tests: every test
/// works on a copy in a temporary workspace.
/// </summary>
public sealed class CrossPlatformDigestTests
{
    private const string ContractId = "digest-fixture";

    private static readonly IReadOnlyDictionary<string, FixtureFacts> Facts = new Dictionary<string, FixtureFacts>
    {
        ["windows-crlf"] = new(
            RunId: "digest-fixture-20261110t104944z-dc3397",
            ReceiptFileSha256: "d6d4c1c09912bd30338aaec4f0e0fba8adc9854c27e73e67605caa42ff3f539e",
            ContractFileSha256: "1ff270acf9e1e56b7a7fdb56256b48276a77ac302fa0da8970e3d8c990357b6a",
            StoredEvidenceDigest: "sha256:cfc059489df91e10ab07169349e8d71e6183b94ef3be5daeec18b0d429fc299a",
            CanonicalEvidenceDigest: "sha256:8250f813f6d770448aaccef5b0336406657e9935191c6c51a1bf1df3857cafc7",
            StoredContractDigest: "sha256:9f5d9cf88ff5bbfc447630ad489e74c4059169bf6edc28daa954925c50615144",
            CanonicalContractDigest: "sha256:d8c81a0780dfc0616a0d9e13baf1303b58a5de25e289beb95beb12ec9fb4cf84"),
        ["linux-lf"] = new(
            RunId: "digest-fixture-20261110t104853z-06df3b",
            ReceiptFileSha256: "04b2bb6773f352015a27627f532fb15f749bab6aaad6f2fbe64eead53143b12a",
            ContractFileSha256: "da78749426a22f82ab79a4981d735eaad063da302cb64fd9eda71ad3969e8145",
            StoredEvidenceDigest: "sha256:ed996a3d714c50645f43e54b210bfe0da7dd8be2c0e628e0988d31cb69669398",
            CanonicalEvidenceDigest: "sha256:ed996a3d714c50645f43e54b210bfe0da7dd8be2c0e628e0988d31cb69669398",
            StoredContractDigest: "sha256:254e949132ee731009061ed844cef8908510b6d473265afe86b47660f8740ce0",
            CanonicalContractDigest: "sha256:254e949132ee731009061ed844cef8908510b6d473265afe86b47660f8740ce0"),
    };

    public static TheoryData<string> Fixtures => new() { "windows-crlf", "linux-lf" };

    public static TheoryData<string, string> FixturesByMutation()
    {
        var data = new TheoryData<string, string>();
        foreach (var f in new[] { "windows-crlf", "linux-lf" })
        {
            foreach (var m in new[] { "evidence-record", "evidence-check", "accept-record", "claims-record", "claims-check", "drift-check" })
            {
                data.Add(f, m);
            }
        }

        return data;
    }

    // ---------- legacy digests verify on every OS ----------

    [Fact]
    public void D00_FixtureFiles_AreByteExact()
    {
        foreach (var (name, facts) in Facts)
        {
            Assert.Equal(facts.ReceiptFileSha256, FileSha256(FixturePath(name, "receipt.json")));
            Assert.Equal(facts.ContractFileSha256, FileSha256(FixturePath(name, "contract.json")));
        }

        // The Windows fixture really is CRLF and the Linux fixture really is LF.
        Assert.Contains("\r\n", File.ReadAllText(FixturePath("windows-crlf", "receipt.json")));
        Assert.DoesNotContain("\r", File.ReadAllText(FixturePath("linux-lf", "receipt.json")));
    }

    [Theory]
    [MemberData(nameof(Fixtures))]
    public void D01_LegacyReceiptDigest_VerifiesStrictly_OnEveryOs(string fixture)
    {
        using var ws = new FixtureWorkspace(fixture);
        var receipt = ws.ReadReceipt();
        Assert.Equal(Facts[fixture].StoredEvidenceDigest, receipt.EvidenceDigest);
        Assert.True(RunReceiptJson.VerifyDigest(receipt), $"{fixture} receipt must verify on {Environment.OSVersion.Platform}");
        RunReceiptJson.EnsureStoredDigest(receipt);
        ws.AssertFixtureBytesUnchanged();
    }

    [Theory]
    [MemberData(nameof(Fixtures))]
    public void D02_NewReceiptDigest_IsCanonicalLfForm_OnEveryOs(string fixture)
    {
        using var ws = new FixtureWorkspace(fixture);
        var receipt = ws.ReadReceipt();
        var computed = RunReceiptJson.ComputeDigest(receipt);
        Assert.Equal(Facts[fixture].CanonicalEvidenceDigest, computed);
        Assert.Equal(CanonicalSha256(RunReceiptJson.Serialize(receipt with { EvidenceDigest = null })), computed);
    }

    [Theory]
    [MemberData(nameof(Fixtures))]
    public void D03_ContractDigest_IsCanonicalLfForm_OnEveryOs(string fixture)
    {
        var contract = RunContractJson.Deserialize(File.ReadAllText(FixturePath(fixture, "contract.json")));
        var computed = RunContractJson.ComputeDigest(contract);
        Assert.Equal(Facts[fixture].CanonicalContractDigest, computed);
        Assert.Equal(CanonicalSha256(RunContractJson.Serialize(contract)), computed);
    }

    [Fact]
    public void D08_MatchDigest_ReportsExactlyWhichAcceptedFormMatched()
    {
        using var win = new FixtureWorkspace("windows-crlf");
        using var lin = new FixtureWorkspace("linux-lf");
        Assert.Equal(AgentsWatch.Core.DigestForm.LegacyCrlf, RunReceiptJson.MatchDigest(win.ReadReceipt()));
        Assert.Equal(AgentsWatch.Core.DigestForm.CanonicalLf, RunReceiptJson.MatchDigest(lin.ReadReceipt()));
        Assert.Equal(AgentsWatch.Core.DigestForm.None, RunReceiptJson.MatchDigest(lin.ReadReceipt() with { EvidenceDigest = null }));
        Assert.Equal(AgentsWatch.Core.DigestForm.None, RunReceiptJson.MatchDigest(lin.ReadReceipt() with { TaskId = "OTHER" }));

        // Legacy CRLF is accepted on read, but never written: re-signing yields the canonical form.
        var resigned = RunReceiptJson.WithDigest(win.ReadReceipt() with { EvidenceDigest = null });
        Assert.Equal(Facts["windows-crlf"].CanonicalEvidenceDigest, resigned.EvidenceDigest);
        Assert.Equal(AgentsWatch.Core.DigestForm.CanonicalLf, RunReceiptJson.MatchDigest(resigned));

        var winContract = RunContractJson.Deserialize(File.ReadAllText(FixturePath("windows-crlf", "contract.json")));
        Assert.True(RunContractJson.MatchesDigest(Facts["windows-crlf"].StoredContractDigest, winContract));
        Assert.True(RunContractJson.MatchesDigest(Facts["windows-crlf"].CanonicalContractDigest, winContract));
        Assert.False(RunContractJson.MatchesDigest(Facts["linux-lf"].StoredContractDigest, winContract));
        Assert.False(RunContractJson.MatchesDigest(null, winContract));
    }

    [Fact]
    public void D09_CanonicalJsonDigest_NormalizesLayout_IndependentOfTheRunningOs()
    {
        // Explicit CRLF/LF inputs, so a regression in the normalization itself is caught on Linux too,
        // not only where Environment.NewLine happens to be CRLF.
        const string lf = "{\n  \"a\": \"x\\r\\ny\"\n}";
        var crlf = lf.Replace("\n", "\r\n");
        var expectedLf = "sha256:" + Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(lf))).ToLowerInvariant();
        var expectedCrlf = "sha256:" + Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(crlf))).ToLowerInvariant();

        Assert.Equal(expectedLf, AgentsWatch.Core.CanonicalJsonDigest.Compute(lf));
        Assert.Equal(expectedLf, AgentsWatch.Core.CanonicalJsonDigest.Compute(crlf));
        Assert.Equal(expectedCrlf, AgentsWatch.Core.CanonicalJsonDigest.ComputeLegacyCrlf(lf));
        Assert.Equal(expectedCrlf, AgentsWatch.Core.CanonicalJsonDigest.ComputeLegacyCrlf(crlf));
        Assert.Equal(AgentsWatch.Core.DigestForm.CanonicalLf, AgentsWatch.Core.CanonicalJsonDigest.Match(expectedLf, crlf));
        Assert.Equal(AgentsWatch.Core.DigestForm.LegacyCrlf, AgentsWatch.Core.CanonicalJsonDigest.Match(expectedCrlf, lf));
        Assert.Equal(AgentsWatch.Core.DigestForm.None, AgentsWatch.Core.CanonicalJsonDigest.Match(expectedLf, lf.Replace("x", "z")));
        // A UTF-8 BOM is never part of the hashed bytes.
        Assert.False(AgentsWatch.Core.CanonicalJsonDigest.ToCanonicalText(crlf).Contains('\uFEFF'));
        // Escaped "\\r\\n" inside a JSON string value is content, not layout, and is preserved.
        Assert.Contains("x\\r\\ny", AgentsWatch.Core.CanonicalJsonDigest.ToCanonicalText(crlf), StringComparison.Ordinal);
    }

    [Fact]
    public void D10_DigestCoversBoundContent_NotFileBytes_DocumentedLimits()
    {
        // Characterization of documented limits (docs/DIGEST_CANONICALIZATION.md, "Limits"):
        // the digest covers the canonical re-serialization of the bound receipt, not the JSON file bytes.
        using var ws = new FixtureWorkspace("linux-lf");
        var text = File.ReadAllText(ws.ReceiptPath);

        // File layout is outside the digest: a BOM, CRLF line endings or compact JSON still verify.
        File.WriteAllBytes(ws.ReceiptPath, [0xEF, 0xBB, 0xBF, .. Encoding.UTF8.GetBytes(text.Replace("\n", "\r\n"))]);
        Assert.True(RunReceiptJson.VerifyDigest(ws.ReadReceipt()));
        File.WriteAllText(ws.ReceiptPath, System.Text.Json.Nodes.JsonNode.Parse(text)!.ToJsonString());
        Assert.DoesNotContain("\n", File.ReadAllText(ws.ReceiptPath));
        Assert.True(RunReceiptJson.VerifyDigest(ws.ReadReceipt()));

        // Unknown properties are ignored by the binder and are therefore outside the digest.
        File.WriteAllText(ws.ReceiptPath, text.Replace("\"schemaVersion\": 1,", "\"schemaVersion\": 1,\n  \"unknownExtra\": \"synthetic-sentinel\","));
        Assert.True(RunReceiptJson.VerifyDigest(ws.ReadReceipt()));

        // A duplicated property binds the LAST value; the digest covers that bound value only.
        File.WriteAllText(ws.ReceiptPath, text.Replace("\"taskId\": \"DIGEST-FIXTURE\",", "\"taskId\": \"SHADOW-SENTINEL\",\n  \"taskId\": \"DIGEST-FIXTURE\","));
        var dup = ws.ReadReceipt();
        Assert.Equal("DIGEST-FIXTURE", dup.TaskId);
        Assert.True(RunReceiptJson.VerifyDigest(dup));

        // Changing the bound value (here: the last duplicate) is detected.
        File.WriteAllText(ws.ReceiptPath, text.Replace("\"taskId\": \"DIGEST-FIXTURE\",", "\"taskId\": \"DIGEST-FIXTURE\",\n  \"taskId\": \"SHADOW-SENTINEL\","));
        Assert.False(RunReceiptJson.VerifyDigest(ws.ReadReceipt()));
    }

    // ---------- read paths ----------

    [Theory]
    [MemberData(nameof(Fixtures))]
    public void D04_ReceiptShow_LegacyDigest_NoMismatchWarning(string fixture)
    {
        using var ws = new FixtureWorkspace(fixture);
        var (o, e) = (new StringWriter(), new StringWriter());
        Assert.Equal(0, ReceiptCommand.Show(ws.Root, ["show", ws.RunId], o, e));
        Assert.Contains(ws.RunId, o.ToString());
        Assert.DoesNotContain("digest mismatch", e.ToString());
        ws.AssertFixtureBytesUnchanged();
    }

    [Theory]
    [MemberData(nameof(Fixtures))]
    public void D05_Handoff_LegacyDigest_NoMismatchWarning(string fixture)
    {
        using var ws = new FixtureWorkspace(fixture);
        var (o, e) = (new StringWriter(), new StringWriter());
        Assert.Equal(0, ReceiptCommand.Handoff(ws.Root, [ws.RunId], o, e));
        Assert.Contains("Handoff:", o.ToString());
        Assert.DoesNotContain("digest mismatch", e.ToString());
        ws.AssertFixtureBytesUnchanged();
    }

    [Theory]
    [MemberData(nameof(Fixtures))]
    public void D06_EvidenceCheck_AcceptsLegacyReceiptAndContractDigests(string fixture)
    {
        using var ws = new FixtureWorkspace(fixture);
        var result = new EvidenceService(ws.Root, new FileRunReceiptStore(ws.Root), new FileRunContractStore(ws.Root), new FileReceiptProjectionWriter())
            .Check(ws.RunId);
        Assert.True(result.Ok, result.Error);
        Assert.DoesNotContain(result.Receipt!.Findings, f => f.Message.Contains("stored contract does not match", StringComparison.Ordinal));
        // The digest captured when the run started is evidence; it is kept verbatim, not re-computed.
        Assert.Equal(Facts[fixture].StoredContractDigest, result.Receipt.ContractDigest);
    }

    // ---------- read paths on a digest mismatch (owner plan 2026-11-10 16:00, A2-A4) ----------

    public static TheoryData<string, string> FixturesByBreakage()
    {
        var data = new TheoryData<string, string>();
        foreach (var f in new[] { "windows-crlf", "linux-lf" })
        {
            data.Add(f, "content-edit");
            data.Add(f, "digest-removed");
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(FixturesByBreakage))]
    public void R01_Handoff_DigestMismatch_RefusesWithExit1_AndWritesNoProjection(string fixture, string breakage)
    {
        using var ws = new FixtureWorkspace(fixture);
        ws.Break(breakage);
        var report = Path.Combine(ws.Root, ".ai", "runs", ws.RunId + ".md");
        var handoff = Path.Combine(ws.Root, ".ai", "handoffs", ws.RunId + ".md");
        var before = File.ReadAllBytes(ws.ReceiptPath);

        var (o, e) = (new StringWriter(), new StringWriter());
        var exit = ReceiptCommand.Handoff(ws.Root, [ws.RunId], o, e);
        // Checked first so that RED shows the actual defect: a projection written from an untrusted receipt.
        Assert.False(File.Exists(handoff), "no handoff projection may be produced from an untrusted receipt");
        Assert.False(File.Exists(report), "no receipt projection may be produced from an untrusted receipt");
        Assert.Equal(1, exit);
        Assert.Contains("evidence digest", e.ToString());
        Assert.DoesNotContain("Handoff:", o.ToString());
        Assert.Equal(before, File.ReadAllBytes(ws.ReceiptPath));
    }

    [Theory]
    [MemberData(nameof(Fixtures))]
    public void R02_Handoff_DigestMismatch_LeavesExistingTrustedProjectionUntouched(string fixture)
    {
        using var ws = new FixtureWorkspace(fixture);
        var (o, e) = (new StringWriter(), new StringWriter());
        Assert.Equal(0, ReceiptCommand.Handoff(ws.Root, [ws.RunId], o, e));
        var handoff = Path.Combine(ws.Root, ".ai", "handoffs", ws.RunId + ".md");
        var trusted = File.ReadAllBytes(handoff);

        ws.Break("content-edit");
        Assert.Equal(1, ReceiptCommand.Handoff(ws.Root, [ws.RunId], new StringWriter(), new StringWriter()));
        Assert.Equal(trusted, File.ReadAllBytes(handoff));
    }

    [Theory]
    [MemberData(nameof(FixturesByBreakage))]
    public void R03_Show_DigestMismatch_Exit1_AndAnyRenderingIsMarkedUntrusted(string fixture, string breakage)
    {
        using var ws = new FixtureWorkspace(fixture);
        ws.Break(breakage);
        var (o, e) = (new StringWriter(), new StringWriter());
        Assert.Equal(1, ReceiptCommand.Show(ws.Root, ["show", ws.RunId], o, e));
        Assert.Contains("UNTRUSTED", e.ToString());
        var shown = o.ToString();
        if (shown.Length > 0)
        {
            Assert.StartsWith("UNTRUSTED", shown.TrimStart());
        }

        Assert.False(Directory.Exists(Path.Combine(ws.Root, ".ai")), "show must not write projections");
    }

    [Theory]
    [MemberData(nameof(Fixtures))]
    public void R04_ShowAndHandoff_ValidLegacyDigest_Exit0_NotMarkedUntrusted(string fixture)
    {
        using var ws = new FixtureWorkspace(fixture);
        var (o, e) = (new StringWriter(), new StringWriter());
        Assert.Equal(0, ReceiptCommand.Show(ws.Root, ["show", ws.RunId], o, e));
        Assert.DoesNotContain("UNTRUSTED", o.ToString() + e.ToString());
        (o, e) = (new StringWriter(), new StringWriter());
        Assert.Equal(0, ReceiptCommand.Handoff(ws.Root, [ws.RunId], o, e));
        Assert.DoesNotContain("UNTRUSTED", o.ToString() + e.ToString());
        Assert.True(File.Exists(Path.Combine(ws.Root, ".ai", "handoffs", ws.RunId + ".md")));
    }

    // ---------- mutation paths ----------

    [Theory]
    [MemberData(nameof(FixturesByMutation))]
    public void D07_MutationPaths_AcceptLegacyDigest_AndResignInCanonicalForm(string fixture, string mutation)
    {
        using var ws = new FixtureWorkspace(fixture);
        var original = ws.ReadReceipt();
        var (exit, stdout, stderr) = RunMutation(ws, mutation);

        // Exact expected outcome. Exit 1 is the decision exit code (NeedsEvidence), not an error; stderr must be empty.
        Assert.Equal(string.Empty, stderr);
        Assert.Equal(1, exit);
        var expectedFirstLine = mutation switch
        {
            "evidence-record" => "Evidence recorded: v1 Pass (source: CI)",
            "accept-record" => "Acceptance recorded: ac1 [Unknown]",
            "claims-record" => "Claim recorded: c1 DocsOnly [Supported]",
            _ => "Decision: NeedsEvidence"
        };
        Assert.Equal(expectedFirstLine, stdout.Split('\n')[0].TrimEnd('\r'));

        var stored = ws.ReadReceipt();
        Assert.Equal(DecisionStatus.NeedsEvidence, stored.Decision.Status);
        switch (mutation)
        {
            case "evidence-record":
                var v = Assert.Single(stored.Validations);
                Assert.Equal(("v1", ValidationStatus.Pass, ValidationSource.CI, 0), (v.ValidationId, v.Status, v.Source, v.ExitCode));
                break;
            case "accept-record":
                Assert.Equal(CriterionStatus.Unknown, Assert.Single(stored.AcceptanceCriteria, a => a.CriterionId == "ac1").Status);
                break;
            case "claims-record":
                Assert.Equal("DocsOnly", Assert.Single(stored.Claims).ClaimType);
                break;
            default:
                // Read-style checks re-evaluate and persist, but must not change validations, claims or acceptance.
                Assert.Equal(original.Validations.Count, stored.Validations.Count);
                Assert.Equal(original.Claims.Count, stored.Claims.Count);
                Assert.Equal(original.AcceptanceCriteria.Select(a => a.Status), stored.AcceptanceCriteria.Select(a => a.Status));
                break;
        }

        // Every persisted receipt is (re)signed in the canonical LF form, whatever OS wrote it.
        Assert.Equal(AgentsWatch.Core.DigestForm.CanonicalLf, RunReceiptJson.MatchDigest(stored));
        Assert.Equal(CanonicalSha256(RunReceiptJson.Serialize(stored with { EvidenceDigest = null })), stored.EvidenceDigest);
        Assert.Equal(Facts[fixture].StoredContractDigest, stored.ContractDigest);
    }

    // ---------- tamper detection (legacy CRLF, legacy LF, new canonical) ----------

    [Theory]
    [MemberData(nameof(Fixtures))]
    public void X01_TamperedLegacyReceipt_FailsIntegrity_OnEveryPath(string fixture)
    {
        using var ws = new FixtureWorkspace(fixture);
        Assert.True(RunReceiptJson.VerifyDigest(ws.ReadReceipt()), "untampered legacy receipt must verify first");

        ws.EditReceiptBytes("\"agent\": \"synthetic-agent\"", "\"agent\": \"synthetic-agenT\"");
        AssertTamperDetected(ws);
    }

    [Theory]
    [MemberData(nameof(Fixtures))]
    public void X02_TamperedDecision_InLegacyReceipt_FailsIntegrity(string fixture)
    {
        using var ws = new FixtureWorkspace(fixture);
        Assert.True(RunReceiptJson.VerifyDigest(ws.ReadReceipt()), "untampered legacy receipt must verify first");

        ws.EditReceiptBytes("\"status\": \"needs-evidence\"", "\"status\": \"done\"");
        AssertTamperDetected(ws);
    }

    [Theory]
    [MemberData(nameof(Fixtures))]
    public void X03_LineEndingOnlyRewrite_IsNotContent_AndStillVerifies(string fixture)
    {
        using var ws = new FixtureWorkspace(fixture);
        var text = File.ReadAllText(ws.ReceiptPath);
        var flipped = text.Contains("\r\n") ? text.Replace("\r\n", "\n") : text.Replace("\n", "\r\n");
        File.WriteAllText(ws.ReceiptPath, flipped);
        Assert.True(RunReceiptJson.VerifyDigest(ws.ReadReceipt()));
    }

    [Fact]
    public void X04_DigestFromOtherReceipt_IsRejected()
    {
        using var win = new FixtureWorkspace("windows-crlf");
        using var lin = new FixtureWorkspace("linux-lf");
        var w = win.ReadReceipt();
        var l = lin.ReadReceipt();
        Assert.False(RunReceiptJson.VerifyDigest(l with { EvidenceDigest = w.EvidenceDigest }));
        Assert.False(RunReceiptJson.VerifyDigest(w with { EvidenceDigest = l.EvidenceDigest }));
        Assert.False(RunReceiptJson.VerifyDigest(w with { EvidenceDigest = Facts["windows-crlf"].StoredContractDigest }));
    }

    [Fact]
    public void X05_NewlineInsideStringValue_IsContent_NotLayout()
    {
        using var ws = new FixtureWorkspace("linux-lf");
        var r = ws.ReadReceipt();
        var lf = r with { MissedWork = ["line1\nline2"], EvidenceDigest = null };
        var crlf = r with { MissedWork = ["line1\r\nline2"], EvidenceDigest = null };
        Assert.NotEqual(RunReceiptJson.ComputeDigest(lf), RunReceiptJson.ComputeDigest(crlf));
        Assert.False(RunReceiptJson.VerifyDigest(crlf with { EvidenceDigest = RunReceiptJson.ComputeDigest(lf) }));
        Assert.False(RunReceiptJson.VerifyDigest(lf with { EvidenceDigest = RunReceiptJson.ComputeDigest(crlf) }));
    }

    [Theory]
    [MemberData(nameof(Fixtures))]
    public void X06_TamperedContract_FailsContractDigest_ForLegacyDigests(string fixture)
    {
        using var ws = new FixtureWorkspace(fixture);
        var text = File.ReadAllText(ws.ContractPath);
        File.WriteAllText(ws.ContractPath, text.Replace("synthetic digest fixture (AW-FIX-DIGEST-001)", "synthetic digest fixture (TAMPERED)"));
        var result = new EvidenceService(ws.Root, new FileRunReceiptStore(ws.Root), new FileRunContractStore(ws.Root)).Check(ws.RunId);
        Assert.True(result.Ok, result.Error);
        Assert.Contains(result.Receipt!.Findings, f => f.Message.Contains("stored contract does not match", StringComparison.Ordinal));
    }

    [Fact]
    public async Task X07_NewRun_StartFinish_UseCanonicalDigests_AndTamperFails()
    {
        using var repo = new TempGitRepo();
        var contracts = new FileRunContractStore(repo.Root);
        var created = new ContractService(contracts).Create(new ContractDraft("T", null, "intent", ["feature works"], ["**"], [],
            PermissionMode.WorkspaceWrite, RunMode.Implementation, [new ValidationRequirement("t", "dotnet test")], ["stop"], ["test log"], null,
            [new CriterionValidationBinding("ac1", ["dotnet test"])]));
        Assert.True(created.Created, created.Error);
        var contract = RunContractJson.Deserialize(contracts.ReadRaw("t"));

        var git = new GitRepositoryStateReader(new GitCommandRunner());
        var started = await new RunStartService(contracts, new FileActiveRunStore(repo.Root), git).StartAsync(repo.Root, "t");
        Assert.True(started.Started, started.Error);
        Assert.Equal(CanonicalSha256(RunContractJson.Serialize(contract)), started.Baseline!.ContractDigest);

        repo.Write("a.txt", "changed");
        var finished = await new RunFinishService(new FileActiveRunStore(repo.Root), new FileRunRecordStore(repo.Root), git, git,
            receipts: new FileRunReceiptStore(repo.Root), contracts: contracts, projections: new FileReceiptProjectionWriter())
            .FinishAsync(repo.Root, started.Baseline.RunId);
        Assert.True(finished.Finished, finished.Error);

        var store = new FileRunReceiptStore(repo.Root);
        var receipt = store.Read(started.Baseline.RunId);
        Assert.Equal(CanonicalSha256(RunReceiptJson.Serialize(receipt with { EvidenceDigest = null })), receipt.EvidenceDigest);
        Assert.Equal(CanonicalSha256(RunContractJson.Serialize(contract)), receipt.ContractDigest);
        Assert.True(RunReceiptJson.VerifyDigest(receipt));

        var path = store.PathFor(receipt.RunId);
        File.WriteAllText(path, File.ReadAllText(path).Replace("\"taskId\": \"T\"", "\"taskId\": \"U\""));
        var tampered = store.Read(receipt.RunId);
        Assert.False(RunReceiptJson.VerifyDigest(tampered));
        Assert.Throws<InvalidDataException>(() => RunReceiptJson.EnsureStoredDigest(tampered));
    }

    // ---------- helpers ----------

    private static void AssertTamperDetected(FixtureWorkspace ws)
    {
        var tampered = ws.ReadReceipt();
        Assert.False(RunReceiptJson.VerifyDigest(tampered));
        var ex = Assert.Throws<InvalidDataException>(() => RunReceiptJson.EnsureStoredDigest(tampered));
        Assert.Contains("does not match", ex.Message);

        var (o, e) = (new StringWriter(), new StringWriter());
        Assert.Equal(1, ReceiptCommand.Show(ws.Root, ["show", ws.RunId], o, e));
        Assert.Contains("digest mismatch", e.ToString());
        Assert.StartsWith("UNTRUSTED", o.ToString());

        (o, e) = (new StringWriter(), new StringWriter());
        Assert.Equal(1, ReceiptCommand.Handoff(ws.Root, [ws.RunId], o, e));
        Assert.Contains("digest mismatch", e.ToString());
        Assert.Equal(string.Empty, o.ToString());

        var before = File.ReadAllBytes(ws.ReceiptPath);
        foreach (var mutation in new[] { "evidence-record", "evidence-check", "accept-record", "claims-record", "claims-check", "drift-check" })
        {
            var (exit, stdout, stderr) = RunMutation(ws, mutation);
            Assert.Equal(1, exit);
            Assert.Equal(string.Empty, stdout);
            Assert.Equal("error: receipt is invalid: receipt evidence digest does not match its contents.", stderr.TrimEnd());
            Assert.Equal(before, File.ReadAllBytes(ws.ReceiptPath));
        }

        Assert.False(Directory.Exists(Path.Combine(ws.Root, ".ai")), "no projection may be written from an untrusted receipt");
    }

    private static (int Exit, string Stdout, string Stderr) RunMutation(FixtureWorkspace ws, string mutation)
    {
        var (o, e) = (new StringWriter(), new StringWriter());
        var run = ws.RunId;
        var exit = mutation switch
        {
            "evidence-record" => EvidenceCommand.Run(ws.Root, ["record", run, "--command", "dotnet test", "--status", "pass", "--source", "ci", "--exit-code", "0", "--summary", "test log"], o, e),
            "evidence-check" => EvidenceCommand.Run(ws.Root, ["check", run], o, e),
            "accept-record" => AcceptCommand.Run(ws.Root, ["record", run, "--id", "ac1", "--status", "unknown"], o, e),
            "claims-record" => ClaimsCommand.Run(ws.Root, ["record", run, "--type", "DocsOnly", "--value", "false", "--source", "synthetic-fixture"], o, e),
            "claims-check" => ClaimsCommand.Run(ws.Root, ["check", run], o, e),
            "drift-check" => DriftCommand.CheckFromArgs(ws.Root, ["check", run], o, e),
            _ => throw new ArgumentOutOfRangeException(nameof(mutation))
        };
        return (exit, o.ToString(), e.ToString());
    }

    internal static string CanonicalSha256(string serializedJson) =>
        "sha256:" + Convert.ToHexString(SHA256.HashData(new UTF8Encoding(false).GetBytes(serializedJson.Replace("\r\n", "\n")))).ToLowerInvariant();

    private static string FileSha256(string path) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();

    private static string FixturePath(string fixture, string file) => Path.Combine(AppContext.BaseDirectory, "Fixtures", "digest", fixture, file);

    private sealed record FixtureFacts(string RunId, string ReceiptFileSha256, string ContractFileSha256, string StoredEvidenceDigest,
        string CanonicalEvidenceDigest, string StoredContractDigest, string CanonicalContractDigest);

    private sealed class FixtureWorkspace : IDisposable
    {
        private readonly string _fixture;

        public FixtureWorkspace(string fixture)
        {
            _fixture = fixture;
            RunId = Facts[fixture].RunId;
            Root = Path.Combine(Path.GetTempPath(), "aw-digest-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(Path.Combine(Root, ".agentwatch", "runs"));
            Directory.CreateDirectory(Path.Combine(Root, ".agentwatch", "contracts"));
            File.Copy(FixturePath(fixture, "receipt.json"), ReceiptPath);
            File.Copy(FixturePath(fixture, "contract.json"), ContractPath);
        }

        public string Root { get; }
        public string RunId { get; }
        public string ReceiptPath => Path.Combine(Root, ".agentwatch", "runs", RunId + ".json");
        public string ContractPath => Path.Combine(Root, ".agentwatch", "contracts", ContractId + ".json");

        public RunReceipt ReadReceipt() => new FileRunReceiptStore(Root).Read(RunId);

        public void EditReceiptBytes(string from, string to)
        {
            var text = File.ReadAllText(ReceiptPath);
            Assert.Contains(from, text);
            File.WriteAllText(ReceiptPath, text.Replace(from, to));
        }

        public void Break(string breakage)
        {
            switch (breakage)
            {
                case "content-edit":
                    EditReceiptBytes("\"agent\": \"synthetic-agent\"", "\"agent\": \"synthetic-agenT\"");
                    break;
                case "digest-removed":
                    var text = File.ReadAllText(ReceiptPath);
                    var digest = Facts[_fixture].StoredEvidenceDigest;
                    Assert.Contains(digest, text);
                    File.WriteAllText(ReceiptPath, text.Replace(digest, "sha256:" + new string('0', 64)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(breakage));
            }
        }

        public void AssertFixtureBytesUnchanged()
        {
            Assert.Equal(Facts[_fixture].ReceiptFileSha256, FileSha256(ReceiptPath));
            Assert.Equal(Facts[_fixture].ReceiptFileSha256, FileSha256(FixturePath(_fixture, "receipt.json")));
        }

        public void Dispose()
        {
            try
            {
                Directory.Delete(Root, recursive: true);
            }
            catch (IOException)
            {
            }
        }
    }
}
