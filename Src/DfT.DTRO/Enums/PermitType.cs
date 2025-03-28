using System.Text.Json.Serialization;

namespace DfT.DTRO.Enums;

/// <summary>
/// List of permit types
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PermitType
{
    /// <summary>
    /// Doctor permit type
    /// </summary>
    [Display(Name = "doctor")]
    Doctor,

    /// <summary>
    /// Business permit type
    /// </summary>
    [Display(Name = "business")]
    Business,

    /// <summary>
    /// Resident permit type
    /// </summary>
    [Display(Name = "resident")]
    Resident,

    /// <summary>
    /// Other permit type
    /// </summary>
    [Display(Name = "other")]
    Other
}