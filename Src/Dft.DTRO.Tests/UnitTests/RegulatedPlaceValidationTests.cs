namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class RegulatedPlaceValidationTests
{
    private readonly IRegulatedPlaceValidation _sut = new RegulatedPlaceValidation();

    [Theory]
    [InlineData("3.3.0", 0)]
    [InlineData("4.0.0", 0)]
    public void ValidateRegulatedPlacesTypesReturnsNoErrors(string version, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""Provision"": [
              {
                ""RegulatedPlace"": [
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
}
