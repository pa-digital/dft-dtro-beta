using System;
using System.Text.Json.Serialization;

namespace DfT.DTRO.Models.Conditions.Base;

/// <summary>
/// A base class for Dtro condition.
/// </summary>
[JsonConverter(typeof(ConditionJsonConverter))]
public abstract class Condition : ICloneable
{
    /// <summary>
    /// If <see langword="true"/> signifies that this condition should be negated.
    /// </summary>
    public bool Negate { get; init; }

    /// <inheritdoc/>
    public abstract object Clone();

    /// <summary>
    /// Creates a negation of this <see cref="Condition"/>.
    /// </summary>
    /// <returns>A copy of this object - negated.</returns>
    public abstract Condition Negated();

    /// <summary>
    /// Checks if this <see cref="Condition"/> contradicts another <see cref="Condition"/> (both can't be true at the same time).
    /// </summary>
    /// <remarks>
    /// This method should be commutative, such that <c>this.Contradicts(other) == other.Contradicts(this)</c> is always <see langword="true"/>.
    /// </remarks>
    /// <param name="other">The <see cref="Condition"/> to check contradiction with.</param>
    /// <returns><see langword="true"/> if this <see cref="Condition"/> contradicts <paramref name="other"/>; otherwise <see langword="false"/>.</returns>
    public abstract bool Contradicts(Condition other);
}
