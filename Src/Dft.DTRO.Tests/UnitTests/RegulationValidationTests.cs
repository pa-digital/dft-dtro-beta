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
        var dtroSubmit = Utils.PrepareDtro(@"
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

        var actual = _sut.ValidateRegulation(dtroSubmit, version);
        Assert.Equal(errorCount, actual.Count);
    }

    [Fact]
    public void ValidateRegulationReturnsErrorsWhenMultipleRegulations()
    {
        var dtroSubmit = Utils.PrepareDtro(@"
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

        var actual = _sut.ValidateRegulation(dtroSubmit, "3.2.5");
        Assert.NotEmpty(actual);
    }

    [Fact]
    public void ValidateRegulationReturnsErrorsWhenWrongRegulation()
    {
        var dtroSubmit = Utils.PrepareDtro(@"
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

        var actual = _sut.ValidateRegulation(dtroSubmit, "3.2.5");
        Assert.NotEmpty(actual);
    }
}