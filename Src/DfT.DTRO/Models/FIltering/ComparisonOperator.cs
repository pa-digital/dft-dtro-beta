using System.Runtime.Serialization;

namespace DfT.DTRO.Models.Filtering;

/// <summary>
/// Supported comparison operators.
/// </summary>
[DataContract]
public enum ComparisonOperator
{
    /// <summary>
    /// Equality operator
    /// </summary>
    [EnumMember(Value = "=")]
    Equal,

    /// <summary>
    /// Greater than operator
    /// </summary>
    [EnumMember(Value = ">")]
    GreaterThan,

    /// <summary>
    /// Greater than or equal operator
    /// </summary>
    [EnumMember(Value = ">=")]
    GreaterThanOrEqual,

    /// <summary>
    /// Less than operator
    /// </summary>
    [EnumMember(Value = "<")]
    LessThan,

    /// <summary>
    /// Less than or equal operator
    /// </summary>
    [EnumMember(Value = "<=")]
    LessThanOrEqual
}