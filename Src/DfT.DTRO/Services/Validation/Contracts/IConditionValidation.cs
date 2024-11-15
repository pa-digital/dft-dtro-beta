using DfT.DTRO.Models.Validation;

namespace DfT.DTRO.Services.Validation.Contracts;

public interface IConditionValidation
{
    IList<SemanticValidationError> ValidateCondition(DtroSubmit dtroSubmit, SchemaVersion schemaVersion);
}