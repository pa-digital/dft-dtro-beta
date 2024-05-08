using DfT.DTRO.Models.Conditions.Base;
using DfT.DTRO.Models.Conditions.Internal;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DfT.DTRO.Models.Conditions;

/// <summary>
/// The type of permit.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PermitType
{
    /// <summary>
    /// A doctor permit.
    /// </summary>
    Doctor,

    /// <summary>
    /// A business permit.
    /// </summary>
    Business,

    /// <summary>
    /// A resident permit.
    /// </summary>
    Resident,

    /// <summary>
    /// A different permit.
    /// </summary>
    Other
}

/// <summary>
/// A condition regarding permits.
/// </summary>
public class PermitCondition : Condition
{
    /// <summary>
    /// Indicates the type of the referenced permit.
    /// </summary>
    public PermitType Type { get; init; }

    /// <summary>
    /// A free text name for the permit scheme referenced.
    /// </summary>
    public string SchemeIdentifier { get; init; }

    /// <summary>
    /// Optionally indicates multiple instances for an identifier for the
    /// permit scheme referenced.
    /// </summary>
    public List<string> PermitIdentifier { get; init; } = new ();

    /// <summary>
    /// Optional web address (URL) of the competent authority
    /// where an application for a permit can be requested.
    /// </summary>
    public Uri WhereToApplyForPermit { get; init; }

    /// <summary>
    /// Optional contact telephone number of the competent authority
    /// where an application for a permit can be requested.
    /// </summary>
    public string WhereToCallForPermit { get; init; }

    /// <summary>
    /// Optionally indicates whether the referenced permit
    /// is related to a specified geography.
    /// </summary>
    public bool? LocationRelatedPermit { get; init; }

    /// <summary>
    /// Optionally indicates the maximum validity duration a permit can have.
    /// </summary>
    public TimeSpan? MaxDurationOfPermit { get; init; }

    /// <summary>
    /// Optionally indicates an authority, that is responsible for the permits.
    /// </summary>
    public Authority Authority { get; init; }

    /// <summary>
    /// Object containing information relating to the cost and restrictions relating to use of a permit.
    /// </summary>
    public PermitSubjectToFee PermitSubjectToFee { get; init; }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public override bool Contradicts(Condition other)
    {
        return false;
    }
}