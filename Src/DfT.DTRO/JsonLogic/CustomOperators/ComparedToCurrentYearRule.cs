using Json.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace DfT.DTRO.JsonLogic.CustomOperators;

/// <summary>
/// A rule that compares the integer value against current year.
/// </summary>
[Operator("compared_to_current_year")]
[JsonConverter(typeof(ComparedToCurrentYearRuleConverter))]
public class ComparedToCurrentYearRule : Rule
{
    /// <summary>
    /// The value to compare against current year.
    /// </summary>
    protected internal Rule Value { get; }

    /// <summary>
    /// The operator to use for the comparison.
    /// </summary>
    protected internal Rule Operator { get; }

    /// <summary>
    /// Map between a string operator and a predicate to apply to the year.
    /// </summary>
    protected internal ReadOnlyDictionary<string, Func<int, int, bool>> OperatorToPredicate { get; } = new (
        new Dictionary<string, Func<int, int, bool>>
        {
            { "==", (l, r) => l == r },
            { "!=", (l, r) => l != r },
            { "<=", (l, r) => l <= r },
            { ">=", (l, r) => l >= r },
            { "<", (l, r) => l < r },
            { ">", (l, r) => l > r }
        });

    /// <summary>
    /// The default operator.
    /// </summary>
    /// <param name="value">A <see cref="Rule" /> that resolves to a <see cref="int" /> to be compared against current year.</param>
    /// <param name="op">A <see cref="Rule" /> that resolves to one of the recognized comparison operators.</param>
    public ComparedToCurrentYearRule(Rule value, Rule op)
    {
        Value = value;
        Operator = op;
    }

    /// <inheritdoc />
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

/// <summary>
/// Converts <see cref="ComparedToCurrentYearRule" /> to and from JSON.
/// </summary>
public class ComparedToCurrentYearRuleConverter : JsonConverter<ComparedToCurrentYearRule>
{
    /// <inheritdoc />
    public override ComparedToCurrentYearRule Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        JsonNode node = JsonSerializer.Deserialize<JsonNode>(ref reader, options);

        Rule[] parameters = node is JsonArray
            ? node.Deserialize<Rule[]>()
            : new[] { node.Deserialize<Rule>() ! };

        if (parameters.Length != 2)
        {
            throw new JsonException("The compared_to_current_year rule needs an array with 2 values.");
        }

        return new ComparedToCurrentYearRule(parameters[0], parameters[1]);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ComparedToCurrentYearRule value, JsonSerializerOptions options)
    {
        List<Rule> rules = new () { value.Value, value.Operator };

        writer.WriteStartObject();
        writer.WritePropertyName("compared_to_current_year");
        writer.WriteRules(rules, options);
        writer.WriteEndObject();
    }
}