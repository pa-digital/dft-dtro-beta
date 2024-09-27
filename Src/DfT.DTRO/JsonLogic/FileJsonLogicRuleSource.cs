using System.Text.Json;

namespace DfT.DTRO.JsonLogic;

public class FileJsonLogicRuleSource : IJsonLogicRuleSource
{
    public async Task<IEnumerable<JsonLogicValidationRule>> GetRules(string rulesetName)
    {
        string path = $"../../../TestFiles/Rules/{rulesetName}.json";

        if (!File.Exists(path))
        {
            return null;
        }

        FileStream fs = File.OpenRead(path);
        var ret = await JsonSerializer.DeserializeAsync<JsonLogicValidationRule[]>(fs);
        return ret;
    }
}
