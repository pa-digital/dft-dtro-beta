namespace DfT.DTRO.Enums;

/// <summary>
/// Indicates point type representation
/// </summary>
public enum PointType
{
    /// <summary>
    /// Centre line point type
    /// </summary>
    [Display(Name = "centreLinePoint")]
    CentreLinePoint = 1,

    /// <summary>
    /// Traffic sign location point type
    /// </summary>
    [Display(Name = "trafficSignLocation")]
    TrafficSignLocation = 2,

    /// <summary>
    /// Other point type
    /// </summary>
    [Display(Name = "other")]
    Other = 3
}