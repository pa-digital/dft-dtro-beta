using Newtonsoft.Json.Linq;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Consts;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Enums;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Extensions;
using DfT.DTRO.ApiTests.ApiTests.Helpers.JsonHelpers;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.TestConfig;

namespace DfT.DTRO.ApiTests.ApiTests.Schema_3_4_0.PublisherScenarios.DtroUpdateScenarios
{
    public class IncorrectPermissions : BaseTest
    {
        readonly static string schemaVersionToTest = SchemaVersions._3_4_0;
        readonly string fileName = "dtro-v3.4.0-example-derbyshire-2024-dj388-partial.json";

        [SkippableFact]
        public async Task DtroSubmittedFromJsonBodyWithNoAuthorizationHeaderShouldBeRejected()
        {
            Skip.If(EnvironmentName == EnvironmentType.Local);

            // Get test user to send DTRO and read it back
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
                                        .AppendTextToTroName(schemaVersionToTest, "UPDATED");

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await dtroUpdateJson.SendJsonInDtroUpdateRequestWithNoAuthorizationHeaderAsync(dtroId);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Unauthorized == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroUpdateResponseJson.Contains("Invalid access token"),
                $"Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");
        }

        [SkippableFact]
        public async Task DtroSubmittedFromFileWithNoAuthorizationHeaderShouldBeRejected()
        {
            Skip.If(EnvironmentName == EnvironmentType.Local);

            // Get test user to send DTRO and read it back
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
                                        .AppendTextToTroName(schemaVersionToTest, "UPDATED");

            string tempFilePathForDtroUpdate = dtroUpdateJson.CreateDtroTempFileForUpdate(fileName, publisher);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await tempFilePathForDtroUpdate.SendFileInDtroUpdateRequestWithNoAuthorizationHeaderAsync(dtroId);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Unauthorized == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroUpdateResponseJson.Contains("Invalid access token"),
                $"Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");
        }

        [SkippableFact]
        public async Task DtroSubmittedFromJsonBodyWithMadeUpTokenShouldBeRejected()
        {
            Skip.If(EnvironmentName == EnvironmentType.Local);

            // Get test user to send DTRO and read it back
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
                                        .AppendTextToTroName(schemaVersionToTest, "UPDATED");

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await dtroUpdateJson.SendJsonInDtroUpdateRequestWithAccessTokenAsync(dtroId, "Rabbit");
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Unauthorized == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroUpdateResponseJson.Contains("Invalid Access Token"),
                $"Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");
        }

        [SkippableFact]
        public async Task DtroSubmittedFromFileWithMadeUpTokenShouldBeRejected()
        {
            Skip.If(EnvironmentName == EnvironmentType.Local);

            // Get test user to send DTRO and read it back
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
                                        .AppendTextToTroName(schemaVersionToTest, "UPDATED");

            string tempFilePathForDtroUpdate = dtroUpdateJson.CreateDtroTempFileForUpdate(fileName, publisher);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await tempFilePathForDtroUpdate.SendFileInDtroUpdateRequestWithAccessTokenAsync(dtroId, "Rabbit");
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Unauthorized == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroUpdateResponseJson.Contains("Invalid Access Token"),
                $"Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");
        }

        [SkippableFact]
        public async Task DtroSubmittedFromJsonBodyWithAnotherPublisherTokenShouldBeRejected()
        {
            Skip.If(EnvironmentName == EnvironmentType.Local);

            // Get test user to send DTRO and read it back
            TestUser publisherWhoOwnsDtro = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, (int)publisherWhoOwnsDtro.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisherWhoOwnsDtro);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            TestUser publisherWhoDoesNotOwnDtro = await TestUsers.GetUser(TestUserType.Publisher2);

            string dtroUpdateJson = dtroCreationJson
                                    .ModifySourceActionType(schemaVersionToTest, "amendment")
                                    .AppendTextToTroName(schemaVersionToTest, "UPDATED")
                                    .SetValueAtJsonPath("data.source.currentTraOwner", publisherWhoOwnsDtro.TraId)
                                    .SetValueAtJsonPath("data.source.traAffected", new[] { publisherWhoDoesNotOwnDtro.TraId })
                                    .SetValueAtJsonPath("data.source.traCreator", publisherWhoDoesNotOwnDtro.TraId);

            // Send DTRO
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await dtroUpdateJson.SendJsonInDtroUpdateRequestAsync(dtroId, publisherWhoDoesNotOwnDtro);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroUpdateResponseJson.Contains("cannot add/update a TRO for another TRA"),
                $"Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");
        }

        [SkippableFact]
        public async Task DtroSubmittedFromFileWithAnotherPublisherTokenShouldBeRejected()
        {
            Skip.If(EnvironmentName == EnvironmentType.Local);

            // Get test user to send DTRO and read it back
            TestUser publisherWhoOwnsDtro = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, (int)publisherWhoOwnsDtro.TraId);

            string tempFilePathForDtroCreation = dtroCreationJson.CreateDtroTempFile(fileName, publisherWhoOwnsDtro);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await tempFilePathForDtroCreation.SendFileInDtroCreationRequestAsync(publisherWhoOwnsDtro);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroCreation)}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            TestUser publisherWhoDoesNotOwnDtro = await TestUsers.GetUser(TestUserType.Publisher2);

            string dtroUpdateJson = dtroCreationJson
                                    .ModifySourceActionType(schemaVersionToTest, "amendment")
                                    .AppendTextToTroName(schemaVersionToTest, "UPDATED")
                                    .SetValueAtJsonPath("data.source.currentTraOwner", publisherWhoOwnsDtro.TraId)
                                    .SetValueAtJsonPath("data.source.traAffected", new[] { publisherWhoDoesNotOwnDtro.TraId })
                                    .SetValueAtJsonPath("data.source.traCreator", publisherWhoDoesNotOwnDtro.TraId);

            string tempFilePathForDtroUpdate = dtroUpdateJson.CreateDtroTempFile(fileName, publisherWhoOwnsDtro);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await tempFilePathForDtroUpdate.SendFileInDtroUpdateRequestAsync(dtroId, publisherWhoDoesNotOwnDtro);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroUpdateResponseJson.Contains("cannot add/update a TRO for another TRA"),
                $"Response JSON for file {fileName}:\n\n{dtroUpdateResponseJson}");
        }
    }
}