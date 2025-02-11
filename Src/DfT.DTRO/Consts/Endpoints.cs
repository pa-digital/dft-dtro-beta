using DfT.DTRO.Controllers;

namespace DfT.DTRO.Consts;

public readonly struct Endpoints
{
    public readonly string Route;
    public readonly string Method;

    private Endpoints(string route, string method)
    {
        Route = route;
        Method = method;
    }

    // Dtros
    public static readonly Endpoints DtrosCreateFromFile = new(RouteTemplates.DtrosCreateFromFile, nameof(DTROsController.CreateFromFile));
    public static readonly Endpoints DtrosUpdateFromFile = new(RouteTemplates.DtrosUpdateFromFile, nameof(DTROsController.UpdateFromFile));
    public static readonly Endpoints DtrosCreateFromBody = new(RouteTemplates.DtrosCreateFromBody, nameof(DTROsController.CreateFromBody));
    public static readonly Endpoints DtrosUpdateFromBody = new(RouteTemplates.DtrosUpdateFromBody, nameof(DTROsController.UpdateFromBody));
    public static readonly Endpoints DtrosFindAll = new(RouteTemplates.DtrosFindAll, nameof(DTROsController.GetAll));
    public static readonly Endpoints DtrosFindById = new(RouteTemplates.DtrosFindById, nameof(DTROsController.GetById));
    public static readonly Endpoints DtrosDeleteById = new(RouteTemplates.DtrosDeleteById, nameof(DTROsController.Delete));
    public static readonly Endpoints DtrosFindSourceHistory = new(RouteTemplates.DtrosFindSourceHistory, nameof(DTROsController.GetSourceHistory));
    public static readonly Endpoints DtrosFindProvisionHistory = new(RouteTemplates.DtrosFindProvisionHistory, nameof(DTROsController.GetProvisionHistory));
    public static readonly Endpoints DtrosAssignOwnership = new(RouteTemplates.DtrosAssignOwnership, nameof(DTROsController.AssignOwnership));

    // Dtro Users
    public static readonly Endpoints DtroUsersFindAll = new(RouteTemplates.DtroUsersFindAll, nameof(DtroUserController.GetDtroUsers));
    public static readonly Endpoints DtroUsersSearch = new(RouteTemplates.DtroUsersSearch, nameof(DtroUserController.SearchDtroUsers));
    public static readonly Endpoints DtroUsersCreateFromBody = new(RouteTemplates.DtroUsersCreateFromBody, nameof(DtroUserController.CreateFromBody));
    public static readonly Endpoints DtroUsersUpdateFromBody = new(RouteTemplates.DtroUsersUpdateFromBody, nameof(DtroUserController.UpdateFromBody));
    public static readonly Endpoints DtroUsersFindById = new(RouteTemplates.DtroUsersFindById, nameof(DtroUserController.GetDtroUser));
    public static readonly Endpoints DtroUsersDeleteRedundant = new(RouteTemplates.DtroUsersDeleteRedundant, nameof(DtroUserController.DeleteDtroUsers));

    // Events
    public static readonly Endpoints EventsFindAll = new(RouteTemplates.EventsFindAll, nameof(EventsController.Events));

    // Metrics
    public static readonly Endpoints HealthApi = new(RouteTemplates.HealthApi, nameof(MetricsController.HealthApi));
    public static readonly Endpoints HealthDatabase = new(RouteTemplates.HealthDatabase, nameof(MetricsController.HealthDatabase));
    public static readonly Endpoints MetricsForDtroUser = new(RouteTemplates.MetricsForDtroUser, nameof(MetricsController.GetMetricsForDtroUser));
    public static readonly Endpoints FullMetricsForDtroUser = new(RouteTemplates.FullMetricsForDtroUser, nameof(MetricsController.GetFullMetricsForDtroUser));

    // Rules
    public static readonly Endpoints RulesFindVersions = new(RouteTemplates.RulesFindVersions, nameof(RulesController.GetVersions));
    public static readonly Endpoints RulesFindAll = new(RouteTemplates.RulesFindAll, nameof(RulesController.Get));
    public static readonly Endpoints RulesFindByVersion = new(RouteTemplates.RulesFindByVersion, nameof(RulesController.GetByVersion));
    public static readonly Endpoints RulesFindById = new(RouteTemplates.RulesFindById, nameof(RulesController.GetById));
    public static readonly Endpoints RulesCreateFromFile = new(RouteTemplates.RulesCreateFromFile, nameof(RulesController.CreateFromFile));
    public static readonly Endpoints RulesUpdateFromFile = new(RouteTemplates.RulesUpdateFromFile, nameof(RulesController.UpdateFromFile));

    // Schemas
    public static readonly Endpoints SchemasFindVersions = new(RouteTemplates.SchemasFindVersions, nameof(SchemasController.GetVersions));
    public static readonly Endpoints SchemasFindAll = new(RouteTemplates.SchemasFindAll, nameof(SchemasController.Get));
    public static readonly Endpoints SchemasFindByVersion = new(RouteTemplates.SchemasFindByVersion, nameof(SchemasController.GetByVersion));
    public static readonly Endpoints SchemasFindById = new(RouteTemplates.SchemasFindById, nameof(SchemasController.GetById));
    public static readonly Endpoints SchemasCreateFromFile = new(RouteTemplates.SchemasCreateFromFile, nameof(SchemasController.CreateFromFileByVersion));
    public static readonly Endpoints SchemasCreateFromBody = new(RouteTemplates.SchemasCreateFromBody, nameof(SchemasController.CreateFromBodyByVersion));
    public static readonly Endpoints SchemasUpdateFromFile = new(RouteTemplates.SchemasUpdateFromFile, nameof(SchemasController.UpdateFromFileByVersion));
    public static readonly Endpoints SchemasUpdateFromBody = new(RouteTemplates.SchemasUpdateFromBody, nameof(SchemasController.UpdateFromBodyByVersion));
    public static readonly Endpoints SchemasActivate = new(RouteTemplates.SchemasActivate, nameof(SchemasController.ActivateByVersion));
    public static readonly Endpoints SchemasDeactivate = new(RouteTemplates.SchemasDeactivate, nameof(SchemasController.DeactivateByVersion));
    public static readonly Endpoints SchemasDeleteByVersion = new(RouteTemplates.SchemasDeleteByVersion, nameof(SchemasController.DeleteByVersion));

    // Search
    public static readonly Endpoints Search = new(RouteTemplates.Search, nameof(SearchController.SearchDtros));

    // System Config
    public static readonly Endpoints SystemConfigFind = new(RouteTemplates.SystemConfigFind, nameof(SystemConfigController.GetSystemConfig));
    public static readonly Endpoints SystemConfigUpdateFromBody = new(RouteTemplates.SystemConfigUpdateFromBody, nameof(SystemConfigController.UpdateFromBody));

    // Tras
    public static readonly Endpoints TrasFindAll = new(RouteTemplates.TrasFindAll, nameof(TraController.FindAll));
}
