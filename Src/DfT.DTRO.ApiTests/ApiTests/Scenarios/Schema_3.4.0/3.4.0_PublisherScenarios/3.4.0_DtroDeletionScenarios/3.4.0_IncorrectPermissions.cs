using Newtonsoft.Json.Linq;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Consts;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Extensions;
using DfT.DTRO.ApiTests.ApiTests.Helpers.JsonHelpers;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Enums;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.TestConfig;

namespace DfT.DTRO.ApiTests.ApiTests.Schema_3_4_0.PublisherScenarios.DtroDeletionScenarios
{
    public class IncorrectPermissions : BaseTest
    {
        readonly static string schemaVersionToTest = SchemaVersions._3_4_0;
        readonly string fileName = "dtro-v3.4.0-example-derbyshire-2024-dj388-partial.json";

        [Fact(Skip = "See bug DPPB-1286")]
        public async Task DtroDeletedWithAnotherPublisherTokenShouldBeRejected()
        {
            Skip.If(EnvironmentName == EnvironmentType.Local);

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

            // Send DTRO deletion - it's deleted when request should be rejected
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            TestUser publisherWhoDoesNotOwnDtro = await TestUsers.GetUser(TestUserType.Publisher2);
            HttpResponseMessage dtroDeletionResponse = await dtroId.DeleteDtroResponseByIdAsync(publisherWhoDoesNotOwnDtro);
            string dtroDeletionResponseJson = await dtroDeletionResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.NoContent == dtroDeletionResponse.StatusCode,
                $"Actual status code: {dtroDeletionResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroDeletionResponseJson}");

            // Try to get deleted DTRO - it's definitely deleted when it should still exist
            HttpResponseMessage dtroGetResponse = await dtroId.GetDtroResponseByIdAsync(publisher);
            string dtroGetResponseJson = await dtroGetResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.NotFound == dtroGetResponse.StatusCode,
                $"Actual status code: {dtroGetResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroGetResponseJson}");

        }
    }
}