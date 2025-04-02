namespace Dft.DTRO.Tests.ServicesTests.Validations;

public class TimeZoneValidationService
{
    private readonly IDtroTimeZoneValidatorService _sut = new DtroTimeZoneValidatorService();

    private SystemClock _clock = new();

    [Theory]
    [InlineData(new[]
    {
        "2020-01-02T01:00:00", 
        "2020-01-01T02:00:00", 
        "2020-01-01T03:00:00", 
        "2020-01-01T04:00:00", 
        "2020-01-01T05:00:00",
        "2020-01-01T06:00:00"
    }, "3.4.0")]
    public async Task FindAllDateTimesValuesReturnsNoErrors(string[] dateTimeValues,string version)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""DirectedLinear"": {{
                                    ""origin"": [
                                        {{
                                            ""lastUpdateDate"": ""{dateTimeValues[0]}""
                                        }}
                                    ]
                                }},
                                ""Polygon"": {{
                                    ""ExternalReference"": [
                                        {{
                                            ""lastUpdateDate"": ""{dateTimeValues[1]}""
                                        }}
                                    ]
                                }}
                            }}
                        ],
                        ""Regulation"": [
                            {{
                                ""Condition"": [
                                    {{
                                        ""TimeValidity"": {{
                                            ""end"": ""{dateTimeValues[2]}"",
                                            ""start"": ""{dateTimeValues[3]}""
                                        }},
                                        ""RateTable"": {{
                                            ""RateLineCollection"": [
                                                {{
                                                    ""startValidUsagePeriod"": ""{dateTimeValues[4]}"",
                                                    ""endValidUsagePeriod"": ""{dateTimeValues[5]}""
                                                }}
                                            ]
                                        }}
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}
        }}", new SchemaVersion(version));


        var actual = await _sut.ValidateDtro(dtroSubmit);
        Assert.Equal(0, actual.RequestComparedToRules.Count());
    }

    [Theory]
    [InlineData(new[]
    {
        "2020-01-02T01:00:00", 
        "2020-01-01T02:00:00", 
        "2020-01-01T03:00:00", 
        "2020-01-01T04:00:00", 
        "2020-01-01T05:00:00",
    }, "3.4.0", 1)]
    public async Task FindAllDateTimesValuesReturnsOneErrors(string[] dateTimeValues,string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""DirectedLinear"": {{
                                    ""origin"": [
                                        {{
                                            ""lastUpdateDate"": ""{dateTimeValues[0]}""
                                        }}
                                    ]
                                }},
                                ""Polygon"": {{
                                    ""ExternalReference"": [
                                        {{
                                            ""lastUpdateDate"": ""{dateTimeValues[1]}""
                                        }}
                                    ]
                                }}
                            }}
                        ],
                        ""Regulation"": [
                            {{
                                ""Condition"": [
                                    {{
                                        ""TimeValidity"": {{
                                            ""end"": ""{dateTimeValues[2]}"",
                                            ""start"": ""{dateTimeValues[3]}""
                                        }},
                                        ""RateTable"": {{
                                            ""RateLineCollection"": [
                                                {{
                                                    ""startValidUsagePeriod"": ""{dateTimeValues[4]}"",
                                                    ""endValidUsagePeriod"": ""{_clock.UtcNow.ToString("s")}""
                                                }}
                                            ]
                                        }}
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}
        }}", new SchemaVersion(version));


        var actual = await _sut.ValidateDtro(dtroSubmit);
        Assert.Equal(errorCount, actual.RequestComparedToRules.Count());
    }

    [Theory]
    [InlineData("3.4.0", 6)]
    public async Task FindAllDateTimesValuesReturnsErrors(string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""DirectedLinear"": {{
                                    ""origin"": [
                                        {{
                                            ""lastUpdateDate"": ""{_clock.UtcNow.ToString("s")}""
                                        }}
                                    ]
                                }},
                                ""Polygon"": {{
                                    ""ExternalReference"": [
                                        {{
                                            ""lastUpdateDate"": ""{_clock.UtcNow.ToString("s")}""
                                        }}
                                    ]
                                }}
                            }}
                        ],
                        ""Regulation"": [
                            {{
                                ""Condition"": [
                                    {{
                                        ""TimeValidity"": {{
                                            ""end"": ""{_clock.UtcNow.ToString("s")}"",
                                            ""start"": ""{_clock.UtcNow.ToString("s")}""
                                        }},
                                        ""RateTable"": {{
                                            ""RateLineCollection"": [
                                                {{
                                                    ""startValidUsagePeriod"": ""{_clock.UtcNow.ToString("s")}"",
                                                    ""endValidUsagePeriod"": ""{_clock.UtcNow.ToString("s")}""
                                                }}
                                            ]
                                        }}
                                    }}
                                ]
                            }}
                        ]
                    }}
                ]
            }}
        }}", new SchemaVersion(version));


        var actual = await _sut.ValidateDtro(dtroSubmit);
        Assert.Equal(errorCount, actual.RequestComparedToRules.Count());
    }
}