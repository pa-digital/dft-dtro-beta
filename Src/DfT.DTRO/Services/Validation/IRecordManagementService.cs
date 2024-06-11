using System.Collections.Generic;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.Validation;

namespace DfT.DTRO.Services.Validation;

public interface IRecordManagementService
{
    List<SemanticValidationError> ValidateCreationRequest(DtroSubmit dtroSubmit);
}