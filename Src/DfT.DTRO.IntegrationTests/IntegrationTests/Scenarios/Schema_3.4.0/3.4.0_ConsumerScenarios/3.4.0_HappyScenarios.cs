using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Enums;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.ConsumerScenarios.DtroCreationScenarios
{
    public class HappyScenarios : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly string fileName = "JSON-3.4.0-example-TTRO-MoreComplexExample.json";

        [Fact]
        public async Task DtroSubmittedFromJsonBodyAppearsInSearchResponse()
        {
            // Get test user to send DTRO and read it back
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

            // Search for DTRO as consumer
            TestUser consumer = await TestUsers.GetUser(TestUserType.Consumer);

            string searchRequestJson = Dtros.GetSearchRequestJson("traCreator", publisher.TraId);
            HttpResponseMessage dtroSearchResponse = await searchRequestJson.GetDtroSearchResponseAsync(consumer);
            string dtroSearchResponseJson = await dtroSearchResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroSearchResponse.StatusCode,
                $"Actual status code: {dtroSearchResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroSearchResponseJson}");

            // Compare response JSON once DPPB-1263 is done
        }

        [Fact]
        public async Task DtroSubmittedFromFileAppearsInSearchResponse()
        {
            // Get test user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            string dtroTempFilePath = dtroCreationJson.CreateDtroTempFile(fileName, publisher);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroTempFilePath.SendFileInDtroCreationRequestAsync(publisher);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");

            // Search for DTRO as consumer
            TestUser consumer = await TestUsers.GetUser(TestUserType.Consumer);

            string searchRequestJson = Dtros.GetSearchRequestJson("traCreator", publisher.TraId);
            HttpResponseMessage dtroSearchResponse = await searchRequestJson.GetDtroSearchResponseAsync(consumer);
            string dtroSearchResponseJson = await dtroSearchResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroSearchResponse.StatusCode,
                $"Actual status code: {dtroSearchResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroSearchResponseJson}");

            // Compare response JSON once DPPB-1263 is done
        }
    }
}