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

        public static async Task<HttpResponseMessage> CreateDtroAsync(UserDetails userDetails, string exampleFileName)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", userDetails.AppId },
                { "Content-Type", "multipart/form-data" }
            };

            var tempFilePath = GetDtroFileWithTraUpdated(exampleFileName, userDetails.TraId);
            var createDtroResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}/dtros/createFromFile", headers, pathToJsonFile: tempFilePath);
            return createDtroResponse;
        }

        private static string GetDtroFileWithTraUpdated(string exampleFileName, string traId)
        {
            var exampleFilePath = $"{AbsolutePathToDtroExamplesDirectory}/{exampleFileName}";
            Directory.CreateDirectory($"{AbsolutePathToDtroExamplesTempDirectory}");
            var tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{exampleFileName}";
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