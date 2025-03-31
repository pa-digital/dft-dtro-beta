using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.DtroUpdateScenarios
{
    public class HappyScenarios : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";

        public static IEnumerable<object[]> GetNamesOfDtroExampleFiles()
        {
            DirectoryInfo directoryPath = new DirectoryInfo($"{PathToDtroExamplesDirectory}/{schemaVersionToTest}");
            FileInfo[] files = directoryPath.GetFiles();

            foreach (FileInfo file in files)
            {
                yield return new object[] { file.Name };
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
                $"Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = dtroCreationJson
                                        .ModifySourceActionType(schemaVersionToTest, "amendment")
                                        .ModifyTroNameForUpdate(schemaVersionToTest);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await dtroUpdateJson.SendJsonInDtroUpdateRequestAsync(dtroId, publisher.AppId);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroUpdateResponse.StatusCode,
                $"Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");

            // Get updated DTRO
            HttpResponseMessage dtroGetResponse = await dtroId.GetDtroResponseByIdAsync(publisher);
            string dtroGetResponseJson = await dtroGetResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroGetResponse.StatusCode,
                $"Response JSON for file {fileName}:\n\n{dtroGetResponseJson}");

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
                $"Response JSON for file {Path.GetFileName(tempFilePathForDtroCreation)}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = dtroCreationJson
                                        .ModifySourceActionType(schemaVersionToTest, "amendment")
                                        .ModifyTroNameForUpdate(schemaVersionToTest);

            string tempFilePathForDtroUpdate = dtroUpdateJson.CreateDtroTempFileForUpdate(fileName, publisher.TraId);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await tempFilePathForDtroUpdate.SendFileInDtroUpdateRequestAsync(dtroId, publisher.AppId);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroUpdateResponse.StatusCode,
                $"Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroUpdateResponseJson}");

            // Get updated DTRO
            HttpResponseMessage dtroGetResponse = await dtroId.GetDtroResponseByIdAsync(publisher);
            string dtroGetResponseJson = await dtroGetResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroGetResponse.StatusCode,
                $"Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroGetResponseJson}");

            // Add ID to updated DTRO and compare
            string expectedUpdateJsonForComparison = dtroUpdateJson
                                                        .AddDtroIdToJson(dtroId);
            JsonMethods.CompareJson(expectedUpdateJsonForComparison, dtroGetResponseJson);
        }

        [Fact]
        public async Task DtroUpdatedWithLaterSchemaFromFileShouldBeSavedCorrectly()
        {
            string oldSchemaVersion = "3.3.1";
            string createFileWithSchema3_3_1 = "JSON-3.3.1-example-Derbyshire 2024 DJ388 partial.json";
            string updateFileWithSchema3_4_0 = "JSON-3.4.0-example-Derbyshire 2024 DJ388 partial.json";

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string tempFilePathForDtroCreation = Dtros.CreateTempFileWithTraModified(oldSchemaVersion, createFileWithSchema3_3_1, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePathForDtroCreation, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == createDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePathForDtroCreation)}: expected status code is {HttpStatusCode.Created} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Prepare DTRO update
            string tempFilePathForDtroUpdate = Dtros.CreateTempFileForDtroUpdate(schemaVersionToTest, updateFileWithSchema3_4_0, publisher.TraId);

            // Send DTRO update
            string dtroId = await Dtros.GetIdFromResponseJsonAsync(createDtroResponse);
            HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromFileAsync(tempFilePathForDtroUpdate, dtroId, publisher);
            string updateDtroResponseJson = await updateDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == updateDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePathForDtroUpdate)}: expected status code is {HttpStatusCode.OK} but actual status code was {updateDtroResponse.StatusCode}, with response body\n{updateDtroResponseJson}");

            // Get updated DTRO
            HttpResponseMessage getDtroResponse = await Dtros.GetDtroAsync(dtroId, publisher);
            string dtroResponseJson = await getDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == getDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePathForDtroUpdate)}: expected status code is {HttpStatusCode.OK} but actual status code was {getDtroResponse.StatusCode}, with response body\n{dtroResponseJson}");

            // Add ID to DTRO update and compare
            string modifiedUpdateJson = Dtros.ModifySentJsonWithinFileForComparison(schemaVersionToTest, tempFilePathForDtroUpdate, dtroId);
            JsonMethods.CompareJson(modifiedUpdateJson, dtroResponseJson);
        }

        [Fact]
        public async Task DtroUpdatedWithLaterSchemaFromJsonBodyShouldBeSavedCorrectly()
        {
            string oldSchemaVersion = "3.3.1";
            string createFileWithSchema3_3_1 = "JSON-3.3.1-example-Derbyshire 2024 DJ388 partial.json";
            string updateFileWithSchema3_4_0 = "JSON-3.4.0-example-Derbyshire 2024 DJ388 partial.json";

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string createDtroJsonWithTraModified = Dtros.GetJsonFromFileAndModifyTra(oldSchemaVersion, createFileWithSchema3_3_1, publisher.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraModified, publisher);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode, $"File {createFileWithSchema3_3_1}: expected status code is {HttpStatusCode.Created} but actual status code was {dtroCreationResponse.StatusCode}, with response body\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = Dtros.CreateJsonForDtroUpdate(schemaVersionToTest, updateFileWithSchema3_4_0, publisher.TraId);

            // Send DTRO update
            string dtroId = await Dtros.GetIdFromResponseJsonAsync(dtroCreationResponse);
            HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromJsonBodyAsync(dtroUpdateJson, dtroId, publisher);
            string updateDtroResponseJson = await updateDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == updateDtroResponse.StatusCode, $"File {updateFileWithSchema3_4_0}: expected status code is {HttpStatusCode.OK} but actual status code was {updateDtroResponse.StatusCode}, with response body\n{updateDtroResponseJson}");

            // Get updated DTRO
            HttpResponseMessage getDtroResponse = await Dtros.GetDtroAsync(dtroId, publisher);
            string dtroResponseJson = await getDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == getDtroResponse.StatusCode, $"File {updateFileWithSchema3_4_0}: expected status code is {HttpStatusCode.OK} but actual status code was {getDtroResponse.StatusCode}, with response body\n{dtroResponseJson}");

            // Add ID to DTRO update and compare
            string modifiedUpdateJson = Dtros.ModifySentJsonForComparison(schemaVersionToTest, dtroUpdateJson, dtroId);
            JsonMethods.CompareJson(modifiedUpdateJson, dtroResponseJson);
        }
    }
}