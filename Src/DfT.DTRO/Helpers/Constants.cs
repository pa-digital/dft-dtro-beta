#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace DfT.DTRO.Helpers;

public static class Constants
{
    public static IEnumerable<string> ConcreteGeometries =>
        new List<string>
        {
            "PointGeometry",
            "LinearGeometry",
            "Polygon",
            "DirectedLinear"
        };
}