using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_3_1.DtroCreationScenarios
{
    public class HappyScenarios : BaseTest
    {
        readonly static string schemaVersionToTest = "3.3.1";

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
        public async Task DtroSubmittedFromJsonBodyShouldBeSavedCorrectly(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Get test user to send DTRO and read it back
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

            // Get created DTRO
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroGetResponse = await dtroId.GetDtroResponseByIdAsync(publisher);
            string dtroGetResponseJson = await dtroGetResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroGetResponse.StatusCode,
                $"Response JSON for file {fileName}:\n\n{dtroGetResponseJson}");

            // Amend sent DTRO and compare
            string expectedCreationJsonForComparison = dtroCreationJson
                                                        .AddDtroIdToJson(dtroId)
                                                        .ConvertJsonKeysToCamelCase();

            JsonMethods.CompareJson(expectedCreationJsonForComparison, dtroGetResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetNamesOfDtroExampleFiles))]
        public async Task DtroSubmittedFromFileShouldBeSavedCorrectly(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Get test user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            string dtroTempFilePath = dtroCreationJson.CreateDtroTempFile(fileName, publisher);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroTempFilePath.SendFileInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");

            // Get created DTRO
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroGetResponse = await dtroId.GetDtroResponseByIdAsync(publisher);
            string dtroGetResponseJson = await dtroGetResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroGetResponse.StatusCode,
                $"Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroGetResponseJson}");

            // Amend sent DTRO and compare
            string expectedCreationJsonForComparison = dtroCreationJson
                                                        .AddDtroIdToJson(dtroId)
                                                        .ConvertJsonKeysToCamelCase();

            JsonMethods.CompareJson(expectedCreationJsonForComparison, dtroGetResponseJson);
        }
    }
}