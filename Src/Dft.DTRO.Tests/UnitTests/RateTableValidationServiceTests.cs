namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class RateTableValidationServiceTests
{
    private readonly IRateTableValidationService _sut = new RateTableValidationService();

    [Theory]
    [InlineData("https://loremipsum.co.uk", 0)]
    [InlineData("http://loremipsum.co.uk", 0)]
    [InlineData("ftp://loremipsum.co.uk", 1)]
    [InlineData("not-a-uri", 1)]
    public void ValidateRateTableAdditionalInformation(string additionalInformation, int errorCount)
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
                                            ""additionalInformation"": ""{additionalInformation}"",
                                            ""type"": ""daily""
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

    [Theory]
    [InlineData("daily", 0)]
    [InlineData("hourly", 0)]
    [InlineData("unknown", 1)]
    public void ValidateRateTableRateType(string rateType, int errorCount)
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
                                            ""additionalInformation"": ""https://loremipsum.co.uk"",
                                            ""type"": ""{rateType}""
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

    //[Theory]
    //[InlineData("EUR", 0)]
    //[InlineData("GBP", 0)]
    //[InlineData("USD", 1)]
    //public void ValidateRateLineCollectionApplicableCurrency(string currencyType, int errorCount)
    //{
    //    SchemaVersion schemaVersion = new("3.3.0");

    //    var dtroSubmit = Utils.PrepareDtro($@"
    //    {{
    //        ""Source"": {{
    //            ""Provision"": [
    //                {{
    //                    ""Regulation"": [
    //                        {{
    //                            ""Condition"": [
    //                                {{
    //                                    ""RateTable"" : {{
    //                                        ""RateLineCollection"": [
    //                                            {{
    //                                                ""applicableCurrency"": ""{currencyType}""
    //                                            }}
    //                                        ]
    //                                    }}
    //                                }}
    //                            ]
    //                        }}
    //                    ]
    //                }}
    //            ]
    //        }}
    //    }}", schemaVersion);

    //    var actual = _sut.Validate(dtroSubmit);
    //    Assert.Equal(errorCount, actual.Count);
    //}
}