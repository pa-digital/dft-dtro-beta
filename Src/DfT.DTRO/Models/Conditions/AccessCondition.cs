using DfT.DTRO.Models.Conditions.Base;

namespace DfT.DTRO.Models.Conditions;

public class AccessCondition : Condition
{
    public List<string> AccessConditionType { get; init; }

    public string OtherAccessRestriction { get; init; }

    public override object Clone()
    {
        return new AccessCondition
        {
            AccessConditionType = AccessConditionType,
            OtherAccessRestriction = OtherAccessRestriction,
            Negate = Negate,
        };
    }

    public override bool Contradicts(Condition other)
    {
        return false;
    }

    public override Condition Negated()
    {
        return new AccessCondition
        {
            AccessConditionType = AccessConditionType,
            OtherAccessRestriction = OtherAccessRestriction,
            Negate = !Negate,
        };
    }
}
