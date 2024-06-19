using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DfT.DTRO.Extensions.DependencyInjection;
using DfT.DTRO.JsonLogic;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.Validation;
using DfT.DTRO.Services;
using DfT.DTRO.Services.Validation;

namespace Dft.DTRO.Tests;

[ExcludeFromCodeCoverage]
public class JsonLogicValidationTests
{
    private readonly Mock<IRuleTemplateDal> ruleDal = new();

    public JsonLogicValidationTests()
    {
        JsonLogicDIExtensions.AddAllRules(typeof(IJsonLogicRuleSource).Assembly);
    }

    [Fact]
    public async Task AllowsOverallPeriodStartTimeBeforeEndTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {   ""regulations"": [
                            {
                                ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowsOverallPeriodEndTimeToBeMissing()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {   ""regulations"": [
                            {
                                ""overallPeriod"": {
                                    ""start"": ""1983-01-05T22:50:50.0Z""
                                }
                            }
                        ]
                    }
                ]
            }
        }");

        await UseRulesByName("OverallPeriodStartLessThanEnd");

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsOverallPeriodEndTimeBeforeStartTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {   ""regulations"": [
                            {
                                ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsValidPeriodStartTimeBeforeEndTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsValidPeriodEndTimeBeforeStartTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsExceptionPeriodStartTimeBeforeEndTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsExceptionPeriodEndTimeBeforeStartTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsExceptionPeriodStartTimeAfterOverallPeriodStartTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsExceptionPeriodStartTimeBeforeOverallPeriodStartTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsExceptionPeriodEndTimeBeforeOverallPeriodEndTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsExceptionPeriodEndTimeAfterOverallPeriodEndTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }


    [Fact]
    public async Task AllowsValidPeriodStartTimeAfterOverallPeriodStartTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsValidPeriodStartTimeBeforeOverallPeriodStartTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsValidPeriodEndTimeBeforeOverallPeriodEndTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsValidPeriodEndTimeAfterOverallPeriodEndTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsRecurringTimePeriodOfDayStartTimeBeforeEndTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        await UseRulesByName(
            "ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd",
            "ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd");

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsRecurringTimePeriodOfDayEndTimeBeforeStartTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        await UseRulesByName(
            "ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd",
            "ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd");

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task AllowsRecurringTimePeriodOfDayToBeEmpty()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        await UseRulesByName(
            "ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd",
            "ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd");

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowsRecurringTimePeriodOfDayToBeMissing()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        await UseRulesByName(
            "ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd",
            "ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd");

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowsValidPeriodToBeEmpty()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
                            ""validPeriod"": []
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName(
            "ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd",
            "ValidPeriodStartLessThanEnd");

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowsExceptionPeriodToBeEmpty()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
                            ""exceptionPeriod"": []
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName(
            "ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd",
            "ExceptionPeriodStartLessThanEnd");

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowsValidPeriodToBeMissing()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName(
            "ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd",
            "ValidPeriodStartLessThanEnd");

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowsExceptionPeriodToBeMissing()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
                        }
                    }
                ]
            }
        ]
    }
}");

        await UseRulesByName(
            "ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd",
            "ExceptionPeriodStartLessThanEnd");

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowsTaInSwaRules()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""ta"": 10
            }
        }");

        await UseRulesByName("TaInSwaCodes");

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsTaNotInSwaRules()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""ta"": 9
            }
        }");

        await UseRulesByName("TaInSwaCodes");

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task DisallowPublicationTimeMoreThanOneMonthOld()
    {
        DateTime time = DateTime.UtcNow.AddMonths(-2);

        DtroSubmit dtro = PrepareDtro(@"
        {
            ""header"": {
                ""publicationTime"": " + JsonSerializer.Serialize(time) + @"
            }
        }");

        await UseRulesByName("PublicationTimeAge");

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowPublicationTimeWithinOneMonthOld()
    {
        DateTime time = DateTime.UtcNow.AddDays(-15);

        DtroSubmit dtro = PrepareDtro(@"
        {
            ""header"": {
                ""publicationTime"": " + JsonSerializer.Serialize(time) + @"
            }
        }");

        await UseRulesByName("PublicationTimeAge");

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowPublicationTimeFromTheFuture()
    {
        DateTime time = DateTime.UtcNow.AddMonths(1);

        DtroSubmit dtro = PrepareDtro(@"
        {
            ""header"": {
                ""publicationTime"": " + JsonSerializer.Serialize(time) + @"
            }
        }");

        await UseRulesByName("PublicationTimeAge");

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/14")]
    public async Task AllowExternalReferenceLastUpdateDateInThePast()
    {
        DateTime time = DateTime.UtcNow.AddMonths(-2);

        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulatedPlaces"": [
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/14")]
    public async Task DisallowExternalReferenceLastUpdateDateFromTheFuture()
    {
        DateTime time = DateTime.UtcNow.AddMonths(2);

        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulatedPlaces"": [
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowHeaderMissing()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
        }");

        await UseRulesByName("PublicationTimeAge");

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowValidUsagePeriodStartLessThanEnd()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {   ""regulations"": [
                            {
                                ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowValidUsagePeriodEndMissing()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {   ""regulations"": [
                            {
                                ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowValidUsagePeriodEndLessThanStart()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {   ""regulations"": [
                            {
                                ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowMinTimeLessThanMaxTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {   ""regulations"": [
                            {
                                ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowMaxTimeLessThanMinTime()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {   ""regulations"": [
                            {
                                ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowMaxTimeAndOrMinTimeMissing()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {   ""regulations"": [
                            {
                                ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisllowValueCollectionMaxLessThanMin()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {   ""regulations"": [
                            {
                                ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowValueCollectionMinLessThanMax()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {   ""regulations"": [
                            {
                                ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowValueCollectionMinAndOrMaxMissing()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {   ""regulations"": [
                            {
                                ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    [Trait("RuleId", "43")]
    public async Task DisallowValueMaxLessThanMin()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
                  {
                    ""overallPeriod"": {
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    [Trait("RuleId", "43")]
    public async Task AllowValueMinLessThanMax()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
                  {
                    ""overallPeriod"": {
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowSequentialProvisionIndex()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsNonSequentialProvisionIndex()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {   
                        ""provisionIndex"": 0
                    },
                    {
                        ""provisionIndex"": 2
                    },
                    {   
                        ""provisionIndex"": 3
                    }
                ]
            }
        }");

        await UseRulesByName("ProvisionIndexSequential");

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowSequentialRateLineCollection()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {   ""regulations"": [
                            {
                                ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowNonSequentialRateLineCollection()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {   ""regulations"": [
                            {
                                ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowSequentialRateLine()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {   ""regulations"": [
                            {
                                ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowNonSequentialRateLine()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
            ""source"": {
                ""provision"": [
                    {   ""regulations"": [
                            {
                                ""overallPeriod"": {
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

        JsonLogicValidationService sut = new JsonLogicValidationService(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/3")]
    public async Task AllowHeightCharacteristicValidForRegulationTypeDimensionMaximumHeightWithTro()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
                  {
                    ""regulationType"": ""dimensionMaximumHeightWithTRO"",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""heightCharacteristic"": [
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/3")]
    public async Task DisallowHeightCharacteristicInvalidForRegulationTypeDimensionMaximumHeightWithTro()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/4")]
    public async Task AllowHeightCharacteristicValidForRegulationTypeDimensionMaximumHeightStructural()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
                  {
                    ""regulationType"": ""dimensionMaximumHeightStructural"",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""heightCharacteristic"": [
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/4")]
    public async Task DisallowHeightCharacteristicInvalidForRegulationTypeDimensionMaximumHeightStructural()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
                  {
                    ""regulationType"": ""dimensionMaximumHeightStructural"",
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

        await UseRulesByName("HeightCharacteristicValidForRegulationTypeDimensionMaximumHeightStructural");

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/5")]
    public async Task AllowLengthCharacteristicValidForRegulationTypeDimensionMaximumLength()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
                  {
                    ""regulationType"": ""dimensionMaximumLength"",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""lengthCharacteristic"": [
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/5")]
    public async Task DisallowLengthCharacteristicInvalidForRegulationTypeDimensionMaximumLength()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
                  {
                    ""regulationType"": ""dimensionMaximumLength"",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""lengthCharacteristic"": [
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/6")]
    public async Task AllowWidthCharacteristicValidForRegulationTypeDimensionMaximumWidth()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
                  {
                    ""regulationType"": ""dimensionMaximumWidth"",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""widthCharacteristic"": [
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    [Trait("RuleId", "DFT-205/6")]
    public async Task DisallowWidthCharacteristicInvalidForRegulationTypeDimensionMaximumWidth()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
                  {
                    ""regulationType"": ""dimensionMaximumWidth"",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""widthCharacteristic"": [
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Theory]
    [Trait("RuleId", "DFT-205/7-8")]
    [InlineData("dimensionMaximumWeightEnvironmental")]
    [InlineData("dimensionMaximumWeightStructural")]
    public async Task AllowGrossWeightCharacteristicValidForRegulationTypes(string regulationType)
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
                  {
                    ""regulationType"": " + JsonSerializer.Serialize(regulationType) + @",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""grossWeightCharacteristic"": [
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Theory]
    [Trait("RuleId", "DFT-205/7-8")]
    [InlineData("dimensionMaximumWeightEnvironmental")]
    [InlineData("dimensionMaximumWeightStructural")]
    public async Task DisallowGrossWeightCharacteristicInvalidForRegulationTypes(string regulationType)
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
                  {
                    ""regulationType"": " + JsonSerializer.Serialize(regulationType) + @",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""grossWeightCharacteristic"": [
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Theory]
    [Trait("RuleId", "DFT-205/7-8")]
    [InlineData("dimensionMaximumWeightEnvironmental")]
    [InlineData("dimensionMaximumWeightStructural")]
    public async Task AllowHeaviestAxleWeightCharacteristicValidForRegulationTypes(string regulationType)
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
                  {
                    ""regulationType"": " + JsonSerializer.Serialize(regulationType) + @",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""heaviestAxleWeightCharacteristic"": [
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Theory]
    [Trait("RuleId", "DFT-205/7-8")]
    [InlineData("dimensionMaximumWeightEnvironmental")]
    [InlineData("dimensionMaximumWeightStructural")]
    public async Task DisallowHeaviestAxleWeightCharacteristicInvalidForRegulationTypes(string regulationType)
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
                  {
                    ""regulationType"": " + JsonSerializer.Serialize(regulationType) + @",
                    ""conditions"": [
                      {
                        ""vehicleCharacteristics"": {
                          ""heaviestAxleWeightCharacteristic"": [
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    [Trait("RuleId", "33")]
    public async Task AllowYearOfFirstRegistrationLessOrEqualToCurrentYearValueInConditions()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    [Trait("RuleId", "33")]
    public async Task DisallowYearOfFirstRegistrationGreaterThanCurrentYearValueInConditions()
    {
        int year = DateTime.UtcNow.AddYears(1).Year;

        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    [Trait("RuleId", "33")]
    public async Task AllowYearOfFirstRegistrationLessOrEqualToCurrentYearValueInOverallPeriod()
    {
        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
                  {
                    ""overallPeriod"": {
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    [Trait("RuleId", "33")]
    public async Task DisallowYearOfFirstRegistrationGreaterThanCurrentYearValueInOverallPeriod()
    {
        int year = DateTime.UtcNow.AddYears(1).Year;

        DtroSubmit dtro = PrepareDtro(@"
        {
          ""source"": {
            ""provision"": [
              {
                ""regulations"": [
                  {
                    ""overallPeriod"": {
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsUniqueWeekInMonthInPeriods()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisllowsDuplicateWeekInMonthInPeriods()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task DisllowsDuplicateApplicableInstanceOfDayWithinMonthInPeriods()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsUniqueApplicableInstanceOfDayWithinMonthInPeriods()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }


    [Fact]
    public async Task DisllowsDuplicateApplicableWeekInPeriods()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsUniqueApplicableWeekInPeriods()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }


    [Fact]
    public async Task AllowsUniqueWeekInMonthInValidityPeriod()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisllowsDuplicateWeekInMonthInValidityPeriod()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task DisllowsDuplicateApplicableInstanceOfDayWithinMonthInValidityPeriod()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsUniqueApplicableInstanceOfDayWithinMonthInValidityPeriod()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }


    [Fact]
    public async Task DisllowsDuplicateApplicableWeekInValidityPeriod()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsUniqueApplicableWeekInValidityPeriod()
    {
        DtroSubmit dtro = PrepareDtro(@"
{
    ""source"": {
        ""provision"": [
            {   ""regulations"": [
                    {
                        ""overallPeriod"": {
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

        JsonLogicValidationService sut = new(ruleDal.Object);

        IList<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    private async Task UseAllRules()
    {
        FileJsonLogicRuleSource source = new FileJsonLogicRuleSource();
        IEnumerable<JsonLogicValidationRule>? rules = await source.GetRules("dtro-3.2.0");

        ruleDal.Setup(it => it.GetRuleTemplateDeserializeAsync(It.IsAny<SchemaVersion>())).ReturnsAsync(rules);
    }

    private async Task UseRulesByName(params string[] names)
    {
        FileJsonLogicRuleSource source = new FileJsonLogicRuleSource();
        IEnumerable<JsonLogicValidationRule>? rules = await source.GetRules("dtro-3.2.0");
        List<JsonLogicValidationRule> subset = rules.Where(it => names.Contains(it.Name)).ToList();

        ruleDal.Setup(it => it.GetRuleTemplateDeserializeAsync(It.IsAny<SchemaVersion>())).ReturnsAsync(subset);
    }

    private async Task UseRulesByPath(params string[] paths)
    {
        FileJsonLogicRuleSource source = new FileJsonLogicRuleSource();
        IEnumerable<JsonLogicValidationRule>? rules = await source.GetRules("dtro-3.2.0");
        List<JsonLogicValidationRule> subset = rules.Where(it => paths.Any(path => it.Path.StartsWith(path))).ToList();

        ruleDal.Setup(it => it.GetRuleTemplateDeserializeAsync(It.IsAny<SchemaVersion>())).ReturnsAsync(subset);
    }

    private async Task UseRulesByIndex(params int[] indexes)
    {
        FileJsonLogicRuleSource source = new FileJsonLogicRuleSource();
        IEnumerable<JsonLogicValidationRule>? rules = await source.GetRules("dtro-3.2.0");

        if (indexes.Max() >= rules.Count())
        {
            throw new InvalidOperationException();
        }

        List<JsonLogicValidationRule> selected = new List<JsonLogicValidationRule>();

        foreach (int index in indexes)
        {
            selected.Add(rules.ElementAt(index));
        }

        ruleDal.Setup(it => it.GetRuleTemplateDeserializeAsync(It.IsAny<SchemaVersion>())).ReturnsAsync(selected);
    }
}