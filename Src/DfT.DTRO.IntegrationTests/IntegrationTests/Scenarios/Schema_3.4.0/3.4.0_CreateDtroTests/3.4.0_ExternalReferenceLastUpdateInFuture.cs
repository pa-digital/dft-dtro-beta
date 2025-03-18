using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.FileHelper;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers.JsonMethods;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.CreateDtroTests
{
    public class ExternalReferenceLastUpdateInFuture : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        // readonly string fileName = "JSON-3.4.0-example-Derbyshire 2024 DJ388 partial.json";

        public static IEnumerable<object[]> GetDtroFileNames()
        {
            DirectoryInfo directoryPath = new DirectoryInfo($"{AbsolutePathToDtroExamplesDirectory}/{schemaVersionToTest}");
            FileInfo[] files = directoryPath.GetFiles();

            foreach (FileInfo file in files)
            {
                yield return new object[] { file.Name };
            }
        }

        [Theory]
        [MemberData(nameof(GetDtroFileNames))]
        public async Task DtroSubmittedFromFileWithDuplicateProvisionReferenceShouldBeRejected(string fileName)
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);
            string userGuidToGenerateFileNamePrefix = await JsonMethods.GetIdFromResponseJsonAsync(createUserResponse);

            // Prepare DTRO
            string tempFilePath = CreateTempFileWithTraModifiedAndExternalReferenceLastUpdateDateInFuture(schemaVersionToTest, fileName, userGuidToGenerateFileNamePrefix, publisher.TraId);

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

        // [Fact]
        // public async Task DtroSubmittedFromJsonBodyWithDuplicateProvisionReferenceShouldBeRejected()
        // {
        //     // Generate user to send DTRO and read it back
        //     TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
        //     HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
        //     Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

        //     // Prepare DTRO
        //     string createDtroJsonWithTraModifiedAndDuplicateProvisionReference = GetJsonFromFileAndModifyTraAndDuplicateProvisionReference(schemaVersionToTest, fileName, publisher.TraId);

        //     // Send DTRO
        //     HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraModifiedAndDuplicateProvisionReference, publisher);
        //     string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
        //     Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {fileName}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

        //     // Evaluate response JSON rule failures
        //     dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(createDtroResponseJson)!;

        //     string expectedName = "'2' duplication reference";
        //     string actualName = jsonDeserialised.ruleError_0.name.ToString();
        //     Assert.True(expectedName == actualName, $"File {fileName}: expected is '{expectedName}' but actual was '{actualName}', with response body\n{createDtroResponseJson}");

        //     string actualMessage = jsonDeserialised.ruleError_0.message.ToString();
        //     Assert.True(actualMessage.StartsWith("Provision reference ") && actualMessage.EndsWith(" is present 2 times."), $"File {fileName}: actual message was '{actualMessage}', with response body\n{createDtroResponseJson}");

        //     string expectedPath = "Source -> Provision -> reference";
        //     string actualPath = jsonDeserialised.ruleError_0.path.ToString();
        //     Assert.True(expectedPath == actualPath, $"File {fileName}: expected is '{expectedPath}' but actual was '{actualPath}', with response body\n{createDtroResponseJson}");

        //     string expectedRule = "Each provision 'reference' must be unique and of type 'string'";
        //     string actualRule = jsonDeserialised.ruleError_0.rule.ToString();
        //     Assert.True(expectedRule == actualRule, $"File {fileName}: expected is '{expectedRule}' but actual was '{actualRule}', with response body\n{createDtroResponseJson}");
        // }
    }
}