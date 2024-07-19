using System.Text.Json.Serialization;

namespace DfT.DTRO.Models.Conditions.ValueRules;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class ValueRulePropertyNamesAttribute : JsonConverterAttribute
{
    private readonly string _operatorPropertyName;
    private readonly string _valuePropertyName;

    public ValueRulePropertyNamesAttribute(string operatorPropertyName = null, string valuePropertyName = null)
    {
        _operatorPropertyName = operatorPropertyName;
        _valuePropertyName = valuePropertyName;
    }

    public override JsonConverter CreateConverter(Type typeToConvert)
        => new ValueRuleJsonConverterFactory(_operatorPropertyName, _valuePropertyName);
}
