using System.Collections.Generic;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelper;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.FileHelper;
using Newtonsoft.Json.Linq;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.UpdateDtroTests.Schema_3_3_2
{
    public class HappyScenarios : BaseTest
    {
        readonly static string schemaVersionToTest = "3.3.2";

        public static IEnumerable<object[]> GetDtroFileNames()
        {
            DirectoryInfo directoryPath = new DirectoryInfo($"{AbsolutePathToDtroExamplesDirectory}/{schemaVersionToTest}");
            FileInfo[] files = directoryPath.GetFiles();

            foreach (FileInfo file in files)
            {
                yield return new object[] { file.Name };
            }
        }

        [Theory]
        [MemberData(nameof(GetDtroFileNames))]
        public async Task DtroSubmittedFromFileShouldBeSavedCorrectly(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUser(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);
            string userGuid = await GetIdFromResponseJsonAsync(createUserResponse);

            // Prepare DTRO
            string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersionToTest}/{fileName}";
            string createDtroJson = File.ReadAllText(createDtroFile);
            string createDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(schemaVersionToTest, createDtroJson, publisher.TraId);
            string nameOfCopyFile = $"{userGuid.Substring(0, 5)}{fileName}";
            string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, createDtroJsonWithTraUpdated);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
            Assert.Equal(HttpStatusCode.Created, createDtroResponse.StatusCode);

            // Prepare DTRO update
            string updateJson = Dtros.UpdateActionTypeAndTroName(schemaVersionToTest, createDtroJsonWithTraUpdated);
            string nameOfUpdateJsonFile = $"updated{nameOfCopyFile}";
            string tempUpdateFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfUpdateJsonFile}";
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfUpdateJsonFile, updateJson);

            // Send DTRO update
            string dtroId = await GetIdFromResponseJsonAsync(createDtroResponse);
            HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromFileAsync(tempUpdateFilePath, dtroId, publisher);
            Assert.Equal(HttpStatusCode.OK, updateDtroResponse.StatusCode);

            // Get updated DTRO
            HttpResponseMessage getDtroResponse = await Dtros.GetDtroAsync(dtroId, publisher);
            Assert.Equal(HttpStatusCode.OK, getDtroResponse.StatusCode);
            string dtroResponseJson = await getDtroResponse.Content.ReadAsStringAsync();

            // Add ID to updated DTRO for comparison purposes
            JObject updateJsonObject = JObject.Parse(updateJson);
            updateJsonObject["id"] = dtroId;

            // Check retrieved DTRO matches updated DTRO
            string sentUpdateJsonWithId = updateJsonObject.ToString();
            string sentUpdateJsonWithIdToCamelCase = ConvertJsonKeysToCamelCase(sentUpdateJsonWithId);
            CompareJson(sentUpdateJsonWithIdToCamelCase, dtroResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetDtroFileNames))]
        public async Task DtroSubmittedFromJsonBodyShouldBeSavedCorrectly(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUser(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

            // Prepare DTRO
            string createDtroFile = $"{AbsolutePathToDtroExamplesDirectory}/{schemaVersionToTest}/{fileName}";
            string createDtroJson = File.ReadAllText(createDtroFile);
            string createDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(schemaVersionToTest, createDtroJson, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraUpdated, publisher);
            Assert.Equal(HttpStatusCode.Created, createDtroResponse.StatusCode);

            // Prepare DTRO update
            string updateJson = Dtros.UpdateActionTypeAndTroName(schemaVersionToTest, createDtroJsonWithTraUpdated);

            // Send DTRO update
            string dtroId = await GetIdFromResponseJsonAsync(createDtroResponse);
            HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromJsonBodyAsync(updateJson, dtroId, publisher);
            Assert.Equal(HttpStatusCode.OK, updateDtroResponse.StatusCode);

            // Get updated DTRO
            HttpResponseMessage getDtroResponse = await Dtros.GetDtroAsync(dtroId, publisher);
            Assert.Equal(HttpStatusCode.OK, getDtroResponse.StatusCode);
            string dtroResponseJson = await getDtroResponse.Content.ReadAsStringAsync();

            // Add ID to updated DTRO for comparison purposes
            JObject updateJsonObject = JObject.Parse(updateJson);
            updateJsonObject["id"] = dtroId;

            // Check retrieved DTRO matches updated DTRO
            string sentUpdateJsonWithId = updateJsonObject.ToString();
            string sentUpdateJsonWithIdToCamelCase = ConvertJsonKeysToCamelCase(sentUpdateJsonWithId);
            CompareJson(sentUpdateJsonWithIdToCamelCase, dtroResponseJson);
        }
    }
}