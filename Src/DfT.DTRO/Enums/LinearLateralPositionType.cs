namespace DfT.DTRO.Enums;

/// <summary>
/// Indicates linear lateral position type
/// </summary>
public enum LinearLateralPositionType
{
    /// <summary>
    /// Centreline linear lateral position
    /// </summary>
    [Display(Name = "centreline")]
    Centreline = 1,

    /// <summary>
    /// Near linear lateral position
    /// </summary>
    [Display(Name = "near")]
    Near = 2,

    /// <summary>
    /// On kerb linear lateral position
    /// </summary>
    [Display(Name = "onKerb")]
    OnKerb = 3,

    /// <summary>
    /// Far linear lateral position
    /// </summary>
    [Display(Name = "far")]
    Far = 4
}