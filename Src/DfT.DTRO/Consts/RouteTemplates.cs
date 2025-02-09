namespace DfT.DTRO.Consts;

public static class RouteTemplates
{
    
    // Dtros
    public const string DtrosBase = "/dtros";
    public const string DtrosCreateFromFile = "/createFromFile";
    public const string DtrosUpdateFromFile = "/updateFromFile/{dtroId:guid}";
    public const string DtrosCreateFromBody = "/createFromBody";
    public const string DtrosUpdateFromBody = "/updateFromBody/{dtroId:guid}";
    public const string DtrosFindAll = "";
    public const string DtrosFindById = "/{id:guid}";
    public const string DtrosDeleteById = "/{dtroId:guid}";
    public const string DtrosFindSourceHistory = "/sourceHistory/{dtroId:guid}";
    public const string DtrosFindProvisionHistory = "/provisionHistory/{dtroId:guid}";
    public const string DtrosAssignOwnership = "/ownership/{dtroId:guid}/{assignToTraId}";

    // Dtro Users
    public const string DtroUsersBase = "/dtroUsers";
    public const string DtroUsersFindAll = "";
    public const string DtroUsersSearch = "/search/{partialName}";
    public const string DtroUsersCreateFromBody = "/createFromBody";
    public const string DtroUsersUpdateFromBody = "/updateFromBody";
    public const string DtroUsersFindById = "/{dtroUserId:guid}";
    public const string DtroUsersDeleteRedundant = "/redundant";

    // Events
    public const string EventsBase = "/events";
    public const string EventsFindAll = "";

    // Metrics
    public const string HealthApi = "/healthApi";
    public const string HealthDatabase = "/healthDatabase";
    public const string MetricsForDtroUser = "/metricsForDtroUser";
    public const string FullMetricsForDtroUser = "/fullMetricsForDtroUser";

    // Rules
    public const string RulesBase = "/rules";
    public const string RulesFindVersions = "/versions";
    public const string RulesFindAll = "";
    public const string RulesFindByVersion = "/{version}";
    public const string RulesFindById = "/{ruleId:guid}";
    public const string RulesCreateFromFile = "/createFromFile/{version}";
    public const string RulesUpdateFromFile = "/updateFromFile/{version}";

    // Schemas
    public const string SchemasBase = "/schemas";
    public const string SchemasFindVersions = "/versions";
    public const string SchemasFindAll = "";
    public const string SchemasFindByVersion = "/{version}";
    public const string SchemasFindById = "/{schemaId:guid}";
    public const string SchemasCreateFromFile = "/createFromFile/{version}";
    public const string SchemasCreateFromBody = "/createFromBody/{version}";
    public const string SchemasUpdateFromFile = "/updateFromFile/{version}";
    public const string SchemasUpdateFromBody = "/updateFromBody/{version}";
    public const string SchemasActivate = "/activate/{version}";
    public const string SchemasDeactivate = "/deactivate/{version}";
    public const string SchemasDeleteByVersion = "/{version}";

    // Search
    public const string Search = "/search";

    // System Config
    public const string SystemConfigBase = "/systemConfig";
    public const string SystemConfigFind = "";
    public const string SystemConfigUpdateFromBody = "/updateFromBody";
    
    // Tras
    public const string TrasBase = "/tras";
    public const string TrasFindAll = "";
}
