using Newtonsoft.Json;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers.ErrorJsonResponseProcessor;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_3_0.UpdateDtroTests
{
    public class InvalidCamelCase : BaseTest
    {
        readonly static string schemaVersionToTest = "3.3.0";
        readonly static string schemaVersionWithInvalidCamelCase = "3.3.2";
        readonly string fileToCreateDtroWithValidPascalCase = "JSON-3.3.0-example-Derbyshire 2024 DJ388 partial.json";

        public static IEnumerable<object[]> GetDtroNamesOfFilesWithInvalidCamelCase()
        {
            DirectoryInfo directoryPath = new DirectoryInfo($"{AbsolutePathToDtroExamplesDirectory}/{schemaVersionWithInvalidCamelCase}");
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
            string tempFilePathForDtroCreation = FileHelper.CreateTempFileWithTraModified(schemaVersionToTest, fileToCreateDtroWithValidPascalCase, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePathForDtroCreation, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == createDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePathForDtroCreation)}: expected status code is {HttpStatusCode.Created} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Prepare DTRO update
            string tempFilePathForDtroUpdate = FileHelper.CreateTempFileWithTraAndSchemaVersionModified(schemaVersionToTest, schemaVersionWithInvalidCamelCase, nameOfFileWithInvalidCamelCase, publisher.TraId);

            // Send DTRO update
            string dtroId = await Dtros.GetIdFromResponseJsonAsync(createDtroResponse);
            HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromFileAsync(tempFilePathForDtroUpdate, dtroId, publisher);
            string updateDtroResponseJson = await updateDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == updateDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePathForDtroUpdate)}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {updateDtroResponse.StatusCode}, with response body\n{updateDtroResponseJson}");

            // Check DTRO response JSON
            ErrorJson jsonErrorResponse = await ErrorJsonResponseProcessor.GetErrorJson(updateDtroResponse);
            Assert.Equal("Case naming convention exception", jsonErrorResponse.Message);
            Assert.StartsWith("All property names must conform to pascal case naming conventions. The following properties violate this: [source, provision, regulatedPlace", jsonErrorResponse.Error);
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
            string createDtroJsonWithTraModified = Dtros.GetJsonFromFileAndModifyTra(schemaVersionToTest, fileToCreateDtroWithValidPascalCase, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraModified, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == createDtroResponse.StatusCode, $"File {fileToCreateDtroWithValidPascalCase}: expected status code is {HttpStatusCode.Created} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = FileHelper.GetJsonFromFileAndModifyTraAndSchemaVersion(schemaVersionToTest, schemaVersionWithInvalidCamelCase, nameOfFileWithInvalidCamelCase, publisher.TraId);

            // Send DTRO update
            string dtroId = await Dtros.GetIdFromResponseJsonAsync(createDtroResponse);
            HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromJsonBodyAsync(dtroUpdateJson, dtroId, publisher);
            string updateDtroResponseJson = await updateDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == updateDtroResponse.StatusCode, $"File {nameOfFileWithInvalidCamelCase}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {updateDtroResponse.StatusCode}, with response body\n{updateDtroResponseJson}");

            // Check DTRO response JSON
            ErrorJson jsonErrorResponse = await ErrorJsonResponseProcessor.GetErrorJson(updateDtroResponse);
            Assert.Equal("Case naming convention exception", jsonErrorResponse.Message);
            Assert.StartsWith("All property names must conform to pascal case naming conventions. The following properties violate this: [source, provision, regulatedPlace", jsonErrorResponse.Error);
        }
    }
}