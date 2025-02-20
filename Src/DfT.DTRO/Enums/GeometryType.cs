﻿namespace DfT.DTRO.Enums;

/// <summary>
/// Geometry type enums
/// </summary>
public enum GeometryType
{
    /// <summary>
    /// Unknown geometry
    /// </summary>
    [Display(Name = "unknown")]
    Unknown = 0,

    /// <summary>
    /// Point geometry
    /// </summary>
    [Display(Name = "pointGeometry")]
    PointGeometry = 1,

    /// <summary>
    /// Linear geometry
    /// </summary>
    [Display(Name = "linearGeometry")]
    LinearGeometry = 2,

    /// <summary>
    /// Polygon geometry
    /// </summary>
    [Display(Name = "polygon")]
    Polygon = 3,

    /// <summary>
    /// Directed linear geometry
    /// </summary>
    [Display(Name = "directedLinear")]
    DirectedLinear = 4,
}