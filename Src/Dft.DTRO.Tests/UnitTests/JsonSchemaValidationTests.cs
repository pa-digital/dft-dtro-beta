namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class JsonSchemaValidationTests
{
    private const string ExampleFilesForSchema311 = "../../../TestFiles/D-TROs/3.1.1";
    private const string ExampleFilesForSchema320 = "../../../TestFiles/D-TROs/3.2.0";
    private const string ExampleFilesForSchema325 = "../../../TestFiles/D-TROs/3.2.5";
    private const string SchemaFolder = "../../../TestFiles/Schemas/";

    [Theory]
    [InlineData("3.1.1", "ha-missing", false)]
    [InlineData("3.1.1", "proper-data", true)]
    [InlineData("3.1.1", "provision-empty", false)]
    [InlineData("3.1.1", "provision-missing", false)]
    [InlineData("3.1.1", "section-missing", false)]
    [InlineData("3.1.1", "tro-name-missing", false)]
    public void ValidateAgainstSchema311ProducesCorrectResults(string schemaVersion, string sourceJson, bool expectedResult)
    {
        JsonSchemaValidationService sut = new();

        string jsonSchema =
            GetJsonSchemaForRequestAsString(new DfT.DTRO.Models.DataBase.DTRO { SchemaVersion = schemaVersion });
        string inputJson = File.ReadAllText(Path.Join(ExampleFilesForSchema311, $"{sourceJson}.json"));

        bool result = !sut.ValidateRequestAgainstJsonSchema(jsonSchema, inputJson).Any();

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("3.2.0", "valid-fullAmendment", false)]
    [InlineData("3.2.0", "valid-new-x", false)]
    [InlineData("3.2.0", "valid-fullRevoke", false)]
    [InlineData("3.2.0", "valid-partialAmendment", false)]
    [InlineData("3.2.0", "valid-partialRevoke", false)]
    public void ValidateAgainstSchema320ProducesCorrectResults(string schemaVersion, string sourceJson, bool expectedResult)
    {
        JsonSchemaValidationService sut = new();
        string jsonSchema =
            GetJsonSchemaForRequestAsString(new DfT.DTRO.Models.DataBase.DTRO { SchemaVersion = schemaVersion });
        string inputJson = File.ReadAllText(Path.Join(ExampleFilesForSchema320, $"{sourceJson}.json"));

        bool actual = sut.ValidateRequestAgainstJsonSchema(jsonSchema, inputJson).Any();
        Assert.Equal(expectedResult, actual);
    }

    [Theory]
    [InlineData("3.2.5", "JSON-example-ControlledParkingZone-CoalOrchard-dtro-3.2.5", false)]
    [InlineData("3.2.5", "JSON-example-directed-linear-dtro-3.2.5", false)]
    [InlineData("3.2.5", "JSON-example-linear-dtro-3.2.5", false)]
    [InlineData("3.2.5", "JSON-example-point-dtro-3.2.5", false)]
    [InlineData("3.2.5", "JSON-example-polygon-dtro-3.2.5", false)]
    public void ValidateAgainstSchema325ProducesCorrectResults(string schemaVersion, string sourceJson, bool expectedResult)
    {
        JsonSchemaValidationService sut = new();
        string jsonSchema =
            GetJsonSchemaForRequestAsString(new DfT.DTRO.Models.DataBase.DTRO { SchemaVersion = schemaVersion });
        string inputJson = File.ReadAllText(Path.Join(ExampleFilesForSchema325, $"{sourceJson}.json"));

        bool actual = sut.ValidateRequestAgainstJsonSchema(jsonSchema, inputJson).Any();
        Assert.Equal(expectedResult, actual);
    }

    private string GetJsonSchemaForRequestAsString(DfT.DTRO.Models.DataBase.DTRO request)
    {
        return File.ReadAllText($"{SchemaFolder}/{request.SchemaVersion}.json");
    }
}