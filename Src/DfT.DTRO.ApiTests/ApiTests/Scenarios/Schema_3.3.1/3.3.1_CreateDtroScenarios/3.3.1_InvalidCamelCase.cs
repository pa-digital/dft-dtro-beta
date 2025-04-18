using Newtonsoft.Json;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Consts;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Enums;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Extensions;
using DfT.DTRO.ApiTests.ApiTests.Helpers.JsonHelpers;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.JsonHelpers.ErrorJsonResponseProcessor;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.TestConfig;

namespace DfT.DTRO.ApiTests.ApiTests.Schema_3_3_1.DtroCreationScenarios
{
    public class InvalidCamelCase : BaseTest
    {
        readonly static string schemaVersionToTest = SchemaVersions._3_3_1;
        readonly static string schemaVersionWithInvalidCamelCase = "3.3.2";

        public static IEnumerable<object[]> GetDtroNamesOfFilesWithInvalidCamelCase()
        {
            DirectoryInfo directoryPath = new DirectoryInfo($"{PathToDtroExamplesDirectory}/{schemaVersionWithInvalidCamelCase}");
            FileInfo[] files = directoryPath.GetFiles();

            if (EnvironmentName == EnvironmentType.Local)
            {
                foreach (FileInfo file in files)
                {
                    yield return new object[] { file.Name };
                }
            }
            else
            {
                yield return new object[] { files[0].Name };
            }
        }

        [Theory]
        [MemberData(nameof(GetDtroNamesOfFilesWithInvalidCamelCase))]
        public async Task DtroSubmittedFromJsonBodyWithCamelCaseShouldBeRejected(string nameOfFileWithInvalidCamelCase)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithInvalidCamelCase}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = nameOfFileWithInvalidCamelCase
                                    .GetJsonFromFile(schemaVersionWithInvalidCamelCase)
                                    .ModifyTraInDtroJson(schemaVersionWithInvalidCamelCase, (int)publisher.TraId)
                                    .ModifySchemaVersion(schemaVersionToTest);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {nameOfFileWithInvalidCamelCase}:\n\n{dtroCreationResponseJson}");

            // Check DTRO response JSON
            ErrorJson jsonErrorResponse = await ErrorJsonResponseProcessor.GetErrorJson(dtroCreationResponse);
            Assert.Equal("Case naming convention exception", jsonErrorResponse.Message);
            Assert.StartsWith("All property names must conform to pascal case naming conventions. The following properties violate this: [source, provision, regulatedPlace", jsonErrorResponse.Error);
        }

        [Theory]
        [MemberData(nameof(GetDtroNamesOfFilesWithInvalidCamelCase))]
        public async Task DtroSubmittedFromFileWithCamelCaseShouldBeRejected(string nameOfFileWithInvalidCamelCase)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithInvalidCamelCase}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = nameOfFileWithInvalidCamelCase
                                    .GetJsonFromFile(schemaVersionWithInvalidCamelCase)
                                    .ModifyTraInDtroJson(schemaVersionWithInvalidCamelCase, (int)publisher.TraId)
                                    .ModifySchemaVersion(schemaVersionToTest);

            string dtroTempFilePath = dtroCreationJson.CreateDtroTempFile(nameOfFileWithInvalidCamelCase, publisher);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroTempFilePath.SendFileInDtroCreationRequestAsync(publisher);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");

            // Check DTRO response JSON
            ErrorJson jsonErrorResponse = await ErrorJsonResponseProcessor.GetErrorJson(dtroCreationResponse);
            Assert.Equal("Case naming convention exception", jsonErrorResponse.Message);
            Assert.StartsWith("All property names must conform to pascal case naming conventions. The following properties violate this: [source, provision, regulatedPlace", jsonErrorResponse.Error);
        }
    }
}