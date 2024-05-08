using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DfT.DTRO.JsonLogic;

/// <summary>
/// Implements <see cref="IJsonLogicRuleSource"/>
/// by retrieving the rules from a file within the project.
/// </summary>
public class FileJsonLogicRuleSource : IJsonLogicRuleSource
{
    /// <inheritdoc/>
    public async Task<IEnumerable<JsonLogicValidationRule>> GetRules(string rulesetName)
    {
        var path = $"{Environment.CurrentDirectory}/JsonLogic/Rules/{rulesetName}.json";

        if (!File.Exists(path))
        {
            return null;
        }

        var fs = File.OpenRead(path);

        var rules = await JsonSerializer.DeserializeAsync<JsonLogicValidationRule[]>(fs);

        return rules;
    }
}
