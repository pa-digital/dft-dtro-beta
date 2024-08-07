namespace DfT.DTRO.Enums;

/// <summary>
/// Provision action type statements.
/// </summary>
public enum ProvisionActionType
{
    /// <summary>
    /// New provision action type.
    /// </summary>
    [Display(Name = "new")]
    New,

    /// <summary>
    /// Partial amendment provision action type.
    /// </summary>
    [Display(Name = "partialAmendment")]
    PartialAmendment,

    /// <summary>
    /// Full amendment provision action type.
    /// </summary>
    [Display(Name = "fullAmendment")]
    FullAmendment,

    /// <summary>
    /// Partial revoke provision action type.
    /// </summary>
    [Display(Name = "partialRevoke")]
    PartialRevoke,

    /// <summary>
    /// Full revoke provision action type.
    /// </summary>
    [Display(Name = "fullRevoke")]
    FullRevoke,

    /// <summary>
    /// No change provision action type.
    /// </summary>
    [Display(Name = "noChange")]
    NoChange,

    /// <summary>
    /// Error fix provision action type.
    /// </summary>
    [Display(Name = "errorFix")]
    ErrorFix
}