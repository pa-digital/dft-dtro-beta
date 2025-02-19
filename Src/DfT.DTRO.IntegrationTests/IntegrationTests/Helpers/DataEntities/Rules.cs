using System.Collections.Generic;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities
{
    public static class Rules
    {
        public static void DeleteExistingRules()
        {
            SqlQueries.TruncateTable("RuleTemplate");
        }

        public static async Task<HttpResponseMessage> CreateRuleSetFromFileAsync()
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" },
                { "Content-Type", "multipart/form-data" }
            };

            var filePath = TestConfig.RulesJsonFile;

            var response = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{TestConfig.BaseUri}/rules/createFromFile/{TestConfig.SchemaVersionUnderTest}", headers, pathToJsonFile: filePath);
            return response;
        }
    }
}