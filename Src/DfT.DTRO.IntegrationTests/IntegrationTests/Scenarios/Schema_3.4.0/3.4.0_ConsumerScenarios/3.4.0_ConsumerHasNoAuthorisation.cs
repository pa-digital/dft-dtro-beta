using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Enums;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.ConsumerScenarios
{
    public class ConsumerHasNoAuthorisation : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly string fileName = "JSON-3.4.0-example-Derbyshire 2024 DJ388 partial.json";

        readonly string expectedErrorJson = """
        {
            "fault": {
                "faultstring": "Required scope(s): [cso, dsp]",
                "detail": {
                    "errorcode": "steps.oauth.v2.InsufficientScope"
                }
            }
        }
        """;

        [SkippableFact]
        public async Task DtroSubmittedByConsumerFromJsonBodyShouldBeRejected()
        {
            Skip.If(EnvironmentName == EnvironmentType.Local);

            // Get test user to send DTRO and read it back
            TestUser consumer = await TestUsers.GetUser(UserGroup.Consumer);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, consumer.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(consumer.AppId, UserGroup.Consumer);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Forbidden == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            JsonMethods.CompareJson(expectedErrorJson, dtroCreationResponseJson);
        }

        [SkippableFact]
        public async Task DtroSubmittedByConsumerFromFileShouldBeRejected()
        {
            Skip.If(EnvironmentName == EnvironmentType.Local);

            Console.WriteLine($"\nTesting with file {fileName}...");

            // Get test user to send DTRO and read it back
            TestUser consumer = await TestUsers.GetUser(UserGroup.Consumer);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, consumer.TraId);

            string dtroTempFilePath = dtroCreationJson.CreateDtroTempFile(fileName, consumer);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroTempFilePath.SendFileInDtroCreationRequestAsync(consumer.AppId, UserGroup.Consumer);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Forbidden == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            JsonMethods.CompareJson(expectedErrorJson, dtroCreationResponseJson);
        }
    }
}