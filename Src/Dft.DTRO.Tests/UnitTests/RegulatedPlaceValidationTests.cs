using DfT.DTRO.Services.Validation.Contracts;

namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class RegulatedPlaceValidationTests
{
    private readonly IRegulatedPlaceValidation _sut = new RegulatedPlaceValidation();

    [Theory]
    [InlineData("3.2.4", 0)]
    [InlineData("3.2.5", 0)]
    [InlineData("3.3.0", 0)]
    public void ValidateRegulatedPlacesTypesReturnsNoErrors(string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulatedPlace"": [
                  {
                  ""type"":  ""regulationLocation""
                  },
                  {
                  ""type"":  ""diversionRoute""
                  }
                ]
              }
            ]
          }
        }", new SchemaVersion(version));

        var actual = _sut.ValidateRegulatedPlacesType(dtroSubmit, version);
        Assert.Equal(errorCount, actual.Count);
    }

    [Fact]
    public void ValidateRegulatedPlacesTypesReturnsErrorsWhenFirstWrongType()
    {
        var dtroSubmit = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""provision"": [
              {
                ""regulatedPlace"": [
                  {
                  ""type"":  ""diversionRoute""
                  },
                  {
                  ""type"":  ""regulationLocation""
                  }
                ]
              }
            ]
          }
        }", new SchemaVersion("3.2.5"));

        var actual = _sut.ValidateRegulatedPlacesType(dtroSubmit, "3.2.5");
        Assert.NotEmpty(actual);
    }
}
