namespace DfT.DTRO.Enums;

/// <summary>
/// Indicates order reporting point type
/// </summary>
public enum OrderReportingPointType
{
    /// <summary>
    /// Experimental amendment
    /// </summary>
    [Display(Name = "experimentalAmendment")]
    ExperimentalAmendment = 1,

    /// <summary>
    /// Experimental making permanent
    /// </summary>
    [Display(Name = "experimentalMakingPermanent")]
    ExperimentalMakingPermanent = 2,

    /// <summary>
    /// Experimental notice of making
    /// </summary>
    [Display(Name = "experimentalNoticeOfMaking")]
    ExperimentalNoticeOfMaking = 3,

    /// <summary>
    /// Experimental revocation
    /// </summary>
    [Display(Name = "experimentalRevocation")]
    ExperimentalRevocation = 4,

    /// <summary>
    /// Permanent amendment
    /// </summary>
    [Display(Name = "permanentAmendment")]
    PermanentAmendment = 5,

    /// <summary>
    /// Permanent notice of making
    /// </summary>
    [Display(Name = "permanentNoticeOfMaking")]
    PermanentNoticeOfMaking = 6,

    /// <summary>
    /// Permanent notice of proposal
    /// </summary>
    [Display(Name = "permanentNoticeOfProposal")]
    PermanentNoticeOfProposal = 7,

    /// <summary>
    /// Permanent notice of proposal
    /// </summary>
    [Display(Name = "permanentRevocation")]
    PermanentRevocation = 8,

    /// <summary>
    /// Special event order notice of making
    /// </summary>
    [Display(Name = "specialEventOrderNoticeOfMaking")]
    SpecialEventOrderNoticeOfMaking = 9,

    /// <summary>
    /// Temporary traffic regulation order, temporary traffic movement order by notice
    /// </summary>
    [Display(Name = "ttroTtmoByNotice")]
    TtroTtmoByNotice = 10,

    /// <summary>
    /// Temporary traffic regulation order, temporary traffic movement order extension
    /// </summary>
    [Display(Name = "ttroTtmoExtension")]
    TtroTtmoExtension = 11,

    /// <summary>
    /// Temporary traffic regulation order, temporary traffic movement order notice after making.
    /// </summary>
    [Display(Name = "ttroTtmoNoticeAfterMaking")]
    TtroTtmoNoticeAfterMaking = 12,

    /// <summary>
    /// Temporary traffic regulation order, temporary traffic movement order notice of intention.
    /// </summary>
    [Display(Name = "ttroTtmoNoticeOfIntention")]
    TtroTtmoNoticeOfIntention = 13,

    /// <summary>
    /// Temporary traffic regulation order, temporary traffic movement order revocation
    /// </summary>
    [Display(Name = "ttroTtmoRevocation")]
    TtroTtmoRevocation = 14,

    /// <summary>
    /// Variation by notice
    /// </summary>
    [Display(Name = "variationByNotice")]
    VariationByNotice = 15,

    /// <summary>
    /// Traffic regulation order on road active status
    /// </summary>
    [Display(Name = "troOnRoadActiveStatus")]
    TroOnRoadActiveStatus = 16
}