using Newtonsoft.Json;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers.ErrorJsonResponseProcessor;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.CreateDtroScenarios
{
    public class InvalidPascalCase : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly static string schemaVersionWithInvalidPascalCase = "3.3.1";

        public static IEnumerable<object[]> GetDtroNamesOfFilesWithInvalidPascalCase()
        {
            DirectoryInfo directoryPath = new DirectoryInfo($"{PathToDtroExamplesDirectory}/{schemaVersionWithInvalidPascalCase}");
            FileInfo[] files = directoryPath.GetFiles();

            foreach (FileInfo file in files)
            {
                yield return new object[] { file.Name };
            }
        }

        [Theory]
        [MemberData(nameof(GetDtroNamesOfFilesWithInvalidPascalCase))]
        public async Task DtroSubmittedFromJsonBodyWithPascalCaseShouldBeRejected(string nameOfFileWithInvalidPascalCase)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithInvalidPascalCase}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string createDtroJsonWithTraModifiedAndInvalidPascalCase = Dtros.GetJsonFromFileAndModifyTraAndSchemaVersion(schemaVersionToTest, schemaVersionWithInvalidPascalCase, nameOfFileWithInvalidPascalCase, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraModifiedAndInvalidPascalCase, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {nameOfFileWithInvalidPascalCase}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Check DTRO response JSON
            Assert.True(createDtroResponseJson.Contains("Property 'Source' has not been defined and the schema does not allow additional properties."),
                $"Response JSON for file {nameOfFileWithInvalidPascalCase}:\n\n{createDtroResponseJson}");
            Assert.True(createDtroResponseJson.Contains("Required properties are missing from object: source."),
                $"Response JSON for file {nameOfFileWithInvalidPascalCase}:\n\n{createDtroResponseJson}");
        }

        [Theory]
        [MemberData(nameof(GetDtroNamesOfFilesWithInvalidPascalCase))]
        public async Task DtroSubmittedFromFileWithPascalCaseShouldBeRejected(string nameOfFileWithInvalidPascalCase)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithInvalidPascalCase}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string tempFilePath = Dtros.CreateTempFileWithTraAndSchemaVersionModified(schemaVersionToTest, schemaVersionWithInvalidPascalCase, nameOfFileWithInvalidPascalCase, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePath)}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Check DTRO response JSON
            Assert.True(createDtroResponseJson.Contains("Property 'Source' has not been defined and the schema does not allow additional properties."),
                $"Response JSON for file {Path.GetFileName(tempFilePath)}:\n\n{createDtroResponseJson}");
            Assert.True(createDtroResponseJson.Contains("Required properties are missing from object: source."),
                $"Response JSON for file {Path.GetFileName(tempFilePath)}:\n\n{createDtroResponseJson}");
        }
    }
}