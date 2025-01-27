namespace Dft.DTRO.Tests.ServicesTests.Validations;

[ExcludeFromCodeCoverage]
public class GeometryValidationServiceTests
{
    private readonly IGeometryValidationService _sut = new GeometryValidationService();

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

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("SRID=27700;POINT(323544 124622)", "3.3.0", 0)]
    [InlineData("SRID=27700;POINT(535595 184790)", "3.3.0", 0)]
    [InlineData("ETR=S89;POINT(323544 124622)", "3.3.0", 1)]
    [InlineData("ETR=S89;POINT(100 1000)", "3.3.0", 1)]
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

        var actual = _sut.Validate(dtroSubmit);
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

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData(1, "3.3.0", 0)]
    [InlineData(null, "3.3.0", 1)]
    [InlineData(0, "3.3.0", 1)]
    public void ValidateLinearGeometryVersion(long version, string schemaVersion, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""LinearGeometry"": {{
                                    ""version"": {version},
                                    ""direction"": ""bidirectional"",
                                    ""lateralPosition"": ""onKerb"",
                                    ""linestring"": ""SRID=27700;LINESTRING(502151 221149,502130 221168,502091 221201,502065 221223,502054 221232,502029 221254,502026 221256,502000 221280,501981 221299,501956 221325,501930 221354,501865 221426)"",
                                    ""representation"": ""representingZone""
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}
        ", new SchemaVersion(schemaVersion));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Fact]
    public void ValidateLinearGeometryMultipleVersionsReturnsNoErrors()
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""LinearGeometry"": {{
                                    ""version"": 1,
                                    ""direction"": ""bidirectional"",
                                    ""lateralPosition"": ""onKerb"",
                                    ""linestring"": ""SRID=27700;LINESTRING(502151 221149,502130 221168,502091 221201,502065 221223,502054 221232,502029 221254,502026 221256,502000 221280,501981 221299,501956 221325,501930 221354,501865 221426)"",
                                    ""representation"": ""representingZone""
                                }}
                            }},
                            {{
                                ""LinearGeometry"": {{
                                    ""version"": 1,
                                    ""direction"": ""bidirectional"",
                                    ""lateralPosition"": ""onKerb"",
                                    ""linestring"": ""SRID=27700;LINESTRING(502151 221149,502130 221168,502091 221201,502065 221223,502054 221232,502029 221254,502026 221256,502000 221280,501981 221299,501956 221325,501930 221354,501865 221426)"",
                                    ""representation"": ""representingZone""
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}
        ", new SchemaVersion("3.3.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Empty(actual);
    }

    [Theory]
    [InlineData("bidirectional", "3.3.0", 0)]
    [InlineData("startToEnd", "3.3.0", 0)]
    [InlineData("endToStart", "3.3.0", 0)]
    [InlineData("wrong", "3.3.0", 1)]
    [InlineData("", "3.3.0", 2)]
    public void ValidateLinearGeometryDirection(string direction, string schemaVersion, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""LinearGeometry"": {{
                                    ""version"": 1,
                                    ""direction"": ""{direction}"",
                                    ""lateralPosition"": ""onKerb"",
                                    ""linestring"": ""SRID=27700;LINESTRING(502151 221149,502130 221168,502091 221201,502065 221223,502054 221232,502029 221254,502026 221256,502000 221280,501981 221299,501956 221325,501930 221354,501865 221426)"",
                                    ""representation"": ""representingZone""
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}
        ", new SchemaVersion(schemaVersion));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("centreline", "3.3.0", 0)]
    [InlineData("near", "3.3.0", 0)]
    [InlineData("onKerb", "3.3.0", 0)]
    [InlineData("far", "3.3.0", 0)]
    [InlineData("wrong", "3.3.0", 1)]
    [InlineData("", "3.3.0", 2)]
    public void ValidateLinearGeometryLateralPosition(string lateralPosition, string schemaVersion, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""LinearGeometry"": {{
                                    ""version"": 1,
                                    ""direction"": ""startToEnd"",
                                    ""lateralPosition"": ""{lateralPosition}"",
                                    ""linestring"": ""SRID=27700;LINESTRING(502151 221149,502130 221168,502091 221201,502065 221223,502054 221232,502029 221254,502026 221256,502000 221280,501981 221299,501956 221325,501930 221354,501865 221426)"",
                                    ""representation"": ""representingZone""
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}
        ", new SchemaVersion(schemaVersion));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("SRID=27700;LINESTRING(502151 221149,502130 221168,502091 221201,502065 221223,502054 221232,502029 221254,502026 221256,502000 221280,501981 221299,501956 221325,501930 221354,501865 221426)", "3.3.0", 0)]
    [InlineData("SRID=27700;LINESTRING(323357 124578, 323338 124640)", "3.3.0", 0)]
    [InlineData("SRID=27700;LINESTRING(502151 0, 502130 0)", "3.3.0", 1)]
    [InlineData("ETR=S89;LINESTRING(502151 221149,502130 221168)", "3.3.0", 1)]
    public void ValidateLinearGeometryLinestring(string linestring, string schemaVersion, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""LinearGeometry"": {{
                                    ""version"": 1,
                                    ""direction"": ""startToEnd"",
                                    ""lateralPosition"": ""far"",
                                    ""linestring"": ""{linestring}"",
                                    ""representation"": ""representingZone""
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}
        ", new SchemaVersion(schemaVersion));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("representingZone", "3.3.0", 0)]
    [InlineData("linear", "3.3.0", 0)]
    [InlineData("wrong", "3.3.0", 1)]
    [InlineData("", "3.3.0", 2)]
    public void ValidateLinearGeometryRepresentation(string representation, string schemaVersion, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""LinearGeometry"": {{
                                    ""version"": 1,
                                    ""direction"": ""startToEnd"",
                                    ""lateralPosition"": ""near"",
                                    ""linestring"": ""SRID=27700;LINESTRING(502151 221149,502130 221168,502091 221201,502065 221223,502054 221232,502029 221254,502026 221256,502000 221280,501981 221299,501956 221325,501930 221354,501865 221426)"",
                                    ""representation"": ""{representation}""
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}
        ", new SchemaVersion(schemaVersion));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("SRID=27700;POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))", "3.3.0", 0)]
    [InlineData("ETR=S89;POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))", "3.3.0", 1)]
    [InlineData("SRID=27700;POLYGON((-3.692895967970722 40.589246298614356,-3.489648897658222 40.687204044606595,-3.357812960158222 40.50367643938022,-3.585779268751972 40.413815011684214,-3.692895967970722 40.589246298614356))", "3.3.0", 1)]
    public void ValidatePolygonPolygon(string polygon, string schemaVersion, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""Polygon"": {{
                                    ""version"": 1,
                                    ""polygon"": ""{polygon}""
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}
        ", new SchemaVersion(schemaVersion));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("SRID=27700;LINESTRING(529050 178750, 529157 178805, 529250 178860)", 0)]
    [InlineData("SRID=27700;LINESTRING(-117.12081232960351 33.915065144, 33.915065 33.915074, -117.120813 -117.120812)", 1)]
    [InlineData("ETR=S89;LINESTRING(529050 178750, 529157 178805, 529250 178860)", 1)]
    [InlineData("SRID=27700;LINESTRING(529050 178750, -117.120813 33.915065144)", 1)]
    public void ValidateDirectedLinear(string directedLineString, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""DirectedLinear"": {{
                                    ""version"": 1,
                                    ""directedLineString"": ""{directedLineString}""
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }
}