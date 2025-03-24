using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.DtroCreationTests
{
    public class MultipleSchemaValidationErrors : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly string fileName = "JSON-3.4.0-example-Derbyshire 2024 DJ388 partial.json";

        [Fact]
        public async Task DtroSubmittedFromFileWithMultipleSchemaErrorsShouldBeRejected()
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string tempFilePath = Dtros.CreateTempFileToFailSchemaValidation(schemaVersionToTest, fileName, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePath)}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Evaluate response JSON rule failures
            string expectedErrorJson = Dtros.GetSchemaValidationErrorJson(publisher.TraId);
            JsonMethods.CompareJson(expectedErrorJson, createDtroResponseJson);
        }

        [Fact]
        public async Task DtroSubmittedFromJsonBodyWithWithMultipleSchemaErrorsShouldBeRejected()
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string createDtroJsonWithTraModifiedAndSchemaValidationToFail = Dtros.GetJsonFromFileAndModifyToFailSchemaValidation(schemaVersionToTest, fileName, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraModifiedAndSchemaValidationToFail, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {fileName}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Evaluate response JSON rule failures
            string expectedErrorJson = Dtros.GetSchemaValidationErrorJson(publisher.TraId);
            JsonMethods.CompareJson(expectedErrorJson, createDtroResponseJson);
        }
    }
}