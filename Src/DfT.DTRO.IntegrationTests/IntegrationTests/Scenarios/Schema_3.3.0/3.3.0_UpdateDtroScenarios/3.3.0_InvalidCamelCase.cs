using Newtonsoft.Json;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers.ErrorJsonResponseProcessor;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_3_0.DtroUpdateScenarios
{
    public class InvalidCamelCase : BaseTest
    {
        readonly static string schemaVersionToTest = "3.3.0";
        readonly static string schemaVersionWithInvalidCamelCase = "3.3.2";
        readonly string fileToCreateDtroWithValidPascalCase = "JSON-3.3.0-example-Derbyshire 2024 DJ388 partial.json";

        public static IEnumerable<object[]> GetDtroNamesOfFilesWithInvalidCamelCase()
        {
            DirectoryInfo directoryPath = new DirectoryInfo($"{PathToDtroExamplesDirectory}/{schemaVersionWithInvalidCamelCase}");
            FileInfo[] files = directoryPath.GetFiles();

            foreach (FileInfo file in files)
            {
                yield return new object[] { file.Name };
            }
        }

        [Theory]
        [MemberData(nameof(GetDtroNamesOfFilesWithInvalidCamelCase))]
        public async Task DtroUpdatedFromJsonBodyWithCamelCaseShouldBeRejected(string nameOfFileWithInvalidCamelCase)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithInvalidCamelCase}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string dtroCreationJson = fileToCreateDtroWithValidPascalCase
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Response JSON for file {fileToCreateDtroWithValidPascalCase}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = nameOfFileWithInvalidCamelCase
                                    .GetJsonFromFile(schemaVersionWithInvalidCamelCase)
                                    .ModifyTraInDtroJson(schemaVersionWithInvalidCamelCase, publisher.TraId)
                                    .ModifySourceActionType(schemaVersionWithInvalidCamelCase, "amendment")
                                    .ModifyTroNameForUpdate(schemaVersionWithInvalidCamelCase)
                                    .ModifySchemaVersion(schemaVersionToTest);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await dtroUpdateJson.SendJsonInDtroUpdateRequestAsync(dtroId, publisher.AppId);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroUpdateResponse.StatusCode,
                $"Response JSON for file {nameOfFileWithInvalidCamelCase}:\n\n{dtroUpdateResponseJson}");

            // Check DTRO response JSON
            ErrorJson jsonErrorResponse = await ErrorJsonResponseProcessor.GetErrorJson(dtroUpdateResponse);
            Assert.Equal("Case naming convention exception", jsonErrorResponse.Message);
            Assert.StartsWith("All property names must conform to pascal case naming conventions. The following properties violate this: [source, provision, regulatedPlace", jsonErrorResponse.Error);
        }

        [Theory]
        [MemberData(nameof(GetDtroNamesOfFilesWithInvalidCamelCase))]
        public async Task DtroUpdatedFromFileWithCamelCaseShouldBeRejected(string nameOfFileWithInvalidCamelCase)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithInvalidCamelCase}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string dtroCreationJson = fileToCreateDtroWithValidPascalCase
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            string tempFilePathForDtroCreation = dtroCreationJson.CreateDtroTempFile(fileToCreateDtroWithValidPascalCase, publisher);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await tempFilePathForDtroCreation.SendFileInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Response JSON for file {Path.GetFileName(tempFilePathForDtroCreation)}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = nameOfFileWithInvalidCamelCase
                                    .GetJsonFromFile(schemaVersionWithInvalidCamelCase)
                                    .ModifyTraInDtroJson(schemaVersionWithInvalidCamelCase, publisher.TraId)
                                    .ModifySourceActionType(schemaVersionWithInvalidCamelCase, "amendment")
                                    .ModifyTroNameForUpdate(schemaVersionWithInvalidCamelCase)
                                    .ModifySchemaVersion(schemaVersionToTest);

            string tempFilePathForDtroUpdate = dtroUpdateJson.CreateDtroTempFileForUpdate(nameOfFileWithInvalidCamelCase, publisher);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await tempFilePathForDtroUpdate.SendFileInDtroUpdateRequestAsync(dtroId, publisher.AppId);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroUpdateResponse.StatusCode,
                $"Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroUpdateResponseJson}");

            // Check DTRO response JSON
            ErrorJson jsonErrorResponse = await ErrorJsonResponseProcessor.GetErrorJson(dtroUpdateResponse);
            Assert.Equal("Case naming convention exception", jsonErrorResponse.Message);
            Assert.StartsWith("All property names must conform to pascal case naming conventions. The following properties violate this: [source, provision, regulatedPlace", jsonErrorResponse.Error);
        }
    }
}