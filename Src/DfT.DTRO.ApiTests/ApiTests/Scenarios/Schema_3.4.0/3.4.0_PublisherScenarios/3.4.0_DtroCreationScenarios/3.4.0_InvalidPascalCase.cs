using Newtonsoft.Json;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Consts;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Enums;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Extensions;
using DfT.DTRO.ApiTests.ApiTests.Helpers.JsonHelpers;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.JsonHelpers.ErrorJsonResponseProcessor;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.TestConfig;

namespace DfT.DTRO.ApiTests.ApiTests.Schema_3_4_0.PublisherScenarios.DtroCreationScenarios
{
    public class InvalidPascalCase : BaseTest
    {
        readonly static string schemaVersionToTest = SchemaVersions._3_4_0;
        readonly static string schemaVersionWithInvalidPascalCase = SchemaVersions._3_3_1;

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
            TestUser publisher = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = nameOfFileWithInvalidPascalCase
                                    .GetJsonFromFile(schemaVersionWithInvalidPascalCase)
                                    .ModifyTraInDtroJson(schemaVersionWithInvalidPascalCase, (int)publisher.TraId)
                                    .ModifySchemaVersion(schemaVersionToTest);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {nameOfFileWithInvalidPascalCase}:\n\n{dtroCreationResponseJson}");

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
            TestUser publisher = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = nameOfFileWithInvalidPascalCase
                                    .GetJsonFromFile(schemaVersionWithInvalidPascalCase)
                                    .ModifyTraInDtroJson(schemaVersionWithInvalidPascalCase, (int)publisher.TraId)
                                    .ModifySchemaVersion(schemaVersionToTest);

            string dtroTempFilePath = dtroCreationJson.CreateDtroTempFile(nameOfFileWithInvalidPascalCase, publisher);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroTempFilePath.SendFileInDtroCreationRequestAsync(publisher);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");

            // Check DTRO response JSON
            Assert.True(dtroCreationResponseJson.Contains("Property 'Source' has not been defined and the schema does not allow additional properties."),
                $"Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");
            Assert.True(dtroCreationResponseJson.Contains("Required properties are missing from object: source."),
                $"Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");
        }
    }
}