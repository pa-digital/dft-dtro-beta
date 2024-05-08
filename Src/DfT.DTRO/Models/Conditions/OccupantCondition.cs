using DfT.DTRO.Models.Conditions.Base;
using DfT.DTRO.Models.Conditions.ValueRules;

namespace DfT.DTRO.Models.Conditions;

/// <summary>
/// Represents a condition regarding the vehicle's occupants.
/// </summary>
public class OccupantCondition : Condition
{
    /// <summary>
    /// Indicates registered disabled permit holder.
    /// </summary>
    public bool DisabledWithPermit { get; init; }

    /// <summary>
    /// Indicates the allowed number of occupants.
    /// </summary>
    public IValueRule<int> NumbersOfOccupants { get; init; }

    /// <inheritdoc/>
    public override object Clone()
    {
        return new OccupantCondition
        {
            DisabledWithPermit = DisabledWithPermit,
            NumbersOfOccupants = NumbersOfOccupants,
            Negate = Negate
        };
    }

    /// <inheritdoc/>
    public override bool Contradicts(Condition other)
    {
        if (other is not OccupantCondition otherOccupantCondition)
        {
            return false;
        }

        var numbersOfOccupants = NumbersOfOccupants?.MaybeInverted(Negate);
        var otherNumbersOfOccupants = otherOccupantCondition.NumbersOfOccupants?.MaybeInverted(other.Negate);

        return numbersOfOccupants?.Contradicts(otherNumbersOfOccupants) ?? false;
    }

    /// <inheritdoc/>
    public override Condition Negated()
    {
        return new OccupantCondition
        {
            DisabledWithPermit = DisabledWithPermit,
            NumbersOfOccupants = NumbersOfOccupants,
            Negate = !Negate
        };
    }
}
