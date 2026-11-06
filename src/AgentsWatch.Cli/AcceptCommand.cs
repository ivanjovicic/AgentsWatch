using AgentsWatch.Core.Contracts;
using AgentsWatch.Core.Runs;
using AgentsWatch.Reports;

namespace AgentsWatch.Cli;

internal static class AcceptCommand
{
    public const string Usage = """
Usage:
  agentswatch accept record <run-id> --id <criterion-id> --status <supported|not-supported|unknown>
      [--evidence <validation-id>] [--reason <text>] [--replace]

Supported and NotSupported require --evidence linking to a recorded validation id.
Supported additionally requires that each linked validation is Pass with exit 0 or no exit code.
Unknown may omit --evidence. Tampered or digest-invalid receipts are rejected.
""";

    public static int Run(string root, string[] args, TextWriter? output = null, TextWriter? error = null)
    {
        var o = output ?? Console.Out;
        var e = error ?? Console.Error;
        if (args.Length < 2 || args[0] != "record")
        {
            e.WriteLine(Usage);
            return 2;
        }

        return Record(root, args[1..], o, e);
    }

    private static int Record(string root, string[] args, TextWriter o, TextWriter e)
    {
        if (args.Length < 1 || args[0].StartsWith("--"))
        {
            e.WriteLine(Usage);
            return 2;
        }

        var runId = args[0];
        var flags = new Dictionary<string, string>(StringComparer.Ordinal);
        var evidence = new List<string>();
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

            var name = args[i][2..];
            var value = args[++i];
            if (name == "evidence")
            {
                evidence.Add(value);
                continue;
            }

            flags[name] = value;
        }

        if (!flags.TryGetValue("id", out var id) || !flags.TryGetValue("status", out var statusText))
        {
            e.WriteLine("--id and --status are required.");
            return 2;
        }

        if (!TryStatus(statusText, out var status))
        {
            e.WriteLine("Invalid --status. Use supported, not-supported, or unknown.");
            return 2;
        }

        flags.TryGetValue("reason", out var reason);
        var service = new AcceptanceService(root, new FileRunReceiptStore(root), new FileRunContractStore(root), new FileReceiptProjectionWriter());
        var result = service.Record(runId, id, status, evidence, reason, replace);
        if (!result.Ok)
        {
            e.WriteLine("error: " + result.Error);
            return 1;
        }

        if (result.Error is not null)
        {
            e.WriteLine(result.Error);
        }

        var marked = result.Receipt!.AcceptanceCriteria.First(c => c.CriterionId == id);
        o.WriteLine("Acceptance recorded: " + marked.CriterionId + " [" + marked.Status + "]");
        return DecisionBanner.WriteCli(result.Receipt, o);
    }

    internal static bool TryStatus(string value, out CriterionStatus status)
    {
        switch (value.Trim().ToLowerInvariant().Replace("_", "-"))
        {
            case "supported":
                status = CriterionStatus.Supported;
                return true;
            case "not-supported":
            case "unsupported":
                status = CriterionStatus.Unsupported;
                return true;
            case "unknown":
                status = CriterionStatus.Unknown;
                return true;
            default:
                status = CriterionStatus.Unknown;
                return false;
        }
    }
}
