using DfT.DTRO.Models.Validation;

namespace DfT.DTRO.Services.Validation;

public interface ISemanticValidationService
{
    Task<Tuple<BoundingBox, List<SemanticValidationError>>> ValidateCreationRequest(DtroSubmit request);
}