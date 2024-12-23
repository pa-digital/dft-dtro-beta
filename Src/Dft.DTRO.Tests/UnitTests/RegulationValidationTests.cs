namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class RegulationValidationTests
{
    private readonly IRegulationValidation _sut = new RegulationValidation();

    [Fact]
    public void ValidateRegulationReturnsNoErrors()
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""Provision"": [
              {
                ""Regulation"": [
                  {
                  ""GeneralRegulation"":  {
                    }
                  }
                ]
              }
            ]
          }
        }", new SchemaVersion("3.3.0"));

        IList<SemanticValidationError>? actual = _sut.ValidateRegulation(dtroSubmit, "3.3.0");
        Assert.Equal(0, actual.Count);
    }

    [Fact]
    public void ValidateRegulationReturnsErrorsWhenMultipleRegulations()
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""Provision"": [
              {
                ""Regulation"": [
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
        }", new SchemaVersion("3.3.0"));

        IList<SemanticValidationError>? actual = _sut.ValidateRegulation(dtroSubmit, "3.3.0");
        Assert.NotEmpty(actual);
    }

    [Fact]
    public void ValidateRegulationReturnsNoErrorsWhenMultipleRegulationsWithTemporaryRegulation()
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""Provision"": [
              {
                ""Regulation"": [
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
        }", new SchemaVersion("3.3.0"));

        IList<SemanticValidationError>? actual = _sut.ValidateRegulation(dtroSubmit, "3.3.0");
        Assert.Empty(actual);
    }

    [Fact]
    public void ValidateRegulationReturnsErrorsWhenWrongRegulation()
    {
        DtroSubmit dtroSubmit = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""Provision"": [
              {
                ""Regulation"": [
                  {
                  ""RandomRegulation"":  {
                    }
                  }
                ]
              }
            ]
          }
        }", new SchemaVersion("3.3.0"));

        IList<SemanticValidationError>? actual = _sut.ValidateRegulation(dtroSubmit, "3.3.0");
        Assert.NotEmpty(actual);
    }
}