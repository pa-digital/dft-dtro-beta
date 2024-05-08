using System;

namespace DfT.DTRO.Models.Conditions.ValueRules;
/// <summary>
/// Represents a rule that is a disjunction of two other rules.
/// </summary>
/// <typeparam name="T">The type of parameter used in this rule.</typeparam>
/// <param name="first">The first rule in the disjunction.</param>
/// <param name="second">The second rule in the disjunction.</param>
public readonly record struct OrRule<T>(IValueRule<T> first, IValueRule<T> second)
    : IValueRule<T>
    where T : IComparable<T>
{

    /// <inheritdoc/>
    public bool Apply(T value)
    {
        return first.Apply(value) || second.Apply(value);
    }

    /// <inheritdoc/>
    public bool Contradicts(IValueRule<T> other)
    {
        return other.Contradicts(first) && other.Contradicts(second);
    }

    /// <inheritdoc/>
    public IValueRule<T> Inverted()
    {
        return new AndRule<T>(first.Inverted(), second.Inverted());
    }

    /// <summary>
    /// Returns a string representation of this rule.
    /// </summary>
    /// <returns>A string representation of this rule.</returns>
    public override string ToString()
    {
        return $"({first} || {second})";
    }
}
