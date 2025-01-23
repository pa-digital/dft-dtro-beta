namespace Dft.DTRO.Tests.ServicesTests.Validations;

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
    [InlineData("3.2.4", "new", 1050, 0)]
    [InlineData("3.2.4", "amendment", 1050, 0)]
    [InlineData("3.2.4", "noChange", 1050, 0)]
    [InlineData("3.2.4", "errorFix", 1050, 0)]
    [InlineData("3.2.4", "something", 1050, 1)]
    [InlineData("3.3.0", "new", 1050, 0)]
    [InlineData("3.3.0", "amendment", 1050, 0)]
    [InlineData("3.3.0", "noChange", 1050, 0)]
    [InlineData("3.3.0", "errorFix", 1050, 0)]
    [InlineData("3.3.0", "something", 1050, 1)]
    public void ValidateSourceActionType(string version, string actionType, int? traCode, int errorCount)
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
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit, traCode);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.2.4", 1050, 0)]
    [InlineData("3.3.0", 9999, 2)]
    public void ValidateSourceCurrentTraOwner(string version, int? traCode, int errorCount)
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
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit, traCode);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.2.4", "D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D", 0)]
    [InlineData("3.3.0", "", 1)]
    public void ValidateSourceReference(string version, string reference, int errorCount)
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
        }}", new SchemaVersion(version));

        int? traCode = 1050;
        var actual = _sut.Validate(dtroSubmit, traCode);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.2.4", "some free text", 0)]
    [InlineData("3.3.0", "", 1)]
    public void ValidateSourceSection(string version, string section, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Source"": {{
            ""actionType"": ""new"",
            ""currentTraOwner"": 1050,
            ""reference"": ""D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D"",
            ""section"": ""{section}"",
            ""traAffected"": [ 1050, 4, 3300 ],
            ""traCreator"": 1050,
            ""troName"": ""D-TRO""
          }}
        }}", new SchemaVersion(version));

        int? traCode = 1050;
        var actual = _sut.Validate(dtroSubmit, traCode);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.2.4", "1050, 4, 3300", 0)]
    [InlineData("3.2.4", "", 1)]
    [InlineData("3.2.4", "9999, 0, 10000", 1)]
    [InlineData("3.3.0", "1050, 4, 3300", 0)]
    [InlineData("3.3.0", "", 1)]
    [InlineData("3.3.0", "9999, 0, 10000", 1)]
    public void ValidateSourceTraAffected(string version, string traAffected, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Source"": {{
            ""actionType"": ""new"",
            ""currentTraOwner"": 1050,
            ""reference"": ""D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D"",
            ""section"": ""some free text"",
            ""traAffected"": [{traAffected}],
            ""traCreator"": 1050,
            ""troName"": ""D-TRO""
          }}
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit, 1050);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.2.4", 1050, 0)]
    [InlineData("3.3.0", 1000, 2)]
    public void ValidateSourceTraCreator(string version, int traCode, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Source"": {{
            ""actionType"": ""new"",
            ""currentTraOwner"": 1050,
            ""reference"": ""D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D"",
            ""section"": ""some free text"",
            ""traAffected"": [ 1050, 4, 3300 ],
            ""traCreator"": {traCode},
            ""troName"": ""D-TRO""
          }}
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit, traCode);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("3.2.4", "D-TRO", 0)]
    [InlineData("3.3.0", "", 1)]
    public void ValidateSourceTroName(string version, string troName, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Source"": {{
            ""actionType"": ""new"",
            ""currentTraOwner"": 1050,
            ""reference"": ""D5E7FBE5-5A7A-4A81-8E27-CDB008EC729D"",
            ""section"": ""some free text"",
            ""traAffected"": [ 1050, 4, 3300 ],
            ""traCreator"": 1050,
            ""troName"": ""{troName}""
          }}
        }}", new SchemaVersion(version));

        var actual = _sut.Validate(dtroSubmit, 1050);
        Assert.Equal(errorCount, actual.Count);
    }
}