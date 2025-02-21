using System.Collections.Generic;
using Newtonsoft.Json;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities
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
                var deleteSchemaResponse = await DeleteSchemaAsync(schemaVersion);
                Assert.Equal(HttpStatusCode.OK, deleteSchemaResponse.StatusCode);
            }
        }

        private static async Task<HttpResponseMessage> DeleteSchemaAsync(string schemaVersion)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
            };

            var deleteSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Delete, $"{BaseUri}/schemas/{schemaVersion}", headers);
            return deleteSchemaResponse;
        }

        private static async Task DeactivateAllSchemasAsync(List<string> schemaVersions)
        {
            foreach (var version in schemaVersions)
            {
                var deactivateSchemasResponse = await DeactivateSchemaAsync(version);
                Assert.Equal(HttpStatusCode.OK, deactivateSchemasResponse.StatusCode);
            }
        }

        private static async Task<HttpResponseMessage> DeactivateSchemaAsync(string schemaVersion)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
            };

            var deactivateSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Patch, $"{BaseUri}/schemas/deactivate/{schemaVersion}", headers);
            return deactivateSchemaResponse;
        }

        public static async Task<HttpResponseMessage> ActivateSchemaAsync(string schemaVersion)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
            };

            var activateSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Patch, $"{BaseUri}/schemas/activate/{schemaVersion}", headers);
            return activateSchemaResponse;
        }

        public static async Task<List<string>> GetAllSchemaVersionsAsync()
        {
            var getAllSchemasResponse = await GetAllSchemasAsync();
            Assert.Equal(HttpStatusCode.OK, getAllSchemasResponse.StatusCode);
            var allSchemas = await getAllSchemasResponse.Content.ReadAsStringAsync();

            dynamic jsonArrayObject = JsonConvert.DeserializeObject<dynamic>(allSchemas);
            List<string> allSchemaVersions = [];

            foreach (var item in jsonArrayObject)
            {
                allSchemaVersions.Add(item.schemaVersion.ToString());
            }
            return allSchemaVersions;
        }

        public static async Task<HttpResponseMessage> GetAllSchemasAsync()
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
            };

            var getAllSchemasResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{BaseUri}/schemas/versions", headers);
            return getAllSchemasResponse;
        }

        public static async Task CreateAndActivateSchemaAsync()
        {
            var createSchemaResponse = await CreateSchemaFromFileAsync();
            Assert.Equal(HttpStatusCode.Created, createSchemaResponse.StatusCode);

            var activateSchemaResponse = await ActivateSchemaAsync(SchemaVersionUnderTest);
            Assert.Equal(HttpStatusCode.OK, activateSchemaResponse.StatusCode);
        }

        public static async Task<HttpResponseMessage> CreateSchemaFromFileAsync()
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" },
                { "Content-Type", "multipart/form-data" }
            };

            var createSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}/schemas/createFromFile/{SchemaVersionUnderTest}", headers, pathToJsonFile: SchemaJsonFile);
            return createSchemaResponse;
        }

        public static async Task<HttpResponseMessage> GetSchemaAsync(string schemaVersion)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
            };

            var getSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{BaseUri}/schemas/{schemaVersion}", headers);
            return getSchemaResponse;
        }
    }
}