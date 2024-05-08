using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace DfT.DTRO.Models.Conditions.ValueRules;

/// <summary>
/// Supports converting JSON to <see cref="IValueRule{T}"/> with <typeparamref name="T"/> as the type used by the rule.
/// </summary>
/// <typeparam name="T">The generic type that will be used in the <see cref="IValueRule{T}"/>.</typeparam>
public class ValueRuleJsonConverter<T> : JsonConverter<IValueRule<T>>
    where T : IComparable<T>
{
    private readonly string _operatorPropertyName;
    private readonly string _valuePropertyName;

    /// <summary>
    /// A constructor that allows overriding parameter names.
    /// </summary>
    /// <param name="operatorPropertyName">The parameter name for the operator (<c>"operator"</c> by default).</param>
    /// <param name="valuePropertyName">The parameter name for the value (<c>"value"</c> by default).</param>
    public ValueRuleJsonConverter(string operatorPropertyName = null, string valuePropertyName = null)
    {
        _operatorPropertyName = operatorPropertyName ?? "operator";
        _valuePropertyName = valuePropertyName ?? "value";
    }

    /// <summary>
    /// The default constructor.
    /// </summary>
    public ValueRuleJsonConverter()
        : this(null)
    {
    }

    /// <inheritdoc/>
    public override IValueRule<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var array = JsonSerializer.Deserialize<JsonArray>(ref reader, options);

        var result = array.OfType<JsonObject>().Select(it => ToValueRule(it)).ToList();

        if (result.Count == 1)
        {
            return result.Single();
        }

        if (result.Count == 2)
        {
            return new AndRule<T>(result[0], result[1]);
        }

        throw new JsonException();
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, IValueRule<T> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    private IValueRule<T> ToValueRule(JsonObject jsonObject)
    {
        if (!jsonObject.TryGetPropertyValue(_operatorPropertyName, out JsonNode op))
        {
            throw new JsonException($"'{_operatorPropertyName}' is required.");
        }

        if (!jsonObject.TryGetPropertyValue(_valuePropertyName, out JsonNode value))
        {
            throw new JsonException($"'{_valuePropertyName}' is required.");
        }

        var convertedValue = value.Deserialize<T>();

        string operationString = op.GetValue<string>();

        bool inclusive = operationString.ToLower().EndsWith("orequalto");

        if (operationString.ToLower().StartsWith("equalto"))
        {
            return new EqualityRule<T>(convertedValue);
        }

        if (operationString.ToLower().StartsWith("greaterthan"))
        {
            return new MoreThanRule<T>(convertedValue, inclusive);
        }

        if (operationString.ToLower().StartsWith("lessthan"))
        {
            return new LessThanRule<T>(convertedValue, inclusive);
        }

        throw new JsonException($"The '{_operatorPropertyName}' value was not one of known operators.");
    }
}
