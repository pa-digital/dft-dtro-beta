namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class RateLineCollectionValidationServiceTests
{
    private readonly IRateLineCollectionValidationService _sut = new RateLineCollectionValidationService();

    [Theory]
    [InlineData("EUR", 0)]
    [InlineData("GBP", 0)]
    [InlineData("USD", 1)]
    [InlineData("", 1)]
    public void ValidateRateLineCollectionApplicableCurrency(string currencyType, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""Condition"": [
                                    {{
                                        ""RateTable"" : {{
                                            ""RateLineCollection"": [
                                                {{
                                                    ""applicableCurrency"": ""{currencyType}"",
                                                    ""endValidUsagePeriod"": ""23:59:59"",
                                                    ""maxTime"": 1,
                                                    ""maxValueCollection"": 20.99,
                                                    ""minTime"": 1,
                                                    ""minValueCollection"": 5.99,
                                                    ""resetTime"": ""00:30:00"",
                                                    ""sequence"": 1,
                                                    ""startValidUsagePeriod"": ""13:00:00""
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("00:00:00", 0)]
    [InlineData("00:10:00", 0)]
    [InlineData("01:59:59", 0)]
    [InlineData("23:59:59", 0)]
    [InlineData("24:00:00", 1)]
    [InlineData("badTime", 1)]
    public void ValidateRateLineCollectionEndValidUsagePeriod(string endValidUsagePeriod, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""Condition"": [
                                    {{
                                        ""RateTable"" : {{
                                            ""RateLineCollection"": [
                                                {{
                                                    ""applicableCurrency"": ""GBP"",
                                                    ""endValidUsagePeriod"": ""{endValidUsagePeriod}"",
                                                    ""maxTime"": 1,
                                                    ""maxValueCollection"": 20.99,
                                                    ""minTime"": 1,
                                                    ""minValueCollection"": 5.99,
                                                    ""resetTime"": ""00:30:00"",
                                                    ""sequence"": 1,
                                                    ""startValidUsagePeriod"": ""13:00:00""
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    public void ValidateRateLineCollectionMaxTime(int maxTime, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""Condition"": [
                                    {{
                                        ""RateTable"" : {{
                                            ""RateLineCollection"": [
                                                {{
                                                    ""applicableCurrency"": ""EUR"",
                                                    ""endValidUsagePeriod"": ""23:59:59"",
                                                    ""maxTime"": {maxTime},
                                                    ""maxValueCollection"": 20.99,
                                                    ""minTime"": 1,
                                                    ""minValueCollection"": 5.99,
                                                    ""resetTime"": ""00:30:00"",
                                                    ""sequence"": 1,
                                                    ""startValidUsagePeriod"": ""13:00:00""
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData(20.99, 0)]
    [InlineData(-20.99, 1)]
    public void ValidateRateLineCollectionMaxValueCollection(double maxValueCollection, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""Condition"": [
                                    {{
                                        ""RateTable"" : {{
                                            ""RateLineCollection"": [
                                                {{
                                                    ""applicableCurrency"": ""EUR"",
                                                    ""endValidUsagePeriod"": ""23:59:59"",
                                                    ""maxTime"": 1,
                                                    ""maxValueCollection"": {maxValueCollection},
                                                    ""minTime"": 1,
                                                    ""minValueCollection"": 5.99,
                                                    ""resetTime"": ""00:30:00"",
                                                    ""sequence"": 1,
                                                    ""startValidUsagePeriod"": ""13:00:00""
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    public void ValidateRateLineCollectionMinTime(int minTime, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""Condition"": [
                                    {{
                                        ""RateTable"" : {{
                                            ""RateLineCollection"": [
                                                {{
                                                    ""applicableCurrency"": ""EUR"",
                                                    ""endValidUsagePeriod"": ""23:59:59"",
                                                    ""maxTime"": 1,
                                                    ""maxValueCollection"": 20.99,
                                                    ""minTime"": {minTime},
                                                    ""minValueCollection"": 5.99,
                                                    ""resetTime"": ""00:30:00"",
                                                    ""sequence"": 1,
                                                    ""startValidUsagePeriod"": ""13:00:00""
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData(20.99, 0)]
    [InlineData(-20.99, 1)]
    public void ValidateRateLineCollectionMinValueCollection(double minValueCollection, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""Condition"": [
                                    {{
                                        ""RateTable"" : {{
                                            ""RateLineCollection"": [
                                                {{
                                                    ""applicableCurrency"": ""EUR"",
                                                    ""endValidUsagePeriod"": ""23:59:59"",
                                                    ""maxTime"": 1,
                                                    ""maxValueCollection"": 5.99,
                                                    ""minTime"": 1,
                                                    ""minValueCollection"": {minValueCollection},
                                                    ""resetTime"": ""00:30:00"",
                                                    ""sequence"": 1,
                                                    ""startValidUsagePeriod"": ""13:00:00""
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("00:00:00", 0)]
    [InlineData("10:00:00", 0)]
    [InlineData("23:59:59", 0)]
    [InlineData("24:00:00", 1)]
    [InlineData("badTime", 1)]
    public void ValidateRateLineCollectionResetTime(string resetTime, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""Condition"": [
                                    {{
                                        ""RateTable"" : {{
                                            ""RateLineCollection"": [
                                                {{
                                                    ""applicableCurrency"": ""GBP"",
                                                    ""endValidUsagePeriod"": ""22:30:00"",
                                                    ""maxTime"": 1,
                                                    ""maxValueCollection"": 20.99,
                                                    ""minTime"": 1,
                                                    ""minValueCollection"": 5.99,
                                                    ""resetTime"": ""{resetTime}"",
                                                    ""sequence"": 1,
                                                    ""startValidUsagePeriod"": ""13:00:00""
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(-1, 1)]
    public void ValidateRateLineCollectionSequence(int sequence, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""Condition"": [
                                    {{
                                        ""RateTable"" : {{
                                            ""RateLineCollection"": [
                                                {{
                                                    ""applicableCurrency"": ""EUR"",
                                                    ""endValidUsagePeriod"": ""23:59:59"",
                                                    ""maxTime"": 1,
                                                    ""maxValueCollection"": 20.99,
                                                    ""minTime"": 1,
                                                    ""minValueCollection"": 5.99,
                                                    ""resetTime"": ""00:30:00"",
                                                    ""sequence"": {sequence},
                                                    ""startValidUsagePeriod"": ""13:00:00""
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("00:00:00", 0)]
    [InlineData("00:10:00", 0)]
    [InlineData("01:59:59", 0)]
    [InlineData("23:59:59", 0)]
    [InlineData("24:00:00", 1)]
    [InlineData("", 1)]
    [InlineData("badTime", 1)]
    public void ValidateRateLineCollectionStartValidUsagePeriod(string startValidUsagePeriod, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""Condition"": [
                                    {{
                                        ""RateTable"" : {{
                                            ""RateLineCollection"": [
                                                {{
                                                    ""applicableCurrency"": ""GBP"",
                                                    ""endValidUsagePeriod"": ""08:00:00"",
                                                    ""maxTime"": 1,
                                                    ""maxValueCollection"": 20.99,
                                                    ""minTime"": 1,
                                                    ""minValueCollection"": 5.99,
                                                    ""resetTime"": ""00:30:00"",
                                                    ""sequence"": 1,
                                                    ""startValidUsagePeriod"": ""{startValidUsagePeriod}""
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }
}