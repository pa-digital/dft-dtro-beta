using DfT.DTRO.Models.Conditions.Base;

namespace DfT.DTRO.Models.Conditions;

public class NonVehicularRoadUserCondition : Condition
{
    public string NonVehicularRoadUser { get; init; }

    public override object Clone()
    {
        return new NonVehicularRoadUserCondition()
        {
            NonVehicularRoadUser = NonVehicularRoadUser,
            Negate = Negate
        };
    }

    public override bool Contradicts(Condition other)
    {
        if (other is not NonVehicularRoadUserCondition otherCondition)
        {
            return false;
        }

        var contradiction = otherCondition.NonVehicularRoadUser != NonVehicularRoadUser;

        return Negate == other.Negate ? !contradiction : contradiction;
    }

    public override Condition Negated()
    {
        return new NonVehicularRoadUserCondition()
        {
            NonVehicularRoadUser = NonVehicularRoadUser,
            Negate = !Negate
        };
    }
}
