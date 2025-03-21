using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.CreateDtroTests
{
    // DPPB-1235
    public class InvalidGeolocation : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly string pointGeometryFileName = "JSON-3.4.0-example-TTRO-HeightRestrictionwithConditions.json";
        readonly string linearGeometryFileName = "JSON-3.4.0-example-Derbyshire 2024 DJ388 partial.json";

        public static IEnumerable<object[]> GetPointGeolocationStrings()
        {
            return new List<object[]>
            {
                new object[] { "SRID=27700;POINT(699999 1300001)" },
                new object[] { "SRID=27700;POINT(700001 1299999)" },
                new object[] { "SRID=27700;POINT(700001 1300001)" },
                new object[] { "SRID=27700;POINT(1 -1)" },
                new object[] { "SRID=27700;POINT(-1 1)" },
                new object[] { "SRID=27700;POINT(-1 -1)" }
            };
        }

        public static IEnumerable<object[]> GetLinearGeolocationStrings()
        {
            return new List<object[]>
            {
                new object[] { "SRID=27700;LINESTRING(699999 1299999, 700000 1300001)" },
                new object[] { "SRID=27700;LINESTRING(699999 1299999, 700001 1300000)" },
                new object[] { "SRID=27700;LINESTRING(699999 1300001, 700000 1300000)" },
                new object[] { "SRID=27700;LINESTRING(700001 1299999, 700000 1300000)" },
                new object[] { "SRID=27700;LINESTRING(1 1, 0 -1)" },
                new object[] { "SRID=27700;LINESTRING(1 1, -1 0)" },
                new object[] { "SRID=27700;LINESTRING(1 -1, 0 0)" },
                new object[] { "SRID=27700;LINESTRING(-1 1, 0 0)" }
            };
        }

        [Theory]
        [MemberData(nameof(GetPointGeolocationStrings))]
        public async Task DtroSubmittedFromFileWithInvalidPointGeometryShouldBeRejected(string pointGeometryString)
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string tempFilePath = Dtros.CreateTempFileWithTraAndPointGeometryModified(schemaVersionToTest, pointGeometryFileName, publisher.TraId, pointGeometryString);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePath)}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Evaluate response JSON rule failures
            string expectedErrorJson = Dtros.GetPointGeolocationErrorJson(pointGeometryString);
            JsonMethods.CompareJson(expectedErrorJson, createDtroResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetPointGeolocationStrings))]
        public async Task DtroSubmittedFromJsonBodyWithInvalidPointGeometryShouldBeRejected(string pointGeometryString)
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string createDtroJsonWithTraAndPointGeometryModified = Dtros.GetJsonFromFileAndModifyTraAndPointGeometry(schemaVersionToTest, pointGeometryFileName, publisher.TraId, pointGeometryString);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraAndPointGeometryModified, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {pointGeometryFileName}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Evaluate response JSON rule failures
            string expectedErrorJson = Dtros.GetPointGeolocationErrorJson(pointGeometryString);
            JsonMethods.CompareJson(expectedErrorJson, createDtroResponseJson);
        }
        [Theory]
        [MemberData(nameof(GetLinearGeolocationStrings))]
        public async Task DtroSubmittedFromFileWithInvalidLinearGeometryShouldBeRejected(string linearGeometryString)
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string tempFilePath = Dtros.CreateTempFileWithTraAndLinearGeometryModified(schemaVersionToTest, linearGeometryFileName, publisher.TraId, linearGeometryString);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {Path.GetFileName(tempFilePath)}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Evaluate response JSON rule failures
            string expectedErrorJson = Dtros.GetLinearGeolocationErrorJson(linearGeometryString);
            JsonMethods.CompareJson(expectedErrorJson, createDtroResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetLinearGeolocationStrings))]
        public async Task DtroSubmittedFromJsonBodyWithInvalidLinearGeometryShouldBeRejected(string linearGeometryString)
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await DtroUsers.CreateUserForDataSetUpAsync(publisher);

            // Prepare DTRO
            string createDtroJsonWithTraAndLinearGeometryModified = Dtros.GetJsonFromFileAndModifyTraAndLinearGeometry(schemaVersionToTest, linearGeometryFileName, publisher.TraId, linearGeometryString);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraAndLinearGeometryModified, publisher);
            string createDtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == createDtroResponse.StatusCode, $"File {linearGeometryFileName}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {createDtroResponse.StatusCode}, with response body\n{createDtroResponseJson}");

            // Evaluate response JSON rule failures
            string expectedErrorJson = Dtros.GetLinearGeolocationErrorJson(linearGeometryString);
            JsonMethods.CompareJson(expectedErrorJson, createDtroResponseJson);
        }
    }
}