namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class ProvisionValidationServiceTests
{
    private readonly IProvisionValidationService _sut = new ProvisionValidationService();

    [Theory]
    [InlineData("3.2.4", "new", 0)]
    [InlineData("3.2.4", "partialAmendment", 0)]
    [InlineData("3.2.4", "fullAmendment", 0)]
    [InlineData("3.2.4", "partialRevoke", 0)]
    [InlineData("3.2.4", "fullRevoke", 0)]
    [InlineData("3.2.4", "noChange", 0)]
    [InlineData("3.2.4", "errorFix", 0)]
    [InlineData("3.2.4", "something", 1)]
    [InlineData("3.3.0", "new", 0)]
    [InlineData("3.3.0", "partialAmendment", 0)]
    [InlineData("3.3.0", "fullAmendment", 0)]
    [InlineData("3.3.0", "partialRevoke", 0)]
    [InlineData("3.3.0", "fullRevoke", 0)]
    [InlineData("3.3.0", "noChange", 0)]
    [InlineData("3.3.0", "errorFix", 0)]
    [InlineData("3.3.0", "something", 1)]
    public void ValidateProvisionActionType(string version, string actionType, int errorCount)
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
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.2.4", "experimentalAmendment", 0)]
    [InlineData("3.2.4", "experimentalMakingPermanent", 0)]
    [InlineData("3.2.4", "experimentalNoticeOfMaking", 0)]
    [InlineData("3.2.4", "experimentalRevocation", 0)]
    [InlineData("3.2.4", "permanentAmendment", 0)]
    [InlineData("3.2.4", "permanentNoticeOfMaking", 0)]
    [InlineData("3.2.4", "permanentNoticeOfProposal", 0)]
    [InlineData("3.2.4", "permanentRevocation", 0)]
    [InlineData("3.2.4", "specialEventOrderNoticeOfMaking", 0)]
    [InlineData("3.2.4", "ttroTtmoByNotice", 0)]
    [InlineData("3.2.4", "ttroTtmoExtension", 0)]
    [InlineData("3.2.4", "ttroTtmoNoticeAfterMaking", 0)]
    [InlineData("3.2.4", "ttroTtmoNoticeOfIntention", 0)]
    [InlineData("3.2.4", "ttroTtmoRevocation", 0)]
    [InlineData("3.2.4", "variationByNotice", 0)]
    [InlineData("3.2.4", "troOnRoadActiveStatus", 0)]
    [InlineData("3.2.4", "something", 1)]
    [InlineData("3.3.0", "experimentalAmendment", 0)]
    [InlineData("3.3.0", "experimentalMakingPermanent", 0)]
    [InlineData("3.3.0", "experimentalNoticeOfMaking", 0)]
    [InlineData("3.3.0", "experimentalRevocation", 0)]
    [InlineData("3.3.0", "permanentAmendment", 0)]
    [InlineData("3.3.0", "permanentNoticeOfMaking", 0)]
    [InlineData("3.3.0", "permanentNoticeOfProposal", 0)]
    [InlineData("3.3.0", "permanentRevocation", 0)]
    [InlineData("3.3.0", "specialEventOrderNoticeOfMaking", 0)]
    [InlineData("3.3.0", "ttroTtmoByNotice", 0)]
    [InlineData("3.3.0", "ttroTtmoExtension", 0)]
    [InlineData("3.3.0", "ttroTtmoNoticeAfterMaking", 0)]
    [InlineData("3.3.0", "ttroTtmoNoticeOfIntention", 0)]
    [InlineData("3.3.0", "ttroTtmoRevocation", 0)]
    [InlineData("3.3.0", "variationByNotice", 0)]
    [InlineData("3.3.0", "troOnRoadActiveStatus", 0)]
    [InlineData("3.3.0", "something", 1)]
    public void ValidateProvisionOrderReportingPointType(string version, string orderReportingPointType, int errorCount)
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
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.2.4", "some free text", 0)]
    [InlineData("3.2.4", "", 1)]
    [InlineData("3.3.0", "some free text", 0)]
    [InlineData("3.3.0", "", 1)]
    public void ValidateProvisionDescription(string version, string provisionDescription, int errorCount)
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
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.2.4", "D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D", 0)]
    [InlineData("3.2.4", "", 1)]
    [InlineData("3.3.0", "D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D", 0)]
    [InlineData("3.3.0", "", 1)]
    public void ValidateProvisionReference(string version, string reference, int errorCount)
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
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.2.4", new[] { "D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D", "0EE20392-DD08-416F-A5E6-3013DB40728C" }, 0)]
    [InlineData("3.2.4", new[] { "A69D75A3-FCCC-4967-A0E5-7DCB82AFBE13", "A69D75A3-FCCC-4967-A0E5-7DCB82AFBE13" }, 1)]
    [InlineData("3.2.4", new[] { "9C88081C-FB1B-4E14-8E20-903DD9F08590", "" }, 1)]
    [InlineData("3.3.0", new[] { "D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D", "0EE20392-DD08-416F-A5E6-3013DB40728C" }, 0)]
    [InlineData("3.3.0", new[] { "A69D75A3-FCCC-4967-A0E5-7DCB82AFBE13", "A69D75A3-FCCC-4967-A0E5-7DCB82AFBE13" }, 1)]
    [InlineData("3.3.0", new[] { "9C88081C-FB1B-4E14-8E20-903DD9F08590", "" }, 1)]
    public void ValidateMultipleProvisionReferences(string version, string[] references, int errorCount)
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
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }
}