namespace Dft.DTRO.Tests.ServicesTests.Validations;

[ExcludeFromCodeCoverage]
public class ExternalReferenceValidationServiceTests
{
    private readonly IExternalReferenceValidationService _sut = new ExternalReferenceValidationService();

    [Theory]
    [InlineData("2024-08-01T00:00:00", "3.3.0", 0)]
    [InlineData("2034-01-01T00:00:00", "3.3.0", 1)]
    public void ValidatedExternalReferenceLastUpdatedDate(string lastUpdateDate, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""PointGeometry"":  {{
                                    ""ExternalReference"": [
                                        {{
                                            ""lastUpdateDate"": ""{lastUpdateDate}""
                                        }}
                                    ]
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}
        ", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Fact]
    public void ValidateMultipleExternalReferencesLastUpdatedDate()
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""PointGeometry"":  {{
                                    ""ExternalReference"": [
                                        {{
                                            ""lastUpdateDate"": ""2023-01-01T00:00:00""
                                        }},
                                        {{
                                            ""lastUpdateDate"": ""2023-02-01T00:00:00""
                                        }}
                                    ]
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}
        ", new SchemaVersion("3.3.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(0, actual.Count);
    }

    [Theory]
    [InlineData("PointGeometry")]
    [InlineData("LinearGeometry")]
    [InlineData("Polygon")]
    [InlineData("DirectedLinear")]
    public void ValidateMultipleExternalReferencesWithinMultipleGeometriesLastUpdatedDate(string geometry)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""{geometry}"":  {{
                                    ""ExternalReference"": [
                                        {{
                                            ""lastUpdateDate"": ""2022-01-01T00:10:00""
                                        }},
                                        {{
                                            ""lastUpdateDate"": ""2024-02-01T00:20:00""
                                        }}
                                    ]
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}
        ", new SchemaVersion("3.3.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(0, actual.Count);
    }


    [Fact]
    public void ValidateMultipleExternalReferencesWithDifferentGeometriesLastUpdatedDate()
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""PointGeometry"":  {{
                                    ""ExternalReference"": [
                                        {{
                                            ""lastUpdateDate"": ""2023-01-01T00:00:00""
                                        }},
                                        {{
                                            ""lastUpdateDate"": ""2023-02-01T00:00:00""
                                        }}
                                    ]
                                }},
                                ""LinearGeometry"":  {{
                                    ""ExternalReference"": [
                                        {{
                                            ""lastUpdateDate"": ""2023-01-01T00:00:00""
                                        }},
                                        {{
                                            ""lastUpdateDate"": ""2023-02-01T00:00:00""
                                        }}
                                    ]
                                }},
                                ""Polygon"":  {{
                                    ""ExternalReference"": [
                                        {{
                                            ""lastUpdateDate"": ""2023-01-01T00:00:00""
                                        }},
                                        {{
                                            ""lastUpdateDate"": ""2023-02-01T00:00:00""
                                        }}
                                    ]
                                }},
                                ""DirectedLinear"":  {{
                                    ""ExternalReference"": [
                                        {{
                                            ""lastUpdateDate"": ""2023-01-01T00:00:00""
                                        }},
                                        {{
                                            ""lastUpdateDate"": ""2023-02-01T00:00:00""
                                        }}
                                    ]
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}
        ", new SchemaVersion("3.3.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(0, actual.Count);
    }

    [Fact]
    public void ValidateNoExternalReferencesWithDifferentGeometries()
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""PointGeometry"":  {{
                                    }},
                                ""LinearGeometry"":  {{
                                    }},
                                ""Polygon"":  {{
                                    }},
                                ""DirectedLinear"":  {{
                                    }}
                            }}
                        ]
                    }}
                ]
            }}
        }}
        ", new SchemaVersion("3.3.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(0, actual.Count);
    }
}