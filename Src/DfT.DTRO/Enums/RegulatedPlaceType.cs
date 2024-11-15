namespace DfT.DTRO.Enums;

/// <summary>
/// Indicates location of the Regulation or Diversion Route.
/// </summary>
public enum RegulatedPlaceType
{
    /// <summary>
    /// Indicates that the given location represents the location of the effect of the Regulation.
    /// </summary>
    [Display(Name = "regulationLocation")]
    RegulationLocation = 1,

    /// <summary>
    /// Indicates that the given location represents a Diversion Route related to the Regulation Location.
    /// </summary>
    [Display(Name = "diversionRoute")]
    DiversionRoute = 2
}