namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class RegulationValidationTests
{
    private readonly IRegulationValidation _sut = new RegulationValidation();

    [Fact]
    public void ValidateRegulationWhenIsDynamicIsTrueReturnsNoErrors()
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Source"": {{
            ""Provision"": [
              {{
                ""Regulation"": [
                  {{
                    ""isDynamic"": true,
                    ""timeZone"": ""Europe/London""
                  }}
                ]
              }}
            ]
          }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.ValidateRegulation(dtroSubmit, "3.3.0");
        Assert.Empty(actual);
    }

    [Fact]
    public void ValidateRegulationWhenIsDynamicIsFalseReturnsNoErrors()
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Source"": {{
            ""Provision"": [
              {{
                ""Regulation"": [
                  {{
                    ""isDynamic"": false,
                    ""timeZone"": ""Europe/London""
                  }}
                ]
              }}
            ]
          }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.ValidateRegulation(dtroSubmit, "3.3.0");
        Assert.Empty(actual);
    }

    [Theory]
    [InlineData("Europe/London", 0)]
    [InlineData("", 1)]
    [InlineData(null, 1)]
    public void ValidateRegulationTimeZone(string timeZone, int errorCount)
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Source"": {{
            ""Provision"": [
              {{
                ""Regulation"": [
                  {{
                    ""isDynamic"": true,
                    ""timeZone"": ""{timeZone}""
                  }}
                ]
              }}
            ]
          }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.ValidateRegulation(dtroSubmit, "3.3.0");
        Assert.Equal(errorCount, actual.Count);
    }



    [Theory]
    [InlineData(10, 0)]
    [InlineData(100, 0)]
    [InlineData(0, 1)]
    public void ValidateSpeedLimitValueBasedSpeedValue(int mphValue, int errorCount)
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Source"": {{
            ""Provision"": [
              {{
                ""Regulation"": [
                  {{
                    ""isDynamic"": false,
                    ""timeZone"": ""Europe/London"",
                    ""SpeedLimitValueBased"": {{
                        ""mphValue"": {mphValue},
                        ""type"": ""maximumSpeedLimit""
                    }}
                  }}
                ]
              }}
            ]
          }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.ValidateRegulation(dtroSubmit, "3.3.0");
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("maximumSpeedLimit", 0)]
    [InlineData("minimumSpeedLimit", 0)]
    [InlineData("nationalSpeedLimitWellLitStreetDefault", 0)]
    [InlineData("unknown", 1)]
    public void ValidateSpeedLimitValueBasedSpeedType(string type, int errorCount)
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Source"": {{
            ""Provision"": [
              {{
                ""Regulation"": [
                  {{
                    ""isDynamic"": false,
                    ""timeZone"": ""Europe/London"",
                    ""SpeedLimitValueBased"": {{
                        ""mphValue"": 100,
                        ""type"": ""{type}""
                    }}
                  }}
                ]
              }}
            ]
          }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.ValidateRegulation(dtroSubmit, "3.3.0");
        Assert.Equal(errorCount, actual.Count);
    }
}