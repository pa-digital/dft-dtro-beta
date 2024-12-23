namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Validate regulation service
/// </summary>
public interface IRegulationValidation
{
    /// <summary>
    /// Validate regulations
    /// </summary>
    /// <param name="dtroSubmit">D-TRO payload to check.</param>
    /// <param name="schemaVersion">Schema version passed in.</param>
    /// <returns>List of validation errors</returns>
    List<SemanticValidationError> ValidateRegulation(DtroSubmit dtroSubmit, SchemaVersion schemaVersion);
}