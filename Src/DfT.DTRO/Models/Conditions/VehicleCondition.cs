using DfT.DTRO.Models.Conditions.Base;
using DfT.DTRO.Models.Conditions.Internal;

namespace DfT.DTRO.Models.Conditions;

/// <summary>
/// Represents a condition regarding the vehicle.
/// </summary>
public class VehicleCondition : Condition
{
    /// <summary>
    /// The characteristics that the vehicle must fulfill.
    /// </summary>
    public VehicleCharacteristics VehicleCharacteristics { get; init; }

    /// <inheritdoc/>
    public override object Clone()
    {
        return new VehicleCondition
        {
            VehicleCharacteristics = VehicleCharacteristics,
            Negate = Negate,
        };
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public override Condition Negated()
    {
        return new VehicleCondition
        {
            VehicleCharacteristics = VehicleCharacteristics,
            Negate = !Negate,
        };
    }
}
