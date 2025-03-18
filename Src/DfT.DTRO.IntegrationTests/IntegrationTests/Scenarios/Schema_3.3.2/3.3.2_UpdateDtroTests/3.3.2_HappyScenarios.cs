using Newtonsoft.Json.Linq;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.FileHelper;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_3_2.UpdateDtroTests
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
        public async Task DtroUpdatedFromFileShouldBeSavedCorrectly(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);
            string userGuidToGenerateFileNamePrefix = await JsonMethods.GetIdFromResponseJsonAsync(createUserResponse);
            // Avoid files being overwritten by using a unique prefix in file names for each test
            string uniquePrefixOnFileName = $"{userGuidToGenerateFileNamePrefix.Substring(0, 5)}-";

            // Prepare DTRO
            string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersionToTest}/{fileName}";
            string createDtroJson = File.ReadAllText(createDtroFile);
            string createDtroJsonWithTraUpdated = Dtros.ModifyTraIdInDtro(schemaVersionToTest, createDtroJson, publisher.TraId);
            string nameOfCopyFile = $"{uniquePrefixOnFileName}{fileName}";
            string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, createDtroJsonWithTraUpdated);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
            Assert.Equal(HttpStatusCode.Created, createDtroResponse.StatusCode);

            // Prepare DTRO update
            string updateJson = Dtros.ModifyActionTypeAndTroName(createDtroJsonWithTraUpdated, schemaVersionToTest);
            string nameOfUpdateJsonFile = $"updated{nameOfCopyFile}";
            string tempUpdateFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfUpdateJsonFile}";
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfUpdateJsonFile, updateJson);

            // Send DTRO update
            string dtroId = await JsonMethods.GetIdFromResponseJsonAsync(createDtroResponse);
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
            JsonMethods.CompareJson(sentUpdateJsonWithId, dtroResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetDtroFileNames))]
        public async Task DtroUpdatedFromJsonBodyShouldBeSavedCorrectly(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

            // Prepare DTRO
            string createDtroFile = $"{AbsolutePathToDtroExamplesDirectory}/{schemaVersionToTest}/{fileName}";
            string createDtroJson = File.ReadAllText(createDtroFile);
            string createDtroJsonWithTraUpdated = Dtros.ModifyTraIdInDtro(schemaVersionToTest, createDtroJson, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraUpdated, publisher);
            Assert.Equal(HttpStatusCode.Created, createDtroResponse.StatusCode);

            // Prepare DTRO update
            string updateJson = Dtros.ModifyActionTypeAndTroName(createDtroJsonWithTraUpdated, schemaVersionToTest);

            // Send DTRO update
            string dtroId = await JsonMethods.GetIdFromResponseJsonAsync(createDtroResponse);
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
            JsonMethods.CompareJson(sentUpdateJsonWithId, dtroResponseJson);
        }

        [Fact]
        public async Task DtroUpdatedWithLaterSchemaFromFileShouldBeSavedCorrectly()
        {
            string oldSchemaVersion = "3.3.1";
            string createFileWithSchema3_3_1 = "JSON-3.3.1-example-Derbyshire 2024 DJ388 partial.json";
            string updateFileWithSchema3_3_2 = "JSON-3.3.2-example-Derbyshire 2024 DJ388 partial.json";

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);
            string userGuidToGenerateFileNamePrefix = await JsonMethods.GetIdFromResponseJsonAsync(createUserResponse);
            // Avoid files being overwritten by using a unique prefix in file names for each test
            string uniquePrefixOnFileName = $"{userGuidToGenerateFileNamePrefix.Substring(0, 5)}-";

            // Prepare DTRO
            string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{oldSchemaVersion}/{createFileWithSchema3_3_1}";
            string createDtroJson = File.ReadAllText(createDtroFile);
            string createDtroJsonWithTraUpdated = Dtros.ModifyTraIdInDtro(oldSchemaVersion, createDtroJson, publisher.TraId);
            string nameOfCopyFile = $"{uniquePrefixOnFileName}{createFileWithSchema3_3_1}";
            string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, createDtroJsonWithTraUpdated);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
            Assert.Equal(HttpStatusCode.Created, createDtroResponse.StatusCode);

            // Prepare DTRO update
            string updateDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersionToTest}/{updateFileWithSchema3_3_2}";
            string updateDtroJson = File.ReadAllText(updateDtroFile);
            string updateDtroJsonWithTraUpdated = Dtros.ModifyTraIdInDtro(schemaVersionToTest, updateDtroJson, publisher.TraId);
            string nameOfCopyUpdateFile = $"update{uniquePrefixOnFileName}{updateFileWithSchema3_3_2}";
            string tempUpdateFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyUpdateFile}";
            string updateJson = Dtros.ModifyActionTypeAndTroName(updateDtroJsonWithTraUpdated, schemaVersionToTest);
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyUpdateFile, updateJson);

            // Send DTRO update
            string dtroId = await JsonMethods.GetIdFromResponseJsonAsync(createDtroResponse);
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
            JsonMethods.CompareJson(sentUpdateJsonWithId, dtroResponseJson);
        }

        [Fact]
        public async Task DtroUpdatedWithLaterSchemaFromJsonBodyShouldBeSavedCorrectly()
        {
            string oldSchemaVersion = "3.3.1";
            string createFileWithSchema3_3_1 = "JSON-3.3.1-example-Derbyshire 2024 DJ388 partial.json";
            string updateFileWithSchema3_3_2 = "JSON-3.3.2-example-Derbyshire 2024 DJ388 partial.json";

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

            // Prepare DTRO
            string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{oldSchemaVersion}/{createFileWithSchema3_3_1}";
            string createDtroJson = File.ReadAllText(createDtroFile);
            string createDtroJsonWithTraUpdated = Dtros.ModifyTraIdInDtro(oldSchemaVersion, createDtroJson, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraUpdated, publisher);
            Assert.Equal(HttpStatusCode.Created, createDtroResponse.StatusCode);

            // Prepare DTRO update
            string updateDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersionToTest}/{updateFileWithSchema3_3_2}";
            string updateDtroJson = File.ReadAllText(updateDtroFile);
            string updateDtroJsonWithTraUpdated = Dtros.ModifyTraIdInDtro(schemaVersionToTest, updateDtroJson, publisher.TraId);
            string updateJson = Dtros.ModifyActionTypeAndTroName(updateDtroJsonWithTraUpdated, schemaVersionToTest);

            // Send DTRO update
            string dtroId = await JsonMethods.GetIdFromResponseJsonAsync(createDtroResponse);
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
            JsonMethods.CompareJson(sentUpdateJsonWithId, dtroResponseJson);
        }
    }
}