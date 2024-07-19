using DfT.DTRO.Models.Conditions.Base;
using DfT.DTRO.Models.Conditions.ValueRules;

namespace DfT.DTRO.Models.Conditions;

public class OccupantCondition : Condition
{
    public bool DisabledWithPermit { get; init; }

    public IValueRule<int> NumbersOfOccupants { get; init; }

    public override object Clone()
    {
        return new OccupantCondition
        {
            DisabledWithPermit = DisabledWithPermit,
            NumbersOfOccupants = NumbersOfOccupants,
            Negate = Negate
        };
    }

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
