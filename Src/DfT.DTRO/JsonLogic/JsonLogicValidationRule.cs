using Json.Logic;
using System.Text.Json.Serialization;

namespace DfT.DTRO.JsonLogic;

/// <summary>
/// Contains a <see cref="Json.Logic.Rule"/> together with information about
/// the path and error message to display when the rule is violated.
/// </summary>
public class JsonLogicValidationRule
{
    /// <summary>
    /// The name of the rule. This is optional and used internally for reference only but should be unique if defined.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// The path to display if the rule fails.
    /// </summary>
    [JsonPropertyName("path")]
    public string Path { get; set; }

    /// <summary>
    /// The error message to display if the rule fails.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }

    /// <summary>
    /// The rule that should be applied.
    /// </summary>
    [JsonConverter(typeof(LogicComponentConverter))]
    [JsonPropertyName("rule")]
    public Rule Rule { get; set; }
}
