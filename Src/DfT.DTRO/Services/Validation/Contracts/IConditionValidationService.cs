using DfT.DTRO.Models.Conditions;
using DfT.DTRO.Models.Validation;

namespace DfT.DTRO.Services.Validation.Contracts;

public interface IConditionValidationService
{
    public List<SemanticValidationError> Validate(ConditionSet conditions);
}
