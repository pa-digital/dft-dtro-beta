﻿namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class JsonSchemaValidationTests
{
    private const string Schema311Files = "../../../TestFiles/D-TROs/3.1.1";
    private const string Schema320Files = "../../../TestFiles/D-TROs/3.2.0";
    private const string SchemaFolder = "../../../TestFiles/Schemas/";

    [Theory]
    [InlineData("3.1.1", "ha-missing", false)]
    [InlineData("3.1.1", "proper-data", true)]
    [InlineData("3.1.1", "provision-empty", false)]
    [InlineData("3.1.1", "provision-missing", false)]
    [InlineData("3.1.1", "section-missing", false)]
    [InlineData("3.1.1", "tro-name-missing", false)]
    public void ProducesCorrectResults(string schemaVersion, string sourceJson, bool expectedResult)
    {
        JsonSchemaValidationService sut = new();

        string jsonSchema =
            GetJsonSchemaForRequestAsString(new DfT.DTRO.Models.DataBase.DTRO { SchemaVersion = schemaVersion });
        string inputJson = File.ReadAllText(Path.Join(Schema311Files, $"{sourceJson}.json"));

        bool result = !sut.ValidateRequestAgainstJsonSchema(jsonSchema, inputJson).Any();

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("3.2.0", "valid-fullAmendment", false)]
    [InlineData("3.2.0", "valid-new-x", false)]
    [InlineData("3.2.0", "valid-fullRevoke", false)]
    [InlineData("3.2.0", "valid-partialAmendment", false)]
    [InlineData("3.2.0", "valid-partialRevoke", false)]
    public void ProcessCorrectResults(string schemaVersion, string sourceJson, bool expectedResult)
    {
        JsonSchemaValidationService sut = new();
        string jsonSchema =
            GetJsonSchemaForRequestAsString(new DfT.DTRO.Models.DataBase.DTRO { SchemaVersion = schemaVersion });
        string inputJson = File.ReadAllText(Path.Join(Schema320Files, $"{sourceJson}.json"));

        bool actual = sut.ValidateRequestAgainstJsonSchema(jsonSchema, inputJson).Any();
        Assert.Equal(expectedResult, actual);
    }

    private string GetJsonSchemaForRequestAsString(DfT.DTRO.Models.DataBase.DTRO request)
    {
        return File.ReadAllText($"{SchemaFolder}/{request.SchemaVersion}.json");
    }
}