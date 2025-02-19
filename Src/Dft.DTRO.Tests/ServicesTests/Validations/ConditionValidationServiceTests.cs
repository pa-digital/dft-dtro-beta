namespace Dft.DTRO.Tests.ServicesTests.Validations;

[ExcludeFromCodeCoverage]
public class ConditionValidationServiceTests
{
    private readonly IConditionValidationService _sut = new ConditionValidationService();

    [Fact]
    public void ValidateConditionReturnsNoErrorsWhenConditionSetIsPresent()
    {
        SchemaVersion schemaVersion = new("3.3.0");

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
                                        ""conditions"": [
                                            {
                                                ""negate"": false,
                                                ""vehicleCharacteristics"": {
                                                    ""maximumHeightCharacteristic"": {
                                                        ""vehicleHeight"": 2.5
                                                    }
                                                }
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        }", schemaVersion);

        var actual = _sut.ValidateCondition(dtroSubmit, schemaVersion);
        Assert.Equal(0, actual.Count);
    }

    [Fact]
    public void ValidateConditionReturnsErrorWhenConditionSetItPresentAndOperatorIsNotPresent()
    {
        SchemaVersion schemaVersion = new("3.3.0");

        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {
                        ""regulation"": [
                            {
                                ""conditionSet"": [
                                    {
                                        ""conditions"": [
                                            {
                                                ""negate"": false,
                                                ""vehicleCharacteristics"": {
                                                    ""maximumHeightCharacteristic"": {
                                                        ""vehicleHeight"": 2.5
                                                    }
                                                }
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        }", schemaVersion);

        var actual = _sut.ValidateCondition(dtroSubmit, schemaVersion);
        Assert.Equal(1, actual.Count);
        Assert.Equal("Operator", actual[0].Name);
        Assert.Equal("Operator is not present or incorrect", actual[0].Message);
        Assert.Equal("source -> provision -> regulation -> conditionSet -> operator", actual[0].Path);
        Assert.Equal("One of 'and, or, xOr' operators must be present", actual[0].Rule);
    }

    [Fact]
    public void ValidateConditionReturnsErrorWhenConditionSetItPresentAndOperatorIsWrong()
    {
        SchemaVersion schemaVersion = new("3.3.0");

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
                                        ""conditions"": [
                                            {
                                                ""negate"": false,
                                                ""vehicleCharacteristics"": {
                                                    ""maximumHeightCharacteristic"": {
                                                        ""vehicleHeight"": 2.5
                                                    }
                                                }
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        }", schemaVersion);

        var actual = _sut.ValidateCondition(dtroSubmit, schemaVersion);
        Assert.Equal(1, actual.Count);
        Assert.Equal("Operator", actual[0].Name);
        Assert.Equal("Operator is not present or incorrect", actual[0].Message);
        Assert.Equal("source -> provision -> regulation -> conditionSet -> operator", actual[0].Path);
        Assert.Equal("One of 'and, or, xOr' operators must be present", actual[0].Rule);
    }

    [Fact]
    public void ValidateConditionReturnsErrorWhenConditionSetItPresentButHasMultipleWrongConditions()
    {
        SchemaVersion schemaVersion = new("3.3.0");

        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {
                        ""regulation"": [
                            {
                                ""conditionSet"": [
                                    {
                                        ""operator"": ""or"",
                                        ""conditions"": [
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
                                                ""UnknownCharacteristics"": {
                                                    ""maximumHeightCharacteristic"": {
                                                        ""vehicleHeight"": 2.5
                                                    }
                                                }
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        }", schemaVersion);

        var actual = _sut.ValidateCondition(dtroSubmit, schemaVersion);
        Assert.Equal(1, actual.Count);
        Assert.Equal("Invalid conditions", actual[0].Name);
        Assert.Equal("One or more conditions are invalid", actual[0].Message);
        Assert.Equal("source -> provision -> regulation -> conditionSet -> conditions", actual[0].Path);
        Assert.Equal("One or more types of 'roadCondition, occupantCondition, driverCondition, accessCondition, timeValidity, nonVehicularRoadUserCondition, permitCondition, vehicleCharacteristics, conditionSet, conditions, rateTable' conditions must be present", actual[0].Rule);
    }

    [Fact]
    public void ValidateConditionReturnsNoErrorsWhenOneConditionIsPresent()
    {
        SchemaVersion schemaVersion = new("3.3.0");

        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {
                        ""regulation"": [
                            {
                                ""condition"": [
                                    {
                                        ""negate"": false,
                                        ""accessCondition"": {
                                        
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
        Assert.Equal(0, actual.Count);
    }

    [Fact]
    public void ValidateConditionReturnsErrorsWhenOneConditionIsPresentButIsWrong()
    {
        SchemaVersion schemaVersion = new("3.3.0");

        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {
                        ""regulation"": [
                            {
                                ""condition"": [
                                    {
                                        ""negate"": false,
                                        ""otherCondition"": {
                                        
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
        Assert.Equal(1, actual.Count);
        Assert.Equal("Condition", actual[0].Name);
        Assert.Equal("Invalid condition", actual[0].Message);
        Assert.Equal("source -> provision -> regulation -> condition", actual[0].Path);
        Assert.Equal("One of 'roadCondition, occupantCondition, driverCondition, accessCondition, timeValidity, nonVehicularRoadUserCondition, permitCondition, vehicleCharacteristics, conditionSet, conditions, rateTable' condition must be present", actual[0].Rule);
    }

    [Fact]
    public void ValidateConditionReturnsErrorsWhenOneConditionIsPresentButNegatePropertyIsWrong()
    {
        SchemaVersion schemaVersion = new("3.3.0");

        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {
                        ""regulation"": [
                            {
                                ""condition"": [
                                    {
                                        ""negate"": ""wrong"",
                                        ""nonVehicularRoadUserCondition"": {
                                        
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

        Assert.Equal(1, actual.Count);
        Assert.Equal("Negate", actual[0].Name);
        Assert.Equal("One or more 'negate' values are incorrect", actual[0].Message);
        Assert.Equal("source -> provision -> regulation -> conditionSet -> conditions -> negate", actual[0].Path);
        Assert.Equal("Negate property must be boolean, 'true' or 'false'", actual[0].Rule);
    }

    [Fact]
    public void ValidateConditionReturnsNoErrorsWhenNestedConditionSetItPresent()
    {
        SchemaVersion schemaVersion = new("3.3.0");

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
                                                ""conditions"": [
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
                                                        ""vehicleCharacteristic"": {
                                                            ""vehicleType"": ""bus""
                                                        }
                                                    },
                                                    {
                                                        ""conditionSet"": [
                                                            {
                                                                ""operator"": ""and"",
                                                                ""conditions"": [
                                                                    {
                                                                        ""negate"": false,
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
                                                ]
                                            }
                                        ],
                                        ""condition"": [
                                            {
                                                ""negate"": false,
                                                ""timeValidity"": {
                                                    ""start"": ""2024-08-22T08:00:00"",
                                                    ""end"": ""202408-22T20:00:00""
                                                }
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        }", schemaVersion);

        var actual = _sut.ValidateCondition(dtroSubmit, schemaVersion);
        Assert.Equal(0, actual.Count);
    }

    [Fact]
    public void ValidateConditionReturnsErrorWhenNegateValueIsIncorrect()
    {
        SchemaVersion schemaVersion = new("3.3.0");

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
                                        ""conditions"": [
                                            {
                                                ""negate"": false,
                                                ""timeValidity"": {
                                                    ""start"": ""2024-08-22T08:00:00"",
                                                    ""end"": ""2024-08-22T20:00:00""
                                                }
                                            },
                                            {
                                                ""negate"": ""some"",
                                                ""vehicleCharacteristics"": {
                                                    ""maximumHeightCharacteristic"": {
                                                        ""vehicleHeight"": 2.5
                                                    }
                                                }
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        }", schemaVersion);

        var actual = _sut.ValidateCondition(dtroSubmit, schemaVersion);

        Assert.Equal(1, actual.Count);
        Assert.Equal("Negate", actual[0].Name);
        Assert.Equal("One or more 'negate' values are incorrect", actual[0].Message);
        Assert.Equal("source -> provision -> regulation -> conditionSet -> conditions -> negate", actual[0].Path);
        Assert.Equal("Negate property must be boolean, 'true' or 'false'", actual[0].Rule);
    }
}