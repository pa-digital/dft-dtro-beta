namespace Dft.DTRO.Tests.ServicesTests.Validations;

public class PermitConditionValidationServiceTests
{
    private readonly IPermitConditionValidationService _sut = new PermitConditionValidationService();

    [Theory]
    [InlineData("doctor", "3.4.0", 0)]
    [InlineData("business", "3.4.0", 0)]
    [InlineData("resident", "3.4.0", 0)]
    [InlineData("other", "3.4.0", 0)]
    public void ValidatePermitTypeExtensionWhenConditionSet(string permitType, string version, int errorCount)
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
                                                ""PermitCondition"": {{
                                                    ""type"": ""{permitType}"",
                                                    ""PermitTypeExtension"": {{
                                                        ""definition"": ""Car Club Permit"",
                                                        ""enumeratedList"": ""permitType"",
                                                        ""value"": ""carClub""
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
    [InlineData("doctor", "3.4.0", 0)]
    [InlineData("business", "3.4.0", 0)]
    [InlineData("resident", "3.4.0", 0)]
    [InlineData("other", "3.4.0", 0)]
    public void ValidatePermitTypeWhenCondition(string permitType, string version, int errorCount)
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
                                        ""PermitCondition"": {{
                                            ""type"": ""{permitType}"",
                                            ""PermitTypeExtension"": {{
                                                ""definition"": ""Car Club Permit"",
                                                ""enumeratedList"": ""permitType"",
                                                ""value"": ""carClub""
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