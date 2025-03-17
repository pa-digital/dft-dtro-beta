namespace Dft.DTRO.Tests.ServicesTests.Validations;

public class ConsultationValidationServiceTests
{
    private readonly ConsultationValidationService _sut = new();

    [Theory]
    [InlineData("free text", 0)]
    [InlineData("", 1)]
    [InlineData(null, 1)]
    public void ValidateConsultationName(string consultationName, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Consultation"": {{
            ""consultationName"": ""{consultationName}"",
            ""endOfConsultation"": ""2020-01-01 00:00:00"",
            ""howToComment"": ""some comment"",
            ""localReference"": ""some reference"",
            ""pointOfContactAddress"": ""some address"",
            ""pointOfContactEmail"": ""some@email.com"",
            ""startOfConsultation"": ""2019-01-01 00:00:00"",
            ""statementOfReason"": ""some reason"",
            ""urlAdditionalInformation"": ""https://www.example.com""
          }}
        }}", new SchemaVersion("3.4.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("2020-01-01 00:00:00", 0)]
    [InlineData("3020-01-01 00:00:00", 1)]
    [InlineData("", 1)]
    [InlineData(null, 1)]
    public void ValidateEndOfConsultation(string endOfConsultation, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Consultation"": {{
            ""consultationName"": ""some name"",
            ""endOfConsultation"": ""{endOfConsultation}"",
            ""howToComment"": ""some comment"",
            ""localReference"": ""some reference"",
            ""pointOfContactAddress"": ""some address"",
            ""pointOfContactEmail"": ""some@email.com"",
            ""startOfConsultation"": ""2019-01-01 00:00:00"",
            ""statementOfReason"": ""some reason"",
            ""urlAdditionalInformation"": ""https://www.example.com""
          }}
        }}", new SchemaVersion("3.4.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("free text", 0)]
    [InlineData("", 1)]
    [InlineData(null, 1)]
    public void ValidateHowToComment(string howToComment, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Consultation"": {{
            ""consultationName"": ""some name"",
            ""endOfConsultation"": ""2020-01-01 00:00:00"",
            ""howToComment"": ""{howToComment}"",
            ""localReference"": ""some reference"",
            ""pointOfContactAddress"": ""some address"",
            ""pointOfContactEmail"": ""some@email.com"",
            ""startOfConsultation"": ""2019-01-01 00:00:00"",
            ""statementOfReason"": ""some reason"",
            ""urlAdditionalInformation"": ""https://www.example.com""
          }}
        }}", new SchemaVersion("3.4.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("free text", 0)]
    [InlineData("", 1)]
    [InlineData(null, 1)]
    public void ValidateLocalReference(string localReference, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Consultation"": {{
            ""consultationName"": ""some name"",
            ""endOfConsultation"": ""2020-01-01 00:00:00"",
            ""howToComment"": ""some comment"",
            ""localReference"": ""{localReference}"",
            ""pointOfContactAddress"": ""some address"",
            ""pointOfContactEmail"": ""some@email.com"",
            ""startOfConsultation"": ""2019-01-01 00:00:00"",
            ""statementOfReason"": ""some reason"",
            ""urlAdditionalInformation"": ""https://www.example.com""
          }}
        }}", new SchemaVersion("3.4.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("free text", 0)]
    [InlineData("", 1)]
    [InlineData(null, 1)]
    public void ValidatePointOfContactAddress(string pointOfContactAddress, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Consultation"": {{
            ""consultationName"": ""some name"",
            ""endOfConsultation"": ""2020-01-01 00:00:00"",
            ""howToComment"": ""some comment"",
            ""localReference"": ""some reference"",
            ""pointOfContactAddress"": ""{pointOfContactAddress}"",
            ""pointOfContactEmail"": ""some@email.com"",
            ""startOfConsultation"": ""2019-01-01 00:00:00"",
            ""statementOfReason"": ""some reason"",
            ""urlAdditionalInformation"": ""https://www.example.com""
          }}
        }}", new SchemaVersion("3.4.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("some@email.com", 0)]
    [InlineData("wrong formatted email", 1)]
    [InlineData("", 1)]
    [InlineData(null, 1)]
    public void ValidatePointOfContactEmail(string pointOfContactEmail, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Consultation"": {{
            ""consultationName"": ""some name"",
            ""endOfConsultation"": ""2020-01-01 00:00:00"",
            ""howToComment"": ""some comment"",
            ""localReference"": ""some reference"",
            ""pointOfContactAddress"": ""some address"",
            ""pointOfContactEmail"": ""{pointOfContactEmail}"",
            ""startOfConsultation"": ""2019-01-01 00:00:00"",
            ""statementOfReason"": ""some reason"",
            ""urlAdditionalInformation"": ""https://www.example.com""
          }}
        }}", new SchemaVersion("3.4.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("2010-01-01 00:00:00", 0)]
    [InlineData("3020-01-01 00:00:00", 1)]
    [InlineData("", 1)]
    [InlineData(null, 1)]
    public void ValidateStartOfConsultation(string startOfConsultation, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Consultation"": {{
            ""consultationName"": ""some name"",
            ""endOfConsultation"": ""2020-01-01 00:00:00"",
            ""howToComment"": ""some comment"",
            ""localReference"": ""some reference"",
            ""pointOfContactAddress"": ""some address"",
            ""pointOfContactEmail"": ""some@email.com"",
            ""startOfConsultation"": ""{startOfConsultation}"",
            ""statementOfReason"": ""some reason"",
            ""urlAdditionalInformation"": ""https://www.example.com""
          }}
        }}", new SchemaVersion("3.4.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("free text", 0)]
    [InlineData("", 1)]
    [InlineData(null, 1)]
    public void ValidateStatementOfReason(string statementOfReason, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Consultation"": {{
            ""consultationName"": ""some name"",
            ""endOfConsultation"": ""2020-01-01 00:00:00"",
            ""howToComment"": ""some comment"",
            ""localReference"": ""some reference"",
            ""pointOfContactAddress"": ""some address"",
            ""pointOfContactEmail"": ""some@email.com"",
            ""startOfConsultation"": ""2019-01-01 00:00:00"",
            ""statementOfReason"": ""{statementOfReason}"",
            ""urlAdditionalInformation"": ""https://www.example.com""
          }}
        }}", new SchemaVersion("3.4.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }

    [Theory]
    [InlineData("https://www.example.com", 0)]
    [InlineData("", 1)]
    [InlineData(null, 1)]
    public void ValidateUrlAdditionalInformation(string urlAdditionalInformation, int errorCount)
    {
        var dtroSubmit = Utils.PrepareDtro($@"
        {{
          ""Consultation"": {{
            ""consultationName"": ""some name"",
            ""endOfConsultation"": ""2020-01-01 00:00:00"",
            ""howToComment"": ""some comment"",
            ""localReference"": ""some reference"",
            ""pointOfContactAddress"": ""some address"",
            ""pointOfContactEmail"": ""some@email.com"",
            ""startOfConsultation"": ""2019-01-01 00:00:00"",
            ""statementOfReason"": ""some reason"",
            ""urlAdditionalInformation"": ""{urlAdditionalInformation}""
          }}
        }}", new SchemaVersion("3.4.0"));

        var actual = _sut.Validate(dtroSubmit);
        Assert.Equal(errorCount, actual.Count);
    }
}