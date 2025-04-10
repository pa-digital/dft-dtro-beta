using Newtonsoft.Json.Linq;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Enums;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Extensions;
using DfT.DTRO.ApiTests.ApiTests.Helpers.JsonHelpers;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.TestConfig;

namespace DfT.DTRO.ApiTests.ApiTests.Schema_3_4_0.ConsumerScenarios
{
    public class ConsumerCannotCreateDtro : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly string fileName = "dtro-v3.4.0-example-derbyshire-2024-dj388-partial.json";

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
            TestUser consumer = await TestUsers.GetUser(TestUserType.Consumer);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(consumer);
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
            TestUser consumer = await TestUsers.GetUser(TestUserType.Consumer);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest);

            string dtroTempFilePath = dtroCreationJson.CreateDtroTempFile(fileName, consumer);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroTempFilePath.SendFileInDtroCreationRequestAsync(consumer);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Forbidden == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            JsonMethods.CompareJson(expectedErrorJson, dtroCreationResponseJson);
        }
    }
}