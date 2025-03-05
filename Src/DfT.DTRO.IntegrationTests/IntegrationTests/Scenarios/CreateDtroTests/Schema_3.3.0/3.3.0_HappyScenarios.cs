using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelper;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.FileHelper;
using Newtonsoft.Json.Linq;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.CreateDtroTests.Schema_3_3_0
{
    public class HappyScenarios : BaseTest
    {
        readonly static string schemaVersionToTest = "3.3.0";

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

            // Get created DTRO
            string dtroId = await GetIdFromResponseJsonAsync(createDtroResponse);
            HttpResponseMessage getDtroResponse = await Dtros.GetDtroAsync(dtroId, publisher);
            Assert.Equal(HttpStatusCode.OK, getDtroResponse.StatusCode);
            string dtroResponseJson = await getDtroResponse.Content.ReadAsStringAsync();

            // Add ID to sent DTRO for comparison purposes
            JObject createJsonObject = JObject.Parse(createDtroJsonWithTraUpdated);
            createJsonObject["id"] = dtroId;

            // Check retrieved DTRO matches sent DTRO
            string sentCreateJsonWithId = createJsonObject.ToString();
            string sentCreateJsonWithIdToCamelCase = ConvertJsonKeysToCamelCase(sentCreateJsonWithId);
            CompareJson(sentCreateJsonWithIdToCamelCase, dtroResponseJson);
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

            // Get created DTRO
            string dtroId = await GetIdFromResponseJsonAsync(createDtroResponse);
            HttpResponseMessage getDtroResponse = await Dtros.GetDtroAsync(dtroId, publisher);
            Assert.Equal(HttpStatusCode.OK, getDtroResponse.StatusCode);
            string dtroResponseJson = await getDtroResponse.Content.ReadAsStringAsync();

            // Add ID to sent DTRO for comparison purposes
            JObject createJsonObject = JObject.Parse(createDtroJsonWithTraUpdated);
            createJsonObject["id"] = dtroId;

            // Check retrieved DTRO matches sent DTRO
            string sentCreateJsonWithId = createJsonObject.ToString();
            string sentCreateJsonWithIdToCamelCase = ConvertJsonKeysToCamelCase(sentCreateJsonWithId);
            CompareJson(sentCreateJsonWithIdToCamelCase, dtroResponseJson);
        }
    }
}