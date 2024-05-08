using DfT.DTRO.Models.Conditions.Base;

namespace DfT.DTRO.Models.Conditions;

/// <summary>
/// Represents a condition regarding road user that is not a vehicle.
/// </summary>
public class NonVehicularRoadUserCondition : Condition
{
    /// <summary>
    /// Indicates the specific type of non-vehicular road user.
    /// </summary>
    public string NonVehicularRoadUser { get; init; }

    /// <inheritdoc/>
    public override object Clone()
    {
        return new NonVehicularRoadUserCondition()
        {
            NonVehicularRoadUser = NonVehicularRoadUser,
            Negate = Negate
        };
    }

    /// <inheritdoc/>
    public override bool Contradicts(Condition other)
    {
        if (other is not NonVehicularRoadUserCondition otherCondition)
        {
            return false;
        }

        var contradiction = otherCondition.NonVehicularRoadUser != NonVehicularRoadUser;

        return Negate == other.Negate ? !contradiction : contradiction;
    }

    /// <inheritdoc/>
    public override Condition Negated()
    {
        return new NonVehicularRoadUserCondition()
        {
            NonVehicularRoadUser = NonVehicularRoadUser,
            Negate = !Negate
        };
    }
}
