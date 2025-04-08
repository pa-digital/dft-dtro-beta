using DfT.DTRO.Consts;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities
{
    public static class Rules
    {
        public static void DeleteExistingRules()
        {
            SqlQueries.TruncateTable("RuleTemplate");
        }

        public static async Task<HttpResponseMessage> CreateRuleSetFromFileAsync(string schemaVersion, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            headers.Add("Content-Type", "multipart/form-data");

            await headers.AddValidHeadersForEnvironment(testUser);

            HttpResponseMessage createRuleResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}{RouteTemplates.RulesBase}/createFromFile/{schemaVersion}", headers, pathToJsonFile: $"{PathToRuleExamplesDirectory}/rules-{schemaVersion}.json");
            return createRuleResponse;
        }

        public static async Task<HttpResponseMessage> GetRuleSetAsync(string schemaVersion, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            await headers.AddValidHeadersForEnvironment(testUser);

            HttpResponseMessage getSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{BaseUri}{RouteTemplates.RulesBase}/{schemaVersion}", headers);
            return getSchemaResponse;
        }
    }
}