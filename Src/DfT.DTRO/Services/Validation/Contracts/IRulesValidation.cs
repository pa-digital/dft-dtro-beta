#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DfT.DTRO.Services.Validation.Contracts;

public interface IRulesValidation
{
    Task<List<SemanticValidationError>> ValidateRules(DtroSubmit request, string schemaVersion);
}
