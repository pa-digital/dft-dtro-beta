using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_3_1.UpdateDtroTests
{
    public class HappyScenarios : BaseTest
    {
        readonly static string schemaVersionToTest = "3.3.1";

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
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string tempFilePathForDtroCreation = FileHelper.CreateTempFileWithTraModified(schemaVersionToTest, fileName, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePathForDtroCreation, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == createDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePathForDtroCreation)}: expected status code is {HttpStatusCode.Created} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Prepare DTRO update
            string tempFilePathForDtroUpdate = FileHelper.CreateTempFileForDtroUpdate(schemaVersionToTest, tempFilePathForDtroCreation);

            // Send DTRO update
            string dtroId = await JsonMethods.GetIdFromResponseJsonAsync(createDtroResponse);
            HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromFileAsync(tempFilePathForDtroUpdate, dtroId, publisher);
            string updateDtroResponseJson = await updateDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == updateDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePathForDtroUpdate)}: expected status code is {HttpStatusCode.OK} but actual status code was {updateDtroResponse.StatusCode}, with response body\n{updateDtroResponseJson}");

            // Get updated DTRO
            HttpResponseMessage getDtroResponse = await Dtros.GetDtroAsync(dtroId, publisher);
            string dtroResponseJson = await getDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == getDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePathForDtroUpdate)}: expected status code is {HttpStatusCode.OK} but actual status code was {getDtroResponse.StatusCode}, with response body\n{dtroResponseJson}");

            // Add ID to DTRO update and compare
            string modifiedUpdateJson = JsonMethods.ModifySentJsonWithinFileForComparison(schemaVersionToTest, tempFilePathForDtroUpdate, dtroId);
            JsonMethods.CompareJson(modifiedUpdateJson, dtroResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetDtroFileNames))]
        public async Task DtroUpdatedFromJsonBodyShouldBeSavedCorrectly(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string createDtroJsonWithTraModified = JsonMethods.GetJsonFromFileAndModifyTra(schemaVersionToTest, fileName, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraModified, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == createDtroResponse.StatusCode, $"File {fileName}: expected status code is {HttpStatusCode.Created} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = Dtros.ModifyActionTypeAndTroName(schemaVersionToTest, createDtroJsonWithTraModified);

            // Send DTRO update
            string dtroId = await JsonMethods.GetIdFromResponseJsonAsync(createDtroResponse);
            HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromJsonBodyAsync(dtroUpdateJson, dtroId, publisher);
            string updateDtroResponseJson = await updateDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == updateDtroResponse.StatusCode, $"File {fileName}: expected status code is {HttpStatusCode.OK} but actual status code was {updateDtroResponse.StatusCode}, with response body\n{updateDtroResponseJson}");

            // Get updated DTRO
            HttpResponseMessage getDtroResponse = await Dtros.GetDtroAsync(dtroId, publisher);
            string dtroResponseJson = await getDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == getDtroResponse.StatusCode, $"File {fileName}: expected status code is {HttpStatusCode.OK} but actual status code was {getDtroResponse.StatusCode}, with response body\n{dtroResponseJson}");

            // Add ID to DTRO update and compare
            string modifiedUpdateJson = JsonMethods.ModifySentJsonForComparison(schemaVersionToTest, dtroUpdateJson, dtroId);
            JsonMethods.CompareJson(modifiedUpdateJson, dtroResponseJson);
        }
    }
}