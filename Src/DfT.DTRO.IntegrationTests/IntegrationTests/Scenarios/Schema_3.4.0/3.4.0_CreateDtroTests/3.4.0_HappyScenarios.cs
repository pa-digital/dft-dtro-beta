using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.CreateDtroTests
{
    public class HappyScenarios : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";

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
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string tempFilePath = FileHelper.CreateTempFileWithTraModified(schemaVersionToTest, fileName, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == createDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePath)}: expected status code is {HttpStatusCode.Created} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Get created DTRO
            string dtroId = await Dtros.GetIdFromResponseJsonAsync(createDtroResponse);
            string getDtroResponseJson = await Dtros.GetDtroResponseJsonAsync(dtroId, publisher);

            // Add ID to sent DTRO and compare
            string modifiedCreateJson = Dtros.ModifySentJsonWithinFileForComparison(schemaVersionToTest, tempFilePath, dtroId);
            JsonMethods.CompareJson(modifiedCreateJson, getDtroResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetDtroFileNames))]
        public async Task DtroSubmittedFromJsonBodyShouldBeSavedCorrectly(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string createDtroJsonWithTraModified = Dtros.GetJsonFromFileAndModifyTra(schemaVersionToTest, fileName, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraModified, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == createDtroResponse.StatusCode, $"File {fileName}: expected status code is {HttpStatusCode.Created} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Get created DTRO
            string dtroId = await Dtros.GetIdFromResponseJsonAsync(createDtroResponse);
            string getDtroResponseJson = await Dtros.GetDtroResponseJsonAsync(dtroId, publisher);

            // Add ID to sent DTRO and compare
            string modifiedCreateJson = Dtros.ModifySentJsonForComparison(schemaVersionToTest, createDtroJsonWithTraModified, dtroId);
            JsonMethods.CompareJson(modifiedCreateJson, getDtroResponseJson);
        }
    }
}