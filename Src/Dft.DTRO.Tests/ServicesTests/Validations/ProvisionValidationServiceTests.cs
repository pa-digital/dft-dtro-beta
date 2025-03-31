using DfT.DTRO.Enums;

namespace Dft.DTRO.Tests.ServicesTests.Validations;

public class ProvisionValidationServiceTests
{
    private readonly IProvisionValidationService _sut = new ProvisionValidationService();

    [Theory]
    [InlineData("new", "3.3.0", 0)]
    [InlineData("partialAmendment", "3.3.0", 0)]
    [InlineData("fullAmendment", "3.3.0", 0)]
    [InlineData("partialRevoke", "3.3.0", 0)]
    [InlineData("fullRevoke", "3.3.0", 0)]
    [InlineData("noChange", "3.3.0", 0)]
    [InlineData("errorFix", "3.3.0", 0)]
    [InlineData("something", "3.3.0", 1)]
    [InlineData("new", "3.4.0", 0)]
    [InlineData("partialAmendment", "3.4.0", 0)]
    [InlineData("fullAmendment", "3.4.0", 0)]
    [InlineData("partialRevoke", "3.4.0", 0)]
    [InlineData("fullRevoke", "3.4.0", 0)]
    [InlineData("noChange", "3.4.0", 0)]
    [InlineData("errorFix", "3.4.0", 0)]
    [InlineData("something", "3.4.0", 1)]
    public void ValidateProvisionActionType(string actionType, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""{actionType}"",
                        ""comingIntoForceDate"": ""2020-01-01"",
                        ""expectedOccupancyDuration"": 10,
                        ""orderReportingPoint"": ""specialEventOrderNoticeOfMaking"",
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
    [InlineData("2024-01-01", "3.4.0", 0)]
    [InlineData("0000-01-01", "3.4.0", 1)]
    [InlineData("3024-01-01", "3.4.0", 1)]
    public void ValidateProvisionComingIntoForceDate(string comingIntoForceDate, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""comingIntoForceDate"": ""{comingIntoForceDate}"",
                        ""expectedOccupancyDuration"": 10,
                        ""orderReportingPoint"": ""specialEventOrderNoticeOfMaking"",
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
    [InlineData(1, "3.4.0", 0)]
    [InlineData(0, "3.4.0", 1)]
    [InlineData(-1, "3.4.0", 1)]
    public void ValidateProvisionExpectedOccupancyDuration(int expectedOccupancyDuration, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""comingIntoForceDate"": ""2020-01-01"",
                        ""expectedOccupancyDuration"": {expectedOccupancyDuration},
                        ""orderReportingPoint"": ""specialEventOrderNoticeOfMaking"",
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
    [InlineData("experimentalAmendment", "3.3.0", 0)]
    [InlineData("experimentalMakingPermanent", "3.3.0", 0)]
    [InlineData("experimentalNoticeOfMaking", "3.3.0", 0)]
    [InlineData("experimentalRevocation", "3.3.0", 0)]
    [InlineData("permanentAmendment", "3.3.0", 0)]
    [InlineData("permanentNoticeOfMaking", "3.3.0", 0)]
    [InlineData("permanentNoticeOfProposal", "3.3.0", 0)]
    [InlineData("permanentRevocation", "3.3.0", 0)]
    [InlineData("specialEventOrderNoticeOfMaking", "3.3.0", 0)]
    [InlineData("ttroTtmoByNotice", "3.3.0", 0)]
    [InlineData("ttroTtmoExtension", "3.3.0", 0)]
    [InlineData("ttroTtmoNoticeAfterMaking", "3.3.0", 0)]
    [InlineData("ttroTtmoNoticeOfIntention", "3.3.0", 0)]
    [InlineData("ttroTtmoRevocation", "3.3.0", 0)]
    [InlineData("variationByNotice", "3.3.0", 0)]
    [InlineData("troOnRoadActiveStatus", "3.3.0", 0)]
    [InlineData("something", "3.3.0", 1)]
    [InlineData("experimentalAmendment", "3.4.0", 0)]
    [InlineData("experimentalMakingPermanent", "3.4.0", 0)]
    [InlineData("experimentalNoticeOfMaking", "3.4.0", 0)]
    [InlineData("experimentalRevocation", "3.4.0", 0)]
    [InlineData("permanentAmendment", "3.4.0", 0)]
    [InlineData("permanentNoticeOfMaking", "3.4.0", 0)]
    [InlineData("permanentNoticeOfProposal", "3.4.0", 0)]
    [InlineData("permanentRevocation", "3.4.0", 0)]
    [InlineData("specialEventOrderNoticeOfMaking", "3.4.0", 0)]
    [InlineData("ttroTtmoByNotice", "3.4.0", 0)]
    [InlineData("ttroTtmoExtension", "3.4.0", 0)]
    [InlineData("ttroTtmoNoticeAfterMaking", "3.4.0", 0)]
    [InlineData("ttroTtmoNoticeOfIntention", "3.4.0", 0)]
    [InlineData("ttroTtmoRevocation", "3.4.0", 0)]
    [InlineData("variationByNotice", "3.4.0", 0)]
    [InlineData("troOnRoadActiveStatus", "3.4.0", 0)]
    [InlineData("something", "3.4.0", 1)]
    public void ValidateProvisionOrderReportingPointType(string orderReportingPointType, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""comingIntoForceDate"": ""2020-01-01"",
                        ""expectedOccupancyDuration"": 10,
                        ""orderReportingPoint"": ""{orderReportingPointType}"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""006A10CE-C4B3-4713-BAA0-35D66450893E"",
                        ""ActualStartOrStop"": [
                            {{
                                ""eventAt"": ""2024-10-03 20:00:00"",
                                ""eventType"": ""start""
                            }}
                        ],
                        ""ExperimentalVariation"": {{
                            ""effectOfChange"": ""some free text"",
                            ""expectedDuration"": 10
                        }},
                        ""ExperimentalCessation"": {{
                            ""actualDateOfCessation"": ""2024-10-03"",
                            ""natureOfCessation"": ""free text""
                        }}
                    }}
                ]

            }}
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("some free text", "3.3.0", 0)]
    [InlineData("", "3.3.0", 1)]
    [InlineData("some free text", "3.4.0", 0)]
    [InlineData("", "3.4.0", 1)]
    public void ValidateProvisionDescription(string provisionDescription, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""comingIntoForceDate"": ""2020-01-01"",
                        ""expectedOccupancyDuration"": 10,
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
    [InlineData("D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D", "3.3.0", 0)]
    [InlineData("", "3.3.0", 1)]
    [InlineData("D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D", "3.4.0", 0)]
    [InlineData("", "3.4.0", 1)]
    public void ValidateProvisionReference(string reference, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""comingIntoForceDate"": ""2020-01-01"",
                        ""expectedOccupancyDuration"": 10,
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
    [InlineData(new[] { "D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D", "0EE20392-DD08-416F-A5E6-3013DB40728C" }, "3.3.0", 0)]
    [InlineData(new[] { "A69D75A3-FCCC-4967-A0E5-7DCB82AFBE13", "A69D75A3-FCCC-4967-A0E5-7DCB82AFBE13" }, "3.3.0", 1)]
    [InlineData(new[] { "9C88081C-FB1B-4E14-8E20-903DD9F08590", "" }, "3.3.0", 1)]
    [InlineData(new[] { "D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D", "0EE20392-DD08-416F-A5E6-3013DB40728C" }, "3.4.0", 0)]
    [InlineData(new[] { "A69D75A3-FCCC-4967-A0E5-7DCB82AFBE13", "A69D75A3-FCCC-4967-A0E5-7DCB82AFBE13" }, "3.4.0", 1)]
    [InlineData(new[] { "9C88081C-FB1B-4E14-8E20-903DD9F08590", "" }, "3.4.0", 1)]
    public void ValidateMultipleProvisionReferences(string[] references, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""comingIntoForceDate"": ""2020-01-01"",
                        ""expectedOccupancyDuration"": 10,
                        ""orderReportingPoint"": ""variationByNotice"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""{references.ElementAt(0)}""
                    }},
                    {{
                        ""actionType"": ""fullRevoke"",
                        ""comingIntoForceDate"": ""2020-01-01"",
                        ""expectedOccupancyDuration"": 10,
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

    [Theory]
    [InlineData("ttroTtmoByNotice", "3.3.0", 0)]
    [InlineData("ttroTtmoExtension", "3.3.0", 0)]
    [InlineData("ttroTtmoNoticeAfterMaking", "3.3.0", 0)]
    [InlineData("ttroTtmoNoticeOfIntention", "3.3.0", 0)]
    [InlineData("ttroTtmoRevocation", "3.3.0", 0)]
    [InlineData("ttroTtmoByNotice", "3.4.0", 0)]
    [InlineData("ttroTtmoExtension", "3.4.0", 0)]
    [InlineData("ttroTtmoNoticeAfterMaking", "3.4.0", 0)]
    [InlineData("ttroTtmoNoticeOfIntention", "3.4.0", 0)]
    [InlineData("ttroTtmoRevocation", "3.4.0", 0)]
    public void ValidateActualStartOrStopWhenOrderReportingPointIsTemporary(string orderReportingPointType, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""comingIntoForceDate"": ""2020-01-01"",
                        ""expectedOccupancyDuration"": 10,
                        ""orderReportingPoint"": ""{orderReportingPointType}"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""006A10CE-C4B3-4713-BAA0-35D66450893E"",
                        ""ActualStartOrStop"": [
                            {{
                                ""eventAt"": ""2024-10-03 20:00:00"",
                                ""eventType"": ""start""
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
    [InlineData("2024-10-03 20:00:00", "3.4.0", 0)]
    [InlineData("3024-10-03 20:00:00", "3.4.0", 1)]
    [InlineData("wrongDateFormat", "3.4.0", 1)]
    [InlineData("", "3.4.0", 1)]
    [InlineData(null, "3.4.0", 1)]
    public void ValidateEventAtWithinActualStartOrStop(string eventAt, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""comingIntoForceDate"": ""2020-01-01"",
                        ""expectedOccupancyDuration"": 10,
                        ""orderReportingPoint"": ""ttroTtmoByNotice"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""006A10CE-C4B3-4713-BAA0-35D66450893E"",
                        ""ActualStartOrStop"": [
                            {{
                                ""eventAt"": ""{eventAt}"",
                                ""eventType"": ""start""
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
    [InlineData("start", "3.4.0", 0)]
    [InlineData("stop", "3.4.0", 0)]
    [InlineData("wrong", "3.4.0", 1)]
    [InlineData("", "3.4.0", 1)]
    [InlineData(null, "3.4.0", 1)]
    public void ValidateEventTypeWithinActualStartOrStop(string eventType, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""comingIntoForceDate"": ""2020-01-01"",
                        ""expectedOccupancyDuration"": 10,
                        ""orderReportingPoint"": ""ttroTtmoByNotice"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""006A10CE-C4B3-4713-BAA0-35D66450893E"",
                        ""ActualStartOrStop"": [
                            {{
                                ""eventAt"": ""2024-10-03 10:00:00"",
                                ""eventType"": ""{eventType}""
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
    [InlineData("experimentalAmendment", "3.3.0", 0)]
    [InlineData("experimentalMakingPermanent", "3.3.0", 0)]
    [InlineData("experimentalNoticeOfMaking", "3.3.0", 0)]
    [InlineData("experimentalRevocation", "3.3.0", 0)]
    [InlineData("experimentalAmendment", "3.4.0", 0)]
    [InlineData("experimentalMakingPermanent", "3.4.0", 0)]
    [InlineData("experimentalNoticeOfMaking", "3.4.0", 0)]
    [InlineData("experimentalRevocation", "3.4.0", 0)]
    public void ValidateExperimentalVariationWhenOrderReportingPointIsExperimental(string orderReportingPointType, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""comingIntoForceDate"": ""2020-01-01"",
                        ""expectedOccupancyDuration"": 10,
                        ""orderReportingPoint"": ""{orderReportingPointType}"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""006A10CE-C4B3-4713-BAA0-35D66450893E"",
                        ""ExperimentalVariation"": {{
                            ""effectOfChange"": ""free text"",
                            ""expectedDuration"": 10
                        }},
                        ""ExperimentalCessation"": {{
                            ""actualDateOfCessation"": ""2020-12-31"",
                            ""natureOfCessation"": ""free text""
                        }}
                    }}
                ]

            }}
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("free text", "3.4.0", 0)]
    [InlineData("", "3.4.0", 1)]
    [InlineData(null, "3.4.0", 1)]
    public void ValidateEffectOfChangeWithinExperimentalVariation(string effectOfChange, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""comingIntoForceDate"": ""2020-01-01"",
                        ""expectedOccupancyDuration"": 10,
                        ""orderReportingPoint"": ""experimentalNoticeOfMaking"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""006A10CE-C4B3-4713-BAA0-35D66450893E"",
                        ""ExperimentalVariation"": {{
                            ""effectOfChange"": ""{effectOfChange}"",
                            ""expectedDuration"": 10
                        }},
                        ""ExperimentalCessation"": {{
                            ""actualDateOfCessation"": ""2020-12-31"",
                            ""natureOfCessation"": ""free text""
                        }}
                    }}
                ]

            }}
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData(1, "3.4.0", 0)]
    [InlineData(0, "3.4.0", 1)]
    [InlineData(null, "3.4.0", 1)]
    public void ValidateExpectedDurationWithinExperimentalVariation(int expectedDuration, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""comingIntoForceDate"": ""2020-01-01"",
                        ""expectedOccupancyDuration"": 10,
                        ""orderReportingPoint"": ""experimentalNoticeOfMaking"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""006A10CE-C4B3-4713-BAA0-35D66450893E"",
                        ""ExperimentalVariation"": {{
                            ""effectOfChange"": ""free text"",
                            ""expectedDuration"": {expectedDuration}
                        }},
                        ""ExperimentalCessation"": {{
                            ""actualDateOfCessation"": ""2020-12-31"",
                            ""natureOfCessation"": ""free text""
                        }}
                    }}
                ]

            }}
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }




    [Theory]
    [InlineData("experimentalAmendment", "3.3.0", 0)]
    [InlineData("experimentalMakingPermanent", "3.3.0", 0)]
    [InlineData("experimentalNoticeOfMaking", "3.3.0", 0)]
    [InlineData("experimentalRevocation", "3.3.0", 0)]
    [InlineData("experimentalAmendment", "3.4.0", 0)]
    [InlineData("experimentalMakingPermanent", "3.4.0", 0)]
    [InlineData("experimentalNoticeOfMaking", "3.4.0", 0)]
    [InlineData("experimentalRevocation", "3.4.0", 0)]
    public void ValidateExperimentalCessationWhenOrderReportingPointIsExperimental(string orderReportingPointType, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""comingIntoForceDate"": ""2020-01-01"",
                        ""expectedOccupancyDuration"": 10,
                        ""orderReportingPoint"": ""{orderReportingPointType}"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""006A10CE-C4B3-4713-BAA0-35D66450893E"",
                        ""ExperimentalVariation"": {{
                            ""effectOfChange"": ""free text"",
                            ""expectedDuration"": 10
                        }},
                        ""ExperimentalCessation"": {{
                            ""actualDateOfCessation"": ""2020-12-31"",
                            ""natureOfCessation"": ""free text""
                        }}
                    }}
                ]

            }}
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("2020-10-01", "3.4.0", 0)]
    [InlineData("0000-01-01", "3.4.0", 1)]
    [InlineData("3025-12-12", "3.4.0", 1)]
    public void ValidateActualDateOfCessationWithExperimentalCessation(string actualDateOfCessation, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""comingIntoForceDate"": ""2020-01-01"",
                        ""expectedOccupancyDuration"": 10,
                        ""orderReportingPoint"": ""experimentalNoticeOfMaking"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""006A10CE-C4B3-4713-BAA0-35D66450893E"",
                        ""ExperimentalVariation"": {{
                            ""effectOfChange"": ""free text"",
                            ""expectedDuration"": 10
                        }},
                        ""ExperimentalCessation"": {{
                            ""actualDateOfCessation"": ""{actualDateOfCessation}"",
                            ""natureOfCessation"": ""free text""
                        }}
                    }}
                ]

            }}
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("free text", "3.4.0", 0)]
    [InlineData("", "3.4.0", 1)]
    [InlineData(null, "3.4.0", 1)]
    public void ValidateNatureOfCessationWithinExperimentalVariation(string natureOfCessation, string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""comingIntoForceDate"": ""2020-01-01"",
                        ""expectedOccupancyDuration"": 10,
                        ""orderReportingPoint"": ""experimentalNoticeOfMaking"",
                        ""provisionDescription"": ""some free text"",
                        ""reference"": ""006A10CE-C4B3-4713-BAA0-35D66450893E"",
                        ""ExperimentalVariation"": {{
                            ""effectOfChange"": ""free text"",
                            ""expectedDuration"": 10
                        }},
                        ""ExperimentalCessation"": {{
                            ""actualDateOfCessation"": ""2020-12-31"",
                            ""natureOfCessation"": ""{natureOfCessation}""
                        }}
                    }}
                ]

            }}
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Fact]
    public void ValidateMultipleProvisionsPermanentAndTemporaryTROs()
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
            ""Source"": {{
                ""Provision"": [
                    {{
                        ""actionType"": ""new"",
                        ""orderReportingPoint"": ""permanentNoticeOfMaking"",
                        ""provisionDescription"": ""Schedule 1: No Waiting Monday to Saturday, 8am to 6pm"",
                        ""reference"": ""A3448229-1DFA-48CD-A785-376ACB9F7C56"",
                    }},
                    {{
                        ""actionType"": ""new"",
                        ""orderReportingPoint"": ""ttroTtmoNoticeOfIntention"",
                        ""provisionDescription"": ""Temporary full road closure, for combined utility works and carriageway repair"",
                        ""reference"": ""b1618e6f-f65c-48c7-9cc7-45da9f45fbda"",
                        ""ActualStartOrStop"": [
                            {{
                                ""eventAt"": ""2025-01-01T09:00:00"",
                                ""eventType"": ""start""
                            }}
                        ],
                    }},
                ]

            }}
        }}", new SchemaVersion("3.4.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Empty(actual);
    }
}