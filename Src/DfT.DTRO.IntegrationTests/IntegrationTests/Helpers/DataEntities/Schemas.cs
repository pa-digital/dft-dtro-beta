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
                var deleteSchemaResponse = await DeleteSchemaAsync(schemaVersion, User.Publisher);
                Assert.Equal(HttpStatusCode.OK, deleteSchemaResponse.StatusCode);
            }
        }

        private static async Task<HttpResponseMessage> DeleteSchemaAsync(string schemaVersion, UserDetails userDetails)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", userDetails.AppId }
            };

            var deleteSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Delete, $"{BaseUri}/schemas/{schemaVersion}", headers);
            return deleteSchemaResponse;
        }

        private static async Task DeactivateAllSchemasAsync(List<string> schemaVersions)
        {
            foreach (var version in schemaVersions)
            {
                var deactivateSchemasResponse = await DeactivateSchemaAsync(version, User.Publisher);
                Assert.Equal(HttpStatusCode.OK, deactivateSchemasResponse.StatusCode);
            }
        }

        private static async Task<HttpResponseMessage> DeactivateSchemaAsync(string schemaVersion, UserDetails userDetails)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", userDetails.AppId }
            };

            var deactivateSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Patch, $"{BaseUri}/schemas/deactivate/{schemaVersion}", headers);
            return deactivateSchemaResponse;
        }

        public static async Task<HttpResponseMessage> ActivateSchemaAsync(string schemaVersion, UserDetails userDetails)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", userDetails.AppId }
            };

            var activateSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Patch, $"{BaseUri}/schemas/activate/{schemaVersion}", headers);
            return activateSchemaResponse;
        }

        public static async Task<List<string>> GetAllSchemaVersionsAsync()
        {
            var getAllSchemasResponse = await GetAllSchemasAsync(User.Publisher);
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

        public static async Task<HttpResponseMessage> GetAllSchemasAsync(UserDetails userDetails)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", userDetails.AppId }
            };

            var getAllSchemasResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{BaseUri}/schemas/versions", headers);
            return getAllSchemasResponse;
        }

        public static async Task CreateAndActivateSchemaAsync()
        {
            var createSchemaResponse = await CreateSchemaFromFileAsync(User.Publisher);
            Assert.Equal(HttpStatusCode.Created, createSchemaResponse.StatusCode);

            var activateSchemaResponse = await ActivateSchemaAsync(SchemaVersionUnderTest, User.Publisher);
            Assert.Equal(HttpStatusCode.OK, activateSchemaResponse.StatusCode);
        }

        public static async Task<HttpResponseMessage> CreateSchemaFromFileAsync(UserDetails userDetails)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", userDetails.AppId },
                { "Content-Type", "multipart/form-data" }
            };

            var createSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}/schemas/createFromFile/{SchemaVersionUnderTest}", headers, pathToJsonFile: SchemaJsonFile);
            return createSchemaResponse;
        }

        public static async Task<HttpResponseMessage> GetSchemaAsync(string schemaVersion, UserDetails userDetails)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", userDetails.AppId }
            };

            var getSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{BaseUri}/schemas/{schemaVersion}", headers);
            return getSchemaResponse;
        }
    }
}