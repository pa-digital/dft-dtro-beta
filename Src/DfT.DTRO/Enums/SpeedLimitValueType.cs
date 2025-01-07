namespace DfT.DTRO.Enums;

/// <summary>
/// Speed limit value types
/// </summary>
public enum SpeedLimitValueType
{
    /// <summary>
    /// Maximum speed limit type
    /// </summary>
    [Display(Name = "maximumSpeedLimit")]
    MaximumSpeedLimit = 1,

    /// <summary>
    /// Minimum speed limit type
    /// </summary>
    [Display(Name = "minimumSpeedLimit")]
    MinimumSpeedLimit = 2,

    /// <summary>
    /// National speed limit well lit street default type
    /// </summary>
    [Display(Name = "nationalSpeedLimitWellLitStreetDefault")]
    NationalSpeedLimitWellLitStreetDefault = 3
}
