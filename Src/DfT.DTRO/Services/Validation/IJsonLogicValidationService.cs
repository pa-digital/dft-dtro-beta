using DfT.DTRO.Models.Validation;

namespace DfT.DTRO.Services.Validation;

public interface IJsonLogicValidationService
{
    Task<IList<SemanticValidationError>> ValidateCreationRequest(DtroSubmit request);
}
