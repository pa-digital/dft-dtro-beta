namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Validation Condition Service
/// </summary>
public interface IConditionValidation
{
    /// <summary>
    /// Check conditions
    /// </summary>
    /// <param name="dtroSubmit">D-TRO payload to check.</param>
    /// <param name="schemaVersion">Schema version passed in.</param>
    /// <returns></returns>
    IList<SemanticValidationError> ValidateCondition(DtroSubmit dtroSubmit, SchemaVersion schemaVersion);
}