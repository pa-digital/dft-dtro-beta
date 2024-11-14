using DfT.DTRO.Models.Validation;

namespace DfT.DTRO.Services.Validation.Contracts;

public interface ISemanticValidationService
{
    Task<Tuple<BoundingBox, List<SemanticValidationError>>> ValidateCreationRequest(DtroSubmit request);
}