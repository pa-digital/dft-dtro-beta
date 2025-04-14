using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Consts;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Enums;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Extensions;
using DfT.DTRO.ApiTests.ApiTests.Helpers.JsonHelpers;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.TestConfig;

namespace DfT.DTRO.ApiTests.ApiTests.Schema_3_4_0.PublisherScenarios.DtroUpdateScenarios
{
    public class DuplicateItems : BaseTest
    {
        readonly static string schemaVersionToTest = SchemaVersions._3_4_0;
        readonly string fileName = "dtro-v3.4.0-example-derbyshire-2024-dj388-partial.json";

        [Fact]
        public async Task DtroUpdatedFromJsonBodyWithDuplicateProvisionReferenceShouldBeRejected()
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, (int)publisher.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = dtroCreationJson
                                    .ModifySourceActionType(schemaVersionToTest, "amendment")
                                    .ModifyTroNameForUpdate(schemaVersionToTest)
                                    .DuplicateProvisionReferenceInDtro();

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await dtroUpdateJson.SendJsonInDtroUpdateRequestAsync(dtroId, publisher);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroUpdateResponse.StatusCode,
                            $"Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");

            // Evaluate response JSON
            string provisionReference = JsonMethods.GetValueAtJsonPath(dtroUpdateJson, "data.source.provision[0].reference").ToString();
            string expectedErrorJson = Dtros.GetDuplicateProvisionReferenceErrorJson(provisionReference);
            JsonMethods.CompareJson(expectedErrorJson, dtroUpdateResponseJson);
        }

        [Fact]
        public async Task DtroUpdatedFromFileWithDuplicateProvisionReferenceShouldBeRejected()
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = fileName
                        .GetJsonFromFile(schemaVersionToTest)
                        .ModifyTraInDtroJson(schemaVersionToTest, (int)publisher.TraId);

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
                                        .DuplicateProvisionReferenceInDtro();

            string tempFilePathForDtroUpdate = dtroUpdateJson.CreateDtroTempFileForUpdate(fileName, publisher);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await tempFilePathForDtroUpdate.SendFileInDtroUpdateRequestAsync(dtroId, publisher);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroUpdateResponseJson}");

            // Evaluate response JSON
            string provisionReference = JsonMethods.GetValueAtJsonPath(dtroUpdateJson, "data.source.provision[0].reference").ToString();
            string expectedErrorJson = Dtros.GetDuplicateProvisionReferenceErrorJson(provisionReference);
            JsonMethods.CompareJson(expectedErrorJson, dtroUpdateResponseJson);
        }
    }
}