using Newtonsoft.Json;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers.ErrorJsonResponseProcessor;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Enums;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.DtroCreationScenarios
{
    public class InvalidPascalCase : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly static string schemaVersionWithInvalidPascalCase = "3.3.1";

        public static IEnumerable<object[]> GetDtroNamesOfFilesWithInvalidPascalCase()
        {
            DirectoryInfo directoryPath = new DirectoryInfo($"{PathToDtroExamplesDirectory}/{schemaVersionWithInvalidPascalCase}");
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
        [MemberData(nameof(GetDtroNamesOfFilesWithInvalidPascalCase))]
        public async Task DtroSubmittedFromJsonBodyWithPascalCaseShouldBeRejected(string nameOfFileWithInvalidPascalCase)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithInvalidPascalCase}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string dtroCreationJson = nameOfFileWithInvalidPascalCase
                                    .GetJsonFromFile(schemaVersionWithInvalidPascalCase)
                                    .ModifyTraInDtroJson(schemaVersionWithInvalidPascalCase, publisher.TraId)
                                    .ModifySchemaVersion(schemaVersionToTest);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode,
                $"Response JSON for file {nameOfFileWithInvalidPascalCase}:\n\n{dtroCreationResponseJson}");

            // Check DTRO response JSON
            Assert.True(dtroCreationResponseJson.Contains("Property 'Source' has not been defined and the schema does not allow additional properties."),
                $"Response JSON for file {nameOfFileWithInvalidPascalCase}:\n\n{dtroCreationResponseJson}");
            Assert.True(dtroCreationResponseJson.Contains("Required properties are missing from object: source."),
                $"Response JSON for file {nameOfFileWithInvalidPascalCase}:\n\n{dtroCreationResponseJson}");
        }

        [Theory]
        [MemberData(nameof(GetDtroNamesOfFilesWithInvalidPascalCase))]
        public async Task DtroSubmittedFromFileWithPascalCaseShouldBeRejected(string nameOfFileWithInvalidPascalCase)
        {
            Console.WriteLine($"\nTesting with file {nameOfFileWithInvalidPascalCase}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string dtroCreationJson = nameOfFileWithInvalidPascalCase
                                    .GetJsonFromFile(schemaVersionWithInvalidPascalCase)
                                    .ModifyTraInDtroJson(schemaVersionWithInvalidPascalCase, publisher.TraId)
                                    .ModifySchemaVersion(schemaVersionToTest);

            string dtroTempFilePath = dtroCreationJson.CreateDtroTempFile(nameOfFileWithInvalidPascalCase, publisher);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroTempFilePath.SendFileInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode,
                $"Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");

            // Check DTRO response JSON
            Assert.True(dtroCreationResponseJson.Contains("Property 'Source' has not been defined and the schema does not allow additional properties."),
                $"Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");
            Assert.True(dtroCreationResponseJson.Contains("Required properties are missing from object: source."),
                $"Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");
        }
    }
}