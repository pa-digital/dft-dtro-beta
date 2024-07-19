namespace DfT.DTRO.Models.Conditions.ValueRules;

public interface IAndRule : IValueRule
{
    IValueRule First { get; }

    IValueRule Second { get; }
}

public readonly record struct AndRule<T>(IValueRule<T> first, IValueRule<T> second)
    : IAndRule, IValueRule<T>
    where T : IComparable<T>
{
    IValueRule IAndRule.First => first;

    IValueRule IAndRule.Second => second;

    public bool Apply(T value)
    {
        return first.Apply(value) && second.Apply(value);
    }

    public bool Contradicts(IValueRule<T> other)
    {
        if (other is null)
        {
            return false;
        }

        return other.Contradicts(first) || other.Contradicts(second);
    }

    public IValueRule<T> Inverted()
    {
        return new OrRule<T>(first.Inverted(), second.Inverted());
    }

    public override string ToString()
    {
        return $"({first} && {second})";
    }
}
