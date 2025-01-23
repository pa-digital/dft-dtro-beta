namespace Dft.DTRO.Tests.ServicesTests.Validations;

[ExcludeFromCodeCoverage]
public class RateLineValidationServiceTests
{
    private readonly IRateLineValidationService _sut = new RateLineValidationService();

    [Theory]
    [InlineData("Condition", "free text", 0)]
    [InlineData("Condition", "", 1)]
    [InlineData("Condition", null, 1)]
    [InlineData("ConditionSet", "free text", 0)]
    [InlineData("ConditionSet", "", 1)]
    [InlineData("ConditionSet", null, 1)]
    public void ValidateRateLineDuration(string conditionType, string description, int errorCount)
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
                                            ""RateLine"": [
                                                {{
                                                    ""description"": ""{description}"",
                                                    ""durationEnd"": 1,
                                                    ""durationStart"": 5,
                                                    ""incrementPeriod"": 58,
                                                    ""maxValue"": 7.20,
                                                    ""minValue"": 1.05,
                                                    ""sequence"": 1,
                                                    ""type"": ""flatRateTier"",
                                                    ""usageCondition"": ""fixedDuration"",
                                                    ""value"": 1.04
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("Condition", 1, 0)]
    [InlineData("Condition", 1000, 0)]
    [InlineData("Condition", 0, 1)]
    [InlineData("Condition", -10, 1)]
    [InlineData("ConditionSet", 1, 0)]
    [InlineData("ConditionSet", 1000, 0)]
    [InlineData("ConditionSet", 0, 1)]
    [InlineData("ConditionSet", -10, 1)]
    public void ValidateRateLineDurationEnd(string conditionType, int durationEnd, int errorCount)
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
                                            ""RateLine"": [
                                                {{
                                                    ""description"": ""freeText"",
                                                    ""durationEnd"": {durationEnd},
                                                    ""durationStart"": 1,
                                                    ""incrementPeriod"": 58,
                                                    ""maxValue"": 7.20,
                                                    ""minValue"": 1.03,
                                                    ""sequence"": 1,
                                                    ""type"": ""flatRateTier"",
                                                    ""usageCondition"": ""fixedDuration"",
                                                    ""value"": 1.02
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("Condition", 1, 0)]
    [InlineData("Condition", 1000, 0)]
    [InlineData("Condition", 0, 1)]
    [InlineData("Condition", -10, 1)]
    [InlineData("ConditionSet", 1, 0)]
    [InlineData("ConditionSet", 1000, 0)]
    [InlineData("ConditionSet", 0, 1)]
    [InlineData("ConditionSet", -10, 1)]
    public void ValidateRateLineDurationStart(string conditionType, int durationStart, int errorCount)
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
                                            ""RateLine"": [
                                                {{
                                                    ""description"": ""freeText"",
                                                    ""durationEnd"": 1,
                                                    ""durationStart"": {durationStart},
                                                    ""incrementPeriod"": 58,
                                                    ""maxValue"": 7.20,
                                                    ""minValue"": 1.02,
                                                    ""sequence"": 1,
                                                    ""type"": ""flatRateTier"",
                                                    ""usageCondition"": ""fixedDuration"",
                                                    ""value"": 1.01
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("Condition", 0, 1)]
    [InlineData("Condition", 1, 0)]
    [InlineData("Condition", -1, 1)]
    [InlineData("ConditionSet", 0, 1)]
    [InlineData("ConditionSet", 1, 0)]
    [InlineData("ConditionSet", -1, 1)]
    public void ValidateRateLineIncrementPeriod(string conditionType, int incrementPeriod, int errorCount)
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
                                            ""RateLine"": [
                                                {{
                                                    ""description"": ""freeText"",
                                                    ""durationEnd"": 1,
                                                    ""durationStart"": 5,
                                                    ""incrementPeriod"": {incrementPeriod},
                                                    ""maxValue"": 7.20,
                                                    ""minValue"": 1.01,
                                                    ""sequence"": 1,
                                                    ""type"": ""flatRateTier"",
                                                    ""usageCondition"": ""fixedDuration"",
                                                    ""value"": 1.01
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("Condition", 1.01, 0)]
    [InlineData("ConditionSet", 1.01, 0)]
    public void ValidateRateLineMinAndMaxValue(string conditionType, double value, int errorCount)
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
                                            ""RateLine"": [
                                                {{
                                                    ""description"": ""freeText"",
                                                    ""durationEnd"": 1,
                                                    ""durationStart"": 5,
                                                    ""incrementPeriod"": 548,
                                                    ""maxValue"": {value},
                                                    ""minValue"": {value},
                                                    ""sequence"": 1,
                                                    ""type"": ""flatRateTier"",
                                                    ""usageCondition"": ""fixedDuration"",
                                                    ""value"": 1.01
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("Condition", 0, 1)]
    [InlineData("Condition", 1, 0)]
    [InlineData("Condition", -1, 1)]
    [InlineData("ConditionSet", 0, 1)]
    [InlineData("ConditionSet", 1, 0)]
    [InlineData("ConditionSet", -1, 1)]
    public void ValidateRateLineSequence(string conditionType, int sequence, int errorCount)
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
                                            ""RateLine"": [
                                                {{
                                                    ""description"": ""freeText"",
                                                    ""durationEnd"": 1,
                                                    ""durationStart"": 5,
                                                    ""incrementPeriod"": 56,
                                                    ""maxValue"": 7.20,
                                                    ""minValue"": 1.04,
                                                    ""sequence"": {sequence},
                                                    ""type"": ""flatRateTier"",
                                                    ""usageCondition"": ""fixedDuration"",
                                                    ""value"": 1.04
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("Condition", "flatRate", 0)]
    [InlineData("Condition", "incrementingRate", 0)]
    [InlineData("Condition", "flatRateTier", 0)]
    [InlineData("Condition", "perUnit", 0)]
    [InlineData("Condition", "unknown", 1)]
    [InlineData("Condition", "", 1)]
    [InlineData("ConditionSet", "flatRate", 0)]
    [InlineData("ConditionSet", "incrementingRate", 0)]
    [InlineData("ConditionSet", "flatRateTier", 0)]
    [InlineData("ConditionSet", "perUnit", 0)]
    [InlineData("ConditionSet", "unknown", 1)]
    [InlineData("ConditionSet", "", 1)]
    public void ValidateRateLineRateLineType(string conditionType, string rateLineType, int errorCount)
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
                                            ""RateLine"": [
                                                {{
                                                    ""description"": ""freeText"",
                                                    ""durationEnd"": 1,
                                                    ""durationStart"": 5,
                                                    ""incrementPeriod"": 56,
                                                    ""maxValue"": 7.20,
                                                    ""minValue"": 1.01,
                                                    ""sequence"": 10,
                                                    ""type"": ""{rateLineType}"",
                                                    ""usageCondition"": ""fixedDuration"",
                                                    ""value"": 1.01
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("Condition", "fixedDuration", 0)]
    [InlineData("Condition", "fixedNumber", 0)]
    [InlineData("Condition", "once", 0)]
    [InlineData("Condition", "unlimited", 0)]
    [InlineData("Condition", "unknown", 1)]
    [InlineData("Condition", "", 1)]
    [InlineData("ConditionSet", "fixedDuration", 0)]
    [InlineData("ConditionSet", "fixedNumber", 0)]
    [InlineData("ConditionSet", "once", 0)]
    [InlineData("ConditionSet", "unlimited", 0)]
    [InlineData("ConditionSet", "unknown", 1)]
    [InlineData("ConditionSet", "", 1)]
    public void ValidateRateLineRateUsageConditionType(string conditionType, string usageCondition, int errorCount)
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
                                            ""RateLine"": [
                                                {{
                                                    ""description"": ""freeText"",
                                                    ""durationEnd"": 1,
                                                    ""durationStart"": 5,
                                                    ""incrementPeriod"": 56,
                                                    ""maxValue"": 7.20,
                                                    ""minValue"": 1.01,
                                                    ""sequence"": 10,
                                                    ""type"": ""flatRateTier"",
                                                    ""usageCondition"": ""{usageCondition}"",
                                                    ""value"": 1.01
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("Condition", 1.01, 0)]
    [InlineData("ConditionSet", 1.01, 0)]
    public void ValidateRateLineValue(string conditionType, double value, int errorCount)
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
                                            ""RateLine"": [
                                                {{
                                                    ""description"": ""freeText"",
                                                    ""durationEnd"": 1,
                                                    ""durationStart"": 5,
                                                    ""incrementPeriod"": 548,
                                                    ""maxValue"": 1.01,
                                                    ""minValue"": 0.85,
                                                    ""sequence"": 1,
                                                    ""type"": ""flatRateTier"",
                                                    ""usageCondition"": ""fixedDuration"",
                                                    ""value"": {value}
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
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }
}