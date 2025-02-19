namespace Dft.DTRO.Tests.ServicesTests.Validations;

[ExcludeFromCodeCoverage]
public class RateLineValidationServiceTests
{
    private readonly IRateLineValidationService _sut = new RateLineValidationService();

    [Theory]
    [InlineData("condition", "free text", 0)]
    [InlineData("condition", "", 1)]
    [InlineData("condition", null, 1)]
    [InlineData("conditionSet", "free text", 0)]
    [InlineData("conditionSet", "", 1)]
    [InlineData("conditionSet", null, 1)]
    public void ValidateRateLineDuration(string conditionType, string description, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""source"": {{
                ""provision"": [
                    {{
                        ""regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false
                                    }}
                                ],
                                ""rateTable"" : {{
                                    ""rateLineCollection"": [
                                        {{
                                            ""rateLine"": [
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
    [InlineData("condition", 1, 0)]
    [InlineData("condition", 1000, 0)]
    [InlineData("condition", 0, 1)]
    [InlineData("condition", -10, 1)]
    [InlineData("conditionSet", 1, 0)]
    [InlineData("conditionSet", 1000, 0)]
    [InlineData("conditionSet", 0, 1)]
    [InlineData("conditionSet", -10, 1)]
    public void ValidateRateLineDurationEnd(string conditionType, int durationEnd, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""source"": {{
                ""provision"": [
                    {{
                        ""regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false
                                    }}
                                ],
                                ""rateTable"" : {{
                                    ""rateLineCollection"": [
                                        {{
                                            ""rateLine"": [
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
    [InlineData("condition", 1, 0)]
    [InlineData("condition", 1000, 0)]
    [InlineData("condition", 0, 1)]
    [InlineData("condition", -10, 1)]
    [InlineData("conditionSet", 1, 0)]
    [InlineData("conditionSet", 1000, 0)]
    [InlineData("conditionSet", 0, 1)]
    [InlineData("conditionSet", -10, 1)]
    public void ValidateRateLineDurationStart(string conditionType, int durationStart, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""source"": {{
                ""provision"": [
                    {{
                        ""regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false
                                    }}
                                ],
                                ""rateTable"" : {{
                                    ""rateLineCollection"": [
                                        {{
                                            ""rateLine"": [
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
    [InlineData("condition", 0, 1)]
    [InlineData("condition", 1, 0)]
    [InlineData("condition", -1, 1)]
    [InlineData("conditionSet", 0, 1)]
    [InlineData("conditionSet", 1, 0)]
    [InlineData("conditionSet", -1, 1)]
    public void ValidateRateLineIncrementPeriod(string conditionType, int incrementPeriod, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""source"": {{
                ""provision"": [
                    {{
                        ""regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false
                                    }}
                                ],
                                ""rateTable"" : {{
                                    ""rateLineCollection"": [
                                        {{
                                            ""rateLine"": [
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
    [InlineData("condition", 1.01, 0)]
    [InlineData("conditionSet", 1.01, 0)]
    public void ValidateRateLineMinAndMaxValue(string conditionType, double value, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""source"": {{
                ""provision"": [
                    {{
                        ""regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false
                                    }}
                                ],
                                ""rateTable"" : {{
                                    ""rateLineCollection"": [
                                        {{
                                            ""rateLine"": [
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
    [InlineData("condition", 0, 1)]
    [InlineData("condition", 1, 0)]
    [InlineData("condition", -1, 1)]
    [InlineData("conditionSet", 0, 1)]
    [InlineData("conditionSet", 1, 0)]
    [InlineData("conditionSet", -1, 1)]
    public void ValidateRateLineSequence(string conditionType, int sequence, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""source"": {{
                ""provision"": [
                    {{
                        ""regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false
                                    }}
                                ],
                                ""rateTable"" : {{
                                    ""rateLineCollection"": [
                                        {{
                                            ""rateLine"": [
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
    [InlineData("condition", "flatRate", 0)]
    [InlineData("condition", "incrementingRate", 0)]
    [InlineData("condition", "flatRateTier", 0)]
    [InlineData("condition", "perUnit", 0)]
    [InlineData("condition", "unknown", 1)]
    [InlineData("condition", "", 1)]
    [InlineData("conditionSet", "flatRate", 0)]
    [InlineData("conditionSet", "incrementingRate", 0)]
    [InlineData("conditionSet", "flatRateTier", 0)]
    [InlineData("conditionSet", "perUnit", 0)]
    [InlineData("conditionSet", "unknown", 1)]
    [InlineData("conditionSet", "", 1)]
    public void ValidateRateLineRateLineType(string conditionType, string rateLineType, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""source"": {{
                ""provision"": [
                    {{
                        ""regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false
                                    }}
                                ],
                                ""rateTable"" : {{
                                    ""rateLineCollection"": [
                                        {{
                                            ""rateLine"": [
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
    [InlineData("condition", "fixedDuration", 0)]
    [InlineData("condition", "fixedNumber", 0)]
    [InlineData("condition", "once", 0)]
    [InlineData("condition", "unlimited", 0)]
    [InlineData("condition", "unknown", 1)]
    [InlineData("condition", "", 1)]
    [InlineData("conditionSet", "fixedDuration", 0)]
    [InlineData("conditionSet", "fixedNumber", 0)]
    [InlineData("conditionSet", "once", 0)]
    [InlineData("conditionSet", "unlimited", 0)]
    [InlineData("conditionSet", "unknown", 1)]
    [InlineData("conditionSet", "", 1)]
    public void ValidateRateLineRateUsageConditionType(string conditionType, string usageCondition, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""source"": {{
                ""provision"": [
                    {{
                        ""regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false
                                    }}
                                ],
                                ""rateTable"" : {{
                                    ""rateLineCollection"": [
                                        {{
                                            ""rateLine"": [
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
    [InlineData("condition", 1.01, 0)]
    [InlineData("conditionSet", 1.01, 0)]
    public void ValidateRateLineValue(string conditionType, double value, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""source"": {{
                ""provision"": [
                    {{
                        ""regulation"": [
                            {{
                                ""{conditionType}"": [
                                    {{
                                        ""negate"": false
                                    }}
                                ],
                                ""rateTable"" : {{
                                    ""rateLineCollection"": [
                                        {{
                                            ""rateLine"": [
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