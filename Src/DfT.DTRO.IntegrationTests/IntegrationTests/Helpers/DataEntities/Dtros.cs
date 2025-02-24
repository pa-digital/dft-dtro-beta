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

        public static async Task<HttpResponseMessage> CreateDtroFromFileAsync(string exampleFileName, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId },
                { "Content-Type", "multipart/form-data" }
            };

            string tempFilePath = GetDtroFileWithTraUpdated(exampleFileName, testUser.TraId);
            HttpResponseMessage createDtroResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}/dtros/createFromFile", headers, pathToJsonFile: tempFilePath);
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

        private static string GetDtroFileWithTraUpdated(string exampleFileName, string traId)
        {
            string exampleFilePath = $"{AbsolutePathToDtroExamplesDirectory}/{exampleFileName}";
            Directory.CreateDirectory($"{AbsolutePathToDtroExamplesTempDirectory}");
            string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{exampleFileName}";
            File.Copy(exampleFilePath, tempFilePath, overwrite: true);

            string jsonString = File.ReadAllText(tempFilePath);
            string jsonWithUpdatedTra = UpdateTraId(jsonString, traId);
            File.WriteAllText(tempFilePath, jsonWithUpdatedTra);
            return tempFilePath;
        }

        public static string UpdateTraId(string jsonString, string traId)
        {
            JObject jsonObj = JObject.Parse(jsonString);
            int tradIdAsInt = int.Parse(traId);
            jsonObj["data"]["Source"]["currentTraOwner"] = tradIdAsInt;
            jsonObj["data"]["Source"]["traAffected"] = new JArray(tradIdAsInt);
            jsonObj["data"]["Source"]["traCreator"] = tradIdAsInt;
            return jsonObj.ToString();
        }
    }
}