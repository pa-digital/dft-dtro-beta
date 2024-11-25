namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class ConditionValidationTests
{
    private readonly IConditionValidation _sut = new ConditionValidation();

    [Theory]
    [InlineData("3.2.4", 0)]
    [InlineData("3.3.0", 0)]
    public void ValidateConditionReturnsNoErrorsWhenConditionSetIsPresent(string version, int errorCount)
    {
        SchemaVersion schemaVersion = (version);
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {
                        ""regulation"": [
                            {
                                ""conditionSet"": [
                                    {
                                        ""operator"": ""and"",
                                        ""condition"": {
                                            ""conditionSet"": [
                                                {
                                                    ""condition"": {
                                                        ""RoadCondition"": {
                                                            
                                                        }
                                                    }
                                                }
                                            ],
                                            ""TimeValidity"": {

                                            }
                                        }
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        }", schemaVersion);

        var actual = _sut.ValidateCondition(dtroSubmit, schemaVersion);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.2.4", 0)]
    [InlineData("3.3.0", 1)]
    public void ValidateConditionReturnsErrorsWhenMultipleConditionsIsPresentWithoutConditionSet(string version, int errorCount)
    {
        SchemaVersion schemaVersion = (version);
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {
                        ""regulation"": [
                            {
                                ""condition"": {
                                    ""TimeValidity"": {

                                    },
                                    ""RoadCondition"": {

                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }", schemaVersion);

        var actual = _sut.ValidateCondition(dtroSubmit, schemaVersion);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.2.4", 0)]
    [InlineData("3.3.0", 0)]
    public void ValidateConditionReturnsNoErrorsWhenOneConditionIsPresent(string version, int errorCount)
    {
        SchemaVersion schemaVersion = new(version);

        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {
                        ""regulation"": [
                            {
                                ""condition"": {
                                    ""TimeValidity"": {

                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }", schemaVersion);

        var actual = _sut.ValidateCondition(dtroSubmit, schemaVersion);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.2.4", 0)]
    [InlineData("3.3.0", 1)]
    public void ValidateConditionReturnsErrorsWhenWrongConditionIsPresent(string version, int errorCount)
    {
        SchemaVersion schemaVersion = new(version);

        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
            ""Source"": {
                ""provision"": [
                    {
                        ""regulation"": [
                            {
                                ""condition"": {
                                    ""SomeCondition"": {

                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        }", schemaVersion);

        var actual = _sut.ValidateCondition(dtroSubmit, schemaVersion);
        Assert.Equal(errorCount, actual.Count);
    }
}