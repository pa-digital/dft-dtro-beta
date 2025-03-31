namespace Dft.DTRO.Tests.ServicesTests.Validations;

public class RateLineValidationServiceTests
{
    private readonly IRateLineValidationService _sut = new RateLineValidationService();

    [Theory]
    [InlineData("Condition", "free text", "3.4.0", 0)]
    [InlineData("Condition", "", "3.4.0", 1)]
    [InlineData("Condition", null, "3.4.0", 1)]
    [InlineData("ConditionSet", "free text", "3.4.0", 0)]
    [InlineData("ConditionSet", "", "3.4.0", 1)]
    [InlineData("ConditionSet", null, "3.4.0", 1)]
    public void ValidateRateLineDuration(string conditionType, string description, string version, int errorCount)
    {
        SchemaVersion schemaVersion = new(version);

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
                                                    ""durationEnd"": ""11:00:00"",
                                                    ""durationStart"": ""10:00:00"",
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
    [InlineData("Condition", "00:00:00", "3.4.0", 0)]
    [InlineData("Condition", "11:00:00", "3.4.0", 0)]
    [InlineData("Condition", "23:59:59", "3.4.0", 0)]
    [InlineData("ConditionSet", "00:00:00", "3.4.0", 0)]
    [InlineData("ConditionSet", "11:00:00", "3.4.0", 0)]
    [InlineData("ConditionSet", "23:59:59", "3.4.0", 0)]
    public void ValidateRateLineDurationEnd(string conditionType, string durationEnd, string version, int errorCount)
    {
        SchemaVersion schemaVersion = new(version);

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
                                                    ""durationEnd"": ""{durationEnd}"",
                                                    ""durationStart"": ""11:00:00"",
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
    [InlineData("Condition", "00:00:00", "3.4.0", 0)]
    [InlineData("Condition", "11:00:00", "3.4.0", 0)]
    [InlineData("Condition", "23:59:59", "3.4.0", 0)]
    [InlineData("ConditionSet", "00:00:00", "3.4.0", 0)]
    [InlineData("ConditionSet", "11:00:00", "3.4.0", 0)]
    [InlineData("ConditionSet", "23:59:59", "3.4.0", 0)]
    public void ValidateRateLineDurationStart(string conditionType, string durationStart, string version, int errorCount)
    {
        SchemaVersion schemaVersion = new(version);

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
                                                    ""durationEnd"": ""11:00:00"",
                                                    ""durationStart"": ""{durationStart}"",
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
    [InlineData("Condition", 0, "3.4.0", 1)]
    [InlineData("Condition", 1, "3.4.0", 0)]
    [InlineData("Condition", -1, "3.4.0", 1)]
    [InlineData("ConditionSet", 0, "3.4.0", 1)]
    [InlineData("ConditionSet", 1, "3.4.0", 0)]
    [InlineData("ConditionSet", -1, "3.4.0", 1)]
    public void ValidateRateLineIncrementPeriod(string conditionType, int incrementPeriod, string version, int errorCount)
    {
        SchemaVersion schemaVersion = new(version);

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
                                                    ""durationEnd"": ""11:00:00"",
                                                    ""durationStart"": ""10:00:00"",
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
    [InlineData("Condition", 1.01, "3.4.0", 0)]
    [InlineData("ConditionSet", 1.01, "3.4.0", 0)]
    public void ValidateRateLineMinAndMaxValue(string conditionType, double value, string version, int errorCount)
    {
        SchemaVersion schemaVersion = new(version);

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
                                                    ""durationEnd"": ""11:00:00"",
                                                    ""durationStart"": ""10:00:00"",
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
    [InlineData("Condition", 0, "3.4.0", 1)]
    [InlineData("Condition", 1, "3.4.0", 0)]
    [InlineData("Condition", -1, "3.4.0", 1)]
    [InlineData("ConditionSet", 0, "3.4.0", 1)]
    [InlineData("ConditionSet", 1, "3.4.0", 0)]
    [InlineData("ConditionSet", -1, "3.4.0", 1)]
    public void ValidateRateLineSequence(string conditionType, int sequence, string version, int errorCount)
    {
        SchemaVersion schemaVersion = new(version);

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
                                                    ""durationEnd"": ""11:00:00"",
                                                    ""durationStart"": ""10:00:00"",
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
    [InlineData("Condition", "flatRate", "3.4.0", 0)]
    [InlineData("Condition", "incrementingRate", "3.4.0", 0)]
    [InlineData("Condition", "flatRateTier", "3.4.0", 0)]
    [InlineData("Condition", "perUnit", "3.4.0", 0)]
    [InlineData("Condition", "unknown", "3.4.0", 1)]
    [InlineData("Condition", "", "3.4.0", 1)]
    [InlineData("ConditionSet", "flatRate", "3.4.0", 0)]
    [InlineData("ConditionSet", "incrementingRate", "3.4.0", 0)]
    [InlineData("ConditionSet", "flatRateTier", "3.4.0", 0)]
    [InlineData("ConditionSet", "perUnit", "3.4.0", 0)]
    [InlineData("ConditionSet", "unknown", "3.4.0", 1)]
    [InlineData("ConditionSet", "", "3.4.0", 1)]
    public void ValidateRateLineRateLineType(string conditionType, string rateLineType, string version, int errorCount)
    {
        SchemaVersion schemaVersion = new(version);

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
                                                    ""durationEnd"": ""11:00:00"",
                                                    ""durationStart"": ""10:00:00"",
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
    [InlineData("Condition", "fixedDuration", "3.4.0", 0)]
    [InlineData("Condition", "fixedNumber", "3.4.0", 0)]
    [InlineData("Condition", "once", "3.4.0", 0)]
    [InlineData("Condition", "unlimited", "3.4.0", 0)]
    [InlineData("Condition", "unknown", "3.4.0", 1)]
    [InlineData("Condition", "", "3.4.0", 1)]
    [InlineData("ConditionSet", "fixedDuration", "3.4.0", 0)]
    [InlineData("ConditionSet", "fixedNumber", "3.4.0", 0)]
    [InlineData("ConditionSet", "once", "3.4.0", 0)]
    [InlineData("ConditionSet", "unlimited", "3.4.0", 0)]
    [InlineData("ConditionSet", "unknown", "3.4.0", 1)]
    [InlineData("ConditionSet", "", "3.4.0", 1)]
    public void ValidateRateLineRateUsageConditionType(string conditionType, string usageCondition, string version, int errorCount)
    {
        SchemaVersion schemaVersion = new(version);

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
                                                    ""durationEnd"": ""11:00:00"",
                                                    ""durationStart"": ""10:00:00"",
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
    [InlineData("Condition", 1.01, "3.4.0", 0)]
    [InlineData("ConditionSet", 1.01, "3.4.0", 0)]
    public void ValidateRateLineValue(string conditionType, double value, string version, int errorCount)
    {
        SchemaVersion schemaVersion = new(version);

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
                                                    ""durationEnd"": ""11:00:00"",
                                                    ""durationStart"": ""10:00:00"",
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