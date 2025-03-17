namespace Dft.DTRO.Tests.ServicesTests.Validations;

public class RegulatedPlaceValidationServiceTests
{
    private readonly IRegulatedPlaceValidationService _sut = new RegulatedPlaceValidationService();

    [Theory]
    [InlineData("true", "3.4.0", 0)]
    [InlineData("false", "3.4.0", 0)]
    public void ValidatedRegulatedPlaceBooleanProperties(string boolean, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""assignment"": {boolean},
                                ""busRoute"": {boolean},
                                ""bywayType"": ""footpath"",
                                ""concession"": {boolean},
                                ""description"": ""some free text"",
                                ""tramcar"": {boolean},
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
    [InlineData("footpath", "3.4.0", 0)]
    [InlineData("road", "3.4.0", 0)]
    [InlineData("bridleway", "3.4.0", 0)]
    [InlineData("cycleTrack", "3.4.0", 0)]
    [InlineData("restrictedByway", "3.4.0", 0)]
    [InlineData("bywayOpenToAllTraffic", "3.4.0", 0)]
    [InlineData("wrong", "3.4.0", 1)]
    public void ValidatedRegulatedPlaceBywayType(string bywayType, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""assignment"": ""false"",
                                ""busRoute"": ""false"",
                                ""bywayType"": ""{bywayType}"",
                                ""concession"": ""false"",
                                ""description"": ""some free text"",
                                ""tramcar"": ""false"",
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
    [InlineData("some free text", "3.3.0",0)]
    [InlineData("","3.3.0", 1)]
    [InlineData(null,"3.3.0", 1)]
    [InlineData("some free text", "3.4.0",0)]
    [InlineData("","3.4.0", 1)]
    [InlineData(null,"3.4.0", 1)]
    public void ValidateRegulatedPlacesDescription(string description, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""assignment"": ""true"",
                                ""busRoute"": ""true"",
                                ""bywayType"": ""footpath"",
                                ""concession"": ""true"",
                                ""description"": ""{description}"",
                                ""tramcar"": ""true"",
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
    [InlineData("regulationLocation","3.3.0", 0)]
    [InlineData("diversionRoute","3.3.0", 0)]
    [InlineData("unknown", "3.3.0",1)]
    [InlineData("", "3.3.0",1)]
    [InlineData("regulationLocation","3.4.0", 0)]
    [InlineData("diversionRoute","3.4.0", 0)]
    [InlineData("unknown", "3.4.0",1)]
    [InlineData("", "3.4.0",1)]
    public void ValidateRegulatedPlacesType(string type,string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""assignment"": ""true"",
                                ""busRoute"": ""true"",
                                ""bywayType"": ""footpath"",
                                ""concession"": ""true"",
                                ""description"": ""free text"",
                                ""tramcar"": ""true"",
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

    [Theory]
    [InlineData(new[] { "regulationLocation", "diversionRoute" },"3.3.0", 0)]
    [InlineData(new[] { "diversionRoute", "unknown" }, "3.3.0",1)]
    [InlineData(new[] { "", "unknown" }, "3.3.0", 1)]
    [InlineData(new[] { "regulationLocation", "diversionRoute" },"3.4.0", 0)]
    [InlineData(new[] { "diversionRoute", "unknown" }, "3.4.0",1)]
    [InlineData(new[] { "", "unknown" }, "3.4.0", 1)]
    public void ValidateMultipleRegulatedPlacesTypes(string[] types, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""RegulatedPlace"": [
                            {{
                                ""assignment"": ""true"",
                                ""busRoute"": ""true"",
                                ""bywayType"": ""footpath"",
                                ""concession"": ""true"",
                                ""description"": ""free text"",
                                ""tramcar"": ""true"",
                                ""type"": ""{types.ElementAt(0)}""
                            }},
                            {{
                                ""assignment"": ""true"",
                                ""busRoute"": ""true"",
                                ""bywayType"": ""footpath"",
                                ""concession"": ""true"",
                                ""description"": ""free text"",
                                ""tramcar"": ""true"",
                                ""type"": ""{types.ElementAt(1)}""                                
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