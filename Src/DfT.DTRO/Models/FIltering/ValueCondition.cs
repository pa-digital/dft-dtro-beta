namespace DfT.DTRO.Models.Filtering;

[DataContract]
public class ValueCondition<T>
    where T : IComparable<T>
{
    [Required]
    [EnumDataType(typeof(ComparisonOperator))]
    [DataMember(Name = "operator")]
    public ComparisonOperator Operator { get; set; }

    [Required]
    [DataMember(Name = "value")]
    public T Value { get; set; }

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