namespace Dft.DTRO.Tests.ServicesTests.Validations;

[ExcludeFromCodeCoverage]
public class RateLineCollectionValidationServiceTests
{
    private readonly IRateLineCollectionValidationService _sut = new RateLineCollectionValidationService();

    [Theory]
    [InlineData("Condition", "EUR", 0)]
    [InlineData("Condition", "GBP", 0)]
    [InlineData("Condition", "USD", 1)]
    [InlineData("ConditionSet", "EUR", 0)]
    [InlineData("ConditionSet", "GBP", 0)]
    [InlineData("ConditionSet", "USD", 1)]
    public void ValidateRateLineCollectionApplicableCurrency(string conditionType, string currencyType, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false                                        
                                    }}
                                ],
                                ""RateTable"" : {{
                                    ""RateLineCollection"": [
                                        {{
                                            ""applicableCurrency"": ""{currencyType}"",
                                            ""endValidUsagePeriod"": ""2024-12-10T09:00:00"",
                                            ""maxTime"": 1,
                                            ""maxValueCollection"": 20.99,
                                            ""minTime"": 1,
                                            ""minValueCollection"": 5.99,
                                            ""resetTime"": ""00:30:00"",
                                            ""sequence"": 1,
                                            ""startValidUsagePeriod"": ""2024-12-10T09:00:00""
                                        }}
                                    ]
                                }}
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
    [InlineData("Condition", "2024-12-10T09:00:00", 0)]
    [InlineData("Condition", "bad-date-time", 1)]
    [InlineData("ConditionSet", "2024-12-10T09:00:00", 0)]
    [InlineData("ConditionSet", "bad-date-time", 1)]
    public void ValidateRateLineCollectionEndValidUsagePeriod(string conditionType, string endValidUsagePeriod, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false
                                    }}
                                ],
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
                                            ""startValidUsagePeriod"": ""2024-12-10T09:00:00""
                                        }}
                                    ]
                                }}
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
    [InlineData("Condition", 1, 0)]
    [InlineData("Condition", 0, 1)]
    [InlineData("ConditionSet", 1, 0)]
    [InlineData("ConditionSet", 0, 1)]
    public void ValidateRateLineCollectionMaxTime(string conditionType, int maxTime, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false
                                    }}
                                ],
                                ""RateTable"" : {{
                                    ""RateLineCollection"": [
                                        {{
                                            ""applicableCurrency"": ""EUR"",
                                            ""endValidUsagePeriod"": ""2024-12-10T09:00:00"",
                                            ""maxTime"": {maxTime},
                                            ""maxValueCollection"": 20.99,
                                            ""minTime"": 1,
                                            ""minValueCollection"": 5.99,
                                            ""resetTime"": ""00:30:00"",
                                            ""sequence"": 1,
                                            ""startValidUsagePeriod"": ""2024-12-10T09:00:00""
                                        }}
                                    ]
                                }}
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
    [InlineData("Condition", 20.99, 0)]
    [InlineData("ConditionSet", 20.99, 0)]
    public void ValidateRateLineCollectionMaxValueCollection(string conditionType, double maxValueCollection, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false
                                    }}
                                ],
                                ""RateTable"" : {{
                                    ""RateLineCollection"": [
                                        {{
                                            ""applicableCurrency"": ""EUR"",
                                            ""endValidUsagePeriod"": ""2024-12-10T09:00:00"",
                                            ""maxTime"": 1,
                                            ""maxValueCollection"": {maxValueCollection},
                                            ""minTime"": 1,
                                            ""minValueCollection"": 5.99,
                                            ""resetTime"": ""00:30:00"",
                                            ""sequence"": 1,
                                            ""startValidUsagePeriod"": ""2024-12-10T09:00:00""
                                        }}
                                    ]
                                }}
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
    [InlineData("Condition", 1, 0)]
    [InlineData("Condition", 0, 1)]
    [InlineData("ConditionSet", 1, 0)]
    [InlineData("ConditionSet", 0, 1)]
    public void ValidateRateLineCollectionMinTime(string conditionType, int minTime, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false
                                    }}
                                ],
                                ""RateTable"" : {{
                                    ""RateLineCollection"": [
                                        {{
                                            ""applicableCurrency"": ""EUR"",
                                            ""endValidUsagePeriod"": ""2024-12-10T09:00:00"",
                                            ""maxTime"": 1,
                                            ""maxValueCollection"": 20.99,
                                            ""minTime"": {minTime},
                                            ""minValueCollection"": 5.99,
                                            ""resetTime"": ""00:30:00"",
                                            ""sequence"": 1,
                                            ""startValidUsagePeriod"": ""2024-12-10T09:00:00""
                                        }}
                                    ]
                                }}
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
    [InlineData("Condition", 20.99, 0)]
    [InlineData("ConditionSet", 20.99, 0)]
    public void ValidateRateLineCollectionMinValueCollection(string conditionType, double minValueCollection, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false
                                    }}
                                ],
                                ""RateTable"" : {{
                                    ""RateLineCollection"": [
                                        {{
                                            ""applicableCurrency"": ""EUR"",
                                            ""endValidUsagePeriod"": ""2024-12-10T09:00:00"",
                                            ""maxTime"": 1,
                                            ""maxValueCollection"": 5.99,
                                            ""minTime"": 1,
                                            ""minValueCollection"": {minValueCollection},
                                            ""resetTime"": ""00:30:00"",
                                            ""sequence"": 1,
                                            ""startValidUsagePeriod"": ""2024-12-10T09:00:00""
                                        }}
                                    ]
                                }}
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
    [InlineData("Condition", "00:00:00", 0)]
    [InlineData("Condition", "10:00:00", 0)]
    [InlineData("Condition", "23:59:59", 0)]
    [InlineData("Condition", "24:00:00", 1)]
    [InlineData("Condition", "badTime", 1)]
    [InlineData("ConditionSet", "00:00:00", 0)]
    [InlineData("ConditionSet", "10:00:00", 0)]
    [InlineData("ConditionSet", "23:59:59", 0)]
    [InlineData("ConditionSet", "24:00:00", 1)]
    [InlineData("ConditionSet", "badTime", 1)]
    public void ValidateRateLineCollectionResetTime(string conditionType, string resetTime, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false
                                    }}
                                ],
                                ""RateTable"" : {{
                                    ""RateLineCollection"": [
                                        {{
                                            ""applicableCurrency"": ""GBP"",
                                            ""endValidUsagePeriod"": ""2024-12-10T09:00:00"",
                                            ""maxTime"": 1,
                                            ""maxValueCollection"": 20.99,
                                            ""minTime"": 1,
                                            ""minValueCollection"": 5.99,
                                            ""resetTime"": ""{resetTime}"",
                                            ""sequence"": 1,
                                            ""startValidUsagePeriod"": ""2024-12-10T09:00:00""
                                        }}
                                    ]
                                }}
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
    [InlineData("Condition", 1, 0)]
    [InlineData("Condition", -1, 1)]
    [InlineData("ConditionSet", 1, 0)]
    [InlineData("ConditionSet", -1, 1)]
    public void ValidateRateLineCollectionSequence(string conditionType, int sequence, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false
                                    }}
                                ],
                                ""RateTable"" : {{
                                    ""RateLineCollection"": [
                                        {{
                                            ""applicableCurrency"": ""EUR"",
                                            ""endValidUsagePeriod"": ""2024-12-10T09:00:00"",
                                            ""maxTime"": 1,
                                            ""maxValueCollection"": 20.99,
                                            ""minTime"": 1,
                                            ""minValueCollection"": 5.99,
                                            ""resetTime"": ""00:30:00"",
                                            ""sequence"": {sequence},
                                            ""startValidUsagePeriod"": ""2024-12-10T09:00:00""
                                        }}
                                    ]
                                }}
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
    [InlineData("Condition", "2024-12-10T09:00:00", 0)]
    [InlineData("Condition", "bad-date-time", 1)]
    [InlineData("ConditionSet", "2024-12-10T09:00:00", 0)]
    [InlineData("ConditionSet", "bad-date-time", 1)]
    public void ValidateRateLineCollectionStartValidUsagePeriod(string conditionType, string startValidUsagePeriod, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false
                                    }}
                                ],
                                ""RateTable"" : {{
                                    ""RateLineCollection"": [
                                        {{
                                            ""applicableCurrency"": ""GBP"",
                                            ""endValidUsagePeriod"": ""2024-12-10T09:00:00"",
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }
}