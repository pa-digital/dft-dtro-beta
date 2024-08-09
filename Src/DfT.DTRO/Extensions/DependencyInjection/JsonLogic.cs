namespace DfT.DTRO.Extensions.DependencyInjection;

public static class JsonLogic
{
    private static MethodInfo _addRuleMethod = typeof(RuleRegistry).GetMethods()
            .Single(it => it.Name == "AddRule");

    public static IServiceCollection AddJsonLogic(this IServiceCollection services, Assembly assembly = null)
    {
        services.AddScoped<IJsonLogicRuleSource, FileJsonLogicRuleSource>();
        services.AddScoped<IJsonLogicValidationService, JsonLogicValidationService>();
        services.AddScoped<IRecordManagementService, RecordManagementService>();

        AddAllRules(assembly ?? Assembly.GetExecutingAssembly());

        return services;
    }

    public static void AddAllRules(Assembly assembly)
    {
        var customRules = assembly.GetTypes()
            .Where(t => typeof(Rule).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var customRule in customRules)
        {
            AddRule(customRule);
        }
    }

    private static void AddRule(Type type)
    {
        _addRuleMethod.MakeGenericMethod(type).Invoke(null, null);
    }
}
