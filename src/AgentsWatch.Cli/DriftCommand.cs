using AgentsWatch.Core.Contracts;
using AgentsWatch.Core.Runs;
using AgentsWatch.Reports;

namespace AgentsWatch.Cli;

internal static class DriftCommand
{
    public static int CheckFromArgs(string root, string[] args, TextWriter? output = null, TextWriter? error = null)
    {
        var o = output ?? Console.Out;
        var e = error ?? Console.Error;
        if (args.Length != 2 || args[0] != "check")
        {
            e.WriteLine("Usage: agentswatch drift check <run-id>");
            return 2;
        }

        return Check(root, args[1], o, e);
    }

    private static int Check(string root, string runId, TextWriter o, TextWriter e)
    {
        var result = new DriftService(root, new FileRunReceiptStore(root), new FileRunContractStore(root), new FileReceiptProjectionWriter()).Check(runId);
        if (!result.Ok)
        {
            e.WriteLine("error: " + result.Error);
            return 1;
        }

        if (result.Error is not null)
        {
            e.WriteLine(result.Error);
        }

        var receipt = result.Receipt!;
        o.WriteLine("Decision: " + receipt.Decision.Status);
        o.WriteLine("Scope findings:");
        var drift = receipt.Findings.Where(f => f.RuleId.StartsWith("AWD", StringComparison.Ordinal)).ToArray();
        if (drift.Length == 0)
        {
            o.WriteLine("- none");
        }

        foreach (var f in drift)
        {
            o.WriteLine("- " + f.RuleId + " [" + f.Severity + "/" + f.Status + "]: " + f.Message);
        }

        return receipt.Decision.Status == DecisionStatus.Done ? 0 : 1;
    }
}
