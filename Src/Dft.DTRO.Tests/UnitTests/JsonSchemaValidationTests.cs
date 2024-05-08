using DfT.DTRO.Services.Validation;

namespace Dft.DTRO.Tests
{
    public class JsonSchemaValidationTests
    {
        private readonly string SourceJsonBasePath = "./DtroJsonDataExamples";
        private const string SchemaFolder = "Schemas";
        [Theory]
        [InlineData("3.1.1", "proper-data", true)]
        [InlineData("3.1.1", "tro-name-missing", false)]
        [InlineData("3.1.1", "section-missing", false)]
        [InlineData("3.1.1", "ha-missing", false)]
        [InlineData("3.1.1", "provision-missing", false)]
        [InlineData("3.1.1", "provision-empty", false)]
        public void ProducesCorrectResults(string schemaVersion, string sourceJson, bool expectedResult)
        {
            var sut = new JsonSchemaValidationService();

            var jsonSchema = GetJsonSchemaForRequestAsString(new() { SchemaVersion = schemaVersion });
            var inputJson = File.ReadAllText(Path.Join(SourceJsonBasePath, $"{sourceJson}.json"));

            var result = !sut.ValidateRequestAgainstJsonSchema(jsonSchema, inputJson).Any();

            Assert.Equal(expectedResult, result);
        }

        public string GetJsonSchemaForRequestAsString(DfT.DTRO.Models.DataBase.DTRO request)
        {
            return File.ReadAllText($"{SchemaFolder}/{request.SchemaVersion}.json");
        }
    }
}