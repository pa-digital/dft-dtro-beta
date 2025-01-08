#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace DfT.DTRO.Helpers;

public static class Constants
{
    public static IEnumerable<string> ConcreteGeometries => typeof(GeometryType).GetDisplayNames<GeometryType>();

    public static IEnumerable<string> ConditionTypes => typeof(ConditionType).GetDisplayNames<ConditionType>();

    public static IEnumerable<string> RegulationInstances => typeof(RegulationInstanceType).GetDisplayNames<RegulationInstanceType>();

    public static IEnumerable<string> RegulationTypes => typeof(RegulationType).GetDisplayNames<RegulationType>().ToList();

    public static IEnumerable<string> PreviousConditionTypes =>
        ConditionTypes
            .Select(conditionType => conditionType
                .ToBackwardCompatibility(new SchemaVersion("3.2.4")));

    public static IEnumerable<string> OperatorTypes => typeof(OperatorType).GetDisplayNames<OperatorType>().ToList();

    public static IEnumerable<string> SpeedLimitValueTypes => typeof(SpeedLimitValueType).GetDisplayNames<SpeedLimitValueType>().ToList();

    public static IEnumerable<string> SpeedLimitProfileTypes => typeof(SpeedLimitBasedType).GetDisplayNames<SpeedLimitBasedType>().ToList();

    public static IEnumerable<string> RateTypes => typeof(RateType).GetDisplayNames<RateType>().ToList();

    public static IEnumerable<string> CurrencyTypes => typeof(CurrencyType).GetDisplayNames<CurrencyType>().ToList();

    public static List<string> PossibleConditions => new() { "conditions", "Condition", "ConditionSet" };

    public static string MaxTime => "maxTime";

    public static string EndValidUsagePeriod => "endValidUsagePeriod";

    public static string ApplicableCurrency => "applicableCurrency";

    public static string AdditionalInformation => "additionalInformation";

    public static string RegulationFullText => "regulationFullText";

    public static string RegulationShortName => "regulationShortName";

    public static string RegulationType => "regulationType";

    public static string MphValue => "mphValue";

    public static string Type => "type";

    public static string IsDynamic => "isDynamic";

    public static string TimeZone => "timeZone";

    public static string Operator => "operator";

    public static string Negate => "negate";

    public static string Version => "version";

    public static string Srid27700 => "SRID=27700";

    public static string Point => "point";

    public static string LineString => "linestring";

    public static string Polygon => "polygon";

    public static string DirectedLineString => "directedLineString";
}