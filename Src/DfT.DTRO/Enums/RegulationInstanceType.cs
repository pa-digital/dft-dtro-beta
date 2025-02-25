namespace DfT.DTRO.Enums;

/// <summary>
/// Indicates instance regulation
/// </summary>
public enum RegulationInstanceType
{
    /// <summary>
    /// Speed limit value based instance regulation
    /// </summary>
    [Display(Name = "speedLimitValueBased")]
    SpeedLimitValueBased = 1,

    /// <summary>
    /// Speed limit profile based instance regulation
    /// </summary>
    [Display(Name = "speedLimitProfileBased")]
    SpeedLimitProfileBased = 2,

    /// <summary>
    /// General regulation instance regulation
    /// </summary>
    [Display(Name = "generalRegulation")]
    GeneralRegulation = 3,

    /// <summary>
    /// Off list instance regulation
    /// </summary>
    [Display(Name = "offListRegulation")]
    OffListRegulation = 4,

    /// <summary>
    /// Temporary instance regulation
    /// </summary>
    [Display(Name = "temporaryRegulation")]
    TemporaryRegulation = 5
}