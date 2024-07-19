using System.Text.Json.Serialization;
using DfT.DTRO.Models.Conditions.Base;

namespace DfT.DTRO.Models.Conditions;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RoadType
{
    Motorway,

    TruckRoad,

    MainRoad,

    Other
}

public class RoadCondition : Condition
{
    public RoadType RoadType { get; init; }

    public override object Clone()
    {
        return new RoadCondition
        {
            RoadType = RoadType,
            Negate = Negate,
        };
    }

    public override Condition Negated()
    {
        return new RoadCondition
        {
            RoadType = RoadType,
            Negate = !Negate,
        };
    }

    public override bool Contradicts(Condition other)
    {
        return false;
    }
}
