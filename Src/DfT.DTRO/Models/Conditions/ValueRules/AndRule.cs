using System;

namespace DfT.DTRO.Models.Conditions.ValueRules;

/// <summary>
/// Represents a rule that is a conjunction of two other rules.
/// </summary>
public interface IAndRule : IValueRule
{
    /// <summary>
    /// The first rule in the conjunction.
    /// </summary>
    IValueRule First { get; }

    /// <summary>
    /// The second rule in the conjunction.
    /// </summary>
    IValueRule Second { get; }
}

/// <summary>
/// Represents a rule that is a conjunction of two other rules.
/// </summary>
/// <typeparam name="T">The type of parameter used in this rule.</typeparam>
/// <param name="first">The first rule in the conjunction.</param>
/// <param name="second">The second rule in the conjunction.</param>
public readonly record struct AndRule<T>(IValueRule<T> first, IValueRule<T> second)
    : IAndRule, IValueRule<T>
    where T : IComparable<T>
{
    IValueRule IAndRule.First => first;

    IValueRule IAndRule.Second => second;

    /// <inheritdoc/>
    public bool Apply(T value)
    {
        return first.Apply(value) && second.Apply(value);
    }

    /// <inheritdoc/>
    public bool Contradicts(IValueRule<T> other)
    {
        if (other is null)
        {
            return false;
        }

        return other.Contradicts(first) || other.Contradicts(second);
    }

    /// <inheritdoc/>
    public IValueRule<T> Inverted()
    {
        return new OrRule<T>(first.Inverted(), second.Inverted());
    }

    /// <summary>
    /// Returns a string representation of this rule.
    /// </summary>
    /// <returns>A string representation of this rule.</returns>
    public override string ToString()
    {
        return $"({first} && {second})";
    }
}
