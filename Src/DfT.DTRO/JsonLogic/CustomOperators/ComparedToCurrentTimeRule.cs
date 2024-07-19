using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Json.Logic;

namespace DfT.DTRO.JsonLogic.CustomOperators;

[Operator("compared_to_current_time")]
[JsonConverter(typeof(ComparedToCurrentTimeRuleConverter))]
public class ComparedToCurrentTimeRule : Rule
{
    protected internal Rule Value { get; }

    protected internal Rule Operator { get; }

    protected internal Rule Offset { get; }

    protected internal ReadOnlyDictionary<string, Func<DateTime, DateTime, bool>> OperatorToPredicate { get; } = new(
        new Dictionary<string, Func<DateTime, DateTime, bool>>
        {
            { "==", (l, r) => l == r },
            { "!=", (l, r) => l != r },
            { "<=", (l, r) => l <= r },
            { ">=", (l, r) => l >= r },
            { "<", (l, r) => l < r },
            { ">", (l, r) => l > r }
        });

    public ComparedToCurrentTimeRule(Rule value, Rule op, Rule offset = null)
    {
        Value = value;
        Operator = op;
        Offset = offset;
    }

    public override JsonNode Apply(JsonNode data, JsonNode contextData = null)
    {
        var value = Value.Apply(data, contextData);
        var op = Operator.Apply(data, contextData);
        var offset = Offset?.Apply(data, contextData);

        var offsetValue = TimeSpan.Zero;
        var now = DateTime.UtcNow;

        if (((DateTime?)value) is not DateTime left)
        {
            return false;
        }

        if ((string)op is not string operation)
        {
            return false;
        }

        if (!OperatorToPredicate.TryGetValue(operation, out Func<DateTime, DateTime, bool> predicate))
        {
            return false;
        }

        if ((string)offset is string off)
        {
            offsetValue = TimeSpan.Parse(off, CultureInfo.InvariantCulture);
        }

        var right = now + offsetValue;

        return predicate.Invoke(left, right);
    }
}

public class ComparedToCurrentTimeRuleConverter : JsonConverter<ComparedToCurrentTimeRule>
{
    public override ComparedToCurrentTimeRule Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var node = JsonSerializer.Deserialize<JsonNode>(ref reader, options);

        var parameters = node is JsonArray
            ? node.Deserialize<Rule[]>()
            : new[] { node.Deserialize<Rule>()! };

        if (parameters.Length < 2 || parameters.Length > 3)
        {
            throw new JsonException("The compared_to_current_time rule needs an array with 2 or 3 values.");
        }

        Rule offset = null;

        if (parameters.Length == 3)
        {
            offset = parameters[2];
        }

        return new ComparedToCurrentTimeRule(parameters[0], parameters[1], offset);
    }

    public override void Write(Utf8JsonWriter writer, ComparedToCurrentTimeRule value, JsonSerializerOptions options)
    {
        var rules = new List<Rule>() { value.Value, value.Operator };

        if (value.Offset is Rule offset)
        {
            rules.Add(offset);
        }

        writer.WriteStartObject();
        writer.WritePropertyName("compared_to_current_time");
        writer.WriteRules(rules, options);
        writer.WriteEndObject();
    }
}