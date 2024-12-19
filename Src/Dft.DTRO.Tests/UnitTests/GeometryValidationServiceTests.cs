namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class GeometryValidationServiceTests
{
    private readonly IGeometryValidationService _sut;

    public GeometryValidationServiceTests()
    {
        _sut = new GeometryValidationService();
    }

    [Theory]
    [InlineData(1, "3.3.0", 0)]
    [InlineData(null, "3.3.0", 1)]
    [InlineData(0, "3.3.0", 1)]
    public void ValidatePointGeometryVersion(long version, string schemaVersion, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""PointGeometry"": {{
                                    ""version"": {version},
                                    ""point"": ""SRID=27700;POINT(323544 124622)"",
                                    ""representation"": ""centreLinePoint""
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}
        ", new SchemaVersion(schemaVersion));

        var actual = _sut.ValidateGeometry(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("SRID=27700;POINT(323544 124622)", "3.3.0", 0)]
    [InlineData("SRID=27700;POINT(535595 184790)", "3.3.0", 0)]
    [InlineData("ETR=S89;POINT(323544 124622)", "3.3.0", 1)]
    [InlineData("ETR=S89;POINT(0 0)", "3.3.0", 2)]
    public void ValidatePointGeometryPoint(string point, string schemaVersion, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""PointGeometry"": {{
                                    ""version"": 1,
                                    ""point"": ""{point}"",
                                    ""representation"": ""centreLinePoint""
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}
        ", new SchemaVersion(schemaVersion));

        var actual = _sut.ValidateGeometry(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("centreLinePoint", "3.3.0", 0)]
    [InlineData("trafficSignLocation", "3.3.0", 0)]
    [InlineData("other", "3.3.0", 0)]
    [InlineData("wrong", "3.3.0", 1)]
    [InlineData("", "3.3.0", 2)]
    public void ValidatePointGeometryRepresentation(string representation, string schemaVersion, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""PointGeometry"": {{
                                    ""version"": 1,
                                    ""point"": ""SRID=27700;POINT(323544 124622)"",
                                    ""representation"": ""{representation}""
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}
        ", new SchemaVersion(schemaVersion));

        var actual = _sut.ValidateGeometry(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }
}