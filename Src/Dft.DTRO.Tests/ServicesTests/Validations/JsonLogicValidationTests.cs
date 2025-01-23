using System.Text.Json;

namespace Dft.DTRO.Tests.ServicesTests.Validations;

[ExcludeFromCodeCoverage]
public class JsonLogicValidationTests
{
    private readonly Mock<IRuleTemplateDal> _ruleDal = new();
    private readonly IRulesValidation _sut;

    public JsonLogicValidationTests()
    {
        ValidationServiceRegistration.AddAllRules(typeof(IJsonLogicRuleSource).Assembly);

        _sut = new RulesValidation(_ruleDal.Object);
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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("OverallPeriodStartLessThanEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("OverallPeriodStartLessThanEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("OverallPeriodStartLessThanEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidPeriodStartLessThanEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidPeriodStartLessThanEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ExceptionPeriodStartLessThanEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ExceptionPeriodStartLessThanEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ExceptionPeriodStartNotLessThanOverallPeriodStart");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ExceptionPeriodStartNotLessThanOverallPeriodStart");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ExceptionPeriodEndNotMoreThanOverallPeriodEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ExceptionPeriodEndNotMoreThanOverallPeriodEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidPeriodStartNotLessThanOverallPeriodStart");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidPeriodStartNotLessThanOverallPeriodStart");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidPeriodEndNotMoreThanOverallPeriodEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidPeriodEndNotMoreThanOverallPeriodEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd",
            "ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd",
            "ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd",
            "ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd",
            "ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd", "ValidPeriodStartLessThanEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd",
            "ExceptionPeriodStartLessThanEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidPeriodRecurringTimePeriodOfDayStartLessThanEnd", "ValidPeriodStartLessThanEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ExceptionPeriodRecurringTimePeriodOfDayStartLessThanEnd",
            "ExceptionPeriodStartLessThanEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("TaInSwaCodes");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("PublicationTimeAge");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("ExternalReferenceLastUpdateDate");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }


    [Fact]
    public async Task AllowHeaderMissing()
    {
        DtroSubmit dtro = Utils.PrepareDtro(@"
        {
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("PublicationTimeAge");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidUsagePeriodStartLessThanEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidUsagePeriodStartLessThanEnd");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("MinTimeLessThanMaxTime");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("MinTimeLessThanMaxTime");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("MinValueCollectionLessThanMax");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("MinValueCollectionLessThanMax");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("ProvisionIndexSequential");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("RateLineCollectionSequential");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("RateLineSequential");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("HeightCharacteristicValidForRegulationTypeDimensionMaximumHeightWithTRO");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("HeightCharacteristicValidForRegulationTypeDimensionMaximumHeightWithTRO");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("HeightCharacteristicValidForRegulationTypeDimensionMaximumHeightStructural");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("HeightCharacteristicValidForRegulationTypeDimensionMaximumHeightStructural");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("LengthCharacteristicValidForRegulationTypeDimensionMaximumLength");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("LengthCharacteristicValidForRegulationTypeDimensionMaximumLength");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("WidthCharacteristicValidForRegulationTypeDimensionMaximumWidth");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("WidthCharacteristicValidForRegulationTypeDimensionMaximumWidth");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("GrossWeightCharacteristicValidForRegulationTypes");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("GrossWeightCharacteristicValidForRegulationTypes");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("HeaviestAxleWeightCharacteristicValidForRegulationTypes");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("HeaviestAxleWeightCharacteristicValidForRegulationTypes");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("YearOfFirstRegistrationLessOrEqualToCurrentYearValueInConditions");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("YearOfFirstRegistrationLessOrEqualToCurrentYearValueInConditions");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
        }", new SchemaVersion("3.2.3"));

        await UseRulesByName("YearOfFirstRegistrationLessOrEqualToCurrentYearValueInOverallPeriod");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ExceptionPeriodWeekInMonthInstancesUnique");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ExceptionPeriodWeekInMonthInstancesUnique");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ExceptionPeriodApplicableInstanceOfDayWithinMonthInstancesUnique");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ExceptionPeriodApplicableInstanceOfDayWithinMonthInstancesUnique");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ExceptionPeriodApplicableWeekInstancesUnique");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ExceptionPeriodApplicableWeekInstancesUnique");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidityPeriodWeekInMonthInstancesUnique");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidityPeriodWeekInMonthInstancesUnique");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidityPeriodApplicableInstanceOfDayWithinMonthInstancesUnique");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidityPeriodApplicableInstanceOfDayWithinMonthInstancesUnique");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidityPeriodApplicableWeekInstancesUnique");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

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
}", new SchemaVersion("3.2.3"));

        await UseRulesByName("ValidityPeriodApplicableWeekInstancesUnique");



        IList<SemanticValidationError>? result = await _sut.ValidateRules(dtro, "3.2.3");

        Assert.Empty(result);
    }

    private async Task UseRulesByName(params string[] names)
    {
        FileJsonLogicRuleSource source = new();
        IEnumerable<JsonLogicValidationRule>? rules = await source.GetRules("rules-3.2.3");
        List<JsonLogicValidationRule> subset = rules.Where(it => names.Contains(it.Name)).ToList();

        _ruleDal.Setup(it => it.GetRuleTemplateDeserializeAsync(It.IsAny<SchemaVersion>())).ReturnsAsync(subset);
    }
}