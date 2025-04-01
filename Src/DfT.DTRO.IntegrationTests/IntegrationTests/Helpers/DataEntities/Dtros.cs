using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.Consts;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities
{
    public static class Dtros
    {
        public static void DeleteExistingDtros()
        {
            SqlQueries.TruncateTable("Dtros");
            SqlQueries.TruncateTable("DtroHistories");
        }

        public static async Task<HttpResponseMessage> CreateDtroFromFileAsync(string filePath, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            headers.Add("Content-Type", "multipart/form-data");

            await headers.AddValidHeadersForEnvironmentAsync(UserGroup.Tra, testUser.AppId);

            HttpResponseMessage dtroCreationResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}{RouteTemplates.DtrosCreateFromFile}", headers, pathToJsonFile: filePath);
            return dtroCreationResponse;
        }

        public static async Task<HttpResponseMessage> GetDtroAsync(string dtroId, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            await headers.AddValidHeadersForEnvironmentAsync(UserGroup.Tra, testUser.AppId);

            HttpResponseMessage getDtroResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{BaseUri}{RouteTemplates.DtrosBase}/{dtroId}", headers);
            return getDtroResponse;
        }

        public static string ModifySchemaVersionInDtro(string jsonString, string schemaVersion)
        {
            JObject jsonObj = JObject.Parse(jsonString);
            jsonObj["schemaVersion"] = schemaVersion;
            return jsonObj.ToString();
        }

        public static string ModifyExternalReferenceLastUpdateDate(string jsonString, string newDate)
        {
            JObject jsonObj = JObject.Parse(jsonString);
            ModifyExternalReferenceLastUpdateDateRecursive(jsonObj, newDate);
            return jsonObj.ToString();
        }

        private static void ModifyExternalReferenceLastUpdateDateRecursive(JToken token, string newDate)
        {
            if (token is JObject obj)
            {
                foreach (var property in obj.Properties())
                {
                    if (property.Value is JArray array && (property.Name == "externalReference" || property.Name == "origin"))
                    {
                        foreach (var item in array.Children<JObject>())
                        {
                            if (item.ContainsKey("lastUpdateDate"))
                            {
                                item["lastUpdateDate"] = newDate;
                            }
                        }
                    }
                    else
                    {
                        ModifyExternalReferenceLastUpdateDateRecursive(property.Value, newDate);
                    }
                }
            }
            else if (token is JArray array)
            {
                foreach (var item in array)
                {
                    ModifyExternalReferenceLastUpdateDateRecursive(item, newDate);
                }
            }
        }

        public static string GetPointGeometryErrorJson(string pointGeometryString)
        {
            string expectedErrorJson = $$"""
            {
                "ruleError_0": {
                    "name": "Invalid coordinates",
                    "message": "Geometry grid linked to 'PointGeometry'",
                    "path": "Source -> Provision -> RegulatedPlace -> PointGeometry -> point",
                    "rule": "Coordinates '{{pointGeometryString}}' are incorrect or not within Great Britain"
                }
            }
            """;

            return expectedErrorJson;
        }

        public static string GetLinearGeometryErrorJson(string linearGeometryString)
        {
            string expectedErrorJson = $$"""
            {
                "ruleError_0": {
                    "name": "Invalid coordinates",
                    "message": "Geometry grid linked to 'LinearGeometry'",
                    "path": "Source -> Provision -> RegulatedPlace -> LinearGeometry -> linestring",
                    "rule": "Coordinates '{{linearGeometryString}}' are incorrect or not within Great Britain"
                }
            }
            """;

            return expectedErrorJson;
        }

        public static string GetDuplicateProvisionReferenceErrorJson(string provisionReference)
        {
            string expectedErrorJson = $$"""
            {
                "ruleError_0": {
                    "name": "'2' duplication reference",
                    "message": "Provision reference '{{provisionReference}}' is present 2 times.",
                    "path": "Source -> Provision -> reference",
                    "rule": "Each provision 'reference' must be unique and of type 'string'"
                }
            }
            """;

            return expectedErrorJson;
        }

        public static string GetExternalReferenceLastUpdateDateErrorJson(string fileName)
        {
            string geometryType = fileName.Contains("Derbyshire") ? "LinearGeometry" : "PointGeometry";
            string expectedErrorJson = $$"""
            {
                "ruleError_0": {
                    "name": "Invalid last update date",
                    "message": "Indicates the date the USRN reference was last updated",
                    "path": "Source -> Provision -> RegulatedPlace -> {{geometryType}} -> ExternalReference -> lastUpdateDate",
                    "rule": "'lastUpdateDate' cannot be in the future"
                }
            }
            """;

            return expectedErrorJson;
        }
    }
}