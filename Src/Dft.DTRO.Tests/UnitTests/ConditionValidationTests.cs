namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class ConditionValidationTests
{
    private readonly IConditionValidation _sut = new ConditionValidation();

    [Theory]
    [InlineData("3.3.0", 0)]
    public void ValidateConditionReturnsNoErrorsWhenConditionSetIsPresent(string version, int errorCount)
    {
        SchemaVersion schemaVersion = (version);
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {
                        ""regulation"": [
                            {
                                ""conditionSet"": [
                                    {
                                        ""operator"": ""and"",
                                        ""conditionSet"": [
                                            {
                                                ""operator"": ""or"",
                                                ""condition"": [
                                                    {
                                                        ""negate"": false,
                                                        ""vehicleCharacteristics"": {
                                                            ""maximumHeightCharacteristic"": {
                                                                ""vehicleHeight"": 2.5
                                                            }
                                                        }
                                                    },
                                                    {
                                                        ""negate"": true,
                                                        ""vehicleCharacteristics"": {
                                                            ""vehicleType"": ""bus""
                                                        }
                                                    },
                                                    {
                                                        ""operator"": ""and"",
                                                        ""condition"": [
                                                            {
                                                                ""negate"": ""and"",
                                                                ""vehicleCharacteristics"": {
                                                                    ""vehicleType"": ""taxi""
                                                                }
                                                            },
                                                            {
                                                                ""negate"": false,
                                                                ""vehicleCharacteristics"": {
                                                                    ""vehicleUsage"": ""access""
                                                                }
                                                            }
                                                        ]
                                                    }
                                                ]
                                            }
                                        ],
                                        ""condition"": {
                                            ""timeValidity"": {
                                                ""start"": ""2024-08-22T08:00:00"",
                                                ""end"": ""2024-08-22T20:00:00""
                                            }
                                        }
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        }", schemaVersion);

        var actual = _sut.ValidateCondition(dtroSubmit, schemaVersion);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.3.0", 1)]
    public void ValidateConditionReturnsErrorsWhenConditionSetIsPresentButWrongOperator(string version, int errorCount)
    {
        SchemaVersion schemaVersion = (version);
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {
                        ""regulation"": [
                            {
                                ""conditionSet"": [
                                    {
                                        ""operator"": ""some"",
                                        ""conditionSet"": [
                                            {
                                                ""operator"": ""or"",
                                                ""condition"": [
                                                    {
                                                        ""negate"": false,
                                                        ""vehicleCharacteristics"": {
                                                            ""maximumHeightCharacteristic"": {
                                                                ""vehicleHeight"": 2.5
                                                            }
                                                        }
                                                    },
                                                    {
                                                        ""negate"": true,
                                                        ""vehicleCharacteristics"": {
                                                            ""vehicleType"": ""bus""
                                                        }
                                                    },
                                                    {
                                                        ""operator"": ""and"",
                                                        ""condition"": [
                                                            {
                                                                ""negate"": ""and"",
                                                                ""vehicleCharacteristics"": {
                                                                    ""vehicleType"": ""taxi""
                                                                }
                                                            },
                                                            {
                                                                ""negate"": false,
                                                                ""vehicleCharacteristics"": {
                                                                    ""vehicleUsage"": ""access""
                                                                }
                                                            }
                                                        ]
                                                    }
                                                ]
                                            }
                                        ],
                                        ""condition"": {
                                            ""timeValidity"": {
                                                ""start"": ""2024-08-22T08:00:00"",
                                                ""end"": ""2024-08-22T20:00:00""
                                            }
                                        }
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        }", schemaVersion);

        var actual = _sut.ValidateCondition(dtroSubmit, schemaVersion);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.3.0", 1)]
    public void ValidateConditionReturnsErrorsWhenMultipleConditionsIsPresentWithoutConditionSet(string version, int errorCount)
    {
        SchemaVersion schemaVersion = (version);
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {
                        ""regulation"": [
                            {
                                ""condition"": {
                                    ""timeValidity"": {

                                    },
                                    ""roadCondition"": {

                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }", schemaVersion);

        var actual = _sut.ValidateCondition(dtroSubmit, schemaVersion);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.3.0", 0)]
    public void ValidateConditionReturnsNoErrorsWhenOneConditionIsPresent(string version, int errorCount)
    {
        SchemaVersion schemaVersion = new(version);

        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {
                        ""regulation"": [
                            {
                                ""condition"": {
                                    ""timeValidity"": {

                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }", schemaVersion);

        var actual = _sut.ValidateCondition(dtroSubmit, schemaVersion);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.3.0", 1)]
    public void ValidateConditionReturnsErrorsWhenWrongConditionIsPresent(string version, int errorCount)
    {
        SchemaVersion schemaVersion = new(version);

        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {
                        ""regulation"": [
                            {
                                ""condition"": {
                                    ""SomeCondition"": {

                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }", schemaVersion);

        var actual = _sut.ValidateCondition(dtroSubmit, schemaVersion);
        Assert.Equal(errorCount, actual.Count);
    }
}