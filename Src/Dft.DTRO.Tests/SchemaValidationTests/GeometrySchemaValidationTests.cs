using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Xunit;
using static Dft.DTRO.Tests.Utils;

namespace Dft.DTRO.Tests.SchemaValidationTests;

public class GeometrySchemaValidationTests : IDisposable
{
    private static readonly JSchema _schema;

    static GeometrySchemaValidationTests()
    {
        string schemaPath = GetSchema340();
        _schema = JSchema.Parse(File.ReadAllText(schemaPath));
    }

    [Fact]
    public void LinearGeometryValid()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "valid", "LinearGeometry.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    [Fact]
    public void PointGeometryValid()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "valid", "PointGeometry.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    [Fact]
    public void PolygonGeometryValid()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "valid", "Polygon.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    [Fact]
    public void DirectedLinearGeometryDestinationValid()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "valid", "DirectedLinearDestination.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    [Fact]
    public void LinearGeometryValidateRequiredProperties()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "valid", "LinearGeometry.json");
        string[] requiredProperties = { "version", "direction", "lateralPosition", "linestring", "representation" };
        foreach (string property in requiredProperties)
        {
            JObject json = JObject.Parse(File.ReadAllText(path));
            JObject linearGeometry = (JObject)json["source"]["provision"][0]["regulatedPlace"][0]["linearGeometry"];
            linearGeometry.Remove(property);
            bool isValid = json.IsValid(_schema, out IList<string> errors);
            Assert.False(isValid);
        }
    }

    [Fact]
    public void PointGeometryValidateRequiredProperties()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "valid", "PointGeometry.json");
        string[] requiredProperties = { "version", "point", "representation" };
        foreach (string property in requiredProperties)
        {
            JObject json = JObject.Parse(File.ReadAllText(path));
            JObject pointGeometry = (JObject)json["source"]["provision"][0]["regulatedPlace"][0]["pointGeometry"];
            pointGeometry.Remove(property);
            bool isValid = json.IsValid(_schema, out IList<string> errors);
            Assert.False(isValid);
        }
    }

    [Fact]
    public void PolygonGeometryValidateRequiredProperties()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "valid", "Polygon.json");
        string[] requiredProperties = { "version", "polygon" };
        foreach (string property in requiredProperties)
        {
            JObject json = JObject.Parse(File.ReadAllText(path));
            JObject polygonGeometry = (JObject)json["source"]["provision"][0]["regulatedPlace"][0]["polygon"];
            polygonGeometry.Remove(property);
            bool isValid = json.IsValid(_schema, out IList<string> errors);
            Assert.False(isValid);
        }
    }

    [Fact]
    public void DirectedLinearGeometryValidateRequiredProperties()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "valid", "DirectedLinearOrigin.json");
        string[] requiredProperties = { "version", "directedLineString" };
        foreach (string property in requiredProperties)
        {
            JObject json = JObject.Parse(File.ReadAllText(path));
            JObject directedLinearGeometry = (JObject)json["source"]["provision"][0]["regulatedPlace"][0]["directedLinear"];
            directedLinearGeometry.Remove(property);
            bool isValid = json.IsValid(_schema, out IList<string> errors);
            Assert.False(isValid);
        }
    }

    [Fact]
    public void DirectedLinearGeometryIntermediateLocationValid()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "valid", "DirectedLinearIntermediateLocation.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    [Fact]
    public void DirectedLinearGeometryOriginValid()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "valid", "DirectedLinearOrigin.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    [Fact]
    public void DirectedLinearGeometryWithDiversionTypeValid()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "valid", "DirectedLinearWithDiversionType.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    [Fact]
    public void LinearGeometryExternalReferenceShouldFailWhenObject()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "invalid", "LinearGeometryExternalReferenceObject.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    [Fact]
    public void PointGeometryExternalReferenceShouldFailWhenArray()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "invalid", "PointGeometryExternalReferenceArray.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    [Fact]
    public void PolygonGeometryExternalReferenceShouldFailWhenObject()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "invalid", "PolygonExternalReferenceObject.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    [Fact]
    public void DirectedLinearGeometryDestinationShouldFailWhenArray()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "invalid", "DirectedLinearDestinationArray.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    [Fact]
    public void DirectedLinearGeometryIntermediateLocationShouldFailWhenObject()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "invalid", "DirectedLinearIntermediateLocationObject.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    [Fact]
    public void DirectedLinearGeometryOriginShouldFailWhenArray()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "invalid", "DirectedLinearOriginArray.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    [Fact]
    public void DirectedLinearGeometryShouldFailWhenExternalReferenceGiven()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "invalid", "DirectedLinearExternalReference.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    [Fact]
    public void UniqueStreetReferenceNumberShouldFailWhenObject()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "invalid", "UniqueStreetReferenceNumberObject.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    [Fact]
    public void DirectedLinearGeometryShouldFailWhenDirectedLineStringDoesNotMatchPattern()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Geometry", "invalid", "DirectedLinearDestinationDirectedLineStringInvalidPattern.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    public void Dispose() { }

}