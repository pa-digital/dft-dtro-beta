using System.ComponentModel.DataAnnotations;

namespace DfT.DTRO.Enums;

public enum SourceActionType
{
    [Display(Name = "new")]
    New,

    [Display(Name = "amendment")]
    Amendment,

    [Display(Name = "noChange")]
    NoChange,

    [Display(Name = "errorFix")]
    ErrorFix
}