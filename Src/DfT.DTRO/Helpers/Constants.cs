#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace DfT.DTRO.Helpers;

public static class Constants
{
    public static IEnumerable<string> SourceActionTypes => typeof(SourceActionType).GetDisplayNames<SourceActionType>().ToList();

    public static IEnumerable<string> ProvisionActionTypes => typeof(ProvisionActionType).GetDisplayNames<ProvisionActionType>().ToList();

    public static IEnumerable<string> OrderReportingPointTypes => typeof(OrderReportingPointType).GetDisplayNames<OrderReportingPointType>().ToList();

    public static IEnumerable<string> RegulatedPlaceTypes => typeof(RegulatedPlaceType).GetDisplayNames<RegulatedPlaceType>();

    public static IEnumerable<string> PointTypes => typeof(PointType).GetDisplayNames<PointType>().ToList();

    public static IEnumerable<string> LinearDirectionTypes => typeof(LinearDirectionType).GetDisplayNames<LinearDirectionType>().ToList();

    public static IEnumerable<string> LinearLateralPositionTypes => typeof(LinearLateralPositionType).GetDisplayNames<LinearLateralPositionType>().ToList();

    public static IEnumerable<string> LinearTypes => typeof(LinearType).GetDisplayNames<LinearType>().ToList();

    public static IEnumerable<string> ConcreteGeometries => typeof(GeometryType).GetDisplayNames<GeometryType>();

    public static IEnumerable<string> ConditionTypes => typeof(ConditionType).GetDisplayNames<ConditionType>();

    public static IEnumerable<string> RegulationInstances => typeof(RegulationInstanceType).GetDisplayNames<RegulationInstanceType>();

    public static IEnumerable<string> RegulationTypes => typeof(RegulationType).GetDisplayNames<RegulationType>().ToList();

    public static IEnumerable<string> PreviousConditionTypes => ConditionTypes.Select(conditionType => conditionType.ToBackwardCompatibility(new SchemaVersion("3.2.4")));

    public static IEnumerable<string> OperatorTypes => typeof(OperatorType).GetDisplayNames<OperatorType>().ToList();

    public static IEnumerable<string> SpeedLimitValueTypes => typeof(SpeedLimitValueType).GetDisplayNames<SpeedLimitValueType>().ToList();

    public static IEnumerable<string> SpeedLimitProfileTypes => typeof(SpeedLimitBasedType).GetDisplayNames<SpeedLimitBasedType>().ToList();

    public static IEnumerable<string> SpeedLimitNatureTypes => typeof(SpeedLimitNatureType).GetDisplayNames<SpeedLimitNatureType>().ToList();

    public static IEnumerable<string> RateTypes => typeof(RateType).GetDisplayNames<RateType>().ToList();

    public static IEnumerable<string> CurrencyTypes => typeof(CurrencyType).GetDisplayNames<CurrencyType>().ToList();

    public static IEnumerable<string> RateUsageConditionsTypes => typeof(RateUsageConditionsType).GetDisplayNames<RateUsageConditionsType>().ToList();

    public static IEnumerable<string> RateLineTypes => typeof(RateLineType).GetDisplayNames<RateLineType>().ToList();

    public static IEnumerable<string> EventTypes => typeof(EventType).GetDisplayNames<EventType>().ToList();

    public static IEnumerable<string> BywayTypes => typeof(BywayType).GetDisplayNames<BywayType>().ToList();
    
    public static List<string> PossibleConditions => new() { "conditions", "Condition", "ConditionSet" };

    public static List<int> MphValues => new() { 10, 20, 30, 40, 50, 60, 70 };

    public static string Value => "value";

    public static string UsageCondition => "usageCondition";

    public static string MinValue => "minValue";

    public static string MaxValue => "maxValue";

    public static string IncrementPeriod => "incrementPeriod";

    public static string DurationStart => "durationStart";

    public static string DurationEnd => "durationEnd";

    public static string Description => "description";

    public static string StartValidUsagePeriod => "startValidUsagePeriod";

    public static string Sequence => "sequence";

    public static string ResetTime => "resetTime";

    public static string MinValueCollection => "minValueCollection";

    public static string MinTime => "minTime";

    public static string MaxValueCollection => "maxValueCollection";

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
  
    public static readonly HashSet<string> KeysToConvert =
    [
        "source", "provision", "regulatedplace", "geometry", "lineargeometry", "pointgeometry", "polygon",
        "directedlinear", "externalreference", "uniquestreetreferencenumber", "elementarystreetunit", "regulation",
        "speedlimitvaluebased", "speedlimitprofilebased", "generalregulation", "offlistregulation", "condition",
        "temporaryregulation", "roadcondition", "numberofoccupants", "occupantcondition", "drivercondition",
        "accesscondition", "timevalidity", "nonvehicularroadusercondition", "permitcondition", "vehiclecharacteristics",
        "conditionset", "authority", "permitsubjecttofee", "specialday", "publicholiday", "changeabletimeperiodstart",
        "changeabletimeperiodend", "calendarweekinmonth", "weekofmonth", "instanceofdaywithinmonth",
        "changeabletimeperiodsource", "changeabletimeperiodentry", "maximumgrossweightcharacteristic",
        "maximumheightcharacteristic", "maximumlengthcharacteristic", "maximumwidthcharacteristic",
        "heaviestaxleweightcharacteristic", "numberofaxlescharacteristic", "emissions", "ratetable",
        "ratelinecollection", "rateline", "consultation"
    ];

    public static string Source => "Source";

    public static string ActionType => "actionType";

    public static string ComingIntoForceDate => "comingIntoForceDate";

    public static string CurrentTraOwner => "currentTraOwner";

    public static string MadeDate => "madeDate";

    public static string Reference => "reference";

    public static string Section => "section";

    public static string StatementDescription => "statementDescription";

    public static string TraAffected => "traAffected";

    public static string TraCreator => "traCreator";

    public static string TroName => "troName";

    public static string Provision => "Provision";

    public static string ExpectedOccupancyDuration => "expectedOccupancyDuration";

    public static string OrderReportingPoint => "orderReportingPoint";

    public static string ProvisionDescription => "provisionDescription";

    public static string ActualStartOrStop => "ActualStartOrStop";

    public static string EventAt => "eventAt";

    public static string EventType => "eventType";

    public static string ExperimentalVariation => "ExperimentalVariation";

    public static string EffectOfChange => "effectOfChange";

    public static string ExpectedDuration => "expectedDuration";

    public static string ExperimentalCessation => "ExperimentalCessation";

    public static string ActualDateOfCessation => "actualDateOfCessation";

    public static string NatureOfCessation => "natureOfCessation";

    public static string RegulatedPlace => "RegulatedPlace";

    public static string Assignment => "assignment";

    public static string BusRoute => "busRoute";

    public static string BywayType => "bywayType";

    public static string Concession => "concession";

    public static string TramCar => "tramCar";

    public static string Nature => "nature";

    public static string ConsultationName => "consultationName";

    public static string EndOfConsultation => "endOfConsultation";

    public static string HowToComment => "howToComment";

    public static string LocalReference => "localReference";

    public static string Consultation => "Consultation";

    public static string PointOfContactAddress => "pointOfContactAddress";

    public static string PointOfContactEmail => "pointOfContactEmail";

    public static string StartOfConsultation => "startOfConsultation";

    public static string StatementOfReason => "statementOfReason";

    public static string UrlAdditionalInformation => "urlAdditionalInformation";
}