using Newtonsoft.Json;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers.ErrorJsonResponseProcessor;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.DtroUpdateScenarios
{
    public class InvalidPascalCase : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly static string schemaVersionWithInvalidPascalCase = "3.3.1";
        readonly string fileToCreateDtroWithValidCamelCase = "JSON-3.4.0-example-Derbyshire 2024 DJ388 partial.json";
        readonly string expectedErrorJson = """
        {
            "ruleError_0": {
                "message": "Property 'Source' has not been defined and the schema does not allow additional properties.",
                "path": "Source",
                "value": "Source",
                "errorType": "AdditionalProperties"
            },
            "ruleError_1": {
                "message": "JSON is valid against no schemas from 'oneOf'.",
                "path": "",
                "value": null,
                "errorType": "OneOf"
            }
        }
        """;

        public static IEnumerable<object[]> GetDtroNamesOfFilesWithInvalidPascalCase()
        {
            DirectoryInfo directoryPath = new DirectoryInfo($"{PathToDtroExamplesDirectory}/{schemaVersionWithInvalidPascalCase}");
            FileInfo[] files = directoryPath.GetFiles();

            foreach (FileInfo file in files)
            {
                yield return new object[] { file.Name };
            }
        }

        [Theory]
        [MemberData(nameof(GetDtroNamesOfFilesWithInvalidPascalCase))]
        public async Task DtroUpdatedFromFileWithCamelCaseShouldBeRejected(string nameOfFileWithInvalidPascalCase)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithInvalidPascalCase}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string tempFilePathForDtroCreation = Dtros.CreateTempFileWithTraModified(schemaVersionToTest, fileToCreateDtroWithValidCamelCase, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePathForDtroCreation, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == createDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePathForDtroCreation)}: expected status code is {HttpStatusCode.Created} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Prepare DTRO update
            string tempFilePathForDtroUpdate = Dtros.CreateTempFileWithTraAndSchemaVersionModified(schemaVersionToTest, schemaVersionWithInvalidPascalCase, nameOfFileWithInvalidPascalCase, publisher.TraId);

            // Send DTRO update
            string dtroId = await Dtros.GetIdFromResponseJsonAsync(createDtroResponse);
            HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromFileAsync(tempFilePathForDtroUpdate, dtroId, publisher);
            string updateDtroResponseJson = await updateDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == updateDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePathForDtroUpdate)}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {updateDtroResponse.StatusCode}, with response body\n{updateDtroResponseJson}");

            // Check DTRO response JSON
            JsonMethods.CompareJson(expectedErrorJson, updateDtroResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetDtroNamesOfFilesWithInvalidPascalCase))]
        public async Task DtroUpdatedFromJsonBodyWithCamelCaseShouldBeRejected(string nameOfFileWithInvalidPascalCase)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithInvalidPascalCase}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string createDtroJsonWithTraModified = Dtros.GetJsonFromFileAndModifyTra(schemaVersionToTest, fileToCreateDtroWithValidCamelCase, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraModified, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == createDtroResponse.StatusCode, $"File {fileToCreateDtroWithValidCamelCase}: expected status code is {HttpStatusCode.Created} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = Dtros.GetJsonFromFileAndModifyTraAndSchemaVersion(schemaVersionToTest, schemaVersionWithInvalidPascalCase, nameOfFileWithInvalidPascalCase, publisher.TraId);

            // Send DTRO update
            string dtroId = await Dtros.GetIdFromResponseJsonAsync(createDtroResponse);
            HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromJsonBodyAsync(dtroUpdateJson, dtroId, publisher);
            string updateDtroResponseJson = await updateDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == updateDtroResponse.StatusCode, $"File {nameOfFileWithInvalidPascalCase}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {updateDtroResponse.StatusCode}, with response body\n{updateDtroResponseJson}");

            // Check DTRO response JSON
            JsonMethods.CompareJson(expectedErrorJson, updateDtroResponseJson);
        }
    }
}