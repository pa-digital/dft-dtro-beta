using DfT.DTRO.Models.Conditions.Base;
using DfT.DTRO.Models.Conditions.Internal;

namespace DfT.DTRO.Models.Conditions;

public class VehicleCharacteristic : Condition
{
    public VehicleCharacteristics VehicleCharacteristics { get; init; }

    public override object Clone()
    {
        return new VehicleCharacteristic
        {
            VehicleCharacteristics = VehicleCharacteristics,
            Negate = Negate,
        };
    }

    public override bool Contradicts(Condition other)
    {
        if (other is not VehicleCharacteristic otherVehicleCondition)
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
        return new VehicleCharacteristic
        {
            VehicleCharacteristics = VehicleCharacteristics,
            Negate = !Negate,
        };
    }
}
