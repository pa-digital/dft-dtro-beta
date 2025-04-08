namespace DfT.DTRO.Consts;

public static class RouteTemplates
{
    // Applications
    public const string ApplicationsBase = "/applications";
    public const string ApplicationsCreate = ApplicationsBase;
    public const string ValidateApplicationName = ApplicationsBase + "/validateName";
    public const string ApplicationsFindById = ApplicationsBase + "/{appId:guid}";
    public const string ApplicationsFindAll = ApplicationsBase;
    public const string ApplicationsFindAllInactive = ApplicationsBase + "/inactive";
    public const string ActivateApplication = ApplicationsBase + "/{appId:guid}" + "/activate";

    // Auth
    public const string AuthBase = "/oauth";
    public const string AuthGetToken = AuthBase + "/token";

    // Dtros
    public const string DtrosBase = "/dtros";
    public const string DtrosCreateFromFile = DtrosBase + "/createFromFile";
    public const string DtrosUpdateFromFile = DtrosBase + "/updateFromFile/{dtroId:guid}";
    public const string DtrosCreateFromBody = DtrosBase + "/createFromBody";
    public const string DtrosUpdateFromBody = DtrosBase + "/updateFromBody/{dtroId:guid}";
    public const string DtrosFindAll = DtrosBase;
    public const string DtrosFindById = DtrosBase + "/{id:guid}";
    public const string DtrosDeleteById = DtrosBase + "/{dtroId:guid}";
    public const string DtrosFindSourceHistory = DtrosBase + "/sourceHistory/{dtroId:guid}";
    public const string DtrosFindProvisionHistory = DtrosBase + "/provisionHistory/{dtroId:guid}";
    public const string DtrosAssignOwnership = DtrosBase + "/ownership/{dtroId:guid}/{assignToTraId}";

    public const string DtrosCount = DtrosBase + "/count";

    // Dtro Users
    public const string DtroUsersBase = "/dtroUsers";
    public const string DtroUsersFindAll = DtroUsersBase;
    public const string DtroUsersSearch = DtroUsersBase + "/search/{partialName}";
    public const string DtroUsersCreateFromBody = DtroUsersBase + "/createFromBody";
    public const string DtroUsersUpdateFromBody = DtroUsersBase + "/updateFromBody";
    public const string DtroUsersFindById = DtroUsersBase + "/{dtroUserId:guid}";
    public const string DtroUsersDeleteRedundant = DtroUsersBase + "/redundant";

    // Environment
    public const string CanRequestProductionAccess = "/canRequestProductionAccess";
    public const string RequestProductionAccess = "/requestAccess";

    // Events
    public const string EventsBase = "/events";
    public const string EventsFindAll = EventsBase;

    // Metrics
    public const string HealthApi = "/healthApi";
    public const string HealthDatabase = "/healthDatabase";
    public const string MetricsForDtroUser = "/metricsForDtroUser";
    public const string FullMetricsForDtroUser = "/fullMetricsForDtroUser";

    // Rules
    public const string RulesBase = "/rules";
    public const string RulesFindVersions = RulesBase + "/versions";
    public const string RulesFindAll = RulesBase;
    public const string RulesFindByVersion = RulesBase + "/{version}";
    public const string RulesFindById = RulesBase + "/{ruleId:guid}";
    public const string RulesCreateFromFile = RulesBase + "/createFromFile/{version}";
    public const string RulesUpdateFromFile = RulesBase + "/updateFromFile/{version}";

    // Schemas
    public const string SchemasBase = "/schemas";
    public const string SchemasFindVersions = SchemasBase + "/versions";
    public const string SchemasFindAll = SchemasBase;
    public const string SchemasFindByVersion = SchemasBase + "/{version}";
    public const string SchemasFindById = SchemasBase + "/{schemaId:guid}";
    public const string SchemasCreateFromFile = SchemasBase + "/createFromFile/{version}";
    public const string SchemasCreateFromBody = SchemasBase + "/createFromBody/{version}";
    public const string SchemasUpdateFromFile = SchemasBase + "/updateFromFile/{version}";
    public const string SchemasUpdateFromBody = SchemasBase + "/updateFromBody/{version}";
    public const string SchemasActivate = SchemasBase + "/activate/{version}";
    public const string SchemasDeactivate = SchemasBase + "/deactivate/{version}";
    public const string SchemasDeleteByVersion = SchemasBase + "/{version}";

    // Search
    public const string Search = "/search";

    // System Config
    public const string SystemConfigBase = "/systemConfig";
    public const string SystemConfigFind = SystemConfigBase;
    public const string SystemConfigUpdateFromBody = SystemConfigBase + "/updateFromBody";

    // Tras
    public const string TrasBase = "/tras";
    public const string TrasFindAll = TrasBase;

    // Users
    public const string UsersBase = "/users";
    public const string UsersFindAll = UsersBase;
    public const string UsersDelete = UsersBase + "/{userId:guid}";

}