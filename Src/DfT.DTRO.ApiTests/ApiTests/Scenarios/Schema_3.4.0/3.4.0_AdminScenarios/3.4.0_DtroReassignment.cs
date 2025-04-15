using Newtonsoft.Json.Linq;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Consts;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Enums;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Extensions;
using DfT.DTRO.ApiTests.ApiTests.Helpers.JsonHelpers;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.TestConfig;

namespace DfT.DTRO.ApiTests.ApiTests.Schema_3_4_0.AdminScenarios
{
    public class DtroReassignment : BaseTest
    {
        readonly static string schemaVersionToTest = SchemaVersions._3_4_0;
        readonly string fileName = "dtro-v3.4.0-example-more-complex-example.json";

        [Fact(Skip = "See bug DPPB-1297 - current design passes in TRA GUID rather than integer")]
        public async Task DtroCanBeReassignedFromOnePublisherToAnotherPublisher()
        {
            // Get test user to send DTRO and read it back
            TestUser originalOwner = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, (int)originalOwner.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(originalOwner);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Prepare reassignment
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            TestUser newOwner = await TestUsers.GetUser(TestUserType.Publisher2);
            TestUser admin = await TestUsers.GetUser(TestUserType.Admin);

            // Reassign DTRO
            HttpResponseMessage dtroReassignmentResponse = await dtroId.ReassignDtroByIdAsync(newOwner, admin);
            string dtroReassignmentResponseJson = await dtroReassignmentResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroReassignmentResponse.StatusCode,
                $"Actual status code: {dtroReassignmentResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroReassignmentResponseJson}");

            // Get DTRO after reassignment 
            HttpResponseMessage dtroGetResponse = await dtroId.GetDtroResponseByIdAsync(newOwner);
            string dtroGetResponseJson = await dtroGetResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroGetResponse.StatusCode,
                $"Actual status code: {dtroGetResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroGetResponseJson}");

            // Add ID to sent DTRO and check that currentTraOwner is updated
            string expectedCreationJsonForComparison = dtroCreationJson
                                                        .SetValueAtJsonPath("data.source.currentTraOwner", newOwner.TraId)
                                                        .AddDtroIdToJson(dtroId);
            JsonMethods.CompareJson(expectedCreationJsonForComparison, dtroGetResponseJson);

            // Original owner tries to update DTRO
            string failedDtroUpdateJson = dtroCreationJson
                                    .ModifySourceActionType(schemaVersionToTest, "amendment")
                                    .ModifyTroNameForUpdate(schemaVersionToTest);

            HttpResponseMessage failedDtroUpdateResponse = await failedDtroUpdateJson.SendJsonInDtroUpdateRequestAsync(dtroId, originalOwner);
            string failedDtroUpdateResponseJson = await failedDtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == failedDtroUpdateResponse.StatusCode,
                $"Actual status code: {failedDtroUpdateResponse.StatusCode}. Response JSON for file {fileName}:\n\n{failedDtroUpdateResponseJson}");

            // Evaluate response JSON
            Assert.True(failedDtroUpdateResponseJson.Contains("cannot add/update a TRO for another TRA"),
                $"Response JSON for file {fileName}:\n\n{failedDtroUpdateResponseJson}");

            // New owner successfully updates DTRO
            string successfuldtroUpdateJson = failedDtroUpdateJson
                                    .SetValueAtJsonPath("data.source.currentTraOwner", newOwner.TraId);

            HttpResponseMessage successfulDtroUpdateResponse = await successfuldtroUpdateJson.SendJsonInDtroUpdateRequestAsync(dtroId, originalOwner);
            string successfulDtroUpdateResponseJson = await successfulDtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == successfulDtroUpdateResponse.StatusCode,
                $"Actual status code: {successfulDtroUpdateResponse.StatusCode}. Response JSON for file {fileName}:\n\n{successfulDtroUpdateResponseJson}");

            // Get DTRO after update 
            HttpResponseMessage dtroGetResponseAfterUpdate = await dtroId.GetDtroResponseByIdAsync(newOwner);
            string dtroGetResponseJsonAfterUpdate = await dtroGetResponseAfterUpdate.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroGetResponseAfterUpdate.StatusCode,
                $"Actual status code: {dtroGetResponseAfterUpdate.StatusCode}. Response JSON for file {fileName}:\n\n{dtroGetResponseJsonAfterUpdate}");

            // Add ID to updated DTRO and compare
            string expectedUpdateJsonForComparison = successfuldtroUpdateJson
                                                        .AddDtroIdToJson(dtroId);
            JsonMethods.CompareJson(expectedUpdateJsonForComparison, dtroGetResponseJson);
        }
    }
}