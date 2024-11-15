using DfT.DTRO.Models.Validation;

namespace DfT.DTRO.Services.Validation.Contracts;

public interface IRegulationValidation
{
    IList<SemanticValidationError> ValidateRegulation(DtroSubmit dtroSubmit, SchemaVersion schemaVersion);
}