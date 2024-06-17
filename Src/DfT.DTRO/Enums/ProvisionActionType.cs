using System.ComponentModel.DataAnnotations;

namespace DfT.DTRO.Enums;

public enum ProvisionActionType
{
    [Display(Name = "new")]
    New,

    [Display(Name = "partialAmendment")]
    PartialAmendment,

    [Display(Name = "fullAmendment")]
    FullAmendment,

    [Display(Name = "partialRevoke")]
    PartialRevoke,

    [Display(Name = "fullRevoke")]
    FullRevoke,

    [Display(Name = "noChange")]
    NoChange,

    [Display(Name = "errorFix")]
    ErrorFix
}