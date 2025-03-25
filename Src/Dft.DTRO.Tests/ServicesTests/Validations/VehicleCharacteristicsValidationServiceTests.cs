namespace Dft.DTRO.Tests.ServicesTests.Validations;

public class VehicleCharacteristicsValidationServiceTests
{
    private readonly IVehicleCharacteristicsValidationService _sut = new VehicleCharacteristicsValidationService();

    [Theory]
    [InlineData("access", "3.4.0", 0)]
    [InlineData("accessToOffStreetPremises", "3.4.0", 0)]
    [InlineData("authorisedVehicle", "3.4.0", 0)]
    [InlineData("guidedBuses", "3.4.0", 0)]
    [InlineData("highwayAuthorityPurpose", "3.4.0", 0)]
    [InlineData("localBuses", "3.4.0", 0)]
    [InlineData("localRegisteredPrivateHireVehicle", "3.4.0", 0)]
    [InlineData("privateHireVehicle", "3.4.0", 0)]
    [InlineData("busOperationPurpose", "3.4.0", 0)]
    [InlineData("statutoryUndertakerPurpose", "3.4.0", 0)]
    [InlineData("military", "3.4.0", 0)]
    [InlineData("other", "3.4.0", 0)]
    public void ValidateVehicleUsageTypeExtensionWhenConditionSet(string vehicleUsageType, string version, int errorCount)
    {
        SchemaVersion schemaVersion = new(version);

        DtroSubmit dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""ConditionSet"": [
                                    {{
                                        ""operator"": ""and"",
                                        ""conditions"": [
                                            {{
                                                ""negate"": false,
                                                ""VehicleCharacteristics"": {{
                                                    ""vehicleUsage"": ""{vehicleUsageType}"",
                                                    ""VehicleUsageTypeExtension"": {{
                                                        ""definition"": ""Police Vehicle"",
                                                        ""enumeratedList"": ""vehicleUsageType"",
                                                        ""value"": ""police""
                                                    }}
                                                }}
                                            }}
                                        ]
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
    [InlineData("access", "3.4.0", 1)]
    [InlineData("accessToOffStreetPremises", "3.4.0", 1)]
    [InlineData("authorisedVehicle", "3.4.0", 1)]
    [InlineData("guidedBuses", "3.4.0", 1)]
    [InlineData("highwayAuthorityPurpose", "3.4.0", 1)]
    [InlineData("localBuses", "3.4.0", 1)]
    [InlineData("localRegisteredPrivateHireVehicle", "3.4.0", 1)]
    [InlineData("privateHireVehicle", "3.4.0", 1)]
    [InlineData("busOperationPurpose", "3.4.0", 1)]
    [InlineData("statutoryUndertakerPurpose", "3.4.0", 1)]
    [InlineData("military", "3.4.0", 1)]
    [InlineData("other", "3.4.0", 0)]
    public void ValidateVehicleUsageTypeWhenCondition(string vehicleUsageType, string version, int errorCount)
    {
        SchemaVersion schemaVersion = new(version);

        DtroSubmit dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""Condition"": [
                                    {{
                                        ""VehicleCharacteristics"": {{
                                            ""vehicleUsage"": ""{vehicleUsageType}"",
                                            ""VehicleUsageTypeExtension"": {{
                                                ""definition"": ""Police Vehicle"",
                                                ""enumeratedList"": ""vehicleUsageType"",
                                                ""value"": ""police""
                                            }}
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