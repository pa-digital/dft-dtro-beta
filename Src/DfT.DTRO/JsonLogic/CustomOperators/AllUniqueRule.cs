using Json.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace DfT.DTRO.JsonLogic.CustomOperators;

/// <summary>
/// A rule that evaluates to <see langword="true"/> if all the arguments have unique values; otherwise <see langword="false"/>.
/// </summary>
[Operator("all_unique")]
[JsonConverter(typeof(AllUniqueRuleConverter))]
public class AllUniqueRule : Rule
{
    /// <summary>
    /// The values that should be compared for inequality.
    /// </summary>
    protected internal Rule[] Arguments { get; }

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="args">The values that should be compared for inequality.</param>
    public AllUniqueRule(params Rule[] args)
    {
        Arguments = args;
    }

    private class JsonNodeEqualityComparer : IEqualityComparer<JsonNode>
    {
        public static JsonNodeEqualityComparer Instance { get; } = new ();

        public bool Equals(JsonNode x, JsonNode y)
        {
            return x.ToString() == y.ToString();
        }

        public int GetHashCode([DisallowNull] JsonNode obj)
        {
            return obj.ToString().GetHashCode();
        }
    }

    /// <inheritdoc/>
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

/// <summary>
/// Converts <see cref="AllUniqueRule"/> to and from JSON.
/// </summary>
public class AllUniqueRuleConverter : JsonConverter<AllUniqueRule>
{
    /// <inheritdoc/>
    public override AllUniqueRule Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var node = JsonSerializer.Deserialize<JsonNode>(ref reader, options);

        var parameters = node is JsonArray
            ? node.Deserialize<Rule[]>()
            : new[] { node.Deserialize<Rule>() ! };

        return new AllUniqueRule(parameters);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, AllUniqueRule value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("all_unique");
        writer.WriteRules(value.Arguments, options);
        writer.WriteEndObject();
    }
}
