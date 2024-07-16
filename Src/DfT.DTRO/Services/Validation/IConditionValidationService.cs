using System.Collections.Generic;
using DfT.DTRO.Models.Conditions;
using DfT.DTRO.Models.Validation;

namespace DfT.DTRO.Services.Validation;

public interface IConditionValidationService
{
    public List<SemanticValidationError> Validate(ConditionSet conditions);
}
