#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace DfT.DTRO.Helpers;

public static class Constants
{
    public static IEnumerable<string> ConcreteGeometries => typeof(GeometryType).GetDisplayNames<GeometryType>();

    public static string Version => "version";

    public static string Srid27000 => "SRID=27700";

    public static string Point => "point";

    public static string LineString => "linestring";

    public static string Polygon => "polygon";

    public static string DirectedLineString => "directedLineString";
}