using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Json.Logic;
using Json.More;

namespace DfT.DTRO.JsonLogic.CustomOperators;

[Operator("indexed")]
[JsonConverter(typeof(IndexedRuleConverter))]
public class IndexedRule : Rule
{
    protected internal Rule Source { get; }

    public IndexedRule(Rule source)
    {
        Source = source;
    }

    public override JsonNode Apply(JsonNode data, JsonNode contextData = null)
    {
        var source = Source.Apply(data, contextData);

        if (source is not JsonArray sourceArray)
        {
            return false;
        }

        var result = new JsonArray();

        for (int i = 0; i < sourceArray.Count; i++)
        {
            var pairs = new Dictionary<string, JsonNode>()
            {
                { "index", JsonValue.Create(i) },
                { "value", sourceArray[i].Copy() }
            };
            result.Add(new JsonObject(pairs));
        }

        return result;
    }
}

public class IndexedRuleConverter : JsonConverter<IndexedRule>
{
    public override IndexedRule Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var node = JsonSerializer.Deserialize<JsonNode>(ref reader, options);

        var parameters = node is JsonArray
            ? node.Deserialize<Rule[]>()
            : new[] { node.Deserialize<Rule>()! };

        if (parameters is not { Length: 1 })
        {
            throw new JsonException("The indexed rule needs an array with a single parameter.");
        }

        return new IndexedRule(parameters[0]);
    }

    public override void Write(Utf8JsonWriter writer, IndexedRule value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("indexed");
        writer.WriteRule(value.Source, options);
        writer.WriteEndObject();
    }
}
