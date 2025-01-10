namespace DfT.DTRO.Enums;

/// <summary>
/// Rate line type
/// </summary>
public enum RateLineType
{
    /// <summary>
    /// Flat rate line type
    /// </summary>
    [Display(Name = "flatRate")]
    FlatRate,

    /// <summary>
    /// Incrementing rate line type
    /// </summary>
    [Display(Name = "incrementingRate")]
    IncrementingRate,

    /// <summary>
    /// Flat rate tier type
    /// </summary>
    [Display(Name = "flatRateTier")]
    FlatRateTier,

    /// <summary>
    /// Unit rate line type
    /// </summary>
    [Display(Name = "perUnit")]
    PerUnit
}