using Newtonsoft.Json.Linq;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Enums;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Extensions;
using DfT.DTRO.ApiTests.ApiTests.Helpers.JsonHelpers;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.TestConfig;

namespace DfT.DTRO.ApiTests.ApiTests.Schema_3_4_0.PublisherScenarios.DtroCreationScenarios
{
    public class IncorrectPermissions : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
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
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestWithNoAuthorizationHeaderAsync();
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Unauthorized == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroCreationResponseJson.Contains("Invalid access token"),
                $"Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");
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
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            string dtroTempFilePath = dtroCreationJson.CreateDtroTempFile(fileName, publisher);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroTempFilePath.SendFileInDtroCreationRequestWithNoAuthorizationHeaderAsync("Rabbit");
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Unauthorized == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroCreationResponseJson.Contains("Invalid access token"),
                $"Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");
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
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestWithAccessTokenAsync("Rabbit");
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Unauthorized == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroCreationResponseJson.Contains("Invalid Access Token"),
                $"Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");
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
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            string dtroTempFilePath = dtroCreationJson.CreateDtroTempFile(fileName, publisher);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroTempFilePath.SendFileInDtroCreationRequestWithAccessTokenAsync("Rabbit");
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Unauthorized == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroCreationResponseJson.Contains("Invalid Access Token"),
                $"Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");
        }

        [SkippableFact]
        public async Task DtroSubmittedFromJsonBodyWithAnotherPublisherTokenShouldBeRejected()
        {
            Skip.If(EnvironmentName == EnvironmentType.Local);

            // Get test user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            TestUser secondPublisher = await TestUsers.GetUser(TestUserType.Publisher2);
            string invalidAccessToken = secondPublisher.AccessToken;

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestWithAccessTokenAsync(invalidAccessToken);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroCreationResponseJson.Contains("cannot add/update a TRO for another TRA"),
                $"Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");
        }

        [SkippableFact]
        public async Task DtroSubmittedFromFileWithAnotherPublisherTokenShouldBeRejected()
        {
            Skip.If(EnvironmentName == EnvironmentType.Local);

            // Get test user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId);

            string dtroTempFilePath = dtroCreationJson.CreateDtroTempFile(fileName, publisher);

            TestUser secondPublisher = await TestUsers.GetUser(TestUserType.Publisher2);
            string invalidAccessToken = secondPublisher.AccessToken;

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroTempFilePath.SendFileInDtroCreationRequestWithAccessTokenAsync(invalidAccessToken);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            Assert.True(dtroCreationResponseJson.Contains("cannot add/update a TRO for another TRA"),
                $"Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");
        }
    }
}