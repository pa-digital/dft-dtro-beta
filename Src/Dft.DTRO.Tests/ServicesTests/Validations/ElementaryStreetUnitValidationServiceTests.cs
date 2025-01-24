namespace Dft.DTRO.Tests.ServicesTests.Validations;

[ExcludeFromCodeCoverage]
public class ElementaryStreetUnitValidationServiceTests
{
    private readonly IElementaryStreetUnitValidationService _sut = new ElementaryStreetUnitValidationService();

    [Theory]
    [InlineData(10000000, 1)]
    [InlineData(10000001, 0)]
    [InlineData(0, 1)]
    [InlineData(null, 1)]
    [InlineData(99999999999, 0)]
    [InlineData(100000000000000, 1)]
    public void ValidateElementaryStreetUnitEsu(long esu, int errorCount)
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
                                                    ""ElementaryStreetUnit"": [
                                                        {{
                                                            ""esu"": {esu}
                                                        }}
                                                    ]
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
    public void ValidateElementaryStreetUnitMultipleEsu()
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""LinearGeometry"":  {{
                                    ""ExternalReference"": [
                                        {{
                                            ""UniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""ElementaryStreetUnit"": [
                                                        {{
                                                            ""esu"": 39605715
                                                        }}
                                                    ]
                                                }}
                                            ]
                                        }},
                                        {{
                                            ""UniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""ElementaryStreetUnit"": [
                                                        {{
                                                            ""esu"": 39605158
                                                        }}
                                                    ]
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
        Assert.Equal(0, actual.Count);
    }

    [Fact]
    public void ValidateElementStreetUnitMultipleIncorrectEsu()
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""DirectedLinear"":  {{
                                    ""ExternalReference"": [
                                        {{
                                            ""UniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""ElementaryStreetUnit"": [
                                                        {{
                                                            ""esu"": 39605158
                                                        }}
                                                    ]
                                                }}
                                            ]
                                        }},
                                        {{
                                            ""UniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""ElementaryStreetUnit"": [
                                                        {{
                                                            ""esu"": ""39605158""
                                                        }}
                                                    ]
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
        Assert.Single(actual);
    }

    [Fact]
    public void ValidateOneElementStreetUnitPresent()
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""DirectedLinear"":  {{
                                    ""ExternalReference"": [
                                        {{
                                            ""UniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""ElementaryStreetUnit"": [
                                                        {{
                                                            ""esu"": 39605158
                                                        }}
                                                    ]
                                                }}
                                            ]
                                        }},
                                        {{
                                            ""UniqueStreetReferenceNumber"": [ 
                                                {{
                                                    
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
    public void ValidateSeveralElementStreetUnitsPresent()
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""DirectedLinear"":  {{
                                    ""ExternalReference"": [
                                        {{
                                            ""UniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""ElementaryStreetUnit"": [
                                                        {{
                                                            ""esu"": 39605158
                                                        }}
                                                    ]
                                                }}
                                            ]
                                        }},
                                        {{
                                            ""UniqueStreetReferenceNumber"": [ 
                                                {{
                                                    
                                                }},
                                                {{
                                                    ""ElementaryStreetUnit"": [
                                                        {{
                                                            ""esu"": ""39605158""
                                                        }}
                                                    ]
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
        Assert.Single(actual);
    }

    [Fact]
    public void ValidateNoElementStreetUnits()
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""DirectedLinear"":  {{
                                    ""ExternalReference"": [
                                        {{
                                            ""UniqueStreetReferenceNumber"": [ 
                                                {{
                                                    
                                                }}
                                            ]
                                        }},
                                        {{
                                            ""UniqueStreetReferenceNumber"": [ 
                                                {{
                                                    
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
}