namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class RegulatedPlaceValidationTests
{
    private readonly IRegulatedPlaceValidation _sut = new RegulatedPlaceValidation();

    [Theory]
    [InlineData("3.2.4", 0)]
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

    [Fact]
    public void ValidateRegulatedPlacesTypesReturnsErrorsWhenFirstWrongType()
    {
        var dtroSubmit = Utils.PrepareDtro(@"
        {
          ""Source"": {
            ""Provision"": [
              {
                ""RegulatedPlace"": [
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
        }", new SchemaVersion("3.3.0"));

        var actual = _sut.ValidateRegulatedPlacesType(dtroSubmit, "3.3.0");
        Assert.NotEmpty(actual);
    }
}
