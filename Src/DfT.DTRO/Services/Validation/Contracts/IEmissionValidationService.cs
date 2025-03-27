namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// 
/// </summary>
public interface IEmissionValidationService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dtroSubmit"></param>
    /// <returns></returns>
    List<SemanticValidationError> Validate(DtroSubmit dtroSubmit);
}