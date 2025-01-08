namespace DfT.DTRO.Enums;

/// <summary>
/// Speed limit based types
/// </summary>
public enum SpeedLimitBasedType
{
    /// <summary>
    /// National speed limit dual carriageway based type
    /// </summary>
    [Display(Name = "nationalSpeedLimitDualCarriageway")]
    NationalSpeedLimitDualCarriageway = 1,

    /// <summary>
    /// National speed limit single carriageway based type
    /// </summary>
    [Display(Name = "nationalSpeedLimitSingleCarriageway")]
    NationalSpeedLimitSingleCarriageway = 2,

    /// <summary>
    /// National speed limit motorway based type
    /// </summary>
    [Display(Name = "nationalSpeedLimitMotorway")]
    NationalSpeedLimitMotorway = 3
}
