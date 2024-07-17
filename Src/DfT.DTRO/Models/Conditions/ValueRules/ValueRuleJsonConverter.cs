using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace DfT.DTRO.Models.Conditions.ValueRules;

public class ValueRuleJsonConverter<T> : JsonConverter<IValueRule<T>>
    where T : IComparable<T>
{
    private readonly string _operatorPropertyName;
    private readonly string _valuePropertyName;

    public ValueRuleJsonConverter(string operatorPropertyName = null, string valuePropertyName = null)
    {
        _operatorPropertyName = operatorPropertyName ?? "operator";
        _valuePropertyName = valuePropertyName ?? "value";
    }

    public ValueRuleJsonConverter()
        : this(null)
    {
    }

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
