using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using DfT.DTRO.Consts;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;

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
                HttpResponseMessage schemaDeletionResponse = await DeleteSchemaAsync(schemaVersion, testUser);
                string schemaDeletionResponseJson = await schemaDeletionResponse.Content.ReadAsStringAsync();
                Assert.True(HttpStatusCode.OK == schemaDeletionResponse.StatusCode,
                    $"Response JSON:\n\n{schemaDeletionResponseJson}");
            }
        }

        private static async Task<HttpResponseMessage> DeleteSchemaAsync(string schemaVersion, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            await headers.AddValidHeadersForEnvironmentAsync(UserGroup.Admin, testUser.AppId);

            HttpResponseMessage deleteSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Delete, $"{BaseUri}{RouteTemplates.SchemasBase}/{schemaVersion}", headers);
            return deleteSchemaResponse;
        }

        private static async Task DeactivateAllSchemasAsync(List<string> schemaVersions, TestUser testUser)
        {
            foreach (string version in schemaVersions)
            {
                HttpResponseMessage schemaDeactivationResponse = await DeactivateSchemaAsync(version, testUser);
                string schemaDeactivationResponseJson = await schemaDeactivationResponse.Content.ReadAsStringAsync();
                Assert.True(HttpStatusCode.OK == schemaDeactivationResponse.StatusCode,
                    $"Response JSON:\n\n{schemaDeactivationResponseJson}");
            }
        }

        private static async Task<HttpResponseMessage> DeactivateSchemaAsync(string schemaVersion, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            await headers.AddValidHeadersForEnvironmentAsync(UserGroup.Admin, testUser.AppId);

            HttpResponseMessage deactivateSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Patch, $"{BaseUri}{RouteTemplates.SchemasBase}/deactivate/{schemaVersion}", headers);
            return deactivateSchemaResponse;
        }

        public static async Task<HttpResponseMessage> ActivateSchemaAsync(string schemaVersion, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            await headers.AddValidHeadersForEnvironmentAsync(UserGroup.Admin, testUser.AppId);

            HttpResponseMessage activateSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Patch, $"{BaseUri}{RouteTemplates.SchemasBase}/activate/{schemaVersion}", headers);
            return activateSchemaResponse;
        }

        public static async Task<List<string>> GetAllSchemaVersionsAsync(TestUser testUser)
        {
            HttpResponseMessage schemasGetAllResponse = await GetAllSchemasAsync(testUser);
            string schemaActivationResponseJson = await schemasGetAllResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == schemasGetAllResponse.StatusCode,
                $"Response JSON:\n\n{schemaActivationResponseJson}");

            dynamic jsonArrayObject = JsonConvert.DeserializeObject<dynamic>(schemaActivationResponseJson);
            List<string> allSchemaVersions = [];

            foreach (dynamic item in jsonArrayObject)
            {
                allSchemaVersions.Add(item.schemaVersion.ToString());
            }
            return allSchemaVersions;
        }

        public static async Task<HttpResponseMessage> GetAllSchemasAsync(TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            await headers.AddValidHeadersForEnvironmentAsync(UserGroup.Admin, testUser.AppId);

            HttpResponseMessage getAllSchemasResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{BaseUri}{RouteTemplates.SchemasFindVersions}", headers);
            return getAllSchemasResponse;
        }

        public static async Task CreateAndActivateSchemaAsync(string schemaVersion, TestUser testUser)
        {
            HttpResponseMessage schemaCreationResponse = await CreateSchemaFromFileAsync(schemaVersion, testUser);
            string schemaCreationResponseJson = await schemaCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == schemaCreationResponse.StatusCode,
                $"Response JSON:\n\n{schemaCreationResponseJson}");

            HttpResponseMessage schemaActivationResponse = await ActivateSchemaAsync(schemaVersion, testUser);
            string schemaActivationResponseJson = await schemaActivationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == schemaActivationResponse.StatusCode,
                $"Response JSON:\n\n{schemaActivationResponseJson}");
        }

        public static async Task<HttpResponseMessage> CreateSchemaFromFileAsync(string schemaVersion, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            headers.Add("Content-Type", "multipart/form-data");

            await headers.AddValidHeadersForEnvironmentAsync(UserGroup.Admin, testUser.AppId);

            HttpResponseMessage createSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}{RouteTemplates.SchemasBase}/createFromFile/{schemaVersion}", headers, pathToJsonFile: $"{PathToSchemaExamplesDirectory}/schemas-{schemaVersion}.json");
            return createSchemaResponse;
        }

        public static async Task<HttpResponseMessage> GetSchemaAsync(string schemaVersion, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            await headers.AddValidHeadersForEnvironmentAsync(UserGroup.Admin, testUser.AppId);

            HttpResponseMessage getSchemaResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{BaseUri}{RouteTemplates.SchemasBase}/{schemaVersion}", headers);
            return getSchemaResponse;
        }

        public static string[] GetSchemaVersions(string[] schemaFiles)
        {
            if (schemaFiles == null || schemaFiles.Length == 0)
            {
                throw new Exception("No schema files available!");
            }

            return schemaFiles
                .Select(fileName =>
                {
                    Match match = Regex.Match(fileName, @"schemas-(\d+\.\d+\.\d+)\.json");
                    if (match.Success)
                    {
                        return match.Groups[1].Value;
                    }
                    else
                    {
                        throw new Exception($"{fileName} doesn't contain schema version!");
                    }
                })
                .ToArray();
        }
    }
}