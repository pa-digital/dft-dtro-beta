namespace Dft.DTRO.Tests.ServicesTests.Validations;

[ExcludeFromCodeCoverage]
public class JsonSchemaValidationTests
{
    private const string ExampleFilesForSchema311 = "../../../TestFiles/D-TROs/3.1.1";
    private const string ExampleFilesForSchema320 = "../../../TestFiles/D-TROs/3.2.0";
    private const string ExampleFilesForSchema330 = "../../../TestFiles/D-TROs/3.3.0";
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

        bool result = !sut.ValidateSchema(jsonSchema, inputJson).Any();

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

        bool actual = sut.ValidateSchema(jsonSchema, inputJson).Any();
        Assert.Equal(expectedResult, actual);
    }

    [Theory]
    [InlineData("3.3.0", "JSON-3.3.0-example-TTRO-HeightRestrictionWithConditions", false)]
    [InlineData("3.3.0", "JSON-3.3.0-example-TTRO-multiple-nested-condition-sets", false)]
    [InlineData("3.3.0", "JSON-3.3.0-example-TTRO-SuspensionOneWay", false)]
    [InlineData("3.3.0", "JSON-3.3.0-example-TTRO-TempOneWayWithConditions", false)]
    [InlineData("3.3.0", "JSON-3.3.0-example-TTRO-WeightRestriction", false)]
    public void ValidateAgainstSchema325ProducesCorrectResults(string schemaVersion, string sourceJson, bool expectedResult)
    {
        JsonSchemaValidationService sut = new();
        string jsonSchema =
            GetJsonSchemaForRequestAsString(new DfT.DTRO.Models.DataBase.DTRO { SchemaVersion = schemaVersion });
        string inputJson = File.ReadAllText(Path.Join(ExampleFilesForSchema330, $"{sourceJson}.json"));

        bool actual = sut.ValidateSchema(jsonSchema, inputJson).Any();
        Assert.Equal(expectedResult, actual);
    }

    private string GetJsonSchemaForRequestAsString(DfT.DTRO.Models.DataBase.DTRO request)
    {
        return File.ReadAllText($"{SchemaFolder}/{request.SchemaVersion}.json");
    }
}