using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using DfT.DTRO.Models.Conditions.Base;

namespace DfT.DTRO.Models.Conditions;

[JsonConverter(typeof(ConditionSetJsonConverter))]
public class ConditionSet : Condition, IEnumerable<Condition>
{
    private readonly IEnumerable<Condition> _conditions;

    public OperatorType Operator { get; init; }

    public ConditionSet(
        IEnumerable<Condition> conditions,
        OperatorType operatorType)
    {
        _conditions = conditions.ToList();
        Operator = operatorType;
    }

    public static ConditionSet XOr(params Condition[] conditions)
    {
        return new ConditionSet(conditions, OperatorType.XOr);
    }

    public static ConditionSet XOr(IEnumerable<Condition> conditions)
    {
        return new ConditionSet(conditions, OperatorType.XOr);
    }

    public static ConditionSet And(params Condition[] conditions)
    {
        return new ConditionSet(conditions, OperatorType.And);
    }

    public static ConditionSet And(IEnumerable<Condition> conditions)
    {
        return new ConditionSet(conditions, OperatorType.And);
    }

    public static ConditionSet Or(params Condition[] conditions)
    {
        return new ConditionSet(conditions, OperatorType.Or);
    }

    public static ConditionSet Or(IEnumerable<Condition> conditions)
    {
        return new ConditionSet(conditions, OperatorType.Or);
    }

    public IEnumerator<Condition> GetEnumerator()
        => _conditions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _conditions.GetEnumerator();

    public override bool Contradicts(Condition other)
    {
        return false;
    }

    public override object Clone()
    {
        return new ConditionSet(_conditions.Select(it => it.Clone()).Cast<Condition>(), Operator)
        {
            Negate = Negate
        };
    }

    public override Condition Negated()
    {
        return new ConditionSet(_conditions.Select(it => it.Clone()).Cast<Condition>(), Operator)
        {
            Negate = !Negate
        };
    }
}

public class ConditionSetJsonConverter : JsonConverter<ConditionSet>
{
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
                "and" => OperatorType.And,
                "or" => OperatorType.Or,
                "xor" => OperatorType.XOr,
                _ => throw new JsonException("Operator field is required to be one of 'and', 'or' or 'xor'.")
            })
        {
            Negate = negate
        };
    }

    public override void Write(Utf8JsonWriter writer, ConditionSet value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}