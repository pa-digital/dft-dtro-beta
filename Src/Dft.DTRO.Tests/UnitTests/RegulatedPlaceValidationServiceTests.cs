namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class RegulatedPlaceValidationServiceTests
{
    private readonly IRegulatedPlaceValidationService _sut = new RegulatedPlaceValidationService();

    [Theory]
    [InlineData("some free text", "3.2.4", "0")]
    [InlineData("some free text", "3.3.0", "0")]
    [InlineData("", "3.3.0", "1")]
    public void ValidateRegulatedPlacesDescription(string description, string version, string errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""description"": ""{description}"",
                                ""type"": ""regulationLocation""
                            }}
                        ]
                    }}
                ]

            }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount.AsInt(), actual.Count);
    }

    [Theory]
    [InlineData("regulationLocation", "3.2.4", "0")]
    [InlineData("diversionRoute", "3.2.4", "0")]
    [InlineData("regulationLocation", "3.3.0", "0")]
    [InlineData("diversionRoute", "3.3.0", "0")]
    [InlineData("unknown", "3.3.0", "1")]
    [InlineData("", "3.3.0", "1")]
    public void ValidateRegulatedPlacesType(string type, string version, string errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""description"": ""some free text"",
                                ""type"": ""{type}""
                            }}
                        ]
                    }}
                ]

            }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount.AsInt(), actual.Count);
    }
}
