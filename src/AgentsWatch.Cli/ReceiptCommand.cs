using System.Text.Json;
using AgentsWatch.Core.Runs;
using AgentsWatch.Reports;

namespace AgentsWatch.Cli;

/// <summary>`receipt show <run-id>` and `handoff <run-id>`: projections regenerated from canonical JSON.</summary>
internal static class ReceiptCommand
{
    internal const string UntrustedBanner = "UNTRUSTED - evidence digest mismatch: this receipt's content does not match its stored digest. Do not rely on anything below.";

    public static int Show(string root, string[] args, TextWriter? output = null, TextWriter? error = null)
    {
        var o = output ?? Console.Out;
        var e = error ?? Console.Error;
        if (args.Length != 2 || args[0] != "show")
        {
            e.WriteLine("Usage: agentswatch receipt show <run-id>");
            return 2;
        }

        if (!TryLoad(root, args[1], e, out var receipt))
        {
            return 1;
        }

        if (!RunReceiptJson.VerifyDigest(receipt))
        {
            // Rendered only for inspection, under an explicit UNTRUSTED banner, and the command fails.
            o.WriteLine(UntrustedBanner);
            o.WriteLine();
            o.Write(ReceiptMarkdownWriter.Write(receipt));
            e.WriteLine("error: UNTRUSTED receipt: evidence digest mismatch; the stored digest does not match the receipt content (edited, or digest missing/replaced).");
            return 1;
        }

        o.Write(ReceiptMarkdownWriter.Write(receipt));
        return 0;
    }

    public static int Handoff(string root, string[] args, TextWriter? output = null, TextWriter? error = null)
    {
        var o = output ?? Console.Out;
        var e = error ?? Console.Error;
        if (args.Length != 1)
        {
            e.WriteLine("Usage: agentswatch handoff <run-id>");
            return 2;
        }

        if (!TryLoad(root, args[0], e, out var receipt))
        {
            return 1;
        }

        if (!RunReceiptJson.VerifyDigest(receipt))
        {
            // Never derive a projection from an untrusted receipt; existing projection files are left as they are.
            e.WriteLine("error: UNTRUSTED receipt: evidence digest mismatch; refusing to write a handoff or receipt projection.");
            return 1;
        }

        var paths = new FileReceiptProjectionWriter().Write(root, receipt);
        o.WriteLine("Handoff: " + paths[1]);
        return 0;
    }

    private static bool TryLoad(string root, string runId, TextWriter e, out RunReceipt receipt)
    {
        receipt = null!;
        try
        {
            var store = new FileRunReceiptStore(root);
            if (!store.Exists(runId))
            {
                e.WriteLine($"error: no receipt for run '{runId}'.");
                return false;
            }

            receipt = store.Read(runId);
            return true;
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidDataException or JsonException)
        {
            e.WriteLine("error: " + ex.Message);
            return false;
        }
    }
}