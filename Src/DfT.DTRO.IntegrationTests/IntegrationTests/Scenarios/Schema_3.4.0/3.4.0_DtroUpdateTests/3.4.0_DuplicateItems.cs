using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.UpdateDtroTests
{
    public class DuplicateItems : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly string fileName = "JSON-3.4.0-example-Derbyshire 2024 DJ388 partial.json";

        [Fact]
        public async Task DtroUpdatedFromFileWithDuplicateProvisionReferenceShouldBeRejected()
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string tempFilePathForDtroCreation = Dtros.CreateTempFileWithTraModified(schemaVersionToTest, fileName, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePathForDtroCreation, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == createDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePathForDtroCreation)}: expected status code is {HttpStatusCode.Created} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Prepare DTRO update
            string tempFilePathForDtroUpdate = Dtros.CreateTempUpdateFileWithTraModifiedAndProvisionReferenceDuplicated(schemaVersionToTest, fileName, publisher.TraId);

            // Send DTRO update
            string dtroId = await Dtros.GetIdFromResponseJsonAsync(createDtroResponse);
            HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromFileAsync(tempFilePathForDtroUpdate, dtroId, publisher);
            string updateDtroResponseJson = await updateDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == updateDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePathForDtroUpdate)}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {updateDtroResponse.StatusCode}, with response body\n{updateDtroResponseJson}");

            // Evaluate response JSON rule failures
            dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(updateDtroResponseJson)!;

            string expectedName = "'2' duplication reference";
            string actualName = jsonDeserialised.ruleError_0.name.ToString();
            Assert.True(expectedName == actualName, $"File {Path.GetFileName(tempFilePathForDtroUpdate)}: expected is '{expectedName}' but actual was '{actualName}', with response body\n{createDtroResponseJson}");

            string actualMessage = jsonDeserialised.ruleError_0.message.ToString();
            Assert.True(actualMessage.StartsWith("Provision reference ") && actualMessage.EndsWith(" is present 2 times."), $"File {Path.GetFileName(tempFilePathForDtroUpdate)}: actual message was '{actualMessage}', with response body\n{createDtroResponseJson}");

            string expectedPath = "Source -> Provision -> reference";
            string actualPath = jsonDeserialised.ruleError_0.path.ToString();
            Assert.True(expectedPath == actualPath, $"File {Path.GetFileName(tempFilePathForDtroUpdate)}: expected is '{expectedPath}' but actual was '{actualPath}', with response body\n{createDtroResponseJson}");

            string expectedRule = "Each provision 'reference' must be unique and of type 'string'";
            string actualRule = jsonDeserialised.ruleError_0.rule.ToString();
            Assert.True(expectedRule == actualRule, $"File {Path.GetFileName(tempFilePathForDtroUpdate)}: expected is '{expectedRule}' but actual was '{actualRule}', with response body\n{createDtroResponseJson}");
        }

        [Fact]
        public async Task DtroUpdatedFromJsonBodyWithDuplicateProvisionReferenceShouldBeRejected()
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string createDtroJsonWithTraModified = Dtros.GetJsonFromFileAndModifyTra(schemaVersionToTest, fileName, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraModified, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == createDtroResponse.StatusCode, $"File {fileName}: expected status code is {HttpStatusCode.Created} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = Dtros.GetJsonAndCreateUpdateJsonWithTraModifiedAndDuplicateProvisionReference(schemaVersionToTest, fileName, publisher.TraId);

            // Send DTRO update
            string dtroId = await Dtros.GetIdFromResponseJsonAsync(createDtroResponse);
            HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromJsonBodyAsync(dtroUpdateJson, dtroId, publisher);
            string updateDtroResponseJson = await updateDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == updateDtroResponse.StatusCode, $"File {fileName}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {updateDtroResponse.StatusCode}, with response body\n{updateDtroResponseJson}");

            // Evaluate response JSON rule failures
            dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(updateDtroResponseJson)!;

            string expectedName = "'2' duplication reference";
            string actualName = jsonDeserialised.ruleError_0.name.ToString();
            Assert.True(expectedName == actualName, $"File {fileName}: expected is '{expectedName}' but actual was '{actualName}', with response body\n{createDtroResponseJson}");

            string actualMessage = jsonDeserialised.ruleError_0.message.ToString();
            Assert.True(actualMessage.StartsWith("Provision reference ") && actualMessage.EndsWith(" is present 2 times."), $"File {fileName}: actual message was '{actualMessage}', with response body\n{createDtroResponseJson}");

            string expectedPath = "Source -> Provision -> reference";
            string actualPath = jsonDeserialised.ruleError_0.path.ToString();
            Assert.True(expectedPath == actualPath, $"File {fileName}: expected is '{expectedPath}' but actual was '{actualPath}', with response body\n{createDtroResponseJson}");

            string expectedRule = "Each provision 'reference' must be unique and of type 'string'";
            string actualRule = jsonDeserialised.ruleError_0.rule.ToString();
            Assert.True(expectedRule == actualRule, $"File {fileName}: expected is '{expectedRule}' but actual was '{actualRule}', with response body\n{createDtroResponseJson}");
        }
    }
}