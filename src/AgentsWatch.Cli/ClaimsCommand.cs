using AgentsWatch.Core.Contracts;
using AgentsWatch.Core.Runs;
using AgentsWatch.Reports;

namespace AgentsWatch.Cli;

internal static class ClaimsCommand
{
    public const string Usage = """
Usage:
  agentswatch claims record <run-id> --type <TestsAdded|DocsOnly|BackendUnchanged|MigrationAdded|ValidationPassed|NoUnrelatedChanges> --value <true|yes|1|false|no|0> --source <text>
      [--id <claim-id>] [--replace] [--text <raw-text>]
  agentswatch claims check <run-id>
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

        var service = new ClaimsService(root, new FileRunReceiptStore(root), new FileRunContractStore(root), new FileReceiptProjectionWriter());
        return args[0] == "record" ? Record(service, args[1..], o, e) : Check(service, args[1], o, e);
    }

    private static int Record(ClaimsService service, string[] args, TextWriter o, TextWriter e)
    {
        if (args.Length < 1 || args[0].StartsWith("--"))
        {
            e.WriteLine(Usage);
            return 2;
        }

        var runId = args[0];
        var flags = new Dictionary<string, string>(StringComparer.Ordinal);
        var replace = false;
        for (var i = 1; i < args.Length; i++)
        {
            if (args[i] == "--replace")
            {
                replace = true;
                continue;
            }

            if (!args[i].StartsWith("--") || i + 1 >= args.Length)
            {
                e.WriteLine("Unexpected argument: " + args[i]);
                return 2;
            }

            flags[args[i][2..]] = args[++i];
        }

        if (!flags.TryGetValue("type", out var type) || !flags.TryGetValue("value", out var value) || !flags.TryGetValue("source", out var source))
        {
            e.WriteLine("--type, --value and --source are required.");
            return 2;
        }

        flags.TryGetValue("id", out var id);
        flags.TryGetValue("text", out var raw);
        var claim = new AgentClaim(id ?? string.Empty, runId, type, source, value, raw);
        var result = service.Record(runId, claim, replace);
        if (!result.Ok)
        {
            e.WriteLine("error: " + result.Error);
            return 1;
        }

        var stored = result.RecordedClaim ?? result.Receipt!.Claims.First(c => c.ClaimType == type && c.Value == value);
        o.WriteLine("Claim recorded: " + stored.ClaimId + " " + stored.ClaimType + " [" + (stored.Status?.ToString() ?? "pending") + "]");
        return Print(result.Receipt!, o);
    }

    private static int Check(ClaimsService service, string runId, TextWriter o, TextWriter e)
    {
        var result = service.Check(runId);
        if (!result.Ok)
        {
            e.WriteLine("error: " + result.Error);
            return 1;
        }

        return Print(result.Receipt!, o);
    }

    private static int Print(RunReceipt receipt, TextWriter o)
    {
        o.WriteLine("Decision: " + receipt.Decision.Status);
        o.WriteLine("Claims:");
        if (receipt.Claims.Count == 0)
        {
            o.WriteLine("- none");
        }

        foreach (var c in receipt.Claims)
        {
            o.WriteLine("- " + c.ClaimType + " = " + c.Value + " [" + (c.Status?.ToString() ?? "pending") + "] (source: " + c.Source + ")" + (c.Reason is null ? "" : " - " + c.Reason));
        }

        return receipt.Decision.Status == DecisionStatus.Done ? 0 : 1;
    }
}
