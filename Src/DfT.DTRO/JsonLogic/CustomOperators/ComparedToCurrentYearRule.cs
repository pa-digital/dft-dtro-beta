using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Json.Logic;

namespace DfT.DTRO.JsonLogic.CustomOperators;

[Operator("compared_to_current_year")]
[JsonConverter(typeof(ComparedToCurrentYearRuleConverter))]
public class ComparedToCurrentYearRule : Rule
{
    protected internal Rule Value { get; }

    protected internal Rule Operator { get; }

    protected internal ReadOnlyDictionary<string, Func<int, int, bool>> OperatorToPredicate { get; } = new(
        new Dictionary<string, Func<int, int, bool>>
        {
            { "==", (l, r) => l == r },
            { "!=", (l, r) => l != r },
            { "<=", (l, r) => l <= r },
            { ">=", (l, r) => l >= r },
            { "<", (l, r) => l < r },
            { ">", (l, r) => l > r }
        });

    public ComparedToCurrentYearRule(Rule value, Rule op)
    {
        Value = value;
        Operator = op;
    }

    public override JsonNode Apply(JsonNode data, JsonNode contextData = null)
    {
        JsonNode value = Value.Apply(data, contextData);
        JsonNode op = Operator.Apply(data, contextData);

        int currentYear = DateTime.UtcNow.Year;

        if ((int?)value is not int left)
        {
            return false;
        }

        if ((string)op is not string operation)
        {
            return false;
        }

        if (!OperatorToPredicate.TryGetValue(operation, out Func<int, int, bool> predicate))
        {
            return false;
        }

        return predicate.Invoke(left, currentYear);
    }
}

public class ComparedToCurrentYearRuleConverter : JsonConverter<ComparedToCurrentYearRule>
{
    public override ComparedToCurrentYearRule Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        JsonNode node = JsonSerializer.Deserialize<JsonNode>(ref reader, options);

        Rule[] parameters = node is JsonArray
            ? node.Deserialize<Rule[]>()
            : new[] { node.Deserialize<Rule>()! };

        if (parameters.Length != 2)
        {
            throw new JsonException("The compared_to_current_year rule needs an array with 2 values.");
        }

        return new ComparedToCurrentYearRule(parameters[0], parameters[1]);
    }

    public override void Write(Utf8JsonWriter writer, ComparedToCurrentYearRule value, JsonSerializerOptions options)
    {
        List<Rule> rules = new() { value.Value, value.Operator };

        writer.WriteStartObject();
        writer.WritePropertyName("compared_to_current_year");
        writer.WriteRules(rules, options);
        writer.WriteEndObject();
    }
}