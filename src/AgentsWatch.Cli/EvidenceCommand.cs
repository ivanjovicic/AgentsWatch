using AgentsWatch.Core.Contracts;
using AgentsWatch.Core.Runs;
using AgentsWatch.Reports;

namespace AgentsWatch.Cli;

internal static class EvidenceCommand
{
    public const string Usage = """
Usage:
  agentswatch evidence record <run-id> --command <text> --status <status> --source <source>
      [--id <validation-id>] [--exit-code <n>] [--summary <text>] [--digest <text>] [--source-ref <text>]
  agentswatch evidence check <run-id>

Status: pass|fail|not-run|blocked-by-environment|timed-out|killed|unknown
Source: agentswatch-command|imported-agent-result|user-declared|ci|unknown
""";

    public static int Run(string root, string[] args, TextWriter? output = null, TextWriter? error = null)
    {
        var o = output ?? Console.Out;
        var e = error ?? Console.Error;
        if (args.Length < 2 || args[0] is not ("record" or "check"))
        {
            e.WriteLine(Usage);
            return 2;
        }

        var service = new EvidenceService(root, new FileRunReceiptStore(root), new FileRunContractStore(root), new FileReceiptProjectionWriter());
        return args[0] == "record" ? Record(service, args[1..], o, e) : Check(service, args[1], o, e);
    }

    private static int Record(EvidenceService service, string[] args, TextWriter o, TextWriter e)
    {
        if (args.Length < 1 || args[0].StartsWith("--"))
        {
            e.WriteLine(Usage);
            return 2;
        }

        var runId = args[0];
        var flags = new Dictionary<string, string>(StringComparer.Ordinal);
        for (var i = 1; i < args.Length; i++)
        {
            if (!args[i].StartsWith("--") || i + 1 >= args.Length)
            {
                e.WriteLine($"Unexpected argument: {args[i]}");
                return 2;
            }

            flags[args[i][2..]] = args[++i];
        }

        if (!flags.TryGetValue("command", out var command) || !flags.TryGetValue("status", out var statusText) || !flags.TryGetValue("source", out var sourceText))
        {
            e.WriteLine("--command, --status and --source are required.");
            return 2;
        }

        if (!TryEnum(statusText, out ValidationStatus status) || !TryEnum(sourceText, out ValidationSource source))
        {
            e.WriteLine("Invalid --status or --source value.");
            return 2;
        }

        int? exit = null;
        if (flags.TryGetValue("exit-code", out var exitText))
        {
            if (!int.TryParse(exitText, out var parsed))
            {
                e.WriteLine("--exit-code must be an integer.");
                return 2;
            }

            exit = parsed;
        }

        flags.TryGetValue("id", out var id);
        flags.TryGetValue("summary", out var summary);
        flags.TryGetValue("digest", out var digest);
        flags.TryGetValue("source-ref", out var sourceRef);
        var evidence = new ValidationEvidence(string.IsNullOrWhiteSpace(id) ? "v1" : id, runId, command, status, source,
            ExitCode: exit, OutputSummary: summary, EvidenceDigest: digest, SourceRef: sourceRef);
        var result = service.Record(runId, evidence);
        if (!result.Ok)
        {
            e.WriteLine("error: " + result.Error);
            return 1;
        }

        if (result.Error is not null)
        {
            e.WriteLine(result.Error);
        }

        o.WriteLine("Evidence recorded: " + evidence.ValidationId + " " + status + " (source: " + source + ")");
        return Print(result.Receipt!, o);
    }

    private static int Check(EvidenceService service, string runId, TextWriter o, TextWriter e)
    {
        var result = service.Check(runId);
        if (!result.Ok)
        {
            e.WriteLine("error: " + result.Error);
            return 1;
        }

        if (result.Error is not null)
        {
            e.WriteLine(result.Error);
        }

        return Print(result.Receipt!, o);
    }

    private static int Print(RunReceipt receipt, TextWriter o)
    {
        o.WriteLine("Decision: " + receipt.Decision.Status);
        o.WriteLine("Reasons:");
        foreach (var reason in receipt.Decision.Reasons)
        {
            o.WriteLine("- " + reason);
        }

        return receipt.Decision.Status == DecisionStatus.Done ? 0 : 1;
    }

    private static bool TryEnum<T>(string value, out T result) where T : struct, Enum =>
        Enum.TryParse(value.Replace("-", string.Empty), ignoreCase: true, out result) && Enum.IsDefined(result);
}