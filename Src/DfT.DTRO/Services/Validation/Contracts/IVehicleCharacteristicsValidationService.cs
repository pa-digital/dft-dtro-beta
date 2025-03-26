namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Vehicle Characteristics Validation Service
/// </summary>
public interface IVehicleCharacteristicsValidationService
{
    /// <summary>
    /// Check vechicle characteristics
    /// </summary>
    /// <param name="dtroSubmit">D-TRO record to validate</param>
    /// <returns>List of validation errors</returns>
    List<SemanticValidationError> Validate(DtroSubmit dtroSubmit);
}