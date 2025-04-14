using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Consts;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Enums;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Extensions;
using DfT.DTRO.ApiTests.ApiTests.Helpers.JsonHelpers;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.TestConfig;

namespace DfT.DTRO.ApiTests.ApiTests.Schema_3_4_0.PublisherScenarios.DtroUpdateScenarios
{
    // DPPB-1235
    public class InvalidGeometry : BaseTest
    {
        readonly static string schemaVersionToTest = SchemaVersions._3_4_0;
        readonly string pointGeometryFileName = "dtro-v3.4.0-example-height-restriction-with-conditions.json";
        readonly string linearGeometryFileName = "dtro-v3.4.0-example-derbyshire-2024-dj388-partial.json";

        public static IEnumerable<object[]> GetPointGeometryStrings()
        {
            if (EnvironmentName == EnvironmentType.Local)
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
            else
            {
                return new List<object[]>
                {
                    new object[] { "SRID=27700;POINT(699999 1300001)" }
                };
            }
        }

        public static IEnumerable<object[]> GetLinearGeometryStrings()
        {
            if (EnvironmentName == EnvironmentType.Local)
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
            else
            {
                return new List<object[]>
                {
                    new object[] { "SRID=27700;LINESTRING(699999 1299999, 700000 1300001)" }
                };
            }

        }

        [Theory]
        [MemberData(nameof(GetPointGeometryStrings))]
        public async Task DtroSubmittedFromJsonBodyWithInvalidPointGeometryShouldBeRejected(string pointGeometryString)
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = pointGeometryFileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, (int)publisher.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {pointGeometryFileName}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = dtroCreationJson
                                        .ModifyPointGeometry(pointGeometryString)
                                        .ModifySourceActionType(schemaVersionToTest, "amendment")
                                        .AppendTextToTroName(schemaVersionToTest, "UPDATED");

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await dtroUpdateJson.SendJsonInDtroUpdateRequestAsync(dtroId, publisher);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {pointGeometryFileName}:\n\n{dtroUpdateResponseJson}");

            // Evaluate response JSON
            string expectedErrorJson = Dtros.GetPointGeometryErrorJson(pointGeometryString);
            JsonMethods.CompareJson(expectedErrorJson, dtroUpdateResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetPointGeometryStrings))]
        public async Task DtroUpdatedFromFileWithInvalidPointGeometryShouldBeRejected(string pointGeometryString)
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = pointGeometryFileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, (int)publisher.TraId);

            string tempFilePathForDtroCreation = dtroCreationJson.CreateDtroTempFile(pointGeometryFileName, publisher);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await tempFilePathForDtroCreation.SendFileInDtroCreationRequestAsync(publisher);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroCreation)}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = dtroCreationJson
                                        .ModifyPointGeometry(pointGeometryString)
                                        .ModifySourceActionType(schemaVersionToTest, "amendment")
                                        .AppendTextToTroName(schemaVersionToTest, "UPDATED");

            string tempFilePathForDtroUpdate = dtroUpdateJson.CreateDtroTempFileForUpdate(pointGeometryFileName, publisher);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await tempFilePathForDtroUpdate.SendFileInDtroUpdateRequestAsync(dtroId, publisher);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroUpdateResponseJson}");

            // Evaluate response JSON rule failures
            string expectedErrorJson = Dtros.GetPointGeometryErrorJson(pointGeometryString);
            JsonMethods.CompareJson(expectedErrorJson, dtroUpdateResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetLinearGeometryStrings))]
        public async Task DtroUpdatedFromJsonBodyWithInvalidLinearGeometryShouldBeRejected(string linearGeometryString)
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = linearGeometryFileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, (int)publisher.TraId);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await dtroCreationJson.SendJsonInDtroCreationRequestAsync(publisher);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {linearGeometryFileName}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = dtroCreationJson
                                        .ModifyLinearGeometry(linearGeometryString)
                                        .ModifySourceActionType(schemaVersionToTest, "amendment")
                                        .AppendTextToTroName(schemaVersionToTest, "UPDATED");

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await dtroUpdateJson.SendJsonInDtroUpdateRequestAsync(dtroId, publisher);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {linearGeometryFileName}:\n\n{dtroUpdateResponseJson}");

            // Evaluate response JSON
            string expectedErrorJson = Dtros.GetLinearGeometryErrorJson(linearGeometryString);
            JsonMethods.CompareJson(expectedErrorJson, dtroUpdateResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetLinearGeometryStrings))]
        public async Task DtroUpdatedFromFileWithInvalidLinearGeometryShouldBeRejected(string linearGeometryString)
        {
            // Generate user to send DTRO and read it back
            TestUser publisher = await TestUsers.GetUser(TestUserType.Publisher1);

            // Prepare DTRO
            string dtroCreationJson = linearGeometryFileName
                                    .GetJsonFromFile(schemaVersionToTest)
                                    .ModifyTraInDtroJson(schemaVersionToTest, (int)publisher.TraId);

            string tempFilePathForDtroCreation = dtroCreationJson.CreateDtroTempFile(linearGeometryFileName, publisher);

            // Send DTRO
            HttpResponseMessage dtroCreationResponse = await tempFilePathForDtroCreation.SendFileInDtroCreationRequestAsync(publisher);
            string dtroCreationResponseJson = await dtroCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == dtroCreationResponse.StatusCode,
                $"Actual status code: {dtroCreationResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroCreation)}:\n\n{dtroCreationResponseJson}");

            // Prepare DTRO update
            string dtroUpdateJson = dtroCreationJson
                                        .ModifyLinearGeometry(linearGeometryString)
                                        .ModifySourceActionType(schemaVersionToTest, "amendment")
                                        .AppendTextToTroName(schemaVersionToTest, "UPDATED");

            string tempFilePathForDtroUpdate = dtroUpdateJson.CreateDtroTempFileForUpdate(linearGeometryFileName, publisher);

            // Send DTRO update
            string dtroId = await dtroCreationResponse.GetIdFromResponseJsonAsync();
            HttpResponseMessage dtroUpdateResponse = await tempFilePathForDtroUpdate.SendFileInDtroUpdateRequestAsync(dtroId, publisher);
            string dtroUpdateResponseJson = await dtroUpdateResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.BadRequest == dtroUpdateResponse.StatusCode,
                $"Actual status code: {dtroUpdateResponse.StatusCode}. Response JSON for file {Path.GetFileName(tempFilePathForDtroUpdate)}:\n\n{dtroUpdateResponseJson}");

            // Evaluate response JSON rule failures
            string expectedErrorJson = Dtros.GetLinearGeometryErrorJson(linearGeometryString);
            JsonMethods.CompareJson(expectedErrorJson, dtroUpdateResponseJson);
        }
    }
}