using System.Collections.Generic;
using System.Threading.Tasks;

namespace DfT.DTRO.JsonLogic;

public interface IJsonLogicRuleSource
{
    Task<IEnumerable<JsonLogicValidationRule>> GetRules(string rulesetName);
}
