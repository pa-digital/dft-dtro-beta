using DfT.DTRO.Models.Conditions.Base;
using System.Collections.Generic;

namespace DfT.DTRO.Models.Conditions;

/// <summary>
/// Represents an access restriction condition.
/// </summary>
public class AccessCondition : Condition
{
    /// <summary>
    /// Indicates an access restriction type. Permissible values include
    /// ‘accessOnly’, ‘loadingAndUnloading’, ‘throughTraffic’, etc.
    /// </summary>
    public List<string> AccessConditionType { get; init; }

    /// <summary>
    /// Indicates additional condition controlling access.
    /// </summary>
    public string OtherAccessRestriction { get; init; }

    /// <inheritdoc/>
    public override object Clone()
    {
        return new AccessCondition
        {
            AccessConditionType = AccessConditionType,
            OtherAccessRestriction = OtherAccessRestriction,
            Negate = Negate,
        };
    }

    /// <inheritdoc/>
    public override bool Contradicts(Condition other)
    {
        return false;
    }

    /// <inheritdoc/>
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
