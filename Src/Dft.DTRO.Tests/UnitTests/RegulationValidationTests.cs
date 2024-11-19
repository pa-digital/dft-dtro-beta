using DfT.DTRO.Services.Validation.Contracts;

namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class RegulationValidationTests
{
    private readonly IRegulationValidation _sut = new RegulationValidation();

    [Theory]
    [InlineData("3.2.4", 0)]
    [InlineData("3.2.5", 0)]
    [InlineData("3.3.0", 0)]
    public void ValidateRegulationReturnsNoErrors(string version, int errorCount)
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                  ""GeneralRegulation"":  {
                    }
                  }
                ]
              }
            ]
          }
        }", new SchemaVersion(version));

        IList<SemanticValidationError>? actual = _sut.ValidateRegulation(dtroSubmit, version);
        Assert.Equal(errorCount, actual.Count);
    }

    [Fact]
    public void ValidateRegulationReturnsErrorsWhenMultipleRegulations()
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                  ""GeneralRegulation"":  {
                    }
                  },
                  {
                    ""OffListRegulation"": {
                    }
                  }
                ]
              }
            ]
          }
        }", new SchemaVersion("3.2.5"));

        IList<SemanticValidationError>? actual = _sut.ValidateRegulation(dtroSubmit, "3.2.5");
        Assert.NotEmpty(actual);
    }

    [Fact]
    public void ValidateRegulationReturnsNoErrorsWhenMultipleRegulationsWithTemporaryRegulation()
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                  ""GeneralRegulation"":  {
                    }
                  },
                  {
                    ""TemporaryRegulation"": {
                    }
                  }
                ]
              }
            ]
          }
        }", new SchemaVersion("3.2.5"));

        IList<SemanticValidationError>? actual = _sut.ValidateRegulation(dtroSubmit, "3.2.5");
        Assert.Empty(actual);
    }

    [Fact]
    public void ValidateRegulationReturnsErrorsWhenWrongRegulation()
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulation"": [
                  {
                  ""RandomRegulation"":  {
                    }
                  }
                ]
              }
            ]
          }
        }", new SchemaVersion("3.2.5"));

        IList<SemanticValidationError>? actual = _sut.ValidateRegulation(dtroSubmit, "3.2.5");
        Assert.NotEmpty(actual);
    }
}