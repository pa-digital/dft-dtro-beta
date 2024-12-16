namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class SourceValidationServiceTests
{
    private readonly ISourceValidationService _sut;

    public SourceValidationServiceTests()
    {
        var mockUserRepository = MockUserRepository.Setup();

        _sut = new SourceValidationService(mockUserRepository.Object);
    }

    [Theory]
    [InlineData("new", 1050, 0)]
    [InlineData("amendment", 1050, 0)]
    [InlineData("noChange", 1050, 0)]
    [InlineData("errorFix", 1050, 0)]
    [InlineData("something", 1050, 1)]
    public void SourceActionTypeValidation(string actionType, int? traCode, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Source"": {{
            ""actionType"": ""{actionType}"",
            ""currentTraOwner"": 1050,
            ""reference"": ""D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D"",
            ""section"": ""some free text"",
            ""traAffected"": [ 1050, 4, 3300 ],
            ""traCreator"": 1050,
            ""troName"": ""D-TRO""
          }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.ValidateSource(dtroSubmit, traCode);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData(1050, 0)]
    [InlineData(9999, 2)]
    public void SourceCurrentTraOwner(int? traCode, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Source"": {{
            ""actionType"": ""new"",
            ""currentTraOwner"": {traCode},
            ""reference"": ""D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D"",
            ""section"": ""some free text"",
            ""traAffected"": [ 1050, 4, 3300 ],
            ""traCreator"": 1050,
            ""troName"": ""D-TRO""
          }}
        }}", new SchemaVersion("3.3.0"));

        var actual = _sut.ValidateSource(dtroSubmit, traCode);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D", 0)]
    [InlineData("", 1)]
    public void SourceReference(string reference, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Source"": {{
            ""actionType"": ""new"",
            ""currentTraOwner"": 1050,
            ""reference"": ""{reference}"",
            ""section"": ""some free text"",
            ""traAffected"": [ 1050, 4, 3300 ],
            ""traCreator"": 1050,
            ""troName"": ""D-TRO""
          }}
        }}", new SchemaVersion("3.3.0"));

        int? traCode = 1050;
        var actual = _sut.ValidateSource(dtroSubmit, traCode);
        Assert.Equal(errorCount, actual.Count);
    }
}