using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.DtroUpdateScenarios
{
    public class ExternalReferenceLastUpdateInFuture : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly string fileName = "JSON-3.4.0-example-Derbyshire 2024 DJ388 partial.json";

        [Fact]
        public async Task DtroUpdatedFromJsonBodyWithExternalReferenceLastUpdateDateInFutureShouldBeRejected()
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = dtroCreationJson
                                    .ModifySourceActionType(schemaVersionToTest, "amendment")
                                    .ModifyTroNameForUpdate(schemaVersionToTest)
                                    .SetExternalReferenceLastUpdatedDateInFuture();

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await dtroUpdateJson.SendJsonInDtroUpdateRequestAsync(dtroId, publisher.AppId);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroUpdateResponse.StatusCode,
                            $"Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");

            // Evaluate response JSON
            string expectedErrorJson = Dtros.GetExternalReferenceLastUpdateDateErrorJson(fileName);
            JsonMethods.CompareJson(expectedErrorJson, dtroUpdateResponseJson);
        }

        [Fact]
        public async Task DtroUpdatedFromFileWithExternalReferenceLastUpdateDateInFutureShouldBeRejected()
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string dtroCreationJson = fileName
                        .GetJsonFromFile(schemaVersionToTest)
                        .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            string tempFilePathForDtroCreation = dtroCreationJson.CreateDtroTempFile(fileName, publisher);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await tempFilePathForDtroCreation.SendFileInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Response JSON for file {Path.GetFileName(tempFilePathForDtroCreation)}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = dtroCreationJson
                                        .ModifySourceActionType(schemaVersionToTest, "amendment")
                                        .ModifyTroNameForUpdate(schemaVersionToTest)
                                        .SetExternalReferenceLastUpdatedDateInFuture();

            string tempFilePathForDtroUpdate = dtroUpdateJson.CreateDtroTempFileForUpdate(fileName, publisher);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await tempFilePathForDtroUpdate.SendFileInDtroUpdateRequestAsync(dtroId, publisher.AppId);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroUpdateResponse.StatusCode,
                $"Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroUpdateResponseJson}");

            // Evaluate response JSON
            string expectedErrorJson = Dtros.GetExternalReferenceLastUpdateDateErrorJson(fileName);
            JsonMethods.CompareJson(expectedErrorJson, dtroUpdateResponseJson);
        }
    }
}