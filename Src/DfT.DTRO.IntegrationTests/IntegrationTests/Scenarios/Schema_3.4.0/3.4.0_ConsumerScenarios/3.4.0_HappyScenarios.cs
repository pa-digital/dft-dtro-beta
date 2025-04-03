using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Enums;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.ConsumerScenarios.DtroCreationScenarios
{
    public class HappyScenarios : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly string fileName = "JSON-3.4.0-example-Derbyshire 2024 DJ388 partial.json";

        [Fact]
        public async Task DtroSubmittedFromJsonBodyShouldBeSavedCorrectly()
        {
            // Get test user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Search for DTRO as consumer
            TestUser consumer = await TestUsers.GetUser(UserGroup.Consumer);

            string searchRequestJson = Dtros.GetSearchRequestJson("traCreator", publisher.TraId);
            HttpResponseMessage dtroSearchResponse = await searchRequestJson.GetDtroSearchResponseAsync(consumer);
            string dtroSearchResponseJson = await dtroSearchResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroSearchResponse.StatusCode,
                $"Actual status code: {dtroSearchResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroSearchResponseJson}");





            // // Get created DTRO
            // string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            // HttpResponseMessage dtroGetResponse = await dtroId.GetDtroResponseByIdAsync(publisher);
            // string dtroGetResponseJson = await dtroGetResponse.Content.ReadAsStringAsync();
            // Assert.True(HttpStatusCode.OK == dtroGetResponse.StatusCode,
            //     $"Actual status code: {dtroGetResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroGetResponseJson}");

            // // Add ID to sent DTRO and compare
            // string expectedCreationJsonForComparison = dtroCreationJson
            //                                             .AddDtroIdToJson(dtroId);
            // JsonMethods.CompareJson(expectedCreationJsonForComparison, dtroGetResponseJson);
        }

        // [Theory]
        // [MemberData(nameof(GetNamesOfDtroExampleFiles))]
        // public async Task DtroSubmittedFromFileShouldBeSavedCorrectly(string fileName)
        // {
        //     Console.WriteLine($"\nTesting with file {fileName}...");

        //     // Get test user to send DTRO and read it back
        //     TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

        //     // Prepare DTRO
        //     string dtroCreationJson = fileName
        //                             .GetJsonFromFile(schemaVersionToTest)
        //                             .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

        //     string dtroTempFilePath = dtroCreationJson.CreateDtroTempFile(fileName, publisher);

        //     // Send DTRO
        //     HttpResponseMessage dtroCreationResponse = await dtroTempFilePath.SendFileInDtroCreationRequestAsync(publisher.AppId);
        //     string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
        //     Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
        //         $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");

        //     // Get created DTRO
        //     string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
        //     HttpResponseMessage dtroGetResponse = await dtroId.GetDtroResponseByIdAsync(publisher);
        //     string dtroGetResponseJson = await dtroGetResponse.Content.ReadAsStringAsync();
        //     Assert.True(HttpStatusCode.OK == dtroGetResponse.StatusCode,
        //         $"Actual status code: {dtroGetResponse.StatusCode}. Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroGetResponseJson}");

        //     // Add ID to sent DTRO and compare
        //     string expectedCreationJsonForComparison = dtroCreationJson
        //                                                 .AddDtroIdToJson(dtroId);
        //     JsonMethods.CompareJson(expectedCreationJsonForComparison, dtroGetResponseJson);
        // }
    }
}