namespace Dft.DTRO.Tests.ServicesTests.Validations;

[ExcludeFromCodeCoverage]
public class ProvisionValidationServiceTests
{
    private readonly IProvisionValidationService _sut = new ProvisionValidationService();

    [Theory]
    [InlineData("new", 0)]
    [InlineData("partialAmendment", 0)]
    [InlineData("fullAmendment", 0)]
    [InlineData("partialRevoke", 0)]
    [InlineData("fullRevoke", 0)]
    [InlineData("noChange", 0)]
    [InlineData("errorFix", 0)]
    [InlineData("something", 1)]
    public void ValidateProvisionActionTypeForSchema330(string actionType, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""{actionType}"",
                        ""orderReportingPoint"": ""ttroTtmoRevocation"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""006A10CE-C4B3-4713-BAA0-35D66450893E""
                    }}
                ]

            }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("new", 0)]
    [InlineData("partialAmendment", 0)]
    [InlineData("fullAmendment", 0)]
    [InlineData("partialRevoke", 0)]
    [InlineData("fullRevoke", 0)]
    [InlineData("noChange", 0)]
    [InlineData("errorFix", 0)]
    [InlineData("something", 1)]
    public void ValidateProvisionActionTypeForSchema324(string actionType, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""provision"": [
                    {{
                        ""actionType"": ""{actionType}"",
                        ""orderReportingPoint"": ""ttroTtmoRevocation"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""006A10CE-C4B3-4713-BAA0-35D66450893E""
                    }}
                ]

            }}
        }}", new SchemaVersion("3.2.4"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("experimentalAmendment", 0)]
    [InlineData("experimentalMakingPermanent", 0)]
    [InlineData("experimentalNoticeOfMaking", 0)]
    [InlineData("experimentalRevocation", 0)]
    [InlineData("permanentAmendment", 0)]
    [InlineData("permanentNoticeOfMaking", 0)]
    [InlineData("permanentNoticeOfProposal", 0)]
    [InlineData("permanentRevocation", 0)]
    [InlineData("specialEventOrderNoticeOfMaking", 0)]
    [InlineData("ttroTtmoByNotice", 0)]
    [InlineData("ttroTtmoExtension", 0)]
    [InlineData("ttroTtmoNoticeAfterMaking", 0)]
    [InlineData("ttroTtmoNoticeOfIntention", 0)]
    [InlineData("ttroTtmoRevocation", 0)]
    [InlineData("variationByNotice", 0)]
    [InlineData("troOnRoadActiveStatus", 0)]
    [InlineData("something", 1)]
    public void ValidateProvisionOrderReportingPointTypeForSchema330(string orderReportingPointType, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""orderReportingPoint"": ""{orderReportingPointType}"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""006A10CE-C4B3-4713-BAA0-35D66450893E""
                    }}
                ]

            }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("experimentalAmendment", 0)]
    [InlineData("experimentalMakingPermanent", 0)]
    [InlineData("experimentalNoticeOfMaking", 0)]
    [InlineData("experimentalRevocation", 0)]
    [InlineData("permanentAmendment", 0)]
    [InlineData("permanentNoticeOfMaking", 0)]
    [InlineData("permanentNoticeOfProposal", 0)]
    [InlineData("permanentRevocation", 0)]
    [InlineData("specialEventOrderNoticeOfMaking", 0)]
    [InlineData("ttroTtmoByNotice", 0)]
    [InlineData("ttroTtmoExtension", 0)]
    [InlineData("ttroTtmoNoticeAfterMaking", 0)]
    [InlineData("ttroTtmoNoticeOfIntention", 0)]
    [InlineData("ttroTtmoRevocation", 0)]
    [InlineData("variationByNotice", 0)]
    [InlineData("troOnRoadActiveStatus", 0)]
    [InlineData("something", 1)]
    public void ValidateProvisionOrderReportingPointTypeForSchema324(string orderReportingPointType, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""orderReportingPoint"": ""{orderReportingPointType}"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""006A10CE-C4B3-4713-BAA0-35D66450893E""
                    }}
                ]

            }}
        }}", new SchemaVersion("3.2.4"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("some free text", 0)]
    [InlineData("", 1)]
    public void ValidateProvisionDescriptionForSchema330(string provisionDescription, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""orderReportingPoint"": ""variationByNotice"",
                        ""provisionDescription"": ""{provisionDescription}"",
                        ""reference"": ""D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D""
                    }}
                ]

            }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("some free text", 0)]
    [InlineData("", 1)]
    public void ValidateProvisionDescriptionForSchema324(string provisionDescription, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""orderReportingPoint"": ""variationByNotice"",
                        ""provisionDescription"": ""{provisionDescription}"",
                        ""reference"": ""D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D""
                    }}
                ]

            }}
        }}", new SchemaVersion("3.2.4"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D", 0)]
    [InlineData("", 1)]
    public void ValidateProvisionReferenceForSchema330(string reference, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""orderReportingPoint"": ""variationByNotice"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""{reference}""
                    }}
                ]
            }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D", 0)]
    [InlineData("", 1)]
    public void ValidateProvisionReferenceForSchema324(string reference, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""orderReportingPoint"": ""variationByNotice"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""{reference}""
                    }}
                ]
            }}
        }}", new SchemaVersion("3.2.4"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData(new[] { "D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D", "0EE20392-DD08-416F-A5E6-3013DB40728C" }, 0)]
    [InlineData(new[] { "A69D75A3-FCCC-4967-A0E5-7DCB82AFBE13", "A69D75A3-FCCC-4967-A0E5-7DCB82AFBE13" }, 1)]
    [InlineData(new[] { "9C88081C-FB1B-4E14-8E20-903DD9F08590", "" }, 1)]
    public void ValidateMultipleProvisionReferencesForSchema330(string[] references, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""orderReportingPoint"": ""variationByNotice"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""{references.ElementAt(0)}""
                    }},
                    {{
                        ""actionType"": ""fullRevoke"",
                        ""orderReportingPoint"": ""permanentRevocation"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""{references.ElementAt(1)}""                        
                    }}
                ]
            }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData(new[] { "D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D", "0EE20392-DD08-416F-A5E6-3013DB40728C" }, 0)]
    [InlineData(new[] { "A69D75A3-FCCC-4967-A0E5-7DCB82AFBE13", "A69D75A3-FCCC-4967-A0E5-7DCB82AFBE13" }, 1)]
    [InlineData(new[] { "9C88081C-FB1B-4E14-8E20-903DD9F08590", "" }, 1)]
    public void ValidateMultipleProvisionReferencesForSchema324(string[] references, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""orderReportingPoint"": ""variationByNotice"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""{references.ElementAt(0)}""
                    }},
                    {{
                        ""actionType"": ""fullRevoke"",
                        ""orderReportingPoint"": ""permanentRevocation"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""{references.ElementAt(1)}""                        
                    }}
                ]
            }}
        }}", new SchemaVersion("3.2.4"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }
}