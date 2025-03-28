using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.CreateDtroScenarios
{
    public class ExternalReferenceLastUpdateInFuture : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";

        public static IEnumerable<object[]> GetDtroFileNames()
        {
            return new List<object[]>
            {
                new object[] { "JSON-3.4.0-example-TTRO-HeightRestrictionwithConditions.json" }, // point geometry
                new object[] { "JSON-3.4.0-example-Derbyshire 2024 DJ388 partial.json" } // linear geometry
            };
        }

        [Theory]
        [MemberData(nameof(GetDtroFileNames))]
        public async Task DtroSubmittedFromJsonBodyWithExternalReferenceLastUpdateDateInFutureShouldBeRejected(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);
            await publisher.CreateUserForDataSetUpAsync();

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId)
                                    .SetExternalReferenceLastUpdatedDateInFuture();

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode,
                $"Response JSON for file {fileName}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            string expectedErrorJson = Dtros.GetExternalReferenceLastUpdateDateErrorJson(fileName);
            JsonMethods.CompareJson(expectedErrorJson, dtroCreationResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetDtroFileNames))]
        public async Task DtroSubmittedFromFileWithExternalReferenceLastUpdateDateInFutureShouldBeRejected(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(UserGroup.Tra);
            await publisher.CreateUserForDataSetUpAsync();

            // Prepare DTRO
            string dtroCreationJson = fileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId)
                                    .SetExternalReferenceLastUpdatedDateInFuture();

            string dtroTempFilePath = dtroCreationJson.CreateDtroTempFile(fileName, publisher.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroTempFilePath.SendFileInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode,
                $"Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            string expectedErrorJson = Dtros.GetExternalReferenceLastUpdateDateErrorJson(fileName);
            JsonMethods.CompareJson(expectedErrorJson, dtroCreationResponseJson);
        }
    }
}