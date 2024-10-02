using System.Reflection;

namespace Dft.DTRO.Admin.Helpers;

public static class EnumsExtensions
{
    public static string GetDisplayName(this Enum enumToDisplay) =>
        enumToDisplay.GetAttribute<DisplayAttribute>().Name;

    private static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute =>
        enumValue
            .GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<TAttribute>();
}