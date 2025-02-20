namespace DfT.DTRO.Enums;

/// <summary>
/// Indicates what regulation are accepted
/// </summary>
public enum RegulationType
{
    /// <summary>
    /// Speed limit value based regulation
    /// </summary>
    [Display(Name = "speedLimitValueBased")]
    SpeedLimitValueBased = 1,

    /// <summary>
    /// Speed limit profile based regulation
    /// </summary>
    [Display(Name = "speedLimitProfileBased")]
    SpeedLimitProfileBased = 2,

    /// <summary>
    /// General regulation
    /// </summary>
    [Display(Name = "generalRegulation")]
    GeneralRegulation = 3,

    /// <summary>
    /// Off list regulation
    /// </summary>
    [Display(Name = "offListRegulation")]
    OffListRegulation = 4,

    /// <summary>
    /// Temporary regulation
    /// </summary>
    [Display(Name = "temporaryRegulation")]
    TemporaryRegulation = 5
}