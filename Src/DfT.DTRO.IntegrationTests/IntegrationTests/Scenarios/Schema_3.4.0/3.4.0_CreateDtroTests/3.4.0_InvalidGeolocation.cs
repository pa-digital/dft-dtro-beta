using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.FileHelper;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers.JsonMethods;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.CreateDtroTests
{
    public class InvalidGeolocation : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly string fileName = "JSON-3.4.0-example-TTRO-HeightRestrictionwithConditions.json";

        public static IEnumerable<object[]> GetPointGeolocationStrings()
        {
            return new List<object[]>
            {
                new object[] { "SRID=27700;POINT(699999 1300000)" },
                new object[] { "SRID=27700;POINT(700000 1299999)" },
                new object[] { "SRID=27700;POINT(700000 1300000)" },
                new object[] { "SRID=27700;POINT(1 0)" },
                new object[] { "SRID=27700;POINT(0 1)" },
                new object[] { "SRID=27700;POINT(1 -1)" },
                new object[] { "SRID=27700;POINT(-1 1)" },
                new object[] { "SRID=27700;POINT(0 0)" },
                new object[] { "SRID=27700;POINT(-1 -1)" }
            };
        }

        [Theory]
        [MemberData(nameof(GetPointGeolocationStrings))]
        public async Task DtroSubmittedFromFileWithInvalidPointGeometryShouldBeRejected(string pointGeometryString)
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);
            string userGuidToGenerateFileNamePrefix = await JsonMethods.GetIdFromResponseJsonAsync(createUserResponse);

            // Prepare DTRO
            string tempFilePath = CreateTempFileWithTraAndPointGeometryModified(schemaVersionToTest, fileName, userGuidToGenerateFileNamePrefix, publisher.TraId, pointGeometryString);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePath)}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Evaluate response JSON rule failures
            dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(createDtroResponseJson)!;

            string expectedName = "Invalid coordinates";
            string actualName = jsonDeserialised.ruleError_0.name.ToString();
            Assert.True(expectedName == actualName, $"File {Path.GetFileName(tempFilePath)}: expected is '{expectedName}' but actual was '{actualName}', with response body\n{createDtroResponseJson}");

            string expectedMessage = "Geometry grid linked to 'PointGeometry'";
            string actualMessage = jsonDeserialised.ruleError_0.message.ToString();
            Assert.True(expectedMessage == actualMessage, $"File {Path.GetFileName(tempFilePath)}: expected is '{expectedMessage}' but actual message was '{actualMessage}', with response body\n{createDtroResponseJson}");

            string expectedPath = "Source -> Provision -> RegulatedPlace -> PointGeometry -> point";
            string actualPath = jsonDeserialised.ruleError_0.path.ToString();
            Assert.True(expectedPath == actualPath, $"File {Path.GetFileName(tempFilePath)}: expected is '{expectedPath}' but actual was '{actualPath}', with response body\n{createDtroResponseJson}");

            string expectedRule = $"Coordinates '{pointGeometryString}' are incorrect or not within Great Britain";
            string actualRule = jsonDeserialised.ruleError_0.rule.ToString();
            Assert.True(expectedRule == actualRule, $"File {Path.GetFileName(tempFilePath)}: expected is '{expectedRule}' but actual was '{actualRule}', with response body\n{createDtroResponseJson}");
        }

        [Theory]
        [MemberData(nameof(GetPointGeolocationStrings))]
        public async Task DtroSubmittedFromJsonBodyWithDuplicateProvisionReferenceShouldBeRejected(string pointGeometryString)
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

            // Prepare DTRO
            string createDtroJsonWithTraModifiedAndDuplicateProvisionReference = GetJsonFromFileAndModifyTraAndPointGeometry(schemaVersionToTest, fileName, publisher.TraId, pointGeometryString);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraModifiedAndDuplicateProvisionReference, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {fileName}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Evaluate response JSON rule failures
            dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(createDtroResponseJson)!;

            string expectedName = "Invalid coordinates";
            string actualName = jsonDeserialised.ruleError_0.name.ToString();
            Assert.True(expectedName == actualName, $"File {fileName}: expected is '{expectedName}' but actual was '{actualName}', with response body\n{createDtroResponseJson}");

            string expectedMessage = "Geometry grid linked to 'PointGeometry'";
            string actualMessage = jsonDeserialised.ruleError_0.message.ToString();
            Assert.True(expectedMessage == actualMessage, $"File {fileName}: expected is '{expectedMessage}' but actual message was '{actualMessage}', with response body\n{createDtroResponseJson}");

            string expectedPath = "Source -> Provision -> RegulatedPlace -> PointGeometry -> point";
            string actualPath = jsonDeserialised.ruleError_0.path.ToString();
            Assert.True(expectedPath == actualPath, $"File {fileName}: expected is '{expectedPath}' but actual was '{actualPath}', with response body\n{createDtroResponseJson}");

            string expectedRule = $"Coordinates '{pointGeometryString}' are incorrect or not within Great Britain";
            string actualRule = jsonDeserialised.ruleError_0.rule.ToString();
            Assert.True(expectedRule == actualRule, $"File {fileName}: expected is '{expectedRule}' but actual was '{actualRule}', with response body\n{createDtroResponseJson}");
        }
    }
}