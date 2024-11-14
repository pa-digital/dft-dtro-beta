namespace DfT.DTRO.Enums;

/// <summary>
/// Condition operator type
/// </summary>
public enum OperatorType
{
    /// <summary>
    /// Two or more conditions are valid
    /// </summary>
    [Display(Name = "and")]
    And = 1,

    /// <summary>
    /// One or another condition is valid
    /// </summary>
    [Display(Name = "or")]
    Or = 2,

    /// <summary>
    /// <see cref="And"/> or <see cref="Or"/>
    /// </summary>

    [Display(Name = "xOr")]
    XOr = 3
}