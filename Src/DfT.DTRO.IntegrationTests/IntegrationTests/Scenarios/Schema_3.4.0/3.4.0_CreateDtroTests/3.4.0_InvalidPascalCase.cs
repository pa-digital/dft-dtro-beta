using Newtonsoft.Json;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers.ErrorJsonResponseProcessor;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.CreateDtroTests
{
    public class InvalidPascalCase : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly static string filesWithInvalidPascalCase = "3.3.1";
        readonly string expectedErrorJson = """
            {
                "ruleError_0": {
                    "message": "Property 'Source' has not been defined and the schema does not allow additional properties.",
                    "path": "Source",
                    "value": "Source",
                    "errorType": "AdditionalProperties"
                },
                "ruleError_1": {
                    "message": "JSON is valid against no schemas from 'oneOf'.",
                    "path": "",
                    "value": null,
                    "errorType": "OneOf"
                }
            }
            """;

        public static IEnumerable<object[]> GetDtroFileNames()
        {
            DirectoryInfo directoryPath = new DirectoryInfo($"{AbsolutePathToDtroExamplesDirectory}/{filesWithInvalidPascalCase}");
            FileInfo[] files = directoryPath.GetFiles();

            foreach (FileInfo file in files)
            {
                yield return new object[] { file.Name };
            }
        }

        [Theory]
        [MemberData(nameof(GetDtroFileNames))]
        public async Task DtroSubmittedFromFileWithPascalCaseShouldBeRejected(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

            // Prepare DTRO
            string tempFilePath = FileHelper.CreateTempFileWithTraAndSchemaVersionModified(schemaVersionToTest, filesWithInvalidPascalCase, fileName, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePath)}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Check DTRO response JSON
            JsonMethods.CompareJson(expectedErrorJson, createDtroResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetDtroFileNames))]
        public async Task DtroSubmittedFromJsonBodyWithPascalCaseShouldBeRejected(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

            // Prepare DTRO
            string createDtroJsonWithTraAndSchemaVersionModified = FileHelper.GetJsonFromFileAndModifyTraAndSchemaVersion(schemaVersionToTest, filesWithInvalidPascalCase, fileName, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraAndSchemaVersionModified, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {fileName}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Check DTRO response JSON
            JsonMethods.CompareJson(expectedErrorJson, createDtroResponseJson);
        }
    }
}