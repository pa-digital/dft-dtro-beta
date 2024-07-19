using System.Text.Json.Serialization;
using Json.Logic;

namespace DfT.DTRO.JsonLogic;

public class JsonLogicValidationRule
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonConverter(typeof(LogicComponentConverter))]
    [JsonPropertyName("rule")]
    public Rule Rule { get; set; }
}
