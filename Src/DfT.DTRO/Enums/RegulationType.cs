namespace DfT.DTRO.Enums;

/// <summary>
/// Indicates what regulation are accepted
/// </summary>
public enum RegulationType
{
    /// <summary>
    /// Speed limit value based regulation
    /// </summary>
    [Display(Name = "SpeedLimitValueBased")]
    SpeedLimitValueBased = 1,

    /// <summary>
    /// Speed limit profile based regulation
    /// </summary>
    [Display(Name = "SpeedLimitProfileBased")]
    SpeedLimitProfileBased = 2,

    /// <summary>
    /// General regulation
    /// </summary>
    [Display(Name = "GeneralRegulation")]
    GeneralRegulation = 3,

    /// <summary>
    /// Off list regulation
    /// </summary>
    [Display(Name = "OffListRegulation")]
    OffListRegulation = 4,

    /// <summary>
    /// Temporary regulation
    /// </summary>
    [Display(Name = "TemporaryRegulation")]
    TemporaryRegulation = 5
}