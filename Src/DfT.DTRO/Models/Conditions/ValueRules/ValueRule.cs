using System;
using System.Text.Json.Serialization;

namespace DfT.DTRO.Models.Conditions.ValueRules;

/// <summary>
/// Represents a logic rule that can be applied to a value.
/// </summary>
public interface IValueRule
{
    /// <summary>
    /// Applies this rule to the provided <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value to apply the rule to.</param>
    /// <returns>
    /// <see langword="true"/> if the rule is fulfilled by the value;
    /// otherwise <see langword="false"/>.
    /// </returns>
    public bool Apply(object value);

    /// <summary>
    /// Checks if this rule contradicts another <see cref="IValueRule"/>.
    /// </summary>
    /// <param name="other">The rule to test.</param>
    /// <returns>
    /// <see langword="true"/> if this <see cref="IValueRule"/> contradicts <paramref name="other"/>;
    /// otherwise <see langword="false"/>.
    /// </returns>
    public bool Contradicts(IValueRule other);
}

/// <summary>
/// Represents a logic rule that can be applied to a value of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of parameter used in this rule.</typeparam>
[JsonConverter(typeof(ValueRuleJsonConverterFactory))]
public interface IValueRule<T> : IValueRule
    where T : IComparable<T>
{
    /// <summary>
    /// Applies this rule to the provided <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value to apply the rule to.</param>
    /// <returns>
    /// <see langword="true"/> if the rule is fulfilled by the value;
    /// otherwise <see langword="false"/>.
    /// </returns>
    public bool Apply(T value);

    bool IValueRule.Apply(object value)
        => value is T valueAsT && Apply(valueAsT);

    /// <summary>
    /// Checks if this rule contradicts another <see cref="IValueRule{T}"/>.
    /// </summary>
    /// <param name="other">The rule to test.</param>
    /// <returns>
    /// <see langword="true"/> if this <see cref="IValueRule{T}"/> contradicts <paramref name="other"/>;
    /// otherwise <see langword="false"/>.
    /// </returns>
    public bool Contradicts(IValueRule<T> other);

    bool IValueRule.Contradicts(IValueRule other)
        => other is IValueRule<T> otherAsRuleOfT && Contradicts(otherAsRuleOfT);

    /// <summary>
    /// Creates a new <see cref="IValueRule{T}"/> that is the inversion of the current instance.
    /// </summary>
    /// <returns>The inversion of this rule.</returns>
    public IValueRule<T> Inverted();

    /// <summary>
    /// Creates a new <see cref="IValueRule{T}"/> that is the inversion of the current instance
    /// if <paramref name="shouldInvert"/> is <see langword="true"/>.
    /// </summary>
    /// <returns>
    /// The inversion of this rule if <paramref name="shouldInvert"/> is <see langword="true"/>;
    /// otherwise the same rule.
    /// </returns>
    public IValueRule<T> MaybeInverted(bool shouldInvert)
        => shouldInvert ? Inverted() : this;
}
