using Json.Logic;
using Json.More;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace DfT.DTRO.JsonLogic.CustomOperators;

/// <summary>
/// Flattens a nested array of conditions.
/// </summary>
[Operator("flatten_conditions")]
[JsonConverter(typeof(FlattenConditionsRuleConverter))]
public class FlattenConditionsRule : Rule
{
    /// <summary>
    /// The <see cref="Rule" /> that should resolve to an array being the data source.
    /// </summary>
    protected internal Rule Source { get; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="source">The <see cref="Rule" /> that should resolve to an array being the data source.</param>
    public FlattenConditionsRule(Rule source)
    {
        Source = source;
    }

    /// <inheritdoc />
    public override JsonNode Apply(JsonNode data, JsonNode contextData = null)
    {
        JsonNode source = Source.Apply(data, contextData);

        if (source is not JsonArray sourceArray)
        {
            return false;
        }

        JsonArray result = new ();

        List<JsonObject> flattenedConditions = Flatten(sourceArray.Select(it => it.AsObject()).ToList());

        foreach (JsonObject condition in flattenedConditions)
        {
            result.Add(condition.Copy());
        }

        return result;
    }

    private List<JsonObject> Flatten(List<JsonObject> sourceConditions)
    {
        List<JsonObject> result = new ();

        foreach (JsonObject condition in sourceConditions)
        {
            if (condition.TryGetPropertyValue("conditions", out JsonNode conditions))
            {
                List<JsonObject> innerConditions =
                    conditions.AsArray().Select(innerCondition => innerCondition.AsObject()).ToList();

                result.AddRange(Flatten(innerConditions));
            }
            else
            {
                result.Add(condition);
            }
        }

        return result;
    }
}

/// <summary>
/// Converts <see cref="FlattenConditionsRule" /> to and from JSON.
/// </summary>
public class FlattenConditionsRuleConverter : JsonConverter<FlattenConditionsRule>
{
    /// <inheritdoc />
    public override FlattenConditionsRule Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        JsonNode node = JsonSerializer.Deserialize<JsonNode>(ref reader, options);

        Rule[] parameters = node is JsonArray
            ? node.Deserialize<Rule[]>()
            : new[] { node.Deserialize<Rule>() ! };

        if (parameters is not { Length: 1 })
        {
            throw new JsonException("The rule needs an array with a single parameter.");
        }

        return new FlattenConditionsRule(parameters[0]);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, FlattenConditionsRule value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("flatten_conditions");
        writer.WriteRule(value.Source, options);
        writer.WriteEndObject();
    }
}