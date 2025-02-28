using System.Collections.Generic;
using Newtonsoft.Json;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities
{
    public static class Schemas
    {
        public static async Task DeleteExistingSchemasAsync(TestUser testUser)
        {
            List<string> allSchemas = await GetAllSchemaVersionsAsync(testUser);
            if (allSchemas.Count == 0)
            {
                return;
            }

            await DeactivateAllSchemasAsync(allSchemas, testUser);
            await DeleteAllSchemasAsync(allSchemas, testUser);
        }

        private static async Task DeleteAllSchemasAsync(List<string> schemaVersions, TestUser testUser)
        {
            foreach (string schemaVersion in schemaVersions)
            {
                HttpResponseMessage deleteSchemaResponse = await DeleteSchemaAsync(schemaVersion, testUser);
                Assert.Equal(HttpStatusCode.OK, deleteSchemaResponse.StatusCode);
            }
        }

        private static async Task<HttpResponseMessage> DeleteSchemaAsync(string schemaVersion, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId }
            };

            HttpResponseMessage deleteSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Delete, $"{BaseUri}/schemas/{schemaVersion}", headers);
            return deleteSchemaResponse;
        }

        private static async Task DeactivateAllSchemasAsync(List<string> schemaVersions, TestUser testUser)
        {
            foreach (string version in schemaVersions)
            {
                HttpResponseMessage deactivateSchemasResponse = await DeactivateSchemaAsync(version, testUser);
                Assert.Equal(HttpStatusCode.OK, deactivateSchemasResponse.StatusCode);
            }
        }

        private static async Task<HttpResponseMessage> DeactivateSchemaAsync(string schemaVersion, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId }
            };

            HttpResponseMessage deactivateSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Patch, $"{BaseUri}/schemas/deactivate/{schemaVersion}", headers);
            return deactivateSchemaResponse;
        }

        public static async Task<HttpResponseMessage> ActivateSchemaAsync(string schemaVersion, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId }
            };

            HttpResponseMessage activateSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Patch, $"{BaseUri}/schemas/activate/{schemaVersion}", headers);
            return activateSchemaResponse;
        }

        public static async Task<List<string>> GetAllSchemaVersionsAsync(TestUser testUser)
        {
            HttpResponseMessage getAllSchemasResponse = await GetAllSchemasAsync(testUser);
            Assert.Equal(HttpStatusCode.OK, getAllSchemasResponse.StatusCode);
            string allSchemas = await getAllSchemasResponse.Content.ReadAsStringAsync();

            dynamic jsonArrayObject = JsonConvert.DeserializeObject<dynamic>(allSchemas);
            List<string> allSchemaVersions = [];

            foreach (dynamic item in jsonArrayObject)
            {
                allSchemaVersions.Add(item.schemaVersion.ToString());
            }
            return allSchemaVersions;
        }

        public static async Task<HttpResponseMessage> GetAllSchemasAsync(TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId }
            };

            HttpResponseMessage getAllSchemasResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{BaseUri}/schemas/versions", headers);
            return getAllSchemasResponse;
        }

        public static async Task CreateAndActivateSchemaAsync(TestUser testUser)
        {
            HttpResponseMessage createSchemaResponse = await CreateSchemaFromFileAsync(testUser);
            Assert.Equal(HttpStatusCode.Created, createSchemaResponse.StatusCode);

            HttpResponseMessage activateSchemaResponse = await ActivateSchemaAsync(SchemaVersionUnderTest, testUser);
            Assert.Equal(HttpStatusCode.OK, activateSchemaResponse.StatusCode);
        }

        public static async Task<HttpResponseMessage> CreateSchemaFromFileAsync(TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId },
                { "Content-Type", "multipart/form-data" }
            };

            HttpResponseMessage createSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}/schemas/createFromFile/{SchemaVersionUnderTest}", headers, pathToJsonFile: SchemaJsonFile);
            return createSchemaResponse;
        }

        public static async Task<HttpResponseMessage> GetSchemaAsync(string schemaVersion, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId }
            };

            HttpResponseMessage getSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{BaseUri}/schemas/{schemaVersion}", headers);
            return getSchemaResponse;
        }
    }
}