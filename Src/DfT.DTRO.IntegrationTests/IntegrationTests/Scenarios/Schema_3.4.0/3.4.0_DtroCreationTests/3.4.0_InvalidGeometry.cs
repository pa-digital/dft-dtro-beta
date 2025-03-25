using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_4_0.DtroCreationTests
{
    // DPPB-1235
    public class InvalidGeometry : BaseTest
    {
        readonly static string schemaVersionToTest = "3.4.0";
        readonly string pointGeometryFileName = "JSON-3.4.0-example-TTRO-HeightRestrictionwithConditions.json";
        readonly string linearGeometryFileName = "JSON-3.4.0-example-Derbyshire 2024 DJ388 partial.json";

        public static IEnumerable<object[]> GetPointGeometryStrings()
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

        public static IEnumerable<object[]> GetLinearGeometryStrings()
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
        [MemberData(nameof(GetPointGeometryStrings))]
        public async Task DtroSubmittedFromJsonBodyWithInvalidPointGeometryShouldBeRejected(string pointGeometryString)
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await publisher.CreateUserForDataSetUpAsync();

            // Prepare DTRO
            string dtroCreationJson = pointGeometryFileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId)
                                    .ModifyPointGeometry(pointGeometryString);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode,
                $"Response JSON for file {pointGeometryFileName}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON
            string expectedErrorJson = Dtros.GetPointGeometryErrorJson(pointGeometryString);
            JsonMethods.CompareJson(expectedErrorJson, dtroCreationResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetPointGeometryStrings))]
        public async Task DtroSubmittedFromFileWithInvalidPointGeometryShouldBeRejected(string pointGeometryString)
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await publisher.CreateUserForDataSetUpAsync();

            // Prepare DTRO
            string dtroCreationJson = pointGeometryFileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId)
                                    .ModifyPointGeometry(pointGeometryString);

            string dtroTempFilePath = dtroCreationJson.CreateDtroTempFile(pointGeometryFileName, publisher.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroTempFilePath.SendFileInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode,
                $"Response JSON for file {Path.GetFileName(dtroTempFilePath)}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON rule failures
            string expectedErrorJson = Dtros.GetPointGeometryErrorJson(pointGeometryString);
            JsonMethods.CompareJson(expectedErrorJson, dtroCreationResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetLinearGeometryStrings))]
        public async Task DtroSubmittedFromJsonBodyWithInvalidLinearGeometryShouldBeRejected(string linearGeometryString)
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await publisher.CreateUserForDataSetUpAsync();

            // Prepare DTRO
            string dtroCreationJson = linearGeometryFileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId)
                                    .ModifyLinearGeometry(linearGeometryString);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher.AppId);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode,
                $"Response JSON for file {linearGeometryFileName}:\n\n{dtroCreationResponseJson}");

            // Evaluate response JSON rule failures
            string expectedErrorJson = Dtros.GetLinearGeometryErrorJson(linearGeometryString);
            JsonMethods.CompareJson(expectedErrorJson, dtroCreationResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetLinearGeometryStrings))]
        public async Task DtroSubmittedFromFileWithInvalidLinearGeometryShouldBeRejected(string linearGeometryString)
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
            await publisher.CreateUserForDataSetUpAsync();

            // Prepare DTRO
            string dtroCreationJson = linearGeometryFileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, publisher.TraId)
                                    .ModifyLinearGeometry(linearGeometryString);

            // Prepare DTRO
            string dtroTempFilePath = dtroCreationJson.CreateDtroTempFile(linearGeometryFileName, publisher.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await Dtros.CreateDtroFromFileAsync(dtroTempFilePath, publisher);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroCreationResponse.StatusCode, $"File {Path.GetFileName(dtroTempFilePath)}: expected status code is {HttpStatusCode.BadRequest} but actual status code was {dtroCreationResponse.StatusCode}, with response body\n{dtroCreationResponseJson}");

            // Evaluate response JSON rule failures
            string expectedErrorJson = Dtros.GetLinearGeometryErrorJson(linearGeometryString);
            JsonMethods.CompareJson(expectedErrorJson, dtroCreationResponseJson);
        }
    }
}