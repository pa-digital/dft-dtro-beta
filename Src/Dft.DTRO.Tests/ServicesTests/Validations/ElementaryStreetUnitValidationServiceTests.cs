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
            ""source"": {{
                ""provision"": [
                    {{
                        ""regulatedPlace"": [
                            {{
                                ""pointGeometry"":  {{
                                    ""externalReference"": [
                                        {{
                                            ""uniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""elementaryStreetUnit"": [
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
            ""source"": {{
                ""provision"": [
                    {{
                        ""regulatedPlace"": [
                            {{
                                ""linearGeometry"":  {{
                                    ""externalReference"": [
                                        {{
                                            ""uniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""elementaryStreetUnit"": [
                                                        {{
                                                            ""esu"": 39605715
                                                        }}
                                                    ]
                                                }}
                                            ]
                                        }},
                                        {{
                                            ""uniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""elementaryStreetUnit"": [
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
            ""source"": {{
                ""provision"": [
                    {{
                        ""regulatedPlace"": [
                            {{
                                ""directedLinear"":  {{
                                    ""externalReference"": [
                                        {{
                                            ""uniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""elementaryStreetUnit"": [
                                                        {{
                                                            ""esu"": 39605158
                                                        }}
                                                    ]
                                                }}
                                            ]
                                        }},
                                        {{
                                            ""uniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""elementaryStreetUnit"": [
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
            ""source"": {{
                ""provision"": [
                    {{
                        ""regulatedPlace"": [
                            {{
                                ""directedLinear"":  {{
                                    ""externalReference"": [
                                        {{
                                            ""uniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""elementaryStreetUnit"": [
                                                        {{
                                                            ""esu"": 39605158
                                                        }}
                                                    ]
                                                }}
                                            ]
                                        }},
                                        {{
                                            ""uniqueStreetReferenceNumber"": [ 
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
            ""source"": {{
                ""provision"": [
                    {{
                        ""regulatedPlace"": [
                            {{
                                ""directedLinear"":  {{
                                    ""externalReference"": [
                                        {{
                                            ""uniqueStreetReferenceNumber"": [ 
                                                {{
                                                    ""elementaryStreetUnit"": [
                                                        {{
                                                            ""esu"": 39605158
                                                        }}
                                                    ]
                                                }}
                                            ]
                                        }},
                                        {{
                                            ""uniqueStreetReferenceNumber"": [ 
                                                {{
                                                    
                                                }},
                                                {{
                                                    ""elementaryStreetUnit"": [
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
            ""source"": {{
                ""provision"": [
                    {{
                        ""regulatedPlace"": [
                            {{
                                ""directedLinear"":  {{
                                    ""externalReference"": [
                                        {{
                                            ""uniqueStreetReferenceNumber"": [ 
                                                {{
                                                    
                                                }}
                                            ]
                                        }},
                                        {{
                                            ""uniqueStreetReferenceNumber"": [ 
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