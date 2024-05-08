using DfT.DTRO.Models.Conditions.Base;
using System.Text.Json.Serialization;

namespace DfT.DTRO.Models.Conditions;

/// <summary>
/// The types of road referenced in <see cref="RoadCondition"/>.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RoadType
{
    /// <summary>
    /// A motorway.
    /// </summary>
    Motorway,

    /// <summary>
    /// A truck road.
    /// </summary>
    TruckRoad,

    /// <summary>
    /// A main road.
    /// </summary>
    MainRoad,

    /// <summary>
    /// A different road.
    /// </summary>
    Other
}

/// <summary>
/// Represents a condition regarding the type of road.
/// </summary>
public class RoadCondition : Condition
{
    /// <summary>
    /// The type of road.
    /// </summary>
    public RoadType RoadType { get; init; }

    /// <inheritdoc/>
    public override object Clone()
    {
        return new RoadCondition
        {
            RoadType = RoadType,
            Negate = Negate,
        };
    }

    /// <inheritdoc/>
    public override Condition Negated()
    {
        return new RoadCondition
        {
            RoadType = RoadType,
            Negate = !Negate,
        };
    }

    /// <inheritdoc/>
    public override bool Contradicts(Condition other)
    {
        return false;
    }
}
