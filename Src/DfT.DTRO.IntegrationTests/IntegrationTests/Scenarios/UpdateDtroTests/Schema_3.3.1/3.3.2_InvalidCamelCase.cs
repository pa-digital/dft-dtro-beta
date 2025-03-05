using System.Collections.Generic;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.FileHelper;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelper;
using Newtonsoft.Json;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.UpdateDtroTests.Schema_3_3_1
{
    public class InvalidCamelCase : BaseTest
    {
        readonly static string schemaVersionToTest = "3.3.1";
        readonly static string schemaVersionOfFilesWithInvalidCamelCase = "3.3.2";
        string fileToUseWithPascalCaseVersion3_3_1 = "JSON-3.3.1-example-Derbyshire 2024 DJ388 partial.json";

        public static IEnumerable<object[]> GetDtroFileNames()
        {
            DirectoryInfo directoryPath = new DirectoryInfo($"{AbsolutePathToDtroExamplesDirectory}/{schemaVersionOfFilesWithInvalidCamelCase}");
            FileInfo[] files = directoryPath.GetFiles();

            foreach (FileInfo file in files)
            {
                yield return new object[] { file.Name };
            }
        }

        [Theory]
        [MemberData(nameof(GetDtroFileNames))]
        public async Task DtroUpdatedFromFileWithCamelCaseShouldBeRejected(string nameOfFileWithCamelCaseVersion3_3_2)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithCamelCaseVersion3_3_2}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUser(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);
            string userGuid = await GetIdFromResponseJsonAsync(createUserResponse);

            // Prepare DTRO
            string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersionToTest}/{fileToUseWithPascalCaseVersion3_3_1}";
            string createDtroJson = File.ReadAllText(createDtroFile);
            string createDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(schemaVersionToTest, createDtroJson, publisher.TraId);
            string nameOfCopyFile = $"{userGuid.Substring(0, 5)}{fileToUseWithPascalCaseVersion3_3_1}";
            string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, createDtroJsonWithTraUpdated);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
            Assert.Equal(HttpStatusCode.Created, createDtroResponse.StatusCode);

            // Prepare DTRO update
            string updateDtroFile = $"{AbsolutePathToDtroExamplesDirectory}/{schemaVersionOfFilesWithInvalidCamelCase}/{nameOfFileWithCamelCaseVersion3_3_2}";
            string updateDtroJson = File.ReadAllText(updateDtroFile);
            string updateDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(schemaVersionOfFilesWithInvalidCamelCase, updateDtroJson, publisher.TraId);
            string updateDtroJsonWithSchemaVersionUpdated = Dtros.UpdateSchemaVersionInDtro(updateDtroJsonWithTraUpdated, schemaVersionToTest);
            string updateJsonWithModifiedActionTypeAndTroName = Dtros.UpdateActionTypeAndTroName(updateDtroJsonWithSchemaVersionUpdated, schemaVersionOfFilesWithInvalidCamelCase);
            string nameOfUpdateCopyFile = $"update{userGuid.Substring(0, 5)}{nameOfFileWithCamelCaseVersion3_3_2}";
            string tempUpdateFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfUpdateCopyFile}";
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfUpdateCopyFile, updateJsonWithModifiedActionTypeAndTroName);

            // Send DTRO update
            string dtroId = await GetIdFromResponseJsonAsync(createDtroResponse);
            HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromFileAsync(tempUpdateFilePath, dtroId, publisher);
            Assert.Equal(HttpStatusCode.BadRequest, updateDtroResponse.StatusCode);

            // Check DTRO response JSON
            string dtroResponseJson = await updateDtroResponse.Content.ReadAsStringAsync();
            dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(dtroResponseJson)!;
            string responseMessage = jsonDeserialised.message.ToString();
            string responseError = jsonDeserialised.error.ToString();
            Assert.Equal(responseMessage, "Case naming convention exception");
            Assert.StartsWith("All property names must conform to pascal case naming conventions. The following properties violate this: [source, provision, regulatedPlace", responseError);
        }

        [Theory]
        [MemberData(nameof(GetDtroFileNames))]
        public async Task DtroUpdatedFromJsonBodyWithCamelCaseShouldBeRejected(string nameOfFileWithCamelCaseVersion3_3_2)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithCamelCaseVersion3_3_2}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUser(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

            // Prepare DTRO
            string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersionToTest}/{fileToUseWithPascalCaseVersion3_3_1}";
            string createDtroJson = File.ReadAllText(createDtroFile);
            string createDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(schemaVersionToTest, createDtroJson, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraUpdated, publisher);
            Assert.Equal(HttpStatusCode.Created, createDtroResponse.StatusCode);

            // Prepare DTRO update
            string updateDtroFile = $"{AbsolutePathToDtroExamplesDirectory}/{schemaVersionOfFilesWithInvalidCamelCase}/{nameOfFileWithCamelCaseVersion3_3_2}";
            string updateDtroJson = File.ReadAllText(updateDtroFile);
            string updateDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(schemaVersionOfFilesWithInvalidCamelCase, updateDtroJson, publisher.TraId);
            string updateDtroJsonWithSchemaVersionUpdated = Dtros.UpdateSchemaVersionInDtro(updateDtroJsonWithTraUpdated, schemaVersionToTest);
            string updateJsonWithModifiedActionTypeAndTroName = Dtros.UpdateActionTypeAndTroName(updateDtroJsonWithSchemaVersionUpdated, schemaVersionOfFilesWithInvalidCamelCase);

            // Send DTRO update
            string dtroId = await GetIdFromResponseJsonAsync(createDtroResponse);
            HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromJsonBodyAsync(updateJsonWithModifiedActionTypeAndTroName, dtroId, publisher);
            Assert.Equal(HttpStatusCode.BadRequest, updateDtroResponse.StatusCode);

            // Check DTRO response JSON
            string dtroResponseJson = await updateDtroResponse.Content.ReadAsStringAsync();
            dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(dtroResponseJson)!;
            string responseMessage = jsonDeserialised.message.ToString();
            string responseError = jsonDeserialised.error.ToString();
            Assert.Equal(responseMessage, "Case naming convention exception");
            Assert.StartsWith("All property names must conform to pascal case naming conventions. The following properties violate this: [source, provision, regulatedPlace", responseError);
        }
    }
}