namespace DfT.DTRO.Enums;

/// <summary>
/// Indicates instance regulation
/// </summary>
public enum RegulationInstanceType
{
    /// <summary>
    /// Speed limit value based instance regulation
    /// </summary>
    [Display(Name = "SpeedLimitValueBased")]
    SpeedLimitValueBased = 1,

    /// <summary>
    /// Speed limit profile based instance regulation
    /// </summary>
    [Display(Name = "SpeedLimitProfileBased")]
    SpeedLimitProfileBased = 2,

    /// <summary>
    /// General regulation instance regulation
    /// </summary>
    [Display(Name = "GeneralRegulation")]
    GeneralRegulation = 3,

    /// <summary>
    /// Off list instance regulation
    /// </summary>
    [Display(Name = "OffListRegulation")]
    OffListRegulation = 4,

    /// <summary>
    /// Temporary instance regulation
    /// </summary>
    [Display(Name = "TemporaryRegulation")]
    TemporaryRegulation = 5
}