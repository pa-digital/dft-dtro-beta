namespace DfT.DTRO.Enums;

/// <summary>
/// Geometry type enums
/// </summary>
public enum GeometryType
{
    /// <summary>
    /// Point geometry
    /// </summary>
    [Display(Name = "PointGeometry")]
    PointGeometry = 1,

    /// <summary>
    /// Linear geometry
    /// </summary>
    [Display(Name = "LinearGeometry")]
    LinearGeometry = 2,

    /// <summary>
    /// Polygon geometry
    /// </summary>
    [Display(Name = "Polygon")]
    Polygon = 3,

    /// <summary>
    /// Directed linear geometry
    /// </summary>
    [Display(Name = "DirectedLinear")]
    DirectedLinear = 4,
}