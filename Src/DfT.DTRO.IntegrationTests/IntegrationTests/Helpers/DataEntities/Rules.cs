using System.Collections.Generic;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

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

            var createRuleResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}/rules/createFromFile/{SchemaVersionUnderTest}", headers, pathToJsonFile: RulesJsonFile);
            return createRuleResponse;
        }

        public static async Task<HttpResponseMessage> GetRuleSetAsync(string schemaVersion)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
            };

            var getSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{BaseUri}/rules/{schemaVersion}", headers);
            return getSchemaResponse;
        }
    }
}