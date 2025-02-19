using System.Collections.Generic;
using Newtonsoft.Json;

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
                var response = await DeleteSchemaAsync(schemaVersion);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        private static async Task<HttpResponseMessage> DeleteSchemaAsync(string schemaVersion)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
            };

            var response = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Delete, $"{TestConfig.BaseUri}/schemas/{schemaVersion}", headers);
            return response;
        }

        private static async Task DeactivateAllSchemasAsync(List<string> schemaVersions)
        {
            foreach (var version in schemaVersions)
            {
                var response = await DeactivateSchemaAsync(version);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        private static async Task<HttpResponseMessage> DeactivateSchemaAsync(string schemaVersion)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
            };

            var response = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Patch, $"{TestConfig.BaseUri}/schemas/deactivate/{schemaVersion}", headers);
            return response;
        }

        private static async Task<HttpResponseMessage> ActivateSchemaAsync(string schemaVersion)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
            };

            var response = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Patch, $"{TestConfig.BaseUri}/schemas/activate/{schemaVersion}", headers);
            return response;
        }

        private static async Task<List<string>> GetAllSchemaVersionsAsync()
        {
            var response = await GetAllSchemasAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var allSchemas = await response.Content.ReadAsStringAsync();

            dynamic jsonArrayObject = JsonConvert.DeserializeObject<dynamic>(allSchemas);
            List<string> allSchemaVersions = [];

            foreach (var item in jsonArrayObject)
            {
                allSchemaVersions.Add(item.schemaVersion.ToString());
            }
            return allSchemaVersions;
        }

        private static async Task<HttpResponseMessage> GetAllSchemasAsync()
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
            };

            var response = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{TestConfig.BaseUri}/schemas/versions", headers);
            return response;
        }

        public static async Task CreateAndActivateSchemaAsync()
        {
            var createSchemaResponse = await CreateSchemaFromFileAsync();
            Assert.Equal(HttpStatusCode.Created, createSchemaResponse.StatusCode);

            var activateSchemaResponse = await ActivateSchemaAsync(TestConfig.SchemaVersionUnderTest);
            Assert.Equal(HttpStatusCode.OK, activateSchemaResponse.StatusCode);
        }

        public static async Task<HttpResponseMessage> CreateSchemaFromFileAsync()
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" },
                { "Content-Type", "multipart/form-data" }
            };

            var filePath = TestConfig.SchemaJsonFile;

            var response = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{TestConfig.BaseUri}/schemas/createFromFile/{TestConfig.SchemaVersionUnderTest}", headers, pathToJsonFile: filePath);
            return response;
        }
    }
}