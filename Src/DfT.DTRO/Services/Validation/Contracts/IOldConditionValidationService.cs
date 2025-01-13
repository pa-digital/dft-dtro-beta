using DfT.DTRO.Models.Conditions;

namespace DfT.DTRO.Services.Validation.Contracts;

public interface IOldConditionValidationService
{
    public List<SemanticValidationError> Validate(ConditionSet conditions);
}
