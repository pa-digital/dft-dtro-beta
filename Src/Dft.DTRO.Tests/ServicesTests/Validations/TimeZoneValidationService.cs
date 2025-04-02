namespace Dft.DTRO.Tests.ServicesTests.Validations;

public class TimeZoneValidationService
{
    private readonly IDtroTimeZoneValidatorService _sut = new DtroTimeZoneValidatorService();

    [Theory]
    [InlineData(new[] { "2020-01-02T01:00:00", "2020-01-01T02:00:00", "2020-01-01T03:00:00", "2020-01-01T04:00:00", "2020-01-01T05:00:00", "2020-01-01T06:00:00" }, "3.3.0")]
    [InlineData(new[] { "2020-01-02T01:00:00", "2020-01-01T02:00:00", "2020-01-01T03:00:00", "2020-01-01T04:00:00", "2020-01-01T05:00:00", "2020-01-01T06:00:00" }, "3.3.1")]
    [InlineData(new[] { "2020-01-02T01:00:00", "2020-01-01T02:00:00", "2020-01-01T03:00:00", "2020-01-01T04:00:00", "2020-01-01T05:00:00", "2020-01-01T06:00:00" }, "3.4.0")]
    public void ValidateReturnsNoErrors(string[] dateTimeValues,string version)
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


        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(0, actual.RequestComparedToRules.Count());
    }

    [Theory]
    [InlineData(new[] { "2024-06-15T01:00:00", "2024-06-15T02:00:00", "2024-06-15T03:00:00", "2024-12-15T04:00:00", "2024-12-15T05:00:00", "2094-12-15T06:00:00" }, "3.3.0", 1)]
    [InlineData(new[] { "2024-06-15T01:00:00", "2024-06-15T02:00:00", "2024-06-15T03:00:00", "2024-12-15T04:00:00", "2024-12-15T05:00:00", "2094-12-15T06:00:00" }, "3.3.1", 1)]
    [InlineData(new[] { "2024-06-15T01:00:00", "2024-06-15T02:00:00", "2024-06-15T03:00:00", "2024-12-15T04:00:00", "2024-12-15T05:00:00", "2094-12-15T06:00:00" }, "3.4.0", 1)]
    public void ValidateReturnsOneErrors(string[] dateTimeValues,string version, int errorCount)
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


        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.RequestComparedToRules.Count());
    }

    [Theory]
    [InlineData(new[] { "2044-06-15T01:00:00", "2054-06-15T02:00:00", "2064-06-15T03:00:00", "2074-12-15T04:00:00", "2084-12-15T05:00:00", "2094-12-15T06:00:00" }, "3.3.0", 6)]
    [InlineData(new[] { "2044-06-15T01:00:00", "2054-06-15T02:00:00", "2064-06-15T03:00:00", "2074-12-15T04:00:00", "2084-12-15T05:00:00", "2094-12-15T06:00:00" }, "3.3.1", 6)]
    [InlineData(new[] { "2044-06-15T01:00:00", "2054-06-15T02:00:00", "2064-06-15T03:00:00", "2074-12-15T04:00:00", "2084-12-15T05:00:00", "2094-12-15T06:00:00" }, "3.4.0", 6)]
    public void ValidateReturnsErrors(string[] dateTimeValues,string version, int errorCount)
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
                                            ""end"": ""{dateTimeValues[2]}]"",
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


        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.RequestComparedToRules.Count());
    }

    [Theory]
    [InlineData(new[] { "2024-06-15T01:00:00", "2024-06-15T02:00:00", "2024-06-15T03:00:00", "2024-12-15T04:00:00", "2024-12-15T05:00:00", "2024-12-15T06:00:00" }, "3.3.0", 0)]
    [InlineData(new[] { "2024-06-15T01:00:00", "2024-06-15T02:00:00", "2024-06-15T03:00:00", "2024-12-15T04:00:00", "2024-12-15T05:00:00", "2024-12-15T06:00:00" }, "3.3.1", 0)]
    [InlineData(new[] { "2024-06-15T01:00:00", "2024-06-15T02:00:00", "2024-06-15T03:00:00", "2024-12-15T04:00:00", "2024-12-15T05:00:00", "2024-12-15T06:00:00" }, "3.4.0", 0)]
    public void ValidateNoErrorRegardlessOfDaylightSavingTime(string[] dateTimeValues, string version, int errorCount)
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
                                            ""end"": ""{dateTimeValues[2]}]"",
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


        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.RequestComparedToRules.Count());
    }
}