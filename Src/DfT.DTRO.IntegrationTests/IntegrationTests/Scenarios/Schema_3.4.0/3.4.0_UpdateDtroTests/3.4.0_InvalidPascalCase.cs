using Newtonsoft.Json;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers.ErrorJsonResponseProcessor;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.UpdateDtroTests
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

        public static IEnumerable<object[]> GetDtroNamesOfFilesWithInvalidCamelCase()
        {
            DirectoryInfo directoryPath = new DirectoryInfo($"{AbsolutePathToDtroExamplesDirectory}/{schemaVersionWithInvalidPascalCase}");
            FileInfo[] files = directoryPath.GetFiles();

            foreach (FileInfo file in files)
            {
                yield return new object[] { file.Name };
            }
        }

        [Theory]
        [MemberData(nameof(GetDtroNamesOfFilesWithInvalidCamelCase))]
        public async Task DtroUpdatedFromFileWithCamelCaseShouldBeRejected(string nameOfFileWithInvalidCamelCase)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithInvalidCamelCase}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string tempFilePathForDtroCreation = FileHelper.CreateTempFileWithTraModified(schemaVersionToTest, fileToCreateDtroWithValidCamelCase, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePathForDtroCreation, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == createDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePathForDtroCreation)}: expected status code is {HttpStatusCode.Created} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Prepare DTRO update
            string tempFilePathForDtroUpdate = FileHelper.CreateTempFileWithTraAndSchemaVersionModified(schemaVersionToTest, schemaVersionWithInvalidPascalCase, nameOfFileWithInvalidCamelCase, publisher.TraId);

            // Send DTRO update
            string dtroId = await JsonMethods.GetIdFromResponseJsonAsync(createDtroResponse);
            HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromFileAsync(tempFilePathForDtroUpdate, dtroId, publisher);
            string updateDtroResponseJson = await updateDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == updateDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePathForDtroUpdate)}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {updateDtroResponse.StatusCode}, with response body\n{updateDtroResponseJson}");

            // Check DTRO response JSON
            JsonMethods.CompareJson(expectedErrorJson, updateDtroResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetDtroNamesOfFilesWithInvalidCamelCase))]
        public async Task DtroUpdatedFromJsonBodyWithCamelCaseShouldBeRejected(string nameOfFileWithInvalidCamelCase)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithInvalidCamelCase}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string createDtroJsonWithTraModified = JsonMethods.GetJsonFromFileAndModifyTra(schemaVersionToTest, fileToCreateDtroWithValidCamelCase, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraModified, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == createDtroResponse.StatusCode, $"File {fileToCreateDtroWithValidCamelCase}: expected status code is {HttpStatusCode.Created} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = FileHelper.GetJsonFromFileAndModifyTraAndSchemaVersion(schemaVersionToTest, schemaVersionWithInvalidPascalCase, nameOfFileWithInvalidCamelCase, publisher.TraId);

            // Send DTRO update
            string dtroId = await JsonMethods.GetIdFromResponseJsonAsync(createDtroResponse);
            HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromJsonBodyAsync(dtroUpdateJson, dtroId, publisher);
            string updateDtroResponseJson = await updateDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == updateDtroResponse.StatusCode, $"File {nameOfFileWithInvalidCamelCase}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {updateDtroResponse.StatusCode}, with response body\n{updateDtroResponseJson}");

            // Check DTRO response JSON
            JsonMethods.CompareJson(expectedErrorJson, updateDtroResponseJson);
        }
    }
}