namespace DfT.DTRO.Extensions.DependencyInjection;

/// <summary>
/// Validation service registration
/// </summary>
public static class ValidationServiceRegistration
{
    private static readonly MethodInfo AddRuleMethod = typeof(RuleRegistry)
        .GetMethods().Single(methodInfo => methodInfo.Name == "AddRule");

    /// <summary>
    /// Register validation services
    /// </summary>
    /// <param name="services">Services to register to</param>
    /// <param name="assembly">Assembly passed in</param>
    public static void AddValidationServices(this IServiceCollection services, Assembly assembly = null)
    {
        services
            .AddScoped<IJsonLogicRuleSource, FileJsonLogicRuleSource>()
            .AddScoped<IRulesValidation, RulesValidation>()
            .AddScoped<ISourceValidationService, SourceValidationService>()
            .AddScoped<IProvisionValidationService, ProvisionValidationService>()
            .AddScoped<IRegulatedPlaceValidationService, RegulatedPlaceValidationService>()
            .AddScoped<IGeometryValidationService, GeometryValidationService>()
            .AddScoped<IExternalReferenceValidationService, ExternalReferenceValidationService>()
            .AddScoped<IUniqueStreetReferenceNumberValidationService, UniqueStreetReferenceNumberValidationService>()
            .AddScoped<IElementaryStreetUnitValidationService, ElementaryStreetUnitValidationService>()
            .AddScoped<IRegulationValidation, RegulationValidation>()
            .AddScoped<IConditionValidationService, ConditionValidationService>()
            .AddScoped<IRateTableValidationService, RateTableValidationService>()
            .AddScoped<IRateLineCollectionValidationService, RateLineCollectionValidationService>()
            .AddScoped<IRateLineValidationService, RateLineValidationService>();

        AddAllRules(assembly ?? Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Add all custom rules
    /// </summary>
    /// <param name="assembly">Assembly that rules is part of</param>
    public static void AddAllRules(Assembly assembly)
    {
        var customRules = assembly
            .GetTypes()
            .Where(type => typeof(Rule).IsAssignableFrom(type) &&
                           !type.IsAbstract)
            .ToList();

        customRules.ForEach(AddRule);
    }

    private static void AddRule(Type type) => AddRuleMethod.MakeGenericMethod(type).Invoke(null, null);
}
