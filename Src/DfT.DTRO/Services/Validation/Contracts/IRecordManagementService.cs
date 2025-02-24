namespace DfT.DTRO.Services.Validation.Contracts;

public interface IRecordManagementService
{
    List<SemanticValidationError> ValidateRecordManagement(DtroSubmit dtroSubmit, int? submittedByTa);
}