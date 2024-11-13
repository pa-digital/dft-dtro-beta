using DfT.DTRO.Models.Validation;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DfT.DTRO.Services.Validation.Contracts;

public interface IJsonLogicValidationService
{
    Task<IList<SemanticValidationError>> ValidateCreationRequest(DtroSubmit request, string schemaVersion);

    IList<SemanticValidationError> ValidateCreationRequest(DtroSubmit request, SchemaVersion schemaVersion);
}
