namespace DfT.DTRO.Models.Filtering;

[DataContract]
public enum ComparisonOperator
{
    [EnumMember(Value = "=")]
    Equal,

    [EnumMember(Value = ">")]
    GreaterThan,

    [EnumMember(Value = ">=")]
    GreaterThanOrEqual,

    [EnumMember(Value = "<")]
    LessThan,

    [EnumMember(Value = "<=")]
    LessThanOrEqual
}