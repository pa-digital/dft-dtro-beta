using Newtonsoft.Json.Linq;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Consts;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Extensions;
using DfT.DTRO.ApiTests.ApiTests.Helpers.JsonHelpers;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Enums;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.TestConfig;

namespace DfT.DTRO.ApiTests.ApiTests.Schema_3_4_0.PublisherScenarios.DtroDeletionScenarios
{
    public class HappyScenarios : BaseTest
    {
        readonly static string schemaVersionToTest = SchemaVersions._3_4_0;
        readonly string fileName = "dtro-v3.4.0-example-derbyshire-2024-dj388-partial.json";

        [Fact]
        public async Task DtroCreatedFromJsonBodyShouldBeDeletedCorrectly()
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

            // Send DTRO deletion
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroDeletionResponse = await dtroId.DeleteDtroResponseByIdAsync(publisher);
            string dtroDeletionResponseJson = await dtroDeletionResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.NoContent == dtroDeletionResponse.StatusCode,
                $"Actual status code: {dtroDeletionResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroDeletionResponseJson}");

            // Try to get deleted DTRO
            HttpResponseMessage dtroGetResponse = await dtroId.GetDtroResponseByIdAsync(publisher);
            string dtroGetResponseJson = await dtroGetResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.NotFound == dtroGetResponse.StatusCode,
                $"Actual status code: {dtroGetResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroGetResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroGetResponseJson.Contains($"'{dtroId}' has either been deleted or cannot be found"),
                $"Response JSON for file {fileName}:\n\n{dtroGetResponseJson}");
        }

        [Fact]
        public async Task DtroCreatedFromFileShouldBeDeletedCorrectly()
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

            // Send DTRO deletion
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroDeletionResponse = await dtroId.DeleteDtroResponseByIdAsync(publisher);
            string dtroDeletionResponseJson = await dtroDeletionResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.NoContent == dtroDeletionResponse.StatusCode,
                $"Actual status code: {dtroDeletionResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroDeletionResponseJson}");

            // Try to get deleted DTRO
            HttpResponseMessage dtroGetResponse = await dtroId.GetDtroResponseByIdAsync(publisher);
            string dtroGetResponseJson = await dtroGetResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.NotFound == dtroGetResponse.StatusCode,
                $"Actual status code: {dtroGetResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroGetResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroGetResponseJson.Contains($"'{dtroId}' has either been deleted or cannot be found"),
                $"Response JSON for file {fileName}:\n\n{dtroGetResponseJson}");
        }
    }
}