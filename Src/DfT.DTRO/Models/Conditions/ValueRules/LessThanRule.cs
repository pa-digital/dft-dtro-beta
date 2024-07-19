namespace DfT.DTRO.Models.Conditions.ValueRules;

public interface ILessThanRule : IValueRule
{
}

public readonly record struct LessThanRule<T>(
    T value,
    bool inclusive)
    : ILessThanRule, IValueRule<T>
    where T : IComparable<T>
{

    public bool Apply(T value)
    {
        var comparison = this.value.CompareTo(value);

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

        if (other is MoreThanRule<T> moreThan)
        {
            return !Apply(moreThan.value);
        }

        if (other is AndRule<T> || other is OrRule<T>)
        {
            return other.Contradicts(this);
        }

        return false;

        throw new NotImplementedException();
    }

    public IValueRule<T> Inverted()
    {
        return new MoreThanRule<T>(value, !inclusive);
    }

    public override string ToString()
    {
        return $"<{(inclusive ? "=" : string.Empty)}{value}";
    }
}
