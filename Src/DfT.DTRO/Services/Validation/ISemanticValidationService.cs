using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DfT.DTRO.Services.Validation;

/// <summary>
/// Service layer for performing semantic validation.
/// </summary>
public interface ISemanticValidationService
{
    /// <summary>
    /// Executes semantic validation against a creation request.
    /// </summary>
    /// <param name="request">The inbound DTRO submission.</param>
    /// <returns>A list of validation errors if encountered.</returns>
    Task<List<SemanticValidationError>> ValidateCreationRequest(DtroSubmit request);
}