namespace DfT.DTRO.Enums;

/// <summary>
/// Indicates what conditions are accepted
/// </summary>
public enum ConditionType
{
    /// <summary>
    /// Road condition
    /// </summary>
    [Display(Name = "RoadCondition")]
    RoadCondition = 1,

    /// <summary>
    /// Occupant condition
    /// </summary>
    [Display(Name = "OccupantCondition")]
    OccupantCondition = 2,

    /// <summary>
    /// Driver condition
    /// </summary>
    [Display(Name = "DriverCondition")]
    DriverCondition = 3,

    /// <summary>
    /// Access condition
    /// </summary>
    [Display(Name = "AccessCondition")]
    AccessCondition = 4,

    /// <summary>
    /// Time validity condition
    /// </summary>
    [Display(Name = "TimeValidity")]
    TimeValidity = 5,

    /// <summary>
    /// Non-vehicular road user condition
    /// </summary>
    [Display(Name = "NonVehicularRoadUserCondition")]
    NonVehicularRoadUserCondition = 6,

    /// <summary>
    /// Permit condition
    /// </summary>
    [Display(Name = "PermitCondition")]
    PermitCondition = 7,

    /// <summary>
    /// Vehicle condition
    /// </summary>
    [Display(Name = "VehicleCharacteristics")]
    VehicleCharacteristics = 8,

    /// <summary>
    /// New set of condition set
    /// </summary>
    [Display(Name = "ConditionSet")]
    ConditionSet = 9,

    /// <summary>
    /// Conditions under the condition set
    /// </summary>
    [Display(Name = "conditions")]
    Conditions = 10,

    /// <summary>
    /// Rate table condition
    /// </summary>
    [Display(Name = "RateTable")]
    RateTable = 11
}