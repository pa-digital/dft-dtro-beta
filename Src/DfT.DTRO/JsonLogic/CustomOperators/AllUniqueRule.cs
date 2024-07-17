using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Json.Logic;

namespace DfT.DTRO.JsonLogic.CustomOperators;

[Operator("all_unique")]
[JsonConverter(typeof(AllUniqueRuleConverter))]
public class AllUniqueRule : Rule
{
    protected internal Rule[] Arguments { get; }

    public AllUniqueRule(params Rule[] args)
    {
        Arguments = args;
    }

    private class JsonNodeEqualityComparer : IEqualityComparer<JsonNode>
    {
        public static JsonNodeEqualityComparer Instance { get; } = new();

        public bool Equals(JsonNode x, JsonNode y)
        {
            return x.ToString() == y.ToString();
        }

        public int GetHashCode([DisallowNull] JsonNode obj)
        {
            return obj.ToString().GetHashCode();
        }
    }

    public override JsonNode Apply(JsonNode data, JsonNode contextData = null)
    {
        var results = new HashSet<JsonNode>(JsonNodeEqualityComparer.Instance);

        foreach (var rule in Arguments)
        {
            var value = rule.Apply(data, contextData);

            if (value is JsonArray arrayValue)
            {
                foreach (var item in arrayValue)
                {
                    if (!results.Add(item))
                    {
                        return false;
                    }
                }
            }
            else if (!results.Add(value))
            {
                return false;
            }
        }

        return true;
    }
}

public class AllUniqueRuleConverter : JsonConverter<AllUniqueRule>
{
    public override AllUniqueRule Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var node = JsonSerializer.Deserialize<JsonNode>(ref reader, options);

        var parameters = node is JsonArray
            ? node.Deserialize<Rule[]>()
            : new[] { node.Deserialize<Rule>()! };

        return new AllUniqueRule(parameters);
    }

    public override void Write(Utf8JsonWriter writer, AllUniqueRule value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("all_unique");
        writer.WriteRules(value.Arguments, options);
        writer.WriteEndObject();
    }
}
