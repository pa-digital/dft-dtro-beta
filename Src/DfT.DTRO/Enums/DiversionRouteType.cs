namespace DfT.DTRO.Enums;

/// <summary>
/// Type of diversion route
/// </summary>
public enum DiversionRouteType
{
    /// <summary>
    /// All traffic route
    /// </summary>
    [Display(Name = "allTraffic")]
    AllTraffic,

    /// <summary>
    /// HGV route
    /// </summary>
    [Display(Name = "hgvRoute")]
    HgvRoute,

    /// <summary>
    /// Non HGV route
    /// </summary>
    [Display(Name = "non-HGVRoute")]
    NonHgvRoute
}