using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities
{
    public static class Dtros
    {
        public static void DeleteExistingDtros()
        {
            SqlQueries.TruncateTable("Dtros");
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
    }
}