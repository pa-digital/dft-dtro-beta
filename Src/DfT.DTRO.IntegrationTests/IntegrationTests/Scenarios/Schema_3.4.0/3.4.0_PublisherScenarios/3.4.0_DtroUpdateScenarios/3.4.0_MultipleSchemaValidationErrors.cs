using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Enums;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.PublisherScenarios.DtroUpdateScenarios
{
    // DPPB-1235
    public class MultipleSchemaValidationErrors : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly string fileName = "dtro-v3.4.0-example-derbyshire-2024-dj388-partial.json";

        [Fact]
        public async Task DtroUpdatedFromJsonBodyWithWithMultipleSchemaErrorsShouldBeRejected()
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = dtroCreationJson
                                        .ModifySourceActionType(schemaVersionToTest, "amendment")
                                        .ModifyTroNameForUpdate(schemaVersionToTest)
                                        .ModifyToFailSchemaValidation();

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await dtroUpdateJson.SendJsonInDtroUpdateRequestAsync(dtroId, publisher);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroUpdateResponseJson.Contains("Invalid type. Expected Integer but got String.\",\"path\":\"source.currentTraOwner"),
                $"Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");
            Assert.True(dtroUpdateResponseJson.Contains("Invalid type. Expected Array but got Integer.\",\"path\":\"source.traAffected"),
                $"Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");
            Assert.True(dtroUpdateResponseJson.Contains("Required properties are missing from object: actionType.\",\"path\":\"source"),
                $"Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");
            Assert.True(dtroUpdateResponseJson.Contains("Property 'apples' has not been defined and the schema does not allow additional properties."),
                $"Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");
        }

        [Fact]
        public async Task DtroUpdatedFromFileWithMultipleSchemaErrorsShouldBeRejected()
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            string tempFilePathForDtroCreation = dtroCreationJson.CreateDtroTempFile(fileName, publisher);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await tempFilePathForDtroCreation.SendFileInDtroCreationRequestAsync(publisher);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroCreation)}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = dtroCreationJson
                                        .ModifySourceActionType(schemaVersionToTest, "amendment")
                                        .ModifyTroNameForUpdate(schemaVersionToTest)
                                        .ModifyToFailSchemaValidation();

            string tempFilePathForDtroUpdate = dtroUpdateJson.CreateDtroTempFileForUpdate(fileName, publisher);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await tempFilePathForDtroUpdate.SendFileInDtroUpdateRequestAsync(dtroId, publisher);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroUpdateResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroUpdateResponseJson.Contains("Invalid type. Expected Integer but got String.\",\"path\":\"source.currentTraOwner"),
                $"Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroUpdateResponseJson}");
            Assert.True(dtroUpdateResponseJson.Contains("Invalid type. Expected Array but got Integer.\",\"path\":\"source.traAffected"),
                $"Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroUpdateResponseJson}");
            Assert.True(dtroUpdateResponseJson.Contains("Required properties are missing from object: actionType.\",\"path\":\"source"),
                $"Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroUpdateResponseJson}");
            Assert.True(dtroUpdateResponseJson.Contains("Property 'apples' has not been defined and the schema does not allow additional properties."),
                $"Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroUpdateResponseJson}");
        }
    }
}