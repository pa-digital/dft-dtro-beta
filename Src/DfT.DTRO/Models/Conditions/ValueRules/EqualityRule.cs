namespace DfT.DTRO.Models.Conditions.ValueRules;
public readonly record struct EqualityRule<T>(T value)
    : IValueRule<T>
    where T : IComparable<T>
{
    public bool Apply(T value)
    {
        return value.CompareTo(this.value) == 0;
    }

    public bool Contradicts(IValueRule<T> other)
    {
        if (other is null)
        {
            return false;
        }

        if (other is LessThanRule<T> lt)
        {
            return !lt.Apply(value);
        }

        if (other is MoreThanRule<T> mt)
        {
            return !mt.Apply(value);
        }

        if (other is EqualityRule<T> otherEquality)
        {
            return !Apply(otherEquality.value);
        }

        if (other is InequalityRule<T> inequality)
        {
            return Apply(inequality.value);
        }

        if (other is AndRule<T> || other is OrRule<T>)
        {
            return other.Contradicts(this);
        }

        return false;
    }

    public IValueRule<T> Inverted()
    {
        return new InequalityRule<T>(value);
    }

    public override string ToString()
    {
        return $"=={value}";
    }
}
