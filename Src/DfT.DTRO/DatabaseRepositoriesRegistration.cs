namespace DfT.DTRO;

/// <summary>
/// Database repositories registration class
/// </summary>
[ExcludeFromCodeCoverage]
public static class DatabaseRepositoriesRegistration
{
    /// <summary>
    /// Add repositories to service collection
    /// </summary>
    /// <param name="services">Service collection passed</param>
    /// <returns>The updated service collection</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<IDtroDal, DtroDal>()
                .AddScoped<IDtroHistoryDal, DtroHistoryDal>()
                .AddScoped<ISchemaTemplateDal, SchemaTemplateDal>()
                .AddScoped<IRuleTemplateDal, RuleTemplateDal>()
                .AddScoped<IMetricDal, MetricDal>()
                .AddScoped<IDtroUserDal, DtroUserDal>()
                .AddScoped<ISystemConfigDal, SystemConfigDal>();

        return services;
    }
}