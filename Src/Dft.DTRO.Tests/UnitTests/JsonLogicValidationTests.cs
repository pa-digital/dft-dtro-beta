using DfT.DTRO.Extensions.DependencyInjection;
using DfT.DTRO.JsonLogic;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.Validation;
using DfT.DTRO.Services;
using DfT.DTRO.Services.Validation;
using Moq;
using System.Collections.Generic;
using System.Text.Json;

namespace Dft.DTRO.Tests
{
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
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task AllowsOverallPeriodEndTimeToBeMissing()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task DisallowsOverallPeriodEndTimeBeforeStartTime()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task AllowsValidPeriodStartTimeBeforeEndTime()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task DisallowsValidPeriodEndTimeBeforeStartTime()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task AllowsExceptionPeriodStartTimeBeforeEndTime()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task DisallowsExceptionPeriodEndTimeBeforeStartTime()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task AllowsExceptionPeriodStartTimeAfterOverallPeriodStartTime()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task DisallowsExceptionPeriodStartTimeBeforeOverallPeriodStartTime()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task AllowsExceptionPeriodEndTimeBeforeOverallPeriodEndTime()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task DisallowsExceptionPeriodEndTimeAfterOverallPeriodEndTime()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.NotEmpty(result);
        }



        [Fact]
        public async Task AllowsValidPeriodStartTimeAfterOverallPeriodStartTime()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task DisallowsValidPeriodStartTimeBeforeOverallPeriodStartTime()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task AllowsValidPeriodEndTimeBeforeOverallPeriodEndTime()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task DisallowsValidPeriodEndTimeAfterOverallPeriodEndTime()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task AllowsRecurringTimePeriodOfDayStartTimeBeforeEndTime()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task DisallowsRecurringTimePeriodOfDayEndTimeBeforeStartTime()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task AllowsRecurringTimePeriodOfDayToBeEmpty()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task AllowsRecurringTimePeriodOfDayToBeMissing()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }
        [Fact]
        public async Task AllowsValidPeriodToBeEmpty()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task AllowsExceptionPeriodToBeEmpty()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task AllowsValidPeriodToBeMissing()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task AllowsExceptionPeriodToBeMissing()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task AllowsTaInSwaRules()
        {
            var dtro = PrepareDtro(@"
        {
            ""source"": {
                ""ta"": 10
            }
        }");

            await UseRulesByName("TaInSwaCodes");

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task DisallowsTaNotInSwaRules()
        {
            var dtro = PrepareDtro(@"
        {
            ""source"": {
                ""ta"": 9
            }
        }");

            await UseRulesByName("TaInSwaCodes");

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task DisallowPublicationTimeMoreThanOneMonthOld()
        {
            var time = DateTime.UtcNow.AddMonths(-2);

            var dtro = PrepareDtro(@"
        {
            ""header"": {
                ""publicationTime"": " + JsonSerializer.Serialize(time) + @"
            }
        }");

            await UseRulesByName("PublicationTimeAge");

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task AllowPublicationTimeWithinOneMonthOld()
        {
            var time = DateTime.UtcNow.AddDays(-15);

            var dtro = PrepareDtro(@"
        {
            ""header"": {
                ""publicationTime"": " + JsonSerializer.Serialize(time) + @"
            }
        }");

            await UseRulesByName("PublicationTimeAge");

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task DisallowPublicationTimeFromTheFuture()
        {
            var time = DateTime.UtcNow.AddMonths(1);

            var dtro = PrepareDtro(@"
        {
            ""header"": {
                ""publicationTime"": " + JsonSerializer.Serialize(time) + @"
            }
        }");

            await UseRulesByName("PublicationTimeAge");

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.NotEmpty(result);
        }

        [Fact]
        [Trait("RuleId", "DFT-205/14")]
        public async Task AllowExternalReferenceLastUpdateDateInThePast()
        {
            DateTime time = DateTime.UtcNow.AddMonths(-2);

            var dtro = PrepareDtro(@"
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

            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
        {
        }");

            await UseRulesByName("PublicationTimeAge");

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task AllowValidUsagePeriodStartLessThanEnd()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task AllowValidUsagePeriodEndMissing()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task DisallowValidUsagePeriodEndLessThanStart()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task AllowMinTimeLessThanMaxTime()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task DisallowMaxTimeLessThanMinTime()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task AllowMaxTimeAndOrMinTimeMissing()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task DisllowValueCollectionMaxLessThanMin()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task AllowValueCollectionMinLessThanMax()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task AllowValueCollectionMinAndOrMaxMissing()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        [Trait("RuleId", "43")]
        public async Task DisallowValueMaxLessThanMin()
        {
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task DisallowsNonSequentialProvisionIndex()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task AllowSequentialRateLineCollection()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task DisallowNonSequentialRateLineCollection()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task AllowSequentialRateLine()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.Empty(result);
        }

        [Fact]
        public async Task DisallowNonSequentialRateLine()
        {
            var dtro = PrepareDtro(@"
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

            var sut = new JsonLogicValidationService(ruleDal.Object);

            var result = await sut.ValidateCreationRequest(dtro);

            Assert.NotEmpty(result);
        }

        [Fact]
        [Trait("RuleId", "DFT-205/3")]
        public async Task AllowHeightCharacteristicValidForRegulationTypeDimensionMaximumHeightWithTro()
        {
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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

            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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

            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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
            var dtro = PrepareDtro(@"
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

            var source = new FileJsonLogicRuleSource();
            var rules = await source.GetRules("dtro-3.2.0");

            ruleDal.Setup(it => it.GetRuleTemplateDeserializeAsync(It.IsAny<SchemaVersion>())).ReturnsAsync(rules);
        }

        private async Task UseRulesByName(params string[] names)
        {
            var source = new FileJsonLogicRuleSource();
            var rules = await source.GetRules("dtro-3.2.0");
            var subset = rules.Where(it => names.Contains(it.Name)).ToList();

            ruleDal.Setup(it => it.GetRuleTemplateDeserializeAsync(It.IsAny<SchemaVersion>())).ReturnsAsync(subset);
        }

        private async Task UseRulesByPath(params string[] paths)
        {
            var source = new FileJsonLogicRuleSource();
            var rules = await source.GetRules("dtro-3.2.0");
            var subset = rules.Where(it => paths.Any(path => it.Path.StartsWith(path))).ToList();

            ruleDal.Setup(it => it.GetRuleTemplateDeserializeAsync(It.IsAny<SchemaVersion>())).ReturnsAsync(subset);
        }

        private async Task UseRulesByIndex(params int[] indexes)
        {
            var source = new FileJsonLogicRuleSource();
            var rules = await source.GetRules("dtro-3.2.0");

            if (indexes.Max() >= rules.Count())
            {
                throw new InvalidOperationException();
            }

            var selected = new List<JsonLogicValidationRule>();

            foreach (var index in indexes)
            {
                selected.Add(rules.ElementAt(index));
            }

            ruleDal.Setup(it => it.GetRuleTemplateDeserializeAsync(It.IsAny<SchemaVersion>())).ReturnsAsync(selected);
        }
    }
}