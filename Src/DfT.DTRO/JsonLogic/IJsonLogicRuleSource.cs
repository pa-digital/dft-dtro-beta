using System.Collections.Generic;
using System.Threading.Tasks;

namespace DfT.DTRO.JsonLogic;

/// <summary>
/// Provides access to JsonLogic rules.
/// </summary>
public interface IJsonLogicRuleSource
{
    /// <summary>
    /// Gets the rules registered under the specified ruleset name.
    /// </summary>
    /// <param name="rulesetName">The name of the set of rules to retrieve.</param>
    /// <returns>
    /// A <see cref="Task"/> that resolves to an enumerable of <see cref="JsonLogicValidationRule"/> objects
    /// or <see langword="null"/> if the <paramref name="rulesetName"/> did not match any
    /// registered ruleset name.
    /// </returns>
    Task<IEnumerable<JsonLogicValidationRule>> GetRules(string rulesetName);
}
