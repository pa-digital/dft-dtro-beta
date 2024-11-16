using DfT.DTRO.Services.Validation.Contracts;

namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class ConditionValidationTests
{
    private readonly IConditionValidation _sut = new ConditionValidation();

    [Theory]
    [InlineData("3.2.4", 0)]
    [InlineData("3.2.5", 0)]
    [InlineData("3.3.0", 0)]
    public void ValidateConditionReturnsNoErrorsWhenSingleCondition(string version, int errorCount)
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""condition"":  [
                        {
                            ""ValidityCondition"": 
                            {
                                
                            }
                        }
                    ]
                  }
                ]
              }
            ]
          }
        }", new SchemaVersion(version));

        IList<SemanticValidationError>? actual = _sut.ValidateCondition(dtroSubmit, version);
        Assert.Equal(errorCount, actual.Count);
    }

    [Fact]
    public void ValidateConditionReturnsErrorsWhenUnknownSingleCondition()
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""condition"":  [
                        {
                            ""RandomCondition"": 
                            {
                                
                            }
                        }
                    ]
                  }
                ]
              }
            ]
          }
        }", new SchemaVersion("3.2.5"));

        IList<SemanticValidationError>? actual = _sut.ValidateCondition(dtroSubmit, "3.2.5");
        Assert.NotEmpty(actual);
    }

    [Fact]
    public void ValidateConditionReturnsNoErrorsWhenConditionSetIsPresent()
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""conditionSet"":  [
                        {
                            ""operator"": ""and"",
                            ""condition"": 
                            {
                                ""ValidityCondition"": 
                                {
                                    
                                },
                                ""VehicleCondition"": 
                                {
                                    
                                }
                            }
                        }
                    ]
                  }
                ]
              }
            ]
          }
        }", new SchemaVersion("3.2.5"));

        IList<SemanticValidationError>? actual = _sut.ValidateCondition(dtroSubmit, "3.2.5");
        Assert.Empty(actual);
    }

    [Fact]
    public void ValidateConditionReturnsErrorsWhenOperatorIsWrong()
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                    ""conditionSet"":  [
                        {
                            ""operator"": ""test"",
                            ""condition"": 
                            {
                                ""ValidityCondition"": 
                                {
                                    
                                },
                                ""VehicleCondition"": 
                                {
                                    
                                }
                            }
                        }
                    ]
                  }
                ]
              }
            ]
          }
        }", new SchemaVersion("3.2.5"));

        IList<SemanticValidationError>? actual = _sut.ValidateCondition(dtroSubmit, "3.2.5");
        Assert.NotEmpty(actual);
    }

    [Fact]
    public void ValidateConditionReturnsErrorsWhenOneConditionWithinConditionSetAreWrong()
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
          ""Source"": 
          {
            ""provision"": 
            [
              {
                ""regulation"": 
                [
                  {
                    ""conditionSet"":
                    [
                        {
                            ""operator"": ""and"",
                            ""condition"": 
                            {
                                ""ValidityCondition"": 
                                {
                                    
                                },
                                ""BadCondition"": 
                                {
                                    
                                }
                            }
                        }
                    ]
                  }
                ]
              }
            ]
          }
        }", new SchemaVersion("3.2.5"));

        IList<SemanticValidationError>? actual = _sut.ValidateCondition(dtroSubmit, "3.2.5");
        Assert.NotEmpty(actual);
    }
}