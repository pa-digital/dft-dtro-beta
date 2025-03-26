using DfT.DTRO.Models.Validation;

namespace DfT.DTRO.Services.Validation.Contracts;

public interface ISemanticValidationService
{
    Tuple<BoundingBox, List<SemanticValidationError>> ValidateCreationRequest(DtroSubmit request);
}