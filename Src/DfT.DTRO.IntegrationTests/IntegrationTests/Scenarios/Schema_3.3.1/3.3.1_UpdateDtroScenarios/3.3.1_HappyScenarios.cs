using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Enums;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_3_1.DtroUpdateScenarios
{
    public class HappyScenarios : BaseTest
    {
        readonly static string schemaVersionToTest = "3.3.1";

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
                                                        .AddDtroIdToJson(dtroId)
                                                        .ConvertJsonKeysToCamelCase();

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
                                                        .AddDtroIdToJson(dtroId)
                                                        .ConvertJsonKeysToCamelCase();

            JsonMethods.CompareJson(expectedUpdateJsonForComparison, dtroGetResponseJson);
        }
    }
}