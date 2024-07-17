using System.Text.Json.Serialization;

namespace DfT.DTRO.Models.Conditions.ValueRules;

public interface IValueRule
{
    public bool Apply(object value);

    public bool Contradicts(IValueRule other);
}

[JsonConverter(typeof(ValueRuleJsonConverterFactory))]
public interface IValueRule<T> : IValueRule
    where T : IComparable<T>
{
    public bool Apply(T value);

    bool IValueRule.Apply(object value)
        => value is T valueAsT && Apply(valueAsT);

    public bool Contradicts(IValueRule<T> other);

    bool IValueRule.Contradicts(IValueRule other)
        => other is IValueRule<T> otherAsRuleOfT && Contradicts(otherAsRuleOfT);

    public IValueRule<T> Inverted();

    public IValueRule<T> MaybeInverted(bool shouldInvert)
        => shouldInvert ? Inverted() : this;
}
