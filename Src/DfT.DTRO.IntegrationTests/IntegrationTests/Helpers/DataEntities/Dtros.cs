using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities
{
    public static class Dtros
    {
        public static void DeleteExistingDtros()
        {
            SqlQueries.TruncateTable("Dtros");
        }

        public static async Task<HttpResponseMessage> CreateDtroAsync(UserDetails userDetails)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", userDetails.AppId },
                { "Content-Type", "multipart/form-data" }
            };

            var exampleFileName = $"JSON-{TestConfig.SchemaVersionUnderTest}-example-Derbyshire 2024 DJ388 partial.json";
            var tempFilePath = GetDtroFileWithTraUpdated(exampleFileName, userDetails.TraId);
            var response = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{TestConfig.BaseUri}/dtros/createFromFile", headers, pathToJsonFile: tempFilePath);
            return response;
        }

        private static string GetDtroFileWithTraUpdated(string exampleFileName, string traId)
        {
            var exampleFilePath = $"{TestConfig.AbsolutePathToDtroExamplesFolder}/{exampleFileName}";
            Directory.CreateDirectory($"{TestConfig.AbsolutePathToDtroExamplesTempFolder}");
            var tempFilePath = $"{TestConfig.AbsolutePathToDtroExamplesTempFolder}/{exampleFileName}";
            File.Copy(exampleFilePath, tempFilePath, overwrite: true);

            string json = File.ReadAllText(tempFilePath);
            JObject jsonObj = JObject.Parse(json);
            var tradIdAsInt = int.Parse(traId);
            jsonObj["data"]["Source"]["currentTraOwner"] = tradIdAsInt;
            jsonObj["data"]["Source"]["traAffected"] = new JArray(tradIdAsInt);
            jsonObj["data"]["Source"]["traCreator"] = tradIdAsInt;

            File.WriteAllText(tempFilePath, jsonObj.ToString());
            return tempFilePath;
        }
    }
}