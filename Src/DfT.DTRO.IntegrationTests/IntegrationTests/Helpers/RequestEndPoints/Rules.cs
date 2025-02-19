using System.Collections.Generic;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.RequestEndPoints
{
    public static class Rules
    {
        public static void DeleteExistingRules()
        {
            SqlQueries.TruncateTable("RuleTemplate");
        }

        public static async Task CreateRuleSetFromFileAsync()
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" },
                { "Content-Type", "multipart/form-data" }
            };

            var filePath = $"{TestConfig.AbsolutePathToExamplesFolder}/Rules/rules-{TestConfig.SchemaVersionUnderTest}.json";

            var response = await HttpRequestHelpers.MakeHttpRequestAsync(HttpMethod.Post, $"{TestConfig.BaseUri}/rules/createFromFile/{TestConfig.SchemaVersionUnderTest}", headers, pathToJsonFile: filePath);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}