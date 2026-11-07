using AgentsWatch.Core.Contracts;

namespace AgentsWatch.Cli;

/// <summary>Thin CLI adapter over <see cref="ContractService"/>.</summary>
internal static class ContractCommand
{
    public const string Usage = """
Usage:
  agentswatch contract create <task-id> --intent <text> --accept <text>... [--accept-validation <cmd>...] --validate <cmd>...
      [--id <contract-id>] [--owned <path>...] [--avoid <path>...] [--stop <text>...] [--evidence <text>...]
      [--mode implementation|investigation|validation|docs] [--permission read-only|workspace-write|approval-required]
      [--source <system>:<reference>] [--force]
  agentswatch contract check <contract-id-or-json-file>
""";

    public static int Run(string root, string[] args, TextWriter? output = null, TextWriter? error = null)
    {
        var o = output ?? Console.Out;
        var e = error ?? Console.Error;
        if (args.Length < 2 || args[0] is not ("create" or "check"))
        {
            e.WriteLine(Usage);
            return 2;
        }

        var service = new ContractService(new FileRunContractStore(root));
        return args[0] == "create" ? Create(service, args[1..], o, e) : Check(service, root, args[1], o);
    }

    private static int Create(ContractService service, string[] args, TextWriter o, TextWriter e)
    {
        var taskId = args[0];
        var multi = new Dictionary<string, List<string>>();
        var acceptTexts = new List<string>();
        var acceptValidations = new List<List<string>>();
        var force = false;
        for (var i = 1; i < args.Length; i++)
        {
            if (args[i] == "--force")
            {
                force = true;
                continue;
            }

            if (!args[i].StartsWith("--") || i + 1 >= args.Length)
            {
                e.WriteLine($"Unexpected argument: {args[i]}");
                e.WriteLine(Usage);
                return 2;
            }

            var key = args[i][2..];
            var value = args[++i];
            if (key == "accept")
            {
                acceptTexts.Add(value);
                acceptValidations.Add([]);
                continue;
            }

            if (key == "accept-validation")
            {
                if (acceptValidations.Count == 0)
                {
                    e.WriteLine("--accept-validation must follow --accept.");
                    return 2;
                }

                acceptValidations[^1].Add(value);
                continue;
            }

            if (!multi.TryGetValue(key, out var list))
            {
                multi[key] = list = [];
            }

            list.Add(value);
        }

        List<string> All(string key) => multi.TryGetValue(key, out var v) ? v : [];
        string? One(string key) => multi.TryGetValue(key, out var v) ? v[^1] : null;

        var known = new HashSet<string> { "id", "intent", "owned", "avoid", "validate", "stop", "evidence", "mode", "permission", "source" };
        var unknown = multi.Keys.FirstOrDefault(k => !known.Contains(k));
        if (unknown is not null)
        {
            e.WriteLine($"Unknown option: --{unknown}");
            return 2;
        }

        if (!TryEnum(One("mode") ?? "implementation", out RunMode mode) || !TryEnum(One("permission") ?? "workspace-write", out PermissionMode permission))
        {
            e.WriteLine("Invalid --mode or --permission value.");
            return 2;
        }

        SourceTaskRef? source = null;
        if (One("source") is { } src)
        {
            var colon = src.IndexOf(':');
            if (colon <= 0 || colon == src.Length - 1)
            {
                e.WriteLine("--source must be <system>:<reference>.");
                return 2;
            }

            source = new SourceTaskRef(src[..colon], src[(colon + 1)..]);
        }

        var validations = All("validate").Select((cmd, i) => new ValidationRequirement($"v{i + 1}", cmd)).ToArray();
        var bindings = acceptTexts
            .Select((text, i) => acceptValidations[i].Count == 0 ? null : new CriterionValidationBinding($"ac{i + 1}", acceptValidations[i]))
            .Where(b => b is not null)
            .Cast<CriterionValidationBinding>()
            .ToArray();
        var draft = new ContractDraft(taskId, One("id"), One("intent"), acceptTexts, All("owned"), All("avoid"),
            permission, mode, validations, All("stop"), All("evidence"), source, bindings.Length == 0 ? null : bindings);

        var result = service.Create(draft, force);
        if (!result.Created)
        {
            e.WriteLine("error: " + result.Error);
            return 1;
        }

        o.WriteLine($"Contract written: {result.Path}");
        Print(result.Check, o);
        return 0;
    }

    private static int Check(ContractService service, string root, string target, TextWriter o)
    {
        var filePath = Path.IsPathRooted(target) ? target : Path.Combine(root, target);
        var result = target.EndsWith(".json", StringComparison.OrdinalIgnoreCase) && File.Exists(filePath)
            ? service.CheckJson(File.ReadAllText(filePath))
            : service.CheckById(target);
        Print(result, o);
        return result.IsValid ? 0 : 1;
    }

    private static void Print(ContractCheckResult result, TextWriter o)
    {
        o.WriteLine($"Valid: {(result.IsValid ? "yes" : "no")}");
        o.WriteLine("Findings:");
        if (result.Findings.Count == 0)
        {
            o.WriteLine("- none");
        }

        foreach (var f in result.Findings)
        {
            o.WriteLine($"- {f.RuleId} [{f.Severity.ToString().ToLowerInvariant()}]: {f.Reason}");
        }
    }

    private static bool TryEnum<T>(string value, out T result) where T : struct, Enum =>
        Enum.TryParse(value.Replace("-", string.Empty), ignoreCase: true, out result) && Enum.IsDefined(result);
}