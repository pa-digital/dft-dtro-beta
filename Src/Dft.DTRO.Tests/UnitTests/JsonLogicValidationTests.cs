using System.Text.Json;

namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class JsonLogicValidationTests
{
    private readonly Mock<IRuleTemplateDal> _ruleDal = new();

    public JsonLogicValidationTests()
    {
        JsonLogic.AddAllRules(typeof(IJsonLogicRuleSource).Assembly);
    }

    [Fact]
    public async Task AllowsOverallPeriodStartTimeBeforeEndTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {   ""regulation"": [
                            {
                                ""timeValidity"": {
                                    ""start"": ""1983-01-05T22:50:50.0Z"",
                                    ""end"": ""1985-01-05T22:50:50.0Z""
                                }
                            }
                        ]
                    }
                ]
            }
        }");

        await UseRulesByName("OverallPeriodStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowsOverallPeriodEndTimeToBeMissing()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {   ""regulation"": [
                            {
                                ""timeValidity"": {
                                    ""start"": ""1983-01-05T22:50:50.0Z""
                                }
                            }
                        ]
                    }
                ]
            }
        }");

        await UseRulesByName("OverallPeriodStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsOverallPeriodEndTimeBeforeStartTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {   ""regulation"": [
                            {
                                ""timeValidity"": {
                                    ""start"": ""1985-01-05T22:50:50.0Z"",
                                    ""end"": ""1983-01-05T22:50:50.0Z""
                                }
                            }
                        ]
                    }
                ]
            }
        }");

        await UseRulesByName("OverallPeriodStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsValidPeriodStartTimeBeforeEndTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""validPeriod"": [{
                                    ""startOfPeriod"": ""1983-01-05T22:50:50.0Z"",
                                    ""endOfPeriod"": ""1985-01-05T22:50:50.0Z""
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidPeriodStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsValidPeriodEndTimeBeforeStartTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""validPeriod"": [{
                                    ""startOfPeriod"": ""1985-01-05T22:50:50.0Z"",
                                    ""endOfPeriod"": ""1983-01-05T22:50:50.0Z""
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidPeriodStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsExceptionPeriodStartTimeBeforeEndTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""exceptionPeriod"": [{
                                    ""startOfPeriod"": ""1983-01-05T22:50:50.0Z"",
                                    ""endOfPeriod"": ""1985-01-05T22:50:50.0Z""
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ExceptionPeriodStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsExceptionPeriodEndTimeBeforeStartTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""exceptionPeriod"": [{
                                    ""startOfPeriod"": ""1985-01-05T22:50:50.0Z"",
                                    ""endOfPeriod"": ""1983-01-05T22:50:50.0Z""
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ExceptionPeriodStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsExceptionPeriodStartTimeAfterOverallPeriodStartTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""start"": ""1984-01-05T22:50:50.0Z"",
                            ""exceptionPeriod"": [{
                                    ""startOfPeriod"": ""1985-01-05T22:50:50.0Z""
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ExceptionPeriodStartNotLessThanOverallPeriodStart");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsExceptionPeriodStartTimeBeforeOverallPeriodStartTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""start"": ""1985-01-05T22:50:50.0Z"",
                            ""exceptionPeriod"": [{
                                    ""startOfPeriod"": ""1984-01-05T22:50:50.0Z""
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ExceptionPeriodStartNotLessThanOverallPeriodStart");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsExceptionPeriodEndTimeBeforeOverallPeriodEndTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""end"": ""1985-01-05T22:50:50.0Z"",
                            ""exceptionPeriod"": [{
                                    ""endOfPeriod"": ""1984-01-05T22:50:50.0Z""
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ExceptionPeriodEndNotMoreThanOverallPeriodEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsExceptionPeriodEndTimeAfterOverallPeriodEndTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""end"": ""1984-01-05T22:50:50.0Z"",
                            ""exceptionPeriod"": [{
                                    ""endOfPeriod"": ""1985-01-05T22:50:50.0Z""
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ExceptionPeriodEndNotMoreThanOverallPeriodEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }


    [Fact]
    public async Task AllowsValidPeriodStartTimeAfterOverallPeriodStartTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""start"": ""1984-01-05T22:50:50.0Z"",
                            ""validPeriod"": [{
                                    ""startOfPeriod"": ""1985-01-05T22:50:50.0Z""
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidPeriodStartNotLessThanOverallPeriodStart");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsValidPeriodStartTimeBeforeOverallPeriodStartTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""start"": ""1985-01-05T22:50:50.0Z"",
                            ""validPeriod"": [{
                                    ""startOfPeriod"": ""1984-01-05T22:50:50.0Z""
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidPeriodStartNotLessThanOverallPeriodStart");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsValidPeriodEndTimeBeforeOverallPeriodEndTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""end"": ""1985-01-05T22:50:50.0Z"",
                            ""validPeriod"": [{
                                    ""endOfPeriod"": ""1984-01-05T22:50:50.0Z""
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidPeriodEndNotMoreThanOverallPeriodEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsValidPeriodEndTimeAfterOverallPeriodEndTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""end"": ""1984-01-05T22:50:50.0Z"",
                            ""validPeriod"": [{
                                    ""endOfPeriod"": ""1985-01-05T22:50:50.0Z""
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidPeriodEndNotMoreThanOverallPeriodEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsRecurringTimePeriodOfDayStartTimeBeforeEndTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""validPeriod"": [{
                                ""recurringTimePeriodOfDay"": 
                                    [{
                                        ""startTimeOfPeriod"": ""07:30:00.0Z"",
                                        ""endTimeOfPeriod"": ""09:00:00.0Z""
                                }]
                            }],
                            ""exceptionPeriod"": [{
                                ""recurringTimePeriodOfDay"": 
                                    [{
                                        ""startTimeOfPeriod"": ""07:30:00.0Z"",
                                        ""endTimeOfPeriod"": ""09:00:00.0Z""
                                }]
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd", "ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsRecurringTimePeriodOfDayEndTimeBeforeStartTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""validPeriod"": [{
                                ""recurringTimePeriodOfDay"": 
                                    [{
                                        ""startTimeOfPeriod"": ""09:00:00.0Z"",
                                        ""endTimeOfPeriod"": ""07:30:00.0Z""
                                }]
                            }],
                            ""exceptionPeriod"": [{
                                ""recurringTimePeriodOfDay"": 
                                    [{
                                        ""startTimeOfPeriod"": ""09:00:00.0Z"",
                                        ""endTimeOfPeriod"": ""07:30:00.0Z""
                                }]
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd", "ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task AllowsRecurringTimePeriodOfDayToBeEmpty()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""validPeriod"": [{
                                ""recurringTimePeriodOfDay"": []
                            }],
                            ""exceptionPeriod"": [{
                                ""recurringTimePeriodOfDay"": []
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd", "ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowsRecurringTimePeriodOfDayToBeMissing()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""validPeriod"": [{
                            }],
                            ""exceptionPeriod"": [{
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd", "ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowsValidPeriodToBeEmpty()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""validPeriod"": []
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd", "ValidPeriodStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowsExceptionPeriodToBeEmpty()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""exceptionPeriod"": []
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd", "ExceptionPeriodStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowsValidPeriodToBeMissing()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd", "ValidPeriodStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowsExceptionPeriodToBeMissing()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd", "ExceptionPeriodStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowsTaInSwaRules()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""traCreator"": 10,  ""currentTraOwner"": 10
            }
        }");

        await UseRulesByName("TaInSwaCodes");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact(Skip = "Method is too complicated")]
    public async Task DisallowPublicationTimeMoreThanOneMonthOld()
    {
        DateTime time = DateTime.UtcNow.AddMonths(-2);

        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""header"": {
                ""publicationTime"": " + JsonSerializer.Serialize(time) + @"
            }
        }");

        await UseRulesByName("PublicationTimeAge");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowPublicationTimeWithinOneMonthOld()
    {
        DateTime time = DateTime.UtcNow.AddDays(-15);

        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""header"": {
                ""publicationTime"": " + JsonSerializer.Serialize(time) + @"
            }
        }");

        await UseRulesByName("PublicationTimeAge");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact(Skip = "Method is too complicated")]
    public async Task DisallowPublicationTimeFromTheFuture()
    {
        DateTime time = DateTime.UtcNow.AddMonths(1);

        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""header"": {
                ""publicationTime"": " + JsonSerializer.Serialize(time) + @"
            }
        }");

        await UseRulesByName("PublicationTimeAge");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/14")]
    public async Task AllowExternalReferenceLastUpdateDateInThePast()
    {
        DateTime time = DateTime.UtcNow.AddMonths(-2);

        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulatedPlace"": [
                  {
                    ""externalReference"": [
                      {
                        ""lastUpdateDate"": " + JsonSerializer.Serialize(time) + @",
                      }
                    ]
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("ExternalReferenceLastUpdateDate");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact(Skip = "Method is too complicated")]
    [Trait("RuleId", "DFT-205/14")]
    public async Task DisallowExternalReferenceLastUpdateDateFromTheFuture()
    {
        DateTime time = DateTime.UtcNow.AddMonths(2);

        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulatedPlace"": [
                  {
                    ""externalReference"": [
                      {
                        ""lastUpdateDate"": " + JsonSerializer.Serialize(time) + @",
                      }
                    ]
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("ExternalReferenceLastUpdateDate");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowHeaderMissing()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
        }");

        await UseRulesByName("PublicationTimeAge");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowValidUsagePeriodStartLessThanEnd()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {   ""regulation"": [
                            {
                                ""timeValidity"": {
                                    ""validityCondition"": {
                                        ""rateTable"": {
                                            ""rateLineCollection"": [
                                                {
                                                    ""startValidUsagePeriod"": ""13:00:00"",
                                                    ""endValidUsagePeriod"": ""14:00:00""
                                                }
                                            ]
                                        }
                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }");

        await UseRulesByName("ValidUsagePeriodStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowValidUsagePeriodEndMissing()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {   ""regulation"": [
                            {
                                ""timeValidity"": {
                                    ""validityCondition"": {
                                        ""rateTable"": {
                                            ""rateLineCollection"": [
                                                {
                                                    ""startValidUsagePeriod"": ""14:00:00""
                                                }
                                            ]
                                        }
                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }");

        await UseRulesByName("ValidUsagePeriodStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact(Skip = "Method is too complicated")]
    public async Task DisallowValidUsagePeriodEndLessThanStart()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {   ""regulation"": [
                            {
                                ""timeValidity"": {
                                    ""validityCondition"": {
                                        ""rateTable"": {
                                            ""rateLineCollection"": [
                                                {
                                                    ""startValidUsagePeriod"": ""14:00:00"",
                                                    ""endValidUsagePeriod"": ""13:00:00""
                                                }
                                            ]
                                        }
                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }");

        await UseRulesByName("ValidUsagePeriodStartLessThanEnd");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowMinTimeLessThanMaxTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {   ""regulation"": [
                            {
                                ""timeValidity"": {
                                    ""validityCondition"": {
                                        ""rateTable"": {
                                            ""rateLineCollection"": [
                                                {
                                                    ""minTime"": ""13:00:00"",
                                                    ""maxTime"": ""14:00:00""
                                                }
                                            ]
                                        }
                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }");

        await UseRulesByName("MinTimeLessThanMaxTime");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact(Skip = "Method is too complicated")]
    public async Task DisallowMaxTimeLessThanMinTime()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {   ""regulation"": [
                            {
                                ""timeValidity"": {
                                    ""validityCondition"": {
                                        ""rateTable"": {
                                            ""rateLineCollection"": [
                                                {
                                                    ""minTime"": ""14:00:00"",
                                                    ""maxTime"": ""13:00:00""
                                                }
                                            ]
                                        }
                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }");

        await UseRulesByName("MinTimeLessThanMaxTime");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowMaxTimeAndOrMinTimeMissing()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {   ""regulation"": [
                            {
                                ""timeValidity"": {
                                    ""validityCondition"": {
                                        ""rateTable"": {
                                            ""rateLineCollection"": [
                                                {
                                                        ""minTime"": ""14:00:00""
                                                },
                                                {
                                                        ""maxTime"": ""14:00:00""
                                                },
                                                {
                                                }
                                            ]
                                        }
                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }");

        await UseRulesByName("MinTimeLessThanMaxTime");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact(Skip = "Method is too complicated")]
    public async Task DisallowValueCollectionMaxLessThanMin()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {   ""regulation"": [
                            {
                                ""timeValidity"": {
                                    ""validityCondition"": {
                                        ""rateTable"": {
                                            ""rateLineCollection"": [
                                                {
                                                        ""minValueCollection"": ""200"",
                                                        ""maxValueCollection"": ""100""
                                                }
                                            ]
                                        }
                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }");

        await UseRulesByName("MinValueCollectionLessThanMax");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowValueCollectionMinLessThanMax()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {   ""regulation"": [
                            {
                                ""timeValidity"": {
                                    ""validityCondition"": {
                                        ""rateTable"": {
                                            ""rateLineCollection"": [
                                                {
                                                        ""minValueCollection"": ""100"",
                                                        ""maxValueCollection"": ""200""
                                                }
                                            ]
                                        }
                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }");

        await UseRulesByName("MinValueCollectionLessThanMax");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowValueCollectionMinAndOrMaxMissing()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {   ""regulation"": [
                            {
                                ""timeValidity"": {
                                    ""validityCondition"": {
                                        ""rateTable"": {
                                            ""rateLineCollection"": [
                                                {
                                                        ""minValueCollection"": ""14:00:00""
                                                },
                                                {
                                                        ""maxValueCollection"": ""14:00:00""
                                                },
                                                {
                                                }
                                            ]
                                        }
                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }");

        await UseRulesByName("MinValueCollectionLessThanMax");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact(Skip = "Method is too complicated")]
    [Trait("RuleId", "43")]
    public async Task DisallowValueMaxLessThanMin()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""timeValidity"": {
                      ""validityCondition"": {
                        ""rateTable"": {
                          ""rateLineCollection"": [
                            {
                              ""rateLine"": [
                                {
                                  ""minValue"": 40,
                                  ""maxValue"": 30
                                }
                              ]
                            }
                          ]
                        }
                      }
                    }
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("MinValueLessThanMax");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    [Trait("RuleId", "43")]
    public async Task AllowValueMinLessThanMax()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""timeValidity"": {
                      ""validityCondition"": {
                        ""rateTable"": {
                          ""rateLineCollection"": [
                            {
                              ""rateLine"": [
                                {
                                  ""minValue"": 10,
                                  ""maxValue"": 20
                                }
                              ]
                            }
                          ]
                        }
                      }
                    }
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("MinValueLessThanMax");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowSequentialProvisionIndex()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {   
                        ""provisionIndex"": 0
                    },
                    {
                        ""provisionIndex"": 1
                    },
                    {   
                        ""provisionIndex"": 2
                    }
                ]
            }
        }");

        await UseRulesByName("ProvisionIndexSequential");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowSequentialRateLineCollection()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {   ""regulation"": [
                            {
                                ""timeValidity"": {
                                    ""validityCondition"": {
                                        ""rateTable"": {
                                            ""rateLineCollection"": [
                                                { ""sequence"": 0 },
                                                { ""sequence"": 1 },
                                                { ""sequence"": 2 }
                                            ]
                                        }
                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }");

        await UseRulesByName("RateLineCollectionSequential");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact(Skip = "Method is too complicated")]
    public async Task DisallowNonSequentialRateLineCollection()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {   ""regulation"": [
                            {
                                ""timeValidity"": {
                                    ""validityCondition"": {
                                        ""rateTable"": {
                                            ""rateLineCollection"": [
                                                { ""sequence"": 1 },
                                                { ""sequence"": 2 },
                                                { ""sequence"": 0 }
                                            ]
                                        }
                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }");

        await UseRulesByName("RateLineCollectionSequential");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowSequentialRateLine()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {   ""regulation"": [
                            {
                                ""timeValidity"": {
                                    ""validityCondition"": {
                                        ""rateTable"": {
                                            ""rateLineCollection"": [
                                                { ""rateLine"": [
                                                    { ""sequence"": 0 },
                                                    { ""sequence"": 1 },
                                                    { ""sequence"": 2 } ]
                                                }
                                            ]
                                        }
                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }");

        await UseRulesByName("RateLineSequential");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact(Skip = "Method is too complicated")]
    public async Task DisallowNonSequentialRateLine()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {   ""regulation"": [
                            {
                                ""timeValidity"": {
                                    ""validityCondition"": {
                                        ""rateTable"": {
                                            ""rateLineCollection"": [
                                                { ""rateLine"": [
                                                    { ""sequence"": 1 },
                                                    { ""sequence"": 2 },
                                                    { ""sequence"": 0 } ]
                                                }
                                            ]
                                        }
                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }");

        await UseRulesByName("RateLineSequential");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/3")]
    public async Task AllowHeightCharacteristicValidForRegulationTypeDimensionMaximumHeightWithTro()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""regulationType"": ""dimensionMaximumHeightWithTRO"",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""MaximumHeightCharacteristic"": [
                            {
                              ""comparisonOperator"": ""lessThanOrEqualTo"",
                              ""vehicleHeight"": 6
                            }
                          ]
                        }
                      }
                    ]
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("HeightCharacteristicValidForRegulationTypeDimensionMaximumHeightWithTRO");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/3")]
    public async Task DisallowHeightCharacteristicInvalidForRegulationTypeDimensionMaximumHeightWithTro()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""regulationType"": ""dimensionMaximumHeightWithTRO"",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""heightCharacteristic"": [
                            {
                              ""comparisonOperator"": ""lessThanOrEqualTo"",
                              ""vehicleHeight"": 7
                            }
                          ]
                        }
                      }
                    ]
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("HeightCharacteristicValidForRegulationTypeDimensionMaximumHeightWithTRO");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/4")]
    public async Task AllowHeightCharacteristicValidForRegulationTypeDimensionMaximumHeightStructural()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""regulationType"": ""dimensionMaximumHeightStructural"",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""MaximumHeightCharacteristic"": [
                            {
                              ""comparisonOperator"": ""lessThanOrEqualTo"",
                              ""vehicleHeight"": 6
                            }
                          ]
                        }
                      }
                    ]
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("HeightCharacteristicValidForRegulationTypeDimensionMaximumHeightStructural");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/4")]
    public async Task DisallowHeightCharacteristicInvalidForRegulationTypeDimensionMaximumHeightStructural()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""regulationType"": ""dimensionMaximumHeightStructural"",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""MaximumHeightCharacteristic"": [
                            {
                              ""comparisonOperator"": ""lessThanOrEqualTo"",
                              ""vehicleHeight"": 7
                            }
                          ]
                        }
                      }
                    ]
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("HeightCharacteristicValidForRegulationTypeDimensionMaximumHeightStructural");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/5")]
    public async Task AllowLengthCharacteristicValidForRegulationTypeDimensionMaximumLength()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""regulationType"": ""dimensionMaximumLength"",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""MaximumLengthCharacteristic"": [
                            {
                              ""comparisonOperator"": ""lessThanOrEqualTo"",
                              ""vehicleLength"": 5
                            }
                          ]
                        }
                      }
                    ]
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("LengthCharacteristicValidForRegulationTypeDimensionMaximumLength");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/5")]
    public async Task DisallowLengthCharacteristicInvalidForRegulationTypeDimensionMaximumLength()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""regulationType"": ""dimensionMaximumLength"",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""MaximumLengthCharacteristic"": [
                            {
                              ""comparisonOperator"": ""lessThanOrEqualTo"",
                              ""vehicleLength"": 41
                            }
                          ]
                        }
                      }
                    ]
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("LengthCharacteristicValidForRegulationTypeDimensionMaximumLength");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/6")]
    public async Task AllowWidthCharacteristicValidForRegulationTypeDimensionMaximumWidth()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""regulationType"": ""dimensionMaximumWidth"",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""MaximumWidthCharacteristic"": [
                            {
                              ""comparisonOperator"": ""lessThanOrEqualTo"",
                              ""vehicleWidth"": 5
                            }
                          ]
                        }
                      }
                    ]
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("WidthCharacteristicValidForRegulationTypeDimensionMaximumWidth");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/6")]
    public async Task DisallowWidthCharacteristicInvalidForRegulationTypeDimensionMaximumWidth()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""regulationType"": ""dimensionMaximumWidth"",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""MaximumWidthCharacteristic"": [
                            {
                              ""comparisonOperator"": ""lessThanOrEqualTo"",
                              ""vehicleWidth"": 7
                            }
                          ]
                        }
                      }
                    ]
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("WidthCharacteristicValidForRegulationTypeDimensionMaximumWidth");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Theory]
    [Trait("RuleId", "DFT-205/7-8")]
    [InlineData("dimensionMaximumWeightEnvironmental")]
    [InlineData("dimensionMaximumWeightStructural")]
    public async Task AllowGrossWeightCharacteristicValidForRegulationTypes(string regulationType)
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""regulationType"": " + JsonSerializer.Serialize(regulationType) + @",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""MaximumGrossWeightCharacteristic"": [
                            {
                              ""comparisonOperator"": ""greaterThan"",
                              ""grossVehicleWeight"": 5
                            }
                          ]
                        }
                      }
                    ]
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("GrossWeightCharacteristicValidForRegulationTypes");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Theory]
    [Trait("RuleId", "DFT-205/7-8")]
    [InlineData("dimensionMaximumWeightEnvironmental")]
    [InlineData("dimensionMaximumWeightStructural")]
    public async Task DisallowGrossWeightCharacteristicInvalidForRegulationTypes(string regulationType)
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""regulationType"": " + JsonSerializer.Serialize(regulationType) + @",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""MaximumGrossWeightCharacteristic"": [
                            {
                              ""comparisonOperator"": ""greaterThan"",
                              ""grossVehicleWeight"": 51
                            }
                          ]
                        }
                      }
                    ]
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("GrossWeightCharacteristicValidForRegulationTypes");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Theory]
    [Trait("RuleId", "DFT-205/7-8")]
    [InlineData("dimensionMaximumWeightEnvironmental")]
    [InlineData("dimensionMaximumWeightStructural")]
    public async Task AllowHeaviestAxleWeightCharacteristicValidForRegulationTypes(string regulationType)
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""regulationType"": " + JsonSerializer.Serialize(regulationType) + @",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""HeaviestAxleWeightCharacteristic"": [
                            {
                              ""comparisonOperator"": ""greaterThan"",
                              ""heaviestAxleWeight"": 5
                            }
                          ]
                        }
                      }
                    ]
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("HeaviestAxleWeightCharacteristicValidForRegulationTypes");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Theory]
    [Trait("RuleId", "DFT-205/7-8")]
    [InlineData("dimensionMaximumWeightEnvironmental")]
    [InlineData("dimensionMaximumWeightStructural")]
    public async Task DisallowHeaviestAxleWeightCharacteristicInvalidForRegulationTypes(string regulationType)
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""regulationType"": " + JsonSerializer.Serialize(regulationType) + @",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""HeaviestAxleWeightCharacteristic"": [
                            {
                              ""comparisonOperator"": ""greaterThan"",
                              ""heaviestAxleWeight"": 51
                            }
                          ]
                        }
                      }
                    ]
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("HeaviestAxleWeightCharacteristicValidForRegulationTypes");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    [Trait("RuleId", "33")]
    public async Task AllowYearOfFirstRegistrationLessOrEqualToCurrentYearValueInConditions()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""yearOfFirstRegistration"": 2020
                        }
                      },
                      {
                        ""operator"": ""and"",
                        ""negate"": false,
                        ""conditions"": [
                          {
                            ""vehicleCharacteristics"": {
                              ""yearOfFirstRegistration"": 2019
                            }
                          }
                        ]
                      }
                    ]
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("YearOfFirstRegistrationLessOrEqualToCurrentYearValueInConditions");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    [Trait("RuleId", "33")]
    public async Task DisallowYearOfFirstRegistrationGreaterThanCurrentYearValueInConditions()
    {
        int year = DateTime.UtcNow.AddYears(1).Year;

        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""yearOfFirstRegistration"": 2019
                        }
                      },
                      {
                        ""operator"": ""and"",
                        ""negate"": false,
                        ""conditions"": [
                          {
                            ""vehicleCharacteristics"": {
                              ""yearOfFirstRegistration"": 2020
                            }
                          },
                          {
                            ""vehicleCharacteristics"": {
                              ""yearOfFirstRegistration"": " + JsonSerializer.Serialize(year) + @"
                            }
                          }
                        ]
                      }
                    ]
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("YearOfFirstRegistrationLessOrEqualToCurrentYearValueInConditions");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    [Trait("RuleId", "33")]
    public async Task AllowYearOfFirstRegistrationLessOrEqualToCurrentYearValueInOverallPeriod()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""timeValidity"": {
                      ""validityCondition"": {
                        ""conditionSet"": [
                          {
                            ""conditions"": [
                              {
                                ""vehicleCharacteristics"": {
                                  ""yearOfFirstRegistration"": 2019
                                }
                              },
                              {
                                ""operator"": ""and"",
                                ""negate"": false,
                                ""conditions"": [
                                  {
                                    ""vehicleCharacteristics"": {
                                      ""yearOfFirstRegistration"": 2010
                                    }
                                  }
                                ]
                              }
                            ]
                          }
                        ]
                      }
                    }
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("YearOfFirstRegistrationLessOrEqualToCurrentYearValueInOverallPeriod");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact(Skip = "Method is too complicated")]
    [Trait("RuleId", "33")]
    public async Task DisallowYearOfFirstRegistrationGreaterThanCurrentYearValueInOverallPeriod()
    {
        int year = DateTime.UtcNow.AddYears(1).Year;

        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""timeValidity"": {
                      ""validityCondition"": {
                        ""conditionSet"": [
                          {
                            ""conditions"": [
                              {
                                ""vehicleCharacteristics"": {
                                  ""yearOfFirstRegistration"": 2019
                                }
                              },
                              {
                                ""operator"": ""and"",
                                ""negate"": false,
                                ""conditions"": [
                                  {
                                    ""vehicleCharacteristics"": {
                                      ""yearOfFirstRegistration"": " + JsonSerializer.Serialize(year) + @"
                                    }
                                  }
                                ]
                              }
                            ]
                          }
                        ]
                      }
                    }
                  }
                ]
              }
            ]
          }
        }");

        await UseRulesByName("YearOfFirstRegistrationLessOrEqualToCurrentYearValueInOverallPeriod");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsUniqueWeekInMonthInPeriods()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""exceptionPeriod"": [{
                                ""recurringDayWeekMonthPeriod"": [
                                    {
                                        ""weekInMonth"": ""firstWeekOfMonth""
                                    },
                                    {
                                        ""weekInMonth"": ""secondWeekOfMonth""
                                    }
                                ]
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ExceptionPeriodWeekInMonthInstancesUnique");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsDuplicateWeekInMonthInPeriods()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""exceptionPeriod"": [{
                                ""recurringDayWeekMonthPeriod"": [
                                    {
                                        ""weekInMonth"": ""firstWeekOfMonth""
                                    },
                                    {
                                        ""weekInMonth"": ""firstWeekOfMonth""
                                    }
                                ]
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ExceptionPeriodWeekInMonthInstancesUnique");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task DisallowsDuplicateApplicableInstanceOfDayWithinMonthInPeriods()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""exceptionPeriod"": [{
                                ""recurringDayWeekMonthPeriod"": [
                                    {
                                        ""applicableInstanceOfDayWithinMonth"": ""firstWeekOfMonth""
                                    },
                                    {
                                        ""applicableInstanceOfDayWithinMonth"": ""firstWeekOfMonth""
                                    }
                                ]
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ExceptionPeriodApplicableInstanceOfDayWithinMonthInstancesUnique");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsUniqueApplicableInstanceOfDayWithinMonthInPeriods()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""exceptionPeriod"": [{
                                ""recurringDayWeekMonthPeriod"": [
                                    {
                                        ""applicableInstanceOfDayWithinMonth"": ""firstWeekOfMonth""
                                    },
                                    {
                                        ""applicableInstanceOfDayWithinMonth"": ""secondWeekOfMonth""
                                    }
                                ]
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ExceptionPeriodApplicableInstanceOfDayWithinMonthInstancesUnique");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }


    [Fact]
    public async Task DisallowsDuplicateApplicableWeekInPeriods()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""exceptionPeriod"": [{
                                ""recurringDayWeekMonthPeriod"": [
                                    {
                                        ""applicableWeek"": ""firstWeekOfMonth""
                                    },
                                    {
                                        ""applicableWeek"": ""firstWeekOfMonth""
                                    }
                                ]
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ExceptionPeriodApplicableWeekInstancesUnique");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsUniqueApplicableWeekInPeriods()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""exceptionPeriod"": [{
                                ""recurringDayWeekMonthPeriod"": [
                                    {
                                        ""applicableWeek"": ""firstWeekOfMonth""
                                    },
                                    {
                                        ""applicableWeek"": ""secondWeekOfMonth""
                                    }
                                ]
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ExceptionPeriodApplicableWeekInstancesUnique");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }


    [Fact]
    public async Task AllowsUniqueWeekInMonthInValidityPeriod()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""validityPeriod"": [{
                                ""recurringDayWeekMonthPeriod"": [
                                    {
                                        ""weekInMonth"": ""firstWeekOfMonth""
                                    },
                                    {
                                        ""weekInMonth"": ""secondWeekOfMonth""
                                    }
                                ]
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidityPeriodWeekInMonthInstancesUnique");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsDuplicateWeekInMonthInValidityPeriod()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""validityPeriod"": [{
                                ""recurringDayWeekMonthPeriod"": [
                                    {
                                        ""weekInMonth"": ""firstWeekOfMonth""
                                    },
                                    {
                                        ""weekInMonth"": ""firstWeekOfMonth""
                                    }
                                ]
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidityPeriodWeekInMonthInstancesUnique");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task DisallowsDuplicateApplicableInstanceOfDayWithinMonthInValidityPeriod()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""validityPeriod"": [{
                                ""recurringDayWeekMonthPeriod"": [
                                    {
                                        ""applicableInstanceOfDayWithinMonth"": ""firstWeekOfMonth""
                                    },
                                    {
                                        ""applicableInstanceOfDayWithinMonth"": ""firstWeekOfMonth""
                                    }
                                ]
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidityPeriodApplicableInstanceOfDayWithinMonthInstancesUnique");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsUniqueApplicableInstanceOfDayWithinMonthInValidityPeriod()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""validityPeriod"": [{
                                ""recurringDayWeekMonthPeriod"": [
                                    {
                                        ""applicableInstanceOfDayWithinMonth"": ""firstWeekOfMonth""
                                    },
                                    {
                                        ""applicableInstanceOfDayWithinMonth"": ""secondWeekOfMonth""
                                    }
                                ]
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidityPeriodApplicableInstanceOfDayWithinMonthInstancesUnique");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }


    [Fact]
    public async Task DisallowsDuplicateApplicableWeekInValidityPeriod()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""validityPeriod"": [{
                                ""recurringDayWeekMonthPeriod"": [
                                    {
                                        ""applicableWeek"": ""firstWeekOfMonth""
                                    },
                                    {
                                        ""applicableWeek"": ""firstWeekOfMonth""
                                    }
                                ]
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidityPeriodApplicableWeekInstancesUnique");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsUniqueApplicableWeekInValidityPeriod()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
{
    ""Source"": {
        ""provision"": [
            {   ""regulation"": [
                    {
                        ""timeValidity"": {
                            ""validityPeriod"": [{
                                ""recurringDayWeekMonthPeriod"": [
                                    {
                                        ""applicableWeek"": ""firstWeekOfMonth""
                                    },
                                    {
                                        ""applicableWeek"": ""secondWeekOfMonth""
                                    }
                                ]
                            }]
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName("ValidityPeriodApplicableWeekInstancesUnique");

        JsonLogicValidationService sut = new(_ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    private async Task UseRulesByName(params string[] names)
    {
        FileJsonLogicRuleSource Source = new();
        var rules = await Source.GetRules("rules-3.2.3");
        var subset = rules.Where(it => names.Contains(it.Name)).ToList();

        _ruleDal.Setup(it => it.GetRuleTemplateDeserializeAsync(It.IsAny<SchemaVersion>())).ReturnsAsync(subset);
    }
}