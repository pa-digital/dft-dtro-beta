using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.CreateDtroTests
{
    public class MultipleSchemaValidationErrors : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly string fileName = "JSON-3.4.0-example-Derbyshire 2024 DJ388 partial.json";

        [Fact]
        public async Task DtroSubmittedFromFileWithDuplicateProvisionReferenceShouldBeRejected()
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

            // Prepare DTRO
            string tempFilePath = FileHelper.CreateTempFileToFailSchemaValidation(schemaVersionToTest, fileName, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePath)}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Evaluate response JSON rule failures
            string expectedErrorJson = JsonMethods.GetSchemaValidationErrorJson(publisher.TraId);

            JsonMethods.CompareJson(expectedErrorJson, createDtroResponseJson);
        }

        [Fact]
        public async Task DtroSubmittedFromJsonBodyWithDuplicateProvisionReferenceShouldBeRejected()
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

            // Prepare DTRO
            string createDtroJsonWithTraModifiedAndDuplicateProvisionReference = FileHelper.GetJsonFromFileAndModifyToFailSchemaValidation(schemaVersionToTest, fileName, publisher.TraId);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraModifiedAndDuplicateProvisionReference, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {fileName}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Evaluate response JSON rule failures
            string expectedErrorJson = JsonMethods.GetSchemaValidationErrorJson(publisher.TraId);

            JsonMethods.CompareJson(expectedErrorJson, createDtroResponseJson);
        }
    }
}