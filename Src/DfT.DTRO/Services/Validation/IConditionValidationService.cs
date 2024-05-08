using DfT.DTRO.Models.Conditions;
using DfT.DTRO.Models.Validation;
using System.Collections.Generic;

namespace DfT.DTRO.Services.Validation;

/// <summary>
/// Provides methods to validate condition sets.
/// </summary>
public interface IConditionValidationService
{
    /// <summary>
    /// Validates the condition set, ensuring it is not contradictory.
    /// </summary>
    /// <param name="conditions">The <see cref="ConditionSet"/> to validate.</param>
    /// <returns>A list of validation errors. Empty if the <see cref="ConditionSet"/> is valid.</returns>
    public List<SemanticValidationError> Validate(ConditionSet conditions);
}
