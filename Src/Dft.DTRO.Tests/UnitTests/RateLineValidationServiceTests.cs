namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class RateLineValidationServiceTests
{
    private readonly IRateLineValidationService _sut = new RateLineValidationService();

    [Theory]
    [InlineData("free text", 0)]
    [InlineData("", 1)]
    [InlineData(null, 1)]
    public void ValidateRateLineDuration(string description, int errorCount)
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
                                                    ""RateLine"": [
                                                        {{
                                                            ""description"": ""{description}"",
                                                            ""durationEnd"": ""00:00:59"",
                                                            ""durationStart"": ""00:00:01"",
                                                            ""incrementPeriod"": 58,
                                                            ""maxValue"": 7.20,
                                                            ""minValue"": 1.00,
                                                            ""sequence"": 1,
                                                            ""type"": ""flatRateTier"",
                                                            ""usageCondition"": ""fixedDuration"",
                                                            ""value"": 1.00
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
                ]
            }}
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("00:00:01", 0)]
    [InlineData("23:59:59", 0)]
    [InlineData("00:00:00", 1)]
    [InlineData("24:00:00", 1)]
    [InlineData("", 1)]
    public void ValidateRateLineDurationEnd(string durationEnd, int errorCount)
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
                                                    ""RateLine"": [
                                                        {{
                                                            ""description"": ""freeText"",
                                                            ""durationEnd"": ""{durationEnd}"",
                                                            ""durationStart"": ""00:00:01"",
                                                            ""incrementPeriod"": 58,
                                                            ""maxValue"": 7.20,
                                                            ""minValue"": 1.00,
                                                            ""sequence"": 1,
                                                            ""type"": ""flatRateTier"",
                                                            ""usageCondition"": ""fixedDuration"",
                                                            ""value"": 1.00
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
                ]
            }}
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("00:00:01", 0)]
    [InlineData("23:59:59", 0)]
    [InlineData("00:00:00", 1)]
    [InlineData("24:00:00", 1)]
    [InlineData("", 1)]
    public void ValidateRateLineDurationStart(string durationStart, int errorCount)
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
                                                    ""RateLine"": [
                                                        {{
                                                            ""description"": ""freeText"",
                                                            ""durationEnd"": ""05:00:00"",
                                                            ""durationStart"": ""{durationStart}"",
                                                            ""incrementPeriod"": 58,
                                                            ""maxValue"": 7.20,
                                                            ""minValue"": 1.00,
                                                            ""sequence"": 1,
                                                            ""type"": ""flatRateTier"",
                                                            ""usageCondition"": ""fixedDuration"",
                                                            ""value"": 1.00
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
                ]
            }}
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(-1, 1)]
    public void ValidateRateLineIncrementPeriod(int incrementPeriod, int errorCount)
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
                                                    ""RateLine"": [
                                                        {{
                                                            ""description"": ""freeText"",
                                                            ""durationEnd"": ""05:00:00"",
                                                            ""durationStart"": ""06:00:00"",
                                                            ""incrementPeriod"": {incrementPeriod},
                                                            ""maxValue"": 7.20,
                                                            ""minValue"": 1.00,
                                                            ""sequence"": 1,
                                                            ""type"": ""flatRateTier"",
                                                            ""usageCondition"": ""fixedDuration"",
                                                            ""value"": 1.00
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
                ]
            }}
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData(1.01, 0)]
    [InlineData(-1.01, 1)]
    public void ValidateRateLineMaxValue(double maxValue, int errorCount)
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
                                                    ""RateLine"": [
                                                        {{
                                                            ""description"": ""freeText"",
                                                            ""durationEnd"": ""05:00:00"",
                                                            ""durationStart"": ""06:00:00"",
                                                            ""incrementPeriod"": 548,
                                                            ""maxValue"": {maxValue},
                                                            ""minValue"": 1.00,
                                                            ""sequence"": 1,
                                                            ""type"": ""flatRateTier"",
                                                            ""usageCondition"": ""fixedDuration"",
                                                            ""value"": 1.00
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
                ]
            }}
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData(1.01, 0)]
    [InlineData(-1.01, 1)]
    public void ValidateRateLineMinValue(double minValue, int errorCount)
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
                                                    ""RateLine"": [
                                                        {{
                                                            ""description"": ""freeText"",
                                                            ""durationEnd"": ""05:00:00"",
                                                            ""durationStart"": ""06:00:00"",
                                                            ""incrementPeriod"": 548,
                                                            ""maxValue"": 1.01,
                                                            ""minValue"": {minValue},
                                                            ""sequence"": 1,
                                                            ""type"": ""flatRateTier"",
                                                            ""usageCondition"": ""fixedDuration"",
                                                            ""value"": 1.00
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
                ]
            }}
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(-1, 1)]
    public void ValidateRateLineSequence(int sequence, int errorCount)
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
                                                    ""RateLine"": [
                                                        {{
                                                            ""description"": ""freeText"",
                                                            ""durationEnd"": ""05:00:00"",
                                                            ""durationStart"": ""06:00:00"",
                                                            ""incrementPeriod"": 56,
                                                            ""maxValue"": 7.20,
                                                            ""minValue"": 1.00,
                                                            ""sequence"": {sequence},
                                                            ""type"": ""flatRateTier"",
                                                            ""usageCondition"": ""fixedDuration"",
                                                            ""value"": 1.00
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
                ]
            }}
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("flatRate", 0)]
    [InlineData("incrementingRate", 0)]
    [InlineData("flatRateTier", 0)]
    [InlineData("perUnit", 0)]
    [InlineData("unknown", 1)]
    [InlineData("", 1)]
    public void ValidateRateLineRateLineType(string rateLineType, int errorCount)
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
                                                    ""RateLine"": [
                                                        {{
                                                            ""description"": ""freeText"",
                                                            ""durationEnd"": ""05:00:00"",
                                                            ""durationStart"": ""06:00:00"",
                                                            ""incrementPeriod"": 56,
                                                            ""maxValue"": 7.20,
                                                            ""minValue"": 1.00,
                                                            ""sequence"": 10,
                                                            ""type"": ""{rateLineType}"",
                                                            ""usageCondition"": ""fixedDuration"",
                                                            ""value"": 1.00
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
                ]
            }}
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("fixedDuration", 0)]
    [InlineData("fixedNumber", 0)]
    [InlineData("once", 0)]
    [InlineData("unlimited", 0)]
    [InlineData("unknown", 1)]
    [InlineData("", 1)]
    public void ValidateRateLineRateUsageConditionType(string usageCondition, int errorCount)
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
                                                    ""RateLine"": [
                                                        {{
                                                            ""description"": ""freeText"",
                                                            ""durationEnd"": ""05:00:00"",
                                                            ""durationStart"": ""06:00:00"",
                                                            ""incrementPeriod"": 56,
                                                            ""maxValue"": 7.20,
                                                            ""minValue"": 1.00,
                                                            ""sequence"": 10,
                                                            ""type"": ""flatRateTier"",
                                                            ""usageCondition"": ""{usageCondition}"",
                                                            ""value"": 1.00
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
                ]
            }}
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData(1.01, 0)]
    [InlineData(-1.01, 1)]
    public void ValidateRateLineValue(double value, int errorCount)
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
                                                    ""RateLine"": [
                                                        {{
                                                            ""description"": ""freeText"",
                                                            ""durationEnd"": ""05:00:00"",
                                                            ""durationStart"": ""06:00:00"",
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
                ]
            }}
        }}", schemaVersion);

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }
}