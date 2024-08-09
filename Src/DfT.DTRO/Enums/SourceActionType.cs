namespace DfT.DTRO.Enums;

/// <summary>
/// Source action type statements.
/// </summary>
public enum SourceActionType
{
    /// <summary>
    /// New source action type.
    /// </summary>
    [Display(Name = "new")]
    New,

    /// <summary>
    /// Amendment source action type.
    /// </summary>
    [Display(Name = "amendment")]
    Amendment,

    /// <summary>
    /// No change source action type.
    /// </summary>
    [Display(Name = "noChange")]
    NoChange,

    /// <summary>
    /// Error fix source action type.
    /// </summary>
    [Display(Name = "errorFix")]
    ErrorFix
}