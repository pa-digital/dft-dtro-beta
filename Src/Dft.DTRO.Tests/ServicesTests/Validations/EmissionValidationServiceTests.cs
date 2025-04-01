namespace Dft.DTRO.Tests.ServicesTests.Validations;

public class EmissionValidationServiceTests
{
    private readonly IEmissionValidationService _sut = new EmissionValidationService();

    [Theory]
    [InlineData("euro5", "3.4.0", 0)]
    [InlineData("euro5a", "3.4.0", 0)]
    [InlineData("euro5b", "3.4.0", 0)]
    [InlineData("euro6", "3.4.0", 0)]
    [InlineData("euro6a", "3.4.0", 0)]
    [InlineData("euro6b", "3.4.0", 0)]
    [InlineData("euro6c", "3.4.0", 0)]
    [InlineData("euroV", "3.4.0", 0)]
    [InlineData("euroVI", "3.4.0", 0)]
    [InlineData("other", "3.4.0", 0)]
    public void ValidateEmissionExtensionWhenConditionSet(string emissionClassificationEuro, string version, int errorCount)
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
                                        ""conditions"": [
                                            {{
                                                ""VehicleCharacteristics"": {{
                                                    ""Emissions"": {{
                                                        ""emissionClassificationEuro"": ""{emissionClassificationEuro}"",
                                                        ""EmissionClassificationEuroTypeExtension"": {{
                                                            ""definition"": ""some definition"",
                                                            ""enumeratedList"": ""some enumerated list"",
                                                            ""value"": ""some value""
                                                        }}
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
    [InlineData("euro5", "3.4.0", 0)]
    [InlineData("euro5a", "3.4.0", 0)]
    [InlineData("euro5b", "3.4.0", 0)]
    [InlineData("euro6", "3.4.0", 0)]
    [InlineData("euro6a", "3.4.0", 0)]
    [InlineData("euro6b", "3.4.0", 0)]
    [InlineData("euro6c", "3.4.0", 0)]
    [InlineData("euroV", "3.4.0", 0)]
    [InlineData("euroVI", "3.4.0", 0)]
    [InlineData("other", "3.4.0", 0)]
    public void ValidateVehicleUsageTypeWhenCondition(string emissionClassificationEuro, string version, int errorCount)
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
                                            ""Emissions"": {{
                                                ""emissionClassificationEuro"": ""{emissionClassificationEuro}"",
                                                ""EmissionClassificationEuroTypeExtension"": {{
                                                    ""definition"": ""some definition"",
                                                    ""enumeratedList"": ""some enumerated list"",
                                                    ""value"": ""some value""
                                                }}
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