using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.CreateDtroTests
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
        public async Task DtroSubmittedFromFileWithExternalReferenceLastUpdateDateInFutureShouldBeRejected(string fileName)
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string tempFilePath = Dtros.CreateTempFileWithTraModifiedAndExternalReferenceLastUpdateDateInFuture(schemaVersionToTest, fileName, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePath)}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Evaluate response JSON rule failures
            dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(createDtroResponseJson)!;

            string expectedName = "Invalid last update date";
            string actualName = jsonDeserialised.ruleError_0.name.ToString();
            Assert.True(expectedName == actualName, $"File {Path.GetFileName(tempFilePath)}: expected is '{expectedName}' but actual was '{actualName}', with response body\n{createDtroResponseJson}");

            string expectedMessage = "Indicates the date the USRN reference was last updated";
            string actualMessage = jsonDeserialised.ruleError_0.message.ToString();
            Assert.True(expectedMessage == actualMessage, $"File {Path.GetFileName(tempFilePath)}: expected is '{expectedMessage}' but actual was '{actualMessage}', with response body\n{createDtroResponseJson}");

            string actualPath = jsonDeserialised.ruleError_0.path.ToString();
            Assert.True(actualPath.StartsWith("Source -> Provision -> RegulatedPlace -> ") && actualPath.EndsWith(" -> ExternalReference -> lastUpdateDate"), $"File {Path.GetFileName(tempFilePath)}: actual was '{actualPath}', with response body\n{createDtroResponseJson}");

            string expectedRule = "'lastUpdateDate' cannot be in the future";
            string actualRule = jsonDeserialised.ruleError_0.rule.ToString();
            Assert.True(expectedRule == actualRule, $"File {Path.GetFileName(tempFilePath)}: expected is '{expectedRule}' but actual was '{actualRule}', with response body\n{createDtroResponseJson}");
        }

        [Theory]
        [MemberData(nameof(GetDtroFileNames))]
        public async Task DtroSubmittedFromJsonBodyWithExternalReferenceLastUpdateDateInFutureShouldBeRejected(string fileName)
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string createDtroJsonWithTraModifiedAndExternalReferenceLastUpdatedDateInFuture = Dtros.GetJsonFromFileAndModifyTraAndSetExternalReferenceLastUpdateDateToFuture(schemaVersionToTest, fileName, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraModifiedAndExternalReferenceLastUpdatedDateInFuture, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {fileName}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Evaluate response JSON rule failures
            dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(createDtroResponseJson)!;

            string expectedName = "Invalid last update date";
            string actualName = jsonDeserialised.ruleError_0.name.ToString();
            Assert.True(expectedName == actualName, $"File {fileName}: expected is '{expectedName}' but actual was '{actualName}', with response body\n{createDtroResponseJson}");

            string expectedMessage = "Indicates the date the USRN reference was last updated";
            string actualMessage = jsonDeserialised.ruleError_0.message.ToString();
            Assert.True(expectedMessage == actualMessage, $"File {fileName}: expected is '{expectedMessage}' but actual was '{actualMessage}', with response body\n{createDtroResponseJson}");

            string actualPath = jsonDeserialised.ruleError_0.path.ToString();
            Assert.True(actualPath.StartsWith("Source -> Provision -> RegulatedPlace -> ") && actualPath.EndsWith(" -> ExternalReference -> lastUpdateDate"), $"File {fileName}: actual was '{actualPath}', with response body\n{createDtroResponseJson}");

            string expectedRule = "'lastUpdateDate' cannot be in the future";
            string actualRule = jsonDeserialised.ruleError_0.rule.ToString();
            Assert.True(expectedRule == actualRule, $"File {fileName}: expected is '{expectedRule}' but actual was '{actualRule}', with response body\n{createDtroResponseJson}");
        }
    }
}