using System.Text.Json;
using System.Text.Json.Serialization;

namespace DfT.DTRO.Models.Conditions.ValueRules;

public class ValueRuleJsonConverterFactory : JsonConverterFactory
{
    private static readonly Type BaseConverterType = typeof(ValueRuleJsonConverter<>);
    private static readonly Type BaseTargetType = typeof(IValueRule<>);

    private readonly string _operatorPropertyName;
    private readonly string _valuePropertyName;

    public ValueRuleJsonConverterFactory(string operatorPropertyName = null, string valuePropertyName = null)
    {
        _operatorPropertyName = operatorPropertyName ?? "operator";
        _valuePropertyName = valuePropertyName ?? "value";
    }

    public ValueRuleJsonConverterFactory()
        : this(null, null)
    {
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.GetGenericTypeDefinition() == BaseTargetType;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type converter = BaseConverterType.MakeGenericType(typeToConvert.GetGenericArguments().First());

        return Activator.CreateInstance(converter, _operatorPropertyName, _valuePropertyName) as JsonConverter;
    }
}
