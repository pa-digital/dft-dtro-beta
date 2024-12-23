namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class RegulatedPlaceValidationServiceTests
{
    private readonly IRegulatedPlaceValidationService _sut = new RegulatedPlaceValidationService();

    [Theory]
    [InlineData("3.2.4", "some free text", 0)]
    [InlineData("3.2.4", "", 1)]
    [InlineData("3.3.0", "some free text", 0)]
    [InlineData("3.3.0", "", 1)]
    public void ValidateRegulatedPlacesDescription(string version, string description, int errorCount)
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
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.2.4", "unknown", 1)]
    [InlineData("3.2.4", "", 1)]
    [InlineData("3.3.0", "regulationLocation", 0)]
    [InlineData("3.3.0", "diversionRoute", 0)]
    [InlineData("3.3.0", "regulationLocation", 0)]
    [InlineData("3.3.0", "diversionRoute", 0)]
    [InlineData("3.3.0", "unknown", 1)]
    [InlineData("3.3.0", "", 1)]
    public void ValidateRegulatedPlacesType(string version, string type, int errorCount)
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
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }
}
