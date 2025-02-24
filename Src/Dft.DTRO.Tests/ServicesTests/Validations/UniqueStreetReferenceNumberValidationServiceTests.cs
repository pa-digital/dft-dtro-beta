namespace Dft.DTRO.Tests.ServicesTests.Validations;

[ExcludeFromCodeCoverage]
public class UniqueStreetReferenceNumberValidationServiceTests
{
    private readonly IUniqueStreetReferenceNumberValidationService _sut =
        new UniqueStreetReferenceNumberValidationService();

    [Theory]
    [InlineData(39605715, 0)]
    [InlineData(0, 1)]
    [InlineData(null, 1)]
    [InlineData(39605158, 0)]
    [InlineData(100000000, 1)]
    public void ValidateUniqueStreetReferenceNumberUsrn(long usrn, int errorCount)
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
                                            ""UniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""usrn"": {usrn}
                                                }}
                                            ]
                                        }}
                                    ]
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

    [Fact]
    public void ValidateUniqueStreetReferenceNumberMultipleDifferentUsrn()
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
                                            ""UniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""usrn"": 39605715
                                                }}
                                            ]
                                        }},
                                        {{
                                            ""UniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""usrn"": 39605158
                                                }}
                                            ]
                                        }}
                                    ]
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Empty(actual);
    }

    [Fact]
    public void ValidateUniqueStreetReferenceNumberMultipleSimilarUsrn()
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
                                            ""UniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""usrn"": 39605715
                                                }}
                                            ]
                                        }},
                                        {{
                                            ""UniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""usrn"": 39605715
                                                }}
                                            ]
                                        }}
                                    ]
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.NotEmpty(actual);
    }

    [Fact]
    public void ValidateUniqueStreetReferenceNumberMultipleIncorrectUsrn()
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""Polygon"":  {{
                                    ""ExternalReference"": [
                                        {{
                                            ""UniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""usrn"": 39605715
                                                }}
                                            ]
                                        }},
                                        {{
                                            ""UniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""usrn"": ""39605158""
                                                }}
                                            ]
                                        }}
                                    ]
                                }}
                            }}
                        ]
                    }}
                ]
            }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.NotEmpty(actual);
    }
}