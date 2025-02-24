namespace Dft.DTRO.Tests.ServicesTests.Validations;

[ExcludeFromCodeCoverage]
public class RegulatedPlaceValidationServiceTests
{
    private readonly IRegulatedPlaceValidationService _sut = new RegulatedPlaceValidation();

    [Theory]
    [InlineData("regulationLocation", 0)]
    [InlineData("unknown", 1)]
    [InlineData("", 1)]
    public void ValidateRegulatedPlacesType(string type, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""source"": {{
                ""provision"": [
                    {{
                        ""regulatedPlace"": [
                            {{
                                ""description"": ""some free text"",
                                ""type"": ""{type}""
                            }}
                        ]
                    }}
                ]

            }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.ValidateRegulatedPlacesType(dtroSubmit, new SchemaVersion("3.3.0"));
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData(new[] { "regulationLocation", "diversionRoute" }, 0)]
    [InlineData(new[] { "diversionRoute", "unknown" }, 1)]
    [InlineData(new[] { "", "unknown" }, 1)]
    public void ValidateMultipleRegulatedPlacesTypes(string[] types, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""source"": {{
                ""provision"": [
                    {{
                        ""regulatedPlace"": [
                            {{
                                ""description"": ""some free text"",
                                ""type"": ""{types.ElementAt(0)}""
                            }},
                            {{
                                ""description"": ""some free text"",
                                ""type"": ""{types.ElementAt(1)}""                                
                            }}
                        ]
                    }}
                ]

            }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.ValidateRegulatedPlacesType(dtroSubmit, new SchemaVersion("3.3.0"));
        Assert.Equal(errorCount, actual.Count);
    }
}
