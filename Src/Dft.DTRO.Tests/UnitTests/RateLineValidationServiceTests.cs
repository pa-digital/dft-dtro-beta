namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class RateLineValidationServiceTests
{
    private readonly IRateLineValidationService _sut = new RateLineValidationService();

    [Theory]
    [InlineData("free text", 0)]
    [InlineData("", 1)]
    [InlineData(null, 1)]
    public void ValidateRateLineDuration(string description, int errorCount)
    {
        SchemaVersion schemaVersion = new("3.3.0");

        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""Regulation"": [
                            {{
                                ""Condition"": [
                                    {{
                                        ""RateTable"" : {{
                                            ""RateLineCollection"": [
                                                {{
                                                    ""RateLine"": [
                                                        {{
                                                            ""description"": ""{description}"",
                                                            ""durationEnd"": ""00:00:59"",
                                                            ""durationStart"": ""00:00:01"",
                                                            ""incrementPeriod"": 58,
                                                            ""maxValue"": 7.20,
                                                            ""minValue"": 1.00,
                                                            ""sequence"": 1,
                                                            ""type"": ""flatRateTier"",
                                                            ""usageCondition"": ""fixedDuration"",
                                                            ""value"": 1.00
                                                        }}
                                                    ]
                                                }}
                                            ]
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