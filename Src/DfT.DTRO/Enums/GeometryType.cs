namespace DfT.DTRO.Enums;

public enum GeometryType
{
    [Display(Name = "Unknown")]
    Unknown = 0,

    [Display(Name = "PointGeometry")]
    PointGeometry = 1,

    [Display(Name = "LinearGeometry")]
    LinearGeometry = 2,

    [Display(Name = "Polygon")]
    Polygon = 3,

    [Display(Name = "DirectedLinear")]
    DirectedLinear = 4,
}