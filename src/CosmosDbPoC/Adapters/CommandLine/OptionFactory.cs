using System.CommandLine;

namespace CosmosDbPoC.Adapters.CommandLine;

public static class OptionFactory
{
    public static Option<T> AddOption<T>(char? shortName = null, string longName = null, object defaultValue = null, string helpText = null)
        where T : class
    {
        var aliases = new List<string>();
        
        if (shortName is not null)
            aliases.Add($"-{shortName}");
        
        if (longName is not null)
            aliases.Add($"--{longName}");

        var value = defaultValue as T;

        return new Option<T>(aliases.ToArray(), () => value, helpText);
    }
}