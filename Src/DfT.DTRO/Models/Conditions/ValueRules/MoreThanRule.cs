namespace DfT.DTRO.Models.Conditions.ValueRules;

public interface IMoreThanRule : IValueRule
{
}

public readonly record struct MoreThanRule<T>(
        T value,
        bool inclusive)
    : IMoreThanRule, IValueRule<T>
    where T : IComparable<T>
{

    public bool Apply(T value)
    {
        var comparison = value.CompareTo(this.value);

        if (comparison > 0)
        {
            return true;
        }

        if (comparison == 0 && inclusive)
        {
            return true;
        }

        return false;
    }

    public bool Contradicts(IValueRule<T> other)
    {
        if (other is null)
        {
            return false;
        }

        if (other is EqualityRule<T> eq)
        {
            return !Apply(eq.value);
        }

        if (other is LessThanRule<T> lessThan)
        {
            return !Apply(lessThan.value);
        }

        if (other is AndRule<T> || other is OrRule<T>)
        {
            return other.Contradicts(this);
        }

        return false;
    }

    public IValueRule<T> Inverted()
    {
        return new LessThanRule<T>(value, !inclusive);
    }

    public override string ToString()
    {
        return $">{(inclusive ? "=" : string.Empty)}{value}";
    }
}
