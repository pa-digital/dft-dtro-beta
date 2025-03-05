using Newtonsoft.Json.Linq;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

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
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId },
                { "Content-Type", "multipart/form-data" }
            };

            HttpResponseMessage createDtroResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}/dtros/createFromFile", headers, pathToJsonFile: filePath);
            return createDtroResponse;
        }

        public static async Task<HttpResponseMessage> UpdateDtroFromFileAsync(string filePath, string dtroId, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId },
                { "Content-Type", "multipart/form-data" }
            };

            HttpResponseMessage createDtroResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Put, $"{BaseUri}/dtros/updateFromFile/{dtroId}", headers, pathToJsonFile: filePath);
            return createDtroResponse;
        }

        public static async Task<HttpResponseMessage> CreateDtroFromJsonBodyAsync(string jsonBody, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId },
                { "Content-Type", "application/json" }
            };

            HttpResponseMessage createDtroResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}/dtros/createFromBody", headers, jsonBody);
            return createDtroResponse;
        }

        public static async Task<HttpResponseMessage> UpdateDtroFromJsonBodyAsync(string jsonBody, string dtroId, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId },
                { "Content-Type", "application/json" }
            };

            HttpResponseMessage createDtroResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Put, $"{BaseUri}/dtros/updateFromBody/{dtroId}", headers, jsonBody);
            return createDtroResponse;
        }

        public static async Task<HttpResponseMessage> GetDtroAsync(string dtroId, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId }
            };

            HttpResponseMessage createDtroResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{BaseUri}/dtros/{dtroId}", headers);
            return createDtroResponse;
        }

        public static string UpdateTraIdInDtro(string schemaVersion, string jsonString, string traId)
        {
            JObject jsonObj = JObject.Parse(jsonString);
            int tradIdAsInt = int.Parse(traId);

            int schemaVersionAsInt = int.Parse(schemaVersion.Replace(".", ""));

            if (schemaVersionAsInt >= 332)
            {
                // New camel case format
                jsonObj["data"]["source"]["currentTraOwner"] = tradIdAsInt;
                jsonObj["data"]["source"]["traAffected"] = new JArray(tradIdAsInt);
                jsonObj["data"]["source"]["traCreator"] = tradIdAsInt;
            }
            else
            {
                // Old Pascal case format
                jsonObj["data"]["Source"]["currentTraOwner"] = tradIdAsInt;
                jsonObj["data"]["Source"]["traAffected"] = new JArray(tradIdAsInt);
                jsonObj["data"]["Source"]["traCreator"] = tradIdAsInt;
            }

            return jsonObj.ToString();
        }

        public static string UpdateSchemaVersionInDtro(string jsonString, string schemaVersion)
        {
            JObject jsonObj = JObject.Parse(jsonString);
            jsonObj["schemaVersion"] = schemaVersion;
            return jsonObj.ToString();
        }

        public static string UpdateActionTypeAndTroName(string jsonString, string schemaVersion)
        {
            JObject jsonObj = JObject.Parse(jsonString);
            int schemaVersionAsInt = int.Parse(schemaVersion.Replace(".", ""));

            if (schemaVersionAsInt >= 332)
            {
                // New camel case format
                jsonObj["data"]["source"]["actionType"] = "amendment";
                jsonObj["data"]["source"]["troName"] = $"{jsonObj["data"]["source"]["troName"]} UPDATED";
            }
            else
            {
                // Old Pascal case format
                jsonObj["data"]["Source"]["actionType"] = "amendment";
                jsonObj["data"]["Source"]["troName"] = $"{jsonObj["data"]["Source"]["troName"]} UPDATED";
            }

            return jsonObj.ToString();
        }
    }
}