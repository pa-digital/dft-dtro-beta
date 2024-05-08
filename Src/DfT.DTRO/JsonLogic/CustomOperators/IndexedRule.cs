using Json.Logic;
using Json.More;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace DfT.DTRO.JsonLogic.CustomOperators;

/// <summary>
/// Converts the array to an array of pairs of each element's index and value.
/// Each element of the resulting array is an object with two fields - 'index' and 'value'.
/// </summary>
[Operator("indexed")]
[JsonConverter(typeof(IndexedRuleConverter))]
public class IndexedRule : Rule
{
    /// <summary>
    /// The <see cref="Rule"/> that should resolve to an array being the data source.
    /// </summary>
    protected internal Rule Source { get; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="source">The <see cref="Rule"/> that should resolve to an array being the data source.</param>
    public IndexedRule(Rule source)
    {
        Source = source;
    }

    /// <inheritdoc/>
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

/// <summary>
/// Converts <see cref="IndexedRule"/> to and from JSON.
/// </summary>
public class IndexedRuleConverter : JsonConverter<IndexedRule>
{
    /// <inheritdoc/>
    public override IndexedRule Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var node = JsonSerializer.Deserialize<JsonNode>(ref reader, options);

        var parameters = node is JsonArray
            ? node.Deserialize<Rule[]>()
            : new[] { node.Deserialize<Rule>() ! };

        if (parameters is not { Length: 1 })
        {
            throw new JsonException("The indexed rule needs an array with a single parameter.");
        }

        return new IndexedRule(parameters[0]);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, IndexedRule value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("indexed");
        writer.WriteRule(value.Source, options);
        writer.WriteEndObject();
    }
}
