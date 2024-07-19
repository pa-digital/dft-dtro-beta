using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Json.Logic;
using Json.More;

namespace DfT.DTRO.JsonLogic.CustomOperators;

[Operator("flatten_conditions")]
[JsonConverter(typeof(FlattenConditionsRuleConverter))]
public class FlattenConditionsRule : Rule
{
    protected internal Rule Source { get; }

    public FlattenConditionsRule(Rule source)
    {
        Source = source;
    }

    public override JsonNode Apply(JsonNode data, JsonNode contextData = null)
    {
        JsonNode source = Source.Apply(data, contextData);

        if (source is not JsonArray sourceArray)
        {
            return false;
        }

        JsonArray result = new();

        List<JsonObject> flattenedConditions = Flatten(sourceArray.Select(it => it.AsObject()).ToList());

        foreach (JsonObject condition in flattenedConditions)
        {
            result.Add(condition.Copy());
        }

        return result;
    }

    private List<JsonObject> Flatten(List<JsonObject> sourceConditions)
    {
        List<JsonObject> result = new();

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

public class FlattenConditionsRuleConverter : JsonConverter<FlattenConditionsRule>
{
    public override FlattenConditionsRule Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        JsonNode node = JsonSerializer.Deserialize<JsonNode>(ref reader, options);

        Rule[] parameters = node is JsonArray
            ? node.Deserialize<Rule[]>()
            : new[] { node.Deserialize<Rule>()! };

        if (parameters is not { Length: 1 })
        {
            throw new JsonException("The rule needs an array with a single parameter.");
        }

        return new FlattenConditionsRule(parameters[0]);
    }

    public override void Write(Utf8JsonWriter writer, FlattenConditionsRule value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("flatten_conditions");
        writer.WriteRule(value.Source, options);
        writer.WriteEndObject();
    }
}