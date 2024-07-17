namespace DfT.DTRO.Models.Conditions.ValueRules;
public readonly record struct OrRule<T>(IValueRule<T> first, IValueRule<T> second)
    : IValueRule<T>
    where T : IComparable<T>
{

    public bool Apply(T value)
    {
        return first.Apply(value) || second.Apply(value);
    }

    public bool Contradicts(IValueRule<T> other)
    {
        return other.Contradicts(first) && other.Contradicts(second);
    }

    public IValueRule<T> Inverted()
    {
        return new AndRule<T>(first.Inverted(), second.Inverted());
    }

    public override string ToString()
    {
        return $"({first} || {second})";
    }
}
