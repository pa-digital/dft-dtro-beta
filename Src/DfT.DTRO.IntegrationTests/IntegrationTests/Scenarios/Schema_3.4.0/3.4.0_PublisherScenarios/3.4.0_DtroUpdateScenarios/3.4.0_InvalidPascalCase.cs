using Newtonsoft.Json;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers.ErrorJsonResponseProcessor;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Enums;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.PublisherScenarios.DtroUpdateScenarios
{
    public class InvalidPascalCase : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly static string schemaVersionWithInvalidPascalCase = "3.3.1";
        readonly string fileToCreateDtroWithValidCamelCase = "JSON-3.4.0-example-Derbyshire 2024 DJ388 partial.json";

        public static IEnumerable<object[]> GetDtroNamesOfFilesWithInvalidPascalCase()
        {
            DirectoryInfo directoryPath = new DirectoryInfo($"{PathToDtroExamplesDirectory}/{schemaVersionWithInvalidPascalCase}");
            FileInfo[] files = directoryPath.GetFiles();

            if (EnvironmentName == EnvironmentType.Local)
            {
                foreach (FileInfo file in files)
                {
                    yield return new object[] { file.Name };
                }
            }
            else
            {
                yield return new object[] { files[0].Name };
            }
        }

        [Theory]
        [MemberData(nameof(GetDtroNamesOfFilesWithInvalidPascalCase))]
        public async Task DtroUpdatedFromJsonBodyWithCamelCaseShouldBeRejected(string nameOfFileWithInvalidPascalCase)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithInvalidPascalCase}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = fileToCreateDtroWithValidCamelCase
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {fileToCreateDtroWithValidCamelCase}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = nameOfFileWithInvalidPascalCase
                                    .GetJsonFromFile(schemaVersionWithInvalidPascalCase)
                                    .ModifyTraInDtroJson(schemaVersionWithInvalidPascalCase, publisher.TraId)
                                    .ModifySourceActionType(schemaVersionWithInvalidPascalCase, "amendment")
                                    .ModifyTroNameForUpdate(schemaVersionWithInvalidPascalCase)
                                    .ModifySchemaVersion(schemaVersionToTest);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await dtroUpdateJson.SendJsonInDtroUpdateRequestAsync(dtroId, publisher);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {nameOfFileWithInvalidPascalCase}:\n\n{dtroUpdateResponseJson}");

            // Check DTRO response JSON
            Assert.True(dtroUpdateResponseJson.Contains("Property 'Source' has not been defined and the schema does not allow additional properties."),
                $"Response JSON for file {nameOfFileWithInvalidPascalCase}:\n\n{dtroUpdateResponseJson}");
            Assert.True(dtroUpdateResponseJson.Contains("Required properties are missing from object: source."),
                $"Response JSON for file {nameOfFileWithInvalidPascalCase}:\n\n{dtroUpdateResponseJson}");
        }

        [Theory]
        [MemberData(nameof(GetDtroNamesOfFilesWithInvalidPascalCase))]
        public async Task DtroUpdatedFromFileWithCamelCaseShouldBeRejected(string nameOfFileWithInvalidPascalCase)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithInvalidPascalCase}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = fileToCreateDtroWithValidCamelCase
                                                .GetJsonFromFile(schemaVersionToTest)
                                                .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            string tempFilePathForDtroCreation = dtroCreationJson.CreateDtroTempFile(fileToCreateDtroWithValidCamelCase, publisher);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await tempFilePathForDtroCreation.SendFileInDtroCreationRequestAsync(publisher);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroCreation)}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = nameOfFileWithInvalidPascalCase
                                    .GetJsonFromFile(schemaVersionWithInvalidPascalCase)
                                    .ModifyTraInDtroJson(schemaVersionWithInvalidPascalCase, publisher.TraId)
                                    .ModifySourceActionType(schemaVersionWithInvalidPascalCase, "amendment")
                                    .ModifyTroNameForUpdate(schemaVersionWithInvalidPascalCase)
                                    .ModifySchemaVersion(schemaVersionToTest);


            string tempFilePathForDtroUpdate = dtroUpdateJson.CreateDtroTempFileForUpdate(nameOfFileWithInvalidPascalCase, publisher);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await tempFilePathForDtroUpdate.SendFileInDtroUpdateRequestAsync(dtroId, publisher);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroUpdateResponseJson}");

            // Check DTRO response JSON
            Assert.True(dtroUpdateResponseJson.Contains("Property 'Source' has not been defined and the schema does not allow additional properties."),
                $"Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroUpdateResponseJson}");
            Assert.True(dtroUpdateResponseJson.Contains("Required properties are missing from object: source."),
                $"Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroUpdateResponseJson}");
        }
    }
}