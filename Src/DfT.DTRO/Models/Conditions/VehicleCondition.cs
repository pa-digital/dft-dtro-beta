using DfT.DTRO.Models.Conditions.Base;
using DfT.DTRO.Models.Conditions.Internal;

namespace DfT.DTRO.Models.Conditions;

public class VehicleCondition : Condition
{
    public VehicleCharacteristics VehicleCharacteristics { get; init; }

    public override object Clone()
    {
        return new VehicleCondition
        {
            VehicleCharacteristics = VehicleCharacteristics,
            Negate = Negate,
        };
    }

    public override bool Contradicts(Condition other)
    {
        if (other is not VehicleCondition otherVehicleCondition)
        {
            return false;
        }

        return VehicleCharacteristics.Contradicts(
            otherVehicleCondition.VehicleCharacteristics,
            invertThis: Negate,
            invertOther: otherVehicleCondition.Negate);
    }

    public override Condition Negated()
    {
        return new VehicleCondition
        {
            VehicleCharacteristics = VehicleCharacteristics,
            Negate = !Negate,
        };
    }
}
