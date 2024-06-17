using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

/// <summary>
/// Object definition for condition related to a single value.
/// </summary>
/// <typeparam name="T">Type of the value.</typeparam>
[DataContract]
public class ValueCondition<T>
    where T : IComparable<T>
{
    /// <summary>
    /// Operator used to apply the condition.
    /// </summary>
    [Required]
    [EnumDataType(typeof(ComparisonOperator))]
    [DataMember(Name = "operator")]
    public ComparisonOperator Operator { get; set; }

    /// <summary>
    /// Value of the condition.
    /// </summary>
    [Required]
    [DataMember(Name = "value")]
    public T Value { get; set; }

    /// <summary>
    /// Checks if the <paramref name="input"/> value satisfies the condition.
    /// </summary>
    /// <param name="input">Value to evaluate against.</param>
    /// <returns>Result of condition evaluation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Operator is not supported.</exception>
    public bool IsSatisfied(T input)
    {
        return Operator switch
        {
            ComparisonOperator.Equal => input.CompareTo(Value) == 0,
            ComparisonOperator.GreaterThan => input.CompareTo(Value) > 0,
            ComparisonOperator.GreaterThanOrEqual => input.CompareTo(Value) >= 0,
            ComparisonOperator.LessThan => input.CompareTo(Value) < 0,
            ComparisonOperator.LessThanOrEqual => input.CompareTo(Value) <= 0,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}