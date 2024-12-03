using DfT.DTRO.Models.Conditions.Base;
using DfT.DTRO.Models.Conditions.Internal;

namespace DfT.DTRO.Models.Conditions;

public class TimeValidity : Condition
{
    public TimeValidities TimeValidities { get; set; }

    public override object Clone()
    {
        return new TimeValidity
        {
            TimeValidities = TimeValidities,
            Negate = Negate
        };
    }

    public override Condition Negated()
    {
        return new TimeValidity
        {
            TimeValidities = TimeValidities,
            Negate = !Negate
        };
    }

    public override bool Contradicts(Condition other)
    {
        if (other is not TimeValidity otherTimeValidity)
        {
            return false;
        }

        return TimeValidities.Contradicts(otherTimeValidity.TimeValidities,
            invertThis: Negate,
            invertOther: otherTimeValidity.Negate);
    }
}