namespace Dft.DTRO.Tests.ServicesTests.Validations;

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
    [InlineData(20, 0)]
    [InlineData(30, 0)]
    [InlineData(40, 0)]
    [InlineData(50, 0)]
    [InlineData(60, 0)]
    [InlineData(70, 0)]
    [InlineData(80, 1)]
    [InlineData(55, 1)]
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
                        ""mphValue"": 70,
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

    [Theory]
    [InlineData("nationalSpeedLimitDualCarriageway", 0)]
    [InlineData("nationalSpeedLimitSingleCarriageway", 0)]
    [InlineData("nationalSpeedLimitMotorway", 0)]
    [InlineData("unknown", 1)]
    public void ValidateSpeedLimitProfileBasedSpeedType(string type, int errorCount)
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
                    ""SpeedLimitProfileBased"": {{
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

    [Theory]
    [InlineData("dimensionMaximumHeightStructural", 0)]
    [InlineData("dimensionMaximumHeightWithTRO", 0)]
    [InlineData("dimensionMaximumLength", 0)]
    [InlineData("dimensionMaximumWeightEnvironmental", 0)]
    [InlineData("dimensionMaximumWeightStructural", 0)]
    [InlineData("dimensionMaximumWidth", 0)]
    [InlineData("bannedMovementNoEntry", 0)]
    [InlineData("bannedMovementNoLeftTurn", 0)]
    [InlineData("bannedMovementNoRightTurn", 0)]
    [InlineData("bannedMovementNoUTurn", 0)]
    [InlineData("mandatoryDirectionAheadOnly", 0)]
    [InlineData("mandatoryDirectionLeftTurnOnly", 0)]
    [InlineData("mandatoryDirectionOneWay", 0)]
    [InlineData("mandatoryDirectionRightTurnOnly", 0)]
    [InlineData("movementOrderNoOvertaking", 0)]
    [InlineData("movementOrderPriorityOverOncomingTraffic", 0)]
    [InlineData("movementOrderProhibitedAccess", 0)]
    [InlineData("kerbsideDisabledBadgeHoldersOnly", 0)]
    [InlineData("kerbsideRuralClearway", 0)]
    [InlineData("kerbsideLimitedWaiting", 0)]
    [InlineData("kerbsideLoadingPlace", 0)]
    [InlineData("kerbsideMotorcycleParkingPlace", 0)]
    [InlineData("kerbsideNoLoading", 0)]
    [InlineData("kerbsideNoStopping", 0)]
    [InlineData("kerbsideNoWaiting", 0)]
    [InlineData("kerbsideTaxiRank", 0)]
    [InlineData("kerbsideSchoolKeepClearYellowZigZagMandatory", 0)]
    [InlineData("kerbsideLoadingBay", 0)]
    [InlineData("kerbsideOtherYellowZigZagMandatory", 0)]
    [InlineData("kerbsidePermitParkingArea", 0)]
    [InlineData("kerbsideParkingPlace", 0)]
    [InlineData("kerbsideUrbanClearway", 0)]
    [InlineData("kerbsideRedRouteClearway", 0)]
    [InlineData("kerbsidePaymentParkingPlace", 0)]
    [InlineData("kerbsidePermitParkingPlace", 0)]
    [InlineData("kerbsideFootwayParking", 0)]
    [InlineData("kerbsideControlledParkingZone", 0)]
    [InlineData("kerbsideRestrictedParkingZone", 0)]
    [InlineData("nonOrderKerbsideBusStop", 0)]
    [InlineData("nonOrderMovementBoxJunction", 0)]
    [InlineData("nonOrderKerbsidePedestrianCrossing", 0)]
    [InlineData("miscBusGate", 0)]
    [InlineData("miscBusLaneWithTrafficFlow", 0)]
    [InlineData("miscBusOnlyStreet", 0)]
    [InlineData("miscContraflowBusLane", 0)]
    [InlineData("miscCongestionLowEmissionZone", 0)]
    [InlineData("miscCycleLane", 0)]
    [InlineData("miscPedestrianZone", 0)]
    [InlineData("miscRoadClosure", 0)]
    [InlineData("miscLaneClosure", 0)]
    [InlineData("miscContraflow", 0)]
    [InlineData("miscFootwayClosure", 0)]
    [InlineData("miscCycleLaneClosure", 0)]
    [InlineData("miscTemporaryParkingRestriction", 0)]
    [InlineData("miscSuspensionOfOneWay", 0)]
    [InlineData("miscSuspensionOfParkingRestriction", 0)]
    [InlineData("miscSuspensionOfWeightRestriction", 0)]
    [InlineData("miscSuspensionOfBusway", 0)]
    [InlineData("miscTemporarySpeedLimit", 0)]
    [InlineData("miscRoadClosureCrossingPoint", 0)]
    [InlineData("miscBaySuspension", 0)]
    [InlineData("miscTemporaryParkingBay", 0)]
    [InlineData("miscPROWClosure", 0)]
    [InlineData("kerbsideDoubleRedLines", 0)]
    [InlineData("kerbsideSingleRedLines", 0)]
    [InlineData("unknown", 1)]
    public void ValidateGeneralRegulationRegulationType(string regulationType, int errorCount)
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
                    ""GeneralRegulation"": {{
                        ""regulationType"": ""{regulationType}""
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
    [InlineData("free text", 0)]
    [InlineData("", 1)]
    [InlineData(null, 1)]
    public void ValidateRegulationOffListRegulationRegulationFullText(string regulationFullText, int errorCount)
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Source"": {{
            ""Provision"": [
              {{
                ""Regulation"": [
                  {{
                    ""isDynamic"": true,
                    ""timeZone"": ""Europe/London"",
                    ""OffListRegulation"": {{
                        ""regulationFullText"": ""{regulationFullText}"",
                        ""regulationShortName"": ""free text""
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
    [InlineData("free text", 0)]
    [InlineData("", 1)]
    [InlineData(null, 1)]
    public void ValidateRegulationOffListRegulationShortName(string shortName, int errorCount)
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Source"": {{
            ""Provision"": [
              {{
                ""Regulation"": [
                  {{
                    ""isDynamic"": true,
                    ""timeZone"": ""Europe/London"",
                    ""OffListRegulation"": {{
                        ""regulationFullText"": ""free text"",
                        ""regulationShortName"": ""{shortName}""
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