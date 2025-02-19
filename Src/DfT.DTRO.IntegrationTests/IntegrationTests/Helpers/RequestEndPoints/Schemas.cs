using System.Collections.Generic;
using Newtonsoft.Json;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.RequestEndPoints
{
    public static class Schemas
    {
        public static async Task DeleteExistingSchemasAsync()
        {
            var allSchemas = await GetAllSchemaVersionsAsync();
            if (allSchemas.Count == 0)
            {
                return;
            }

            await DeactivateAllSchemasAsync(allSchemas);
            await DeleteAllSchemasAsync(allSchemas);
        }

        private static async Task DeleteAllSchemasAsync(List<string> schemaVersions)
        {
            foreach (var schemaVersion in schemaVersions)
            {
                await DeleteSchemaAsync(schemaVersion);
            }
        }

        private static async Task DeleteSchemaAsync(string schemaVersion)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
            };

            var response = await HttpRequestHelpers.MakeHttpRequestAsync(HttpMethod.Delete, $"{TestConfig.BaseUri}/schemas/{schemaVersion}", headers);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private static async Task DeactivateAllSchemasAsync(List<string> schemaVersions)
        {
            foreach (var version in schemaVersions)
            {
                await DeactivateSchemaAsync(version);
            }
        }

        private static async Task DeactivateSchemaAsync(string schemaVersion)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
            };

            var response = await HttpRequestHelpers.MakeHttpRequestAsync(HttpMethod.Patch, $"{TestConfig.BaseUri}/schemas/deactivate/{schemaVersion}", headers);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private static async Task ActivateSchemaAsync(string schemaVersion)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
            };

            var response = await HttpRequestHelpers.MakeHttpRequestAsync(HttpMethod.Patch, $"{TestConfig.BaseUri}/schemas/activate/{schemaVersion}", headers);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private static async Task<List<string>> GetActiveSchemaVersionsAsync()
        {
            var allSchemas = await GetAllSchemasAsync();

            dynamic jsonArrayObject = JsonConvert.DeserializeObject<dynamic>(allSchemas);
            List<string> activeSchemaVersions = [];

            foreach (var item in jsonArrayObject)
            {
                if ((bool)item.isActive)
                {
                    activeSchemaVersions.Add(item.schemaVersion.ToString());
                }
            }
            return activeSchemaVersions;
        }

        private static async Task<List<string>> GetAllSchemaVersionsAsync()
        {
            var allSchemas = await GetAllSchemasAsync();

            dynamic jsonArrayObject = JsonConvert.DeserializeObject<dynamic>(allSchemas);
            List<string> allSchemaVersions = [];

            foreach (var item in jsonArrayObject)
            {
                allSchemaVersions.Add(item.schemaVersion.ToString());
            }
            return allSchemaVersions;
        }

        private static async Task<string> GetAllSchemasAsync()
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
            };

            var response = await HttpRequestHelpers.MakeHttpRequestAsync(HttpMethod.Get, $"{TestConfig.BaseUri}/schemas/versions", headers);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task CreateAndActivateSchemaAsync()
        {
            await CreateSchemaFromFileAsync();
            await ActivateSchemaAsync(TestConfig.SchemaVersionUnderTest);
        }

        public static async Task CreateSchemaFromFileAsync()
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" },
                { "Content-Type", "multipart/form-data" }
            };

            var filePath = $"{TestConfig.AbsolutePathToExamplesFolder}/Schemas/schemas-{TestConfig.SchemaVersionUnderTest}.json";

            var response = await HttpRequestHelpers.MakeHttpRequestAsync(HttpMethod.Post, $"{TestConfig.BaseUri}/schemas/createFromFile/{TestConfig.SchemaVersionUnderTest}", headers, pathToJsonFile: filePath);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}