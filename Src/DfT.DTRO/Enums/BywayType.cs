namespace DfT.DTRO.Enums;

/// <summary>
/// Indicates the nature byways
/// </summary>
public enum BywayType
{
    /// <summary>
    /// footpath byway
    /// </summary>
    [Display(Name = "footpath")]
    FootPath = 1,

    /// <summary>
    /// road byway
    /// </summary>
    [Display(Name = "road")]
    Road = 2,

    /// <summary>
    /// bridleway byway
    /// </summary>
    [Display(Name = "bridleway")]
    Bridleway = 3,

    /// <summary>
    /// cycle track byway
    /// </summary>
    [Display(Name = "cycleTrack")]
    CycleTrack = 4,

    /// <summary>
    /// restricted byway
    /// </summary>
    [Display(Name = "restrictedByway")]
    RestrictedByway = 5,

    /// <summary>
    /// open to all traffic byway
    /// </summary>
    [Display(Name = "bywayOpenToAllTraffic")]
    BywayOpenToAllTraffic = 6
}