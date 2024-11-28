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
            ""Source"": {
                ""Provision"": [
                    {
                        ""Regulation"": [
                            {
                                ""ConditionSet"": [
                                    {
                                        ""operator"": ""and"",
                                        ""ConditionSet"": [
                                            {
                                                ""operator"": ""or"",
                                                ""Condition"": [
                                                    {
                                                        ""negate"": false,
                                                        ""VehicleCharacteristics"": {
                                                            ""MaximumHeightCharacteristic"": {
                                                                ""vehicleHeight"": 2.5
                                                            }
                                                        }
                                                    },
                                                    {
                                                        ""negate"": true,
                                                        ""VehicleCharacteristics"": {
                                                            ""vehicleType"": ""bus""
                                                        }
                                                    },
                                                    {
                                                        ""operator"": ""and"",
                                                        ""Condition"": [
                                                            {
                                                                ""negate"": ""and"",
                                                                ""VehicleCharacteristics"": {
                                                                    ""vehicleType"": ""taxi""
                                                                }
                                                            },
                                                            {
                                                                ""negate"": false,
                                                                ""VehicleCharacteristics"": {
                                                                    ""vehicleUsage"": ""access""
                                                                }
                                                            }
                                                        ]
                                                    }
                                                ]
                                            }
                                        ],
                                        ""Condition"": {
                                            ""TimeValidity"": {
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
            ""Source"": {
                ""Provision"": [
                    {
                        ""Regulation"": [
                            {
                                ""ConditionSet"": [
                                    {
                                        ""operator"": ""some"",
                                        ""ConditionSet"": [
                                            {
                                                ""operator"": ""or"",
                                                ""Condition"": [
                                                    {
                                                        ""negate"": false,
                                                        ""VehicleCharacteristics"": {
                                                            ""MaximumHeightCharacteristic"": {
                                                                ""vehicleHeight"": 2.5
                                                            }
                                                        }
                                                    },
                                                    {
                                                        ""negate"": true,
                                                        ""VehicleCharacteristics"": {
                                                            ""vehicleType"": ""bus""
                                                        }
                                                    },
                                                    {
                                                        ""operator"": ""and"",
                                                        ""Condition"": [
                                                            {
                                                                ""negate"": ""and"",
                                                                ""VehicleCharacteristics"": {
                                                                    ""vehicleType"": ""taxi""
                                                                }
                                                            },
                                                            {
                                                                ""negate"": false,
                                                                ""VehicleCharacteristics"": {
                                                                    ""vehicleUsage"": ""access""
                                                                }
                                                            }
                                                        ]
                                                    }
                                                ]
                                            }
                                        ],
                                        ""Condition"": {
                                            ""TimeValidity"": {
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
            ""Source"": {
                ""Provision"": [
                    {
                        ""Regulation"": [
                            {
                                ""Condition"": {
                                    ""TimeValidity"": {

                                    },
                                    ""RoadCondition"": {

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
            ""Source"": {
                ""Provision"": [
                    {
                        ""Regulation"": [
                            {
                                ""Condition"": {
                                    ""TimeValidity"": {

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
            ""Source"": {
                ""Provision"": [
                    {
                        ""Regulation"": [
                            {
                                ""Condition"": {
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