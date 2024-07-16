using System.Collections.Generic;
using System.Threading.Tasks;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.Validation;

namespace DfT.DTRO.Services.Validation;

public interface ISemanticValidationService
{
    Task<List<SemanticValidationError>> ValidateCreationRequest(DtroSubmit request);
}