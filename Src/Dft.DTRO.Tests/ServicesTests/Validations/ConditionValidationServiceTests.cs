namespace Dft.DTRO.Tests.ServicesTests.Validations;

[ExcludeFromCodeCoverage]
public class ConditionValidationServiceTests
{
    private readonly IConditionValidationService _sut = new ConditionValidation();

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
                                        ""condition"": [
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
        Assert.Equal("Source -> Provision -> Regulation -> ConditionSet -> operator", actual[0].Path);
        Assert.Equal("One or more of 'and, or, xOr' operators must be present", actual[0].Rule);
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
                                        ""condition"": [
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
        Assert.Equal("Source -> Provision -> Regulation -> ConditionSet -> operator", actual[0].Path);
        Assert.Equal("One or more of 'and, or, xOr' operators must be present", actual[0].Rule);
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
                                                        ""vehicleCharacteristics"": {
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
}