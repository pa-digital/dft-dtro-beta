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
            ""Source"": {
                ""Provision"": [
                    {
                        ""Regulation"": [
                            {
                                ""ConditionSet"": [
                                    {
                                        ""operator"": ""and"",
                                        ""conditions"": [
                                            {
                                                ""negate"": false,
                                                ""VehicleCharacteristics"": {
                                                    ""MaximumHeightCharacteristic"": {
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
            ""Source"": {
                ""Provision"": [
                    {
                        ""Regulation"": [
                            {
                                ""ConditionSet"": [
                                    {
                                        ""conditions"": [
                                            {
                                                ""negate"": false,
                                                ""VehicleCharacteristics"": {
                                                    ""MaximumHeightCharacteristic"": {
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
        Assert.Equal("Source -> Provision -> Regulation -> ConditionSet -> operator", actual[0].Path);
        Assert.Equal("One of 'and, or, xOr' operators must be present", actual[0].Rule);
    }

    [Fact]
    public void ValidateConditionReturnsErrorWhenConditionSetItPresentAndOperatorIsWrong()
    {
        SchemaVersion schemaVersion = new("3.3.0");

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
                                        ""conditions"": [
                                            {
                                                ""negate"": false,
                                                ""VehicleCharacteristics"": {
                                                    ""MaximumHeightCharacteristic"": {
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
        Assert.Equal("Source -> Provision -> Regulation -> ConditionSet -> operator", actual[0].Path);
        Assert.Equal("One of 'and, or, xOr' operators must be present", actual[0].Rule);
    }

    [Fact]
    public void ValidateConditionReturnsErrorWhenConditionSetItPresentButHasMultipleWrongConditions()
    {
        SchemaVersion schemaVersion = new("3.3.0");

        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""Provision"": [
                    {
                        ""Regulation"": [
                            {
                                ""ConditionSet"": [
                                    {
                                        ""operator"": ""or"",
                                        ""conditions"": [
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
                                                ""UnknownCharacteristics"": {
                                                    ""MaximumHeightCharacteristic"": {
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
        Assert.Equal("Source -> Provision -> Regulation -> ConditionSet -> conditions", actual[0].Path);
        Assert.Equal("One or more types of 'RoadCondition, OccupantCondition, DriverCondition, AccessCondition, TimeValidity, NonVehicularRoadUserCondition, PermitCondition, VehicleCharacteristics, ConditionSet, conditions, RateTable' conditions must be present", actual[0].Rule);
    }

    [Fact]
    public void ValidateConditionReturnsNoErrorsWhenOneConditionIsPresent()
    {
        SchemaVersion schemaVersion = new("3.3.0");

        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""Provision"": [
                    {
                        ""Regulation"": [
                            {
                                ""Condition"": [
                                    {
                                        ""negate"": false,
                                        ""AccessCondition"": {
                                        
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
            ""Source"": {
                ""Provision"": [
                    {
                        ""Regulation"": [
                            {
                                ""Condition"": [
                                    {
                                        ""negate"": false,
                                        ""OtherCondition"": {
                                        
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
        Assert.Equal("Source -> Provision -> Regulation -> Condition", actual[0].Path);
        Assert.Equal("One of 'RoadCondition, OccupantCondition, DriverCondition, AccessCondition, TimeValidity, NonVehicularRoadUserCondition, PermitCondition, VehicleCharacteristics, ConditionSet, conditions, RateTable' condition must be present", actual[0].Rule);
    }

    [Fact]
    public void ValidateConditionReturnsErrorsWhenOneConditionIsPresentButNegatePropertyIsWrong()
    {
        SchemaVersion schemaVersion = new("3.3.0");

        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""Provision"": [
                    {
                        ""Regulation"": [
                            {
                                ""Condition"": [
                                    {
                                        ""negate"": ""wrong"",
                                        ""NonVehicularRoadUserCondition"": {
                                        
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
        Assert.Equal("Source -> Provision -> Regulation -> ConditionSet -> conditions -> negate", actual[0].Path);
        Assert.Equal("Negate property must be boolean, 'true' or 'false'", actual[0].Rule);
    }

    [Fact]
    public void ValidateConditionReturnsNoErrorsWhenNestedConditionSetItPresent()
    {
        SchemaVersion schemaVersion = new("3.3.0");

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
                                                ""conditions"": [
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
                                                        ""VehicleCharacteristic"": {
                                                            ""vehicleType"": ""bus""
                                                        }
                                                    },
                                                    {
                                                        ""ConditionSet"": [
                                                            {
                                                                ""operator"": ""and"",
                                                                ""conditions"": [
                                                                    {
                                                                        ""negate"": false,
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
                                                ]
                                            }
                                        ],
                                        ""Condition"": [
                                            {
                                                ""negate"": false,
                                                ""TimeValidity"": {
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
            ""Source"": {
                ""Provision"": [
                    {
                        ""Regulation"": [
                            {
                                ""ConditionSet"": [
                                    {
                                        ""operator"": ""and"",
                                        ""conditions"": [
                                            {
                                                ""negate"": false,
                                                ""TimeValidity"": {
                                                    ""start"": ""2024-08-22T08:00:00"",
                                                    ""end"": ""2024-08-22T20:00:00""
                                                }
                                            },
                                            {
                                                ""negate"": ""some"",
                                                ""VehicleCharacteristics"": {
                                                    ""MaximumHeightCharacteristic"": {
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
        Assert.Equal("Source -> Provision -> Regulation -> ConditionSet -> conditions -> negate", actual[0].Path);
        Assert.Equal("Negate property must be boolean, 'true' or 'false'", actual[0].Rule);
    }
}