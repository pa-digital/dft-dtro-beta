using System.Text.Json.Serialization;
using DfT.DTRO.Models.Conditions.Base;
using DfT.DTRO.Models.Conditions.Internal;

namespace DfT.DTRO.Models.Conditions;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PermitType
{
    Doctor,

    Business,

    Resident,

    Other
}

public class PermitCondition : Condition
{
    public PermitType Type { get; init; }

    public string SchemeIdentifier { get; init; }

    public List<string> PermitIdentifier { get; init; } = new();

    public Uri WhereToApplyForPermit { get; init; }

    public string WhereToCallForPermit { get; init; }

    public bool? LocationRelatedPermit { get; init; }

    public TimeSpan? MaxDurationOfPermit { get; init; }

    public Authority Authority { get; init; }

    public PermitSubjectToFee PermitSubjectToFee { get; init; }

    public override object Clone()
    {
        return new PermitCondition
        {
            Type = Type,
            SchemeIdentifier = SchemeIdentifier,
            PermitIdentifier = PermitIdentifier,
            WhereToApplyForPermit = WhereToApplyForPermit,
            WhereToCallForPermit = WhereToCallForPermit,
            LocationRelatedPermit = LocationRelatedPermit,
            MaxDurationOfPermit = MaxDurationOfPermit,
            Authority = Authority,
            PermitSubjectToFee = PermitSubjectToFee,
            Negate = Negate
        };
    }

    public override Condition Negated()
    {
        return new PermitCondition
        {
            Type = Type,
            SchemeIdentifier = SchemeIdentifier,
            PermitIdentifier = PermitIdentifier,
            WhereToApplyForPermit = WhereToApplyForPermit,
            WhereToCallForPermit = WhereToCallForPermit,
            LocationRelatedPermit = LocationRelatedPermit,
            MaxDurationOfPermit = MaxDurationOfPermit,
            Authority = Authority,
            PermitSubjectToFee = PermitSubjectToFee,
            Negate = !Negate
        };
    }

    public override bool Contradicts(Condition other)
    {
        return false;
    }
}