using Newtonsoft.Json;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers.ErrorJsonResponseProcessor;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_3_0.CreateDtroScenarios
{
    public class InvalidCamelCase : BaseTest
    {
        readonly static string schemaVersionToTest = "3.3.0";
        readonly static string schemaVersionWithInvalidCamelCase = "3.3.2";

        public static IEnumerable<object[]> GetDtroNamesOfFilesWithInvalidCamelCase()
        {
            DirectoryInfo directoryPath = new DirectoryInfo($"{PathToDtroExamplesDirectory}/{schemaVersionWithInvalidCamelCase}");
            FileInfo[] files = directoryPath.GetFiles();

            foreach (FileInfo file in files)
            {
                yield return new object[] { file.Name };
            }
        }

        [Theory]
        [MemberData(nameof(GetDtroNamesOfFilesWithInvalidCamelCase))]
        public async Task DtroSubmittedFromFileWithCamelCaseShouldBeRejected(string nameOfFileWithInvalidCamelCase)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithInvalidCamelCase}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string tempFilePath = Dtros.CreateTempFileWithTraAndSchemaVersionModified(schemaVersionToTest, schemaVersionWithInvalidCamelCase, nameOfFileWithInvalidCamelCase, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePath)}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Check DTRO response JSON
            ErrorJson jsonErrorResponse = await ErrorJsonResponseProcessor.GetErrorJson(createDtroResponse);
            Assert.Equal("Case naming convention exception", jsonErrorResponse.Message);
            Assert.StartsWith("All property names must conform to pascal case naming conventions. The following properties violate this: [source, provision, regulatedPlace", jsonErrorResponse.Error);
        }

        [Theory]
        [MemberData(nameof(GetDtroNamesOfFilesWithInvalidCamelCase))]
        public async Task DtroSubmittedFromJsonBodyWithCamelCaseShouldBeRejected(string nameOfFileWithInvalidCamelCase)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithInvalidCamelCase}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string createDtroJsonWithTraModifiedAndInvalidCamelCase = Dtros.GetJsonFromFileAndModifyTraAndSchemaVersion(schemaVersionToTest, schemaVersionWithInvalidCamelCase, nameOfFileWithInvalidCamelCase, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraModifiedAndInvalidCamelCase, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {nameOfFileWithInvalidCamelCase}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Check DTRO response JSON
            ErrorJson jsonErrorResponse = await ErrorJsonResponseProcessor.GetErrorJson(createDtroResponse);
            Assert.Equal("Case naming convention exception", jsonErrorResponse.Message);
            Assert.StartsWith("All property names must conform to pascal case naming conventions. The following properties violate this: [source, provision, regulatedPlace", jsonErrorResponse.Error);
        }
    }
}