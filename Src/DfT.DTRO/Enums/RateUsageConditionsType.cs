namespace DfT.DTRO.Enums;

/// <summary>
/// Rate usage conditions type
/// </summary>
public enum RateUsageConditionsType
{
    /// <summary>
    /// Fixed duration usage conditions type
    /// </summary>
    [Display(Name = "fixedDuration")]
    FixedDuration,

    /// <summary>
    /// Fixed number usage conditions type
    /// </summary>
    [Display(Name = "fixedNumber")]
    FixedNumber,

    /// <summary>
    /// Once usage conditions 
    /// </summary>
    [Display(Name = "once")]
    Once,

    /// <summary>
    /// Unlimited usage conditions 
    /// </summary>
    [Display(Name = "unlimited")]
    Unlimited
}