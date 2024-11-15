using DfT.DTRO.Services.Validation.Contracts;

namespace Dft.DTRO.Tests.UnitTests;

public class RegulatedPlaceValidationTests
{
    private readonly IRegulatedPlaceValidation _sut;

    public RegulatedPlaceValidationTests()
    {
        _sut = new RegulatedPlaceValidation();
    }

    [Fact]
    public void ValidateRegulatedPlacesTypesReturnsNoErrors()
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
        }", new SchemaVersion("3.2.5"));

        var actual = _sut.ValidateRegulatedPlacesType(dtroSubmit, "3.2.5");
        Assert.Empty(actual);
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
