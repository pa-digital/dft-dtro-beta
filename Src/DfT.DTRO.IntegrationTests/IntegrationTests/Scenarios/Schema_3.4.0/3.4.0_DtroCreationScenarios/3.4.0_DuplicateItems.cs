using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.DtroCreationScenarios
{
    public class DuplicateItems : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly string fileName = "JSON-3.4.0-example-Derbyshire 2024 DJ388 partial.json";

        [Fact]
        public async Task DtroSubmittedFromJsonBodyWithDuplicateProvisionReferenceShouldBeRejected()
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId)
                                    .DuplicateProvisionReferenceInDtro();

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            string provisionReference = JsonMethods.GetValueAtJsonPath(dtroCreationJson, "data.source.provision[0].reference").ToString();
            string expectedErrorJson = Dtros.GetDuplicateProvisionReferenceErrorJson(provisionReference);
            JsonMethods.CompareJson(expectedErrorJson, dtroCreationResponseJson);
        }

        [Fact]
        public async Task DtroSubmittedFromFileWithDuplicateProvisionReferenceShouldBeRejected()
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId)
                                    .DuplicateProvisionReferenceInDtro();

            string dtroTempFilePath = dtroCreationJson.CreateDtroTempFile(fileName, publisher);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroTempFilePath.SendFileInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            string provisionReference = JsonMethods.GetValueAtJsonPath(dtroCreationJson, "data.source.provision[0].reference").ToString();
            string expectedErrorJson = Dtros.GetDuplicateProvisionReferenceErrorJson(provisionReference);
            JsonMethods.CompareJson(expectedErrorJson, dtroCreationResponseJson);
        }
    }
}