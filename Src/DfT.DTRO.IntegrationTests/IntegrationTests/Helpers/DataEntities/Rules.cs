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

        public static async Task<HttpResponseMessage> CreateRuleSetFromFileAsync(TestUser testUser)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId },
                { "Content-Type", "multipart/form-data" }
            };

            var createRuleResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}/rules/createFromFile/{SchemaVersionUnderTest}", headers, pathToJsonFile: RulesJsonFile);
            return createRuleResponse;
        }

        public static async Task<HttpResponseMessage> GetRuleSetAsync(string schemaVersion, TestUser testUser)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId }
            };

            var getSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{BaseUri}/rules/{schemaVersion}", headers);
            return getSchemaResponse;
        }
    }
}