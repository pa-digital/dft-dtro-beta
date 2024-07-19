namespace DfT.DTRO.Models.Conditions.ValueRules;
public readonly record struct InequalityRule<T>(T value)
    : IValueRule<T>
    where T : IComparable<T>
{
    public bool Apply(T value)
    {
        return value.CompareTo(this.value) != 0;
    }

    public bool Contradicts(IValueRule<T> other)
    {
        if (other is null)
        {
            return false;
        }

        if (other is EqualityRule<T> equality)
        {
            return Apply(equality.value);
        }

        if (other is AndRule<T> || other is OrRule<T>)
        {
            return other.Contradicts(this);
        }

        return false;
    }

    public IValueRule<T> Inverted()
    {
        return new EqualityRule<T>(value);
    }

    public override string ToString()
    {
        return $"!={value}";
    }
}
