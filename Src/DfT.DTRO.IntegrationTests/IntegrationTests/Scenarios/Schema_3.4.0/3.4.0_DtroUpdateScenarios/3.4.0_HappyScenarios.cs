using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Enums;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.DtroUpdateScenarios
{
    public class HappyScenarios : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";

        public static IEnumerable<object[]> GetNamesOfDtroExampleFiles()
        {
            DirectoryInfo directoryPath = new DirectoryInfo($"{PathToDtroExamplesDirectory}/{schemaVersionToTest}");
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
        [MemberData(nameof(GetNamesOfDtroExampleFiles))]
        public async Task DtroUpdatedFromJsonBodyShouldBeSavedCorrectly(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = dtroCreationJson
                                        .ModifySourceActionType(schemaVersionToTest, "amendment")
                                        .ModifyTroNameForUpdate(schemaVersionToTest);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await dtroUpdateJson.SendJsonInDtroUpdateRequestAsync(dtroId, publisher.AppId);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");

            // Get updated DTRO
            HttpResponseMessage dtroGetResponse = await dtroId.GetDtroResponseByIdAsync(publisher);
            string dtroGetResponseJson = await dtroGetResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroGetResponse.StatusCode,
                $"Actual status code: {dtroGetResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroGetResponseJson}");

            // Add ID to DTRO update and compare
            string expectedUpdateJsonForComparison = dtroUpdateJson
                                                        .AddDtroIdToJson(dtroId);
            JsonMethods.CompareJson(expectedUpdateJsonForComparison, dtroGetResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetNamesOfDtroExampleFiles))]
        public async Task DtroUpdatedFromFileShouldBeSavedCorrectly(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            string tempFilePathForDtroCreation = dtroCreationJson.CreateDtroTempFile(fileName, publisher);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await tempFilePathForDtroCreation.SendFileInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroCreation)}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = dtroCreationJson
                                        .ModifySourceActionType(schemaVersionToTest, "amendment")
                                        .ModifyTroNameForUpdate(schemaVersionToTest);

            string tempFilePathForDtroUpdate = dtroUpdateJson.CreateDtroTempFileForUpdate(fileName, publisher);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await tempFilePathForDtroUpdate.SendFileInDtroUpdateRequestAsync(dtroId, publisher.AppId);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroUpdateResponseJson}");

            // Get updated DTRO
            HttpResponseMessage dtroGetResponse = await dtroId.GetDtroResponseByIdAsync(publisher);
            string dtroGetResponseJson = await dtroGetResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroGetResponse.StatusCode,
                $"Actual status code: {dtroGetResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroGetResponseJson}");

            // Add ID to updated DTRO and compare
            string expectedUpdateJsonForComparison = dtroUpdateJson
                                                        .AddDtroIdToJson(dtroId);
            JsonMethods.CompareJson(expectedUpdateJsonForComparison, dtroGetResponseJson);
        }

        [Fact]
        public async Task DtroUpdatedWithLaterSchemaFromJsonBodyShouldBeSavedCorrectly()
        {
            string oldSchemaVersion = "3.3.1";
            string createFileWithSchema3_3_1 = "JSON-3.3.1-example-Derbyshire 2024 DJ388 partial.json";
            string updateFileWithSchema3_4_0 = "dtro-v3.4.0-example-derbyshire-2024-dj388-partial.json";

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string dtroCreationJson = createFileWithSchema3_3_1
                                    .GetJsonFromFile(oldSchemaVersion)
                                    .ModifyTraInDtroJson(oldSchemaVersion, publisher.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {createFileWithSchema3_3_1}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = updateFileWithSchema3_4_0
                                        .GetJsonFromFile(schemaVersionToTest)
                                        .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId)
                                        .ModifySourceActionType(schemaVersionToTest, "amendment")
                                        .ModifyTroNameForUpdate(schemaVersionToTest);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await dtroUpdateJson.SendJsonInDtroUpdateRequestAsync(dtroId, publisher.AppId);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {updateFileWithSchema3_4_0}:\n\n{dtroUpdateResponseJson}");

            // Get updated DTRO
            HttpResponseMessage dtroGetResponse = await dtroId.GetDtroResponseByIdAsync(publisher);
            string dtroGetResponseJson = await dtroGetResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroGetResponse.StatusCode,
                $"Actual status code: {dtroGetResponse.StatusCode}. Response JSON for file {updateFileWithSchema3_4_0}:\n\n{dtroGetResponseJson}");

            // Add ID to DTRO update and compare
            string expectedUpdateJsonForComparison = dtroUpdateJson
                                                        .AddDtroIdToJson(dtroId);
            JsonMethods.CompareJson(expectedUpdateJsonForComparison, dtroGetResponseJson);
        }

        [Fact]
        public async Task DtroUpdatedWithLaterSchemaFromFileShouldBeSavedCorrectly()
        {
            string oldSchemaVersion = "3.3.1";
            string createFileWithSchema3_3_1 = "JSON-3.3.1-example-Derbyshire 2024 DJ388 partial.json";
            string updateFileWithSchema3_4_0 = "dtro-v3.4.0-example-derbyshire-2024-dj388-partial.json";

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string dtroCreationJson = createFileWithSchema3_3_1
                                    .GetJsonFromFile(oldSchemaVersion)
                                    .ModifyTraInDtroJson(oldSchemaVersion, publisher.TraId);

            string tempFilePathForDtroCreation = dtroCreationJson.CreateDtroTempFile(createFileWithSchema3_3_1, publisher);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await tempFilePathForDtroCreation.SendFileInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroCreation)}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = updateFileWithSchema3_4_0
                                        .GetJsonFromFile(schemaVersionToTest)
                                        .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId)
                                        .ModifySourceActionType(schemaVersionToTest, "amendment")
                                        .ModifyTroNameForUpdate(schemaVersionToTest);

            string tempFilePathForDtroUpdate = dtroUpdateJson.CreateDtroTempFileForUpdate(updateFileWithSchema3_4_0, publisher);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await tempFilePathForDtroUpdate.SendFileInDtroUpdateRequestAsync(dtroId, publisher.AppId);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroUpdateResponseJson}");

            // Get updated DTRO
            HttpResponseMessage dtroGetResponse = await dtroId.GetDtroResponseByIdAsync(publisher);
            string dtroGetResponseJson = await dtroGetResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroGetResponse.StatusCode,
                $"Actual status code: {dtroGetResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroGetResponseJson}");

            // Add ID to DTRO update and compare
            string expectedUpdateJsonForComparison = dtroUpdateJson
                                            .AddDtroIdToJson(dtroId);
            JsonMethods.CompareJson(expectedUpdateJsonForComparison, dtroGetResponseJson);
        }
    }
}