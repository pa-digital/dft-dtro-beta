using System;

namespace DfT.DTRO.Models.Conditions.ValueRules;
/// <summary>
/// Represents a rule that checks inequality against a value.
/// </summary>
/// <typeparam name="T">The type of parameter used in this rule.</typeparam>
/// <param name="value">The value to check inequality against.</param>
public readonly record struct InequalityRule<T>(T value)
    : IValueRule<T>
    where T : IComparable<T>
{
    /// <inheritdoc/>
    public bool Apply(T value)
    {
        return value.CompareTo(this.value) != 0;
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public IValueRule<T> Inverted()
    {
        return new EqualityRule<T>(value);
    }

    /// <summary>
    /// Returns a string representation of this rule.
    /// </summary>
    /// <returns>A string representation of this rule.</returns>
    public override string ToString()
    {
        return $"!={value}";
    }
}
