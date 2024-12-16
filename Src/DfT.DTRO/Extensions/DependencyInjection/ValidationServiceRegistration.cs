namespace DfT.DTRO.Extensions.DependencyInjection;

/// <summary>
/// Validation service registration
/// </summary>
public static class ValidationServiceRegistration
{
    private static readonly MethodInfo AddRuleMethod = typeof(RuleRegistry).GetMethods()
            .Single(it => it.Name == "AddRule");

    /// <summary>
    /// Register validation services
    /// </summary>
    /// <param name="services">Services to register to</param>
    /// <param name="assembly">Assembly passed in</param>
    public static void AddValidationServices(this IServiceCollection services, Assembly assembly = null)
    {
        services.AddScoped<IJsonLogicRuleSource, FileJsonLogicRuleSource>();
        services.AddScoped<IRulesValidation, RulesValidation>();
        services.AddScoped<ISourceValidationService, SourceValidationService>();
        services.AddScoped<IProvisionValidationService, ProvisionValidationService>();
        services.AddScoped<IRegulatedPlaceValidation, RegulatedPlaceValidation>();
        services.AddScoped<IRegulationValidation, RegulationValidation>();
        services.AddScoped<IConditionValidation, ConditionValidation>();

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
