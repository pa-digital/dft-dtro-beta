using DfT.DTRO.JsonLogic;
using DfT.DTRO.Services.Validation;
using Json.Logic;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace DfT.DTRO.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for registering JsonLogic.
/// </summary>
public static class JsonLogicDIExtensions
{
    private static MethodInfo _addRuleMethod = typeof(RuleRegistry).GetMethods()
            .Single(it => it.Name == "AddRule");

    /// <summary>
    /// Adds services required to make use of JsonLogic
    /// as well as all JsonLogic rules created in this assembly.
    /// </summary>
    /// <param name="services">
    /// The service collection to use.
    /// </param>
    /// <param name="assembly">
    /// The assembly to retrieve the rules from.
    /// By default read from <see cref="Assembly.GetExecutingAssembly"/>.
    /// </param>
    public static IServiceCollection AddJsonLogic(this IServiceCollection services, Assembly assembly = null)
    {
        services.AddScoped<IJsonLogicRuleSource, FileJsonLogicRuleSource>();
        services.AddScoped<IJsonLogicValidationService, JsonLogicValidationService>();
        services.AddScoped<IRecordManagementService, RecordManagementService>();

        AddAllRules(assembly ?? Assembly.GetExecutingAssembly());

        return services;
    }

    /// <summary>
    /// Adds all rules found in the provided assembly to the <see cref="RuleRegistry"/>.
    /// </summary>
    /// <param name="assembly">The assembly to retrieve the rules from.</param>
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
