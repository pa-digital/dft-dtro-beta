using DfT.DTRO.Models.Conditions;
using DfT.DTRO.Models.Conditions.Base;
using ConditionConjunction = System.Collections.Generic.List<DfT.DTRO.Models.Conditions.Base.Condition>;
using ConditionDnf = System.Collections.Generic.List<System.Collections.Generic.List<DfT.DTRO.Models.Conditions.Base.Condition>>;

namespace DfT.DTRO.Services.Validation.Implementation;

public class OldConditionValidationService : IOldConditionValidationService
{
    public List<SemanticValidationError> Validate(ConditionSet conditions)
    {
        var errors = new List<SemanticValidationError>();

        var dnf = ConvertToDnf(conditions);

        if (dnf.All(it => it.Pairs().Any(it => it.Item1.Contradicts(it.Item2))))
        {
            errors.Add(new SemanticValidationError { Message = "The expression is always false." });
        }

        return errors;
    }

    private ConditionDnf ConvertToDnf(ConditionSet conditions)
    {
        conditions = ExpandXOr(conditions);
        conditions = PropagateNegation(conditions);
        return ToDnf(conditions);
    }

    private ConditionDnf ToDnf(Condition condition)
    {
        if (condition is ConditionSet conditionSet)
        {
            ToDnf(conditionSet);
        }

        return new ConditionDnf { new ConditionConjunction { condition } };
    }

    private ConditionDnf ToDnf(ConditionSet conditions)
        => conditions.Operator switch
        {
            OperatorType.And => AndToDnf(conditions),
            OperatorType.Or => OrToDnf(conditions),

            _ => throw new InvalidOperationException()
        };

    private Condition PropagateNegation(Condition target)
    {
        if (target is ConditionSet conditionSet)
        {
            return PropagateNegation(conditionSet);
        }

        return target;
    }

    private ConditionSet PropagateNegation(ConditionSet conditionSet)
    {
        var newConditions = new ConditionConjunction();

        if (conditionSet.Negate)
        {
            var newOp =
                conditionSet.Operator == OperatorType.And
                ? OperatorType.Or
                : OperatorType.And;

            foreach (var condition in conditionSet)
            {
                newConditions.Add(PropagateNegation(condition.Negated()));
            }

            return new ConditionSet(newConditions, newOp);
        }

        foreach (var condition in conditionSet)
        {
            newConditions.Add(PropagateNegation(condition));
        }

        return new ConditionSet(newConditions, conditionSet.Operator);
    }

    private Condition ExpandXOr(Condition target)
    {
        if (target is ConditionSet conditions)
        {
            ExpandXOr(conditions);
        }

        return target;
    }

    private ConditionSet ExpandXOr(ConditionSet conditions)
    {
        var newConditions = new ConditionConjunction();

        if (conditions.Operator != OperatorType.XOr)
        {
            foreach (var condition in conditions)
            {
                newConditions.Add(ExpandXOr(condition));
            }

            return new ConditionSet(newConditions, conditions.Operator)
            {
                Negate = conditions.Negate
            };
        }

        var expanded = ExpandXOr(conditions.ElementAt(0), conditions.ElementAt(1));

        foreach (var condition in conditions.Skip(2))
        {
            expanded = ExpandXOr(ConditionSet.XOr(expanded, condition));
        }

        return expanded;
    }

    private ConditionSet ExpandXOr(Condition left, Condition right)
    {
        left = ExpandXOr(left);
        right = ExpandXOr(right);

        return
            ConditionSet.And(
                ConditionSet.Or(left, right),
                ConditionSet.And(left, right).Negated());
    }

    private ConditionDnf AndToDnf(ConditionSet conditions)
    {
        var result = DnfConjunction(ToDnf(conditions.ElementAt(0)), ToDnf(conditions.ElementAt(1)));

        foreach (var condition in conditions.Skip(2))
        {
            result = DnfConjunction(result, ToDnf(condition));
        }

        return result;
    }

    private ConditionDnf OrToDnf(ConditionSet conditions)
    {
        var result = DnfDisjunction(ToDnf(conditions.ElementAt(0)), ToDnf(conditions.ElementAt(1)));

        foreach (var condition in conditions.Skip(2))
        {
            result = DnfDisjunction(result, ToDnf(condition));
        }

        return result;
    }

    private ConditionDnf DnfConjunction(ConditionDnf first, ConditionDnf second)
    {
        var result = new ConditionDnf();

        foreach (var firstConditions in first)
        {
            foreach (var secondConditions in second)
            {
                var newList = new ConditionConjunction(firstConditions);
                newList.AddRange(secondConditions);

                result.Add(newList);
            }
        }

        return result;
    }

    private ConditionDnf DnfDisjunction(ConditionDnf first, ConditionDnf second)
    {
        var result = new ConditionDnf(first);
        result.AddRange(second);

        return result;
    }
}
