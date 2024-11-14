using DfT.DTRO.Models.Validation;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DfT.DTRO.Services.Validation.Contracts;

public interface IJsonLogicValidationService
{
    Task<IList<SemanticValidationError>> ValidateRules(DtroSubmit request, string schemaVersion);

    IList<SemanticValidationError> ValidateRegulatedPlacesType(DtroSubmit request, SchemaVersion schemaVersion);

    IList<SemanticValidationError> ValidateRegulation(DtroSubmit dtroSubmit, SchemaVersion schemaVersion);

    IList<SemanticValidationError> ValidateCondition(DtroSubmit dtroSubmit, SchemaVersion schemaVersion);
}
