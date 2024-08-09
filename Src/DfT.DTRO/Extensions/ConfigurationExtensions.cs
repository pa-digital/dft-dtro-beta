namespace DfT.DTRO.Extensions;

public static class ConfigurationExtensions
{
    public static T GetProperty<T>(this IConfiguration configuration, string section, string property) =>
        configuration.GetSection(section).GetValue<T>(property);
}