using System.Text.Json.Serialization;

namespace DfT.DTRO.Models.Conditions.Base;

[JsonConverter(typeof(ConditionJsonConverter))]
public abstract class Condition : ICloneable
{
    public bool Negate { get; init; }

    public abstract object Clone();

    public abstract Condition Negated();

    public abstract bool Contradicts(Condition other);
}
