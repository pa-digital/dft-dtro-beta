using DfT.DTRO.Models.Conditions.Base;
using Json.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace DfT.DTRO.Models.Conditions;

/// <summary>
/// A set of conditions joined by an operator.
/// </summary>
[JsonConverter(typeof(ConditionSetJsonConverter))]
public class ConditionSet : Condition, IEnumerable<Condition>
{
    /// <summary>
    /// Operator types used within <see cref="ConditionSet"/>.
    /// </summary>
    public enum OperatorType
    {
        /// <summary>
        /// A logical conjunction (and) operator.
        /// </summary>
        And,

        /// <summary>
        /// A logical disjunction (or) operator.
        /// </summary>
        Or,

        /// <summary>
        /// A logical exclusive disjunction (exclusive or) operator.
        /// </summary>
        XOr,
    }

    private readonly IEnumerable<Condition> _conditions;

    /// <summary>
    /// The operator applied to the conditions contained in this set.
    /// </summary>
    public OperatorType Operator { get; init; }

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="conditions">The collection of conditions to be contained in this set.</param>
    /// <param name="operatorType">The operator to apply to the conditions.</param>
    public ConditionSet(
        IEnumerable<Condition> conditions,
        OperatorType operatorType)
    {
        _conditions = conditions.ToList();
        Operator = operatorType;
    }

    /// <summary>
    /// Creates a new condition set that represents an exclusive disjunction of provided conditions.
    /// </summary>
    /// <param name="conditions">The collection of conditions to be contained in this set.</param>
    /// <returns>A new condition set that represents an exclusive disjunction of provided conditions.</returns>
    public static ConditionSet XOr(params Condition[] conditions)
    {
        return new ConditionSet(conditions, OperatorType.XOr);
    }

    /// <summary>
    /// Creates a new condition set that represents an exclusive disjunction of provided conditions.
    /// </summary>
    /// <param name="conditions">The collection of conditions to be contained in this set.</param>
    /// <returns>A new condition set that represents an exclusive disjunction of provided conditions.</returns>
    public static ConditionSet XOr(IEnumerable<Condition> conditions)
    {
        return new ConditionSet(conditions, OperatorType.XOr);
    }

    /// <summary>
    /// Creates a new condition set that represents a conjunction of provided conditions.
    /// </summary>
    /// <param name="conditions">The collection of conditions to be contained in this set.</param>
    /// <returns>A new condition set that represents a conjunction of provided conditions.</returns>
    public static ConditionSet And(params Condition[] conditions)
    {
        return new ConditionSet(conditions, OperatorType.And);
    }

    /// <summary>
    /// Creates a new condition set that represents a conjunction of provided conditions.
    /// </summary>
    /// <param name="conditions">The collection of conditions to be contained in this set.</param>
    /// <returns>A new condition set that represents a conjunction of provided conditions.</returns>
    public static ConditionSet And(IEnumerable<Condition> conditions)
    {
        return new ConditionSet(conditions, OperatorType.And);
    }

    /// <summary>
    /// Creates a new condition set that represents a disjunction of provided conditions.
    /// </summary>
    /// <param name="conditions">The collection of conditions to be contained in this set.</param>
    /// <returns>A new condition set that represents a disjunction of provided conditions.</returns>
    public static ConditionSet Or(params Condition[] conditions)
    {
        return new ConditionSet(conditions, OperatorType.Or);
    }

    /// <summary>
    /// Creates a new condition set that represents a disjunction of provided conditions.
    /// </summary>
    /// <param name="conditions">The collection of conditions to be contained in this set.</param>
    /// <returns>A new condition set that represents a disjunction of provided conditions.</returns>
    public static ConditionSet Or(IEnumerable<Condition> conditions)
    {
        return new ConditionSet(conditions, OperatorType.Or);
    }

    /// <inheritdoc/>
    public IEnumerator<Condition> GetEnumerator()
        => _conditions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _conditions.GetEnumerator();

    /// <inheritdoc/>
    public override bool Contradicts(Condition other)
    {
        return false;
    }

    /// <inheritdoc/>
    public override object Clone()
    {
        return new ConditionSet(_conditions.Select(it => it.Clone()).Cast<Condition>(), Operator)
        {
            Negate = Negate
        };
    }

    /// <inheritdoc/>
    public override Condition Negated()
    {
        return new ConditionSet(_conditions.Select(it => it.Clone()).Cast<Condition>(), Operator)
        {
            Negate = !Negate
        };
    }
}

/// <summary>
/// Converts <see cref="ConditionSet"/> from JSON.
/// </summary>
public class ConditionSetJsonConverter : JsonConverter<ConditionSet>
{
    /// <inheritdoc/>
    public override ConditionSet Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = JsonSerializer.Deserialize<JsonNode>(ref reader, options);

        if (json is null)
        {
            return null;
        }

        if (json is not JsonObject jsonObject)
        {
            throw new JsonException("Json must be an object.");
        }

        bool negate = jsonObject.TryGetPropertyValue("negate", out JsonNode negateNode)
                      && (negateNode?.IsTruthy() ?? false);

        List<Condition> conditions = jsonObject.TryGetPropertyValue("conditions", out JsonNode conditionsNode)
            ? JsonSerializer.Deserialize<List<Condition>>(conditionsNode) : new List<Condition>();

        if (!jsonObject.TryGetPropertyValue("operator", out JsonNode operatorNode))
        {
            throw new JsonException("The 'operator' field is required.");
        }

        string operatorString = null;

        try
        {
            operatorString = operatorNode.GetValue<string>();
        }
        catch (Exception ex)
        {
            throw new JsonException("The 'operator' field must be a string.", ex);
        }

        return new ConditionSet(
            conditions,
            operatorString.ToLower() switch
            {
                "and" => ConditionSet.OperatorType.And,
                "or" => ConditionSet.OperatorType.Or,
                "xor" => ConditionSet.OperatorType.XOr,
                _ => throw new JsonException("Operator field is required to be one of 'and', 'or' or 'xor'.")
            })
        {
            Negate = negate
        };
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, ConditionSet value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}