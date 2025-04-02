using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.DtroCreationScenarios
{
    public class MultipleSchemaValidationErrors : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly string fileName = "JSON-3.4.0-example-Derbyshire 2024 DJ388 partial.json";

        [Fact]
        public async Task DtroSubmittedFromJsonBodyWithWithMultipleSchemaErrorsShouldBeRejected()
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId)
                                    .ModifyToFailSchemaValidation();

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroCreationResponseJson.Contains("Invalid type. Expected Integer but got String.\",\"path\":\"source.currentTraOwner"),
                $"Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");
            Assert.True(dtroCreationResponseJson.Contains("Invalid type. Expected Array but got Integer.\",\"path\":\"source.traAffected"),
                $"Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");
            Assert.True(dtroCreationResponseJson.Contains("Required properties are missing from object: actionType.\",\"path\":\"source"),
                $"Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");
            Assert.True(dtroCreationResponseJson.Contains("Property 'apples' has not been defined and the schema does not allow additional properties."),
                $"Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");
        }

        [Fact]
        public async Task DtroSubmittedFromFileWithMultipleSchemaErrorsShouldBeRejected()
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId)
                                    .ModifyToFailSchemaValidation();

            string dtroTempFilePath = dtroCreationJson.CreateDtroTempFile(fileName, publisher);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroTempFilePath.SendFileInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroCreationResponseJson.Contains("Invalid type. Expected Integer but got String.\",\"path\":\"source.currentTraOwner"),
                $"Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");
            Assert.True(dtroCreationResponseJson.Contains("Invalid type. Expected Array but got Integer.\",\"path\":\"source.traAffected"),
                $"Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");
            Assert.True(dtroCreationResponseJson.Contains("Required properties are missing from object: actionType.\",\"path\":\"source"),
                $"Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");
            Assert.True(dtroCreationResponseJson.Contains("Property 'apples' has not been defined and the schema does not allow additional properties."),
                $"Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");

        }
    }
}