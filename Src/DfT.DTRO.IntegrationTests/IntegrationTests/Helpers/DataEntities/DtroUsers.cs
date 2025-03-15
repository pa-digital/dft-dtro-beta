using DfT.DTRO.Consts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities
{
    public static class DtroUsers
    {
        public static async Task DeleteExistingUsersAsync(TestUser testUser)
        {
            List<string> userIds = await GetAllUserIdsAsync(testUser);
            if (userIds.Count > 0)
            {
                HttpResponseMessage deleteUsersResponse = await DeleteUsersAsync(userIds, testUser);
                Assert.Equal(HttpStatusCode.OK, deleteUsersResponse.StatusCode);
            }
        }

        public static async Task<HttpResponseMessage> GetAllUsersAsync(TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { RequestHeaderNames.AppId, testUser.AppId }
            };

            HttpResponseMessage getAllUsersResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{BaseUri}{RouteTemplates.DtroUsersBase}", headers);
            return getAllUsersResponse;
        }

        private static async Task<List<string>> GetAllUserIdsAsync(TestUser testUser)
        {
            HttpResponseMessage getAllUsersResponse = await GetAllUsersAsync(testUser);
            Assert.Equal(HttpStatusCode.OK, getAllUsersResponse.StatusCode);

            string responseJson = await getAllUsersResponse.Content.ReadAsStringAsync();

            JArray jsonArray = JArray.Parse(responseJson);

            List<string> ids = new List<string>();
            foreach (JToken obj in jsonArray)
            {
                if (obj["id"] != null)
                {
                    ids.Add(obj["id"].Value<string>());
                }
            }
            return ids;
        }

        public static async Task<HttpResponseMessage> DeleteUsersAsync(List<string> ids, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { RequestHeaderNames.AppId, testUser.AppId },
                { "Content-Type", "application/json" }
            };

            JObject requestBody = new JObject { ["ids"] = JArray.FromObject(ids) };

            HttpResponseMessage deleteUsersResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Delete, $"{BaseUri}{RouteTemplates.DtroUsersDeleteRedundant}", headers, requestBody.ToString(Formatting.None));
            return deleteUsersResponse;
        }

        public static async Task<HttpResponseMessage> CreateUserAsync(TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { RequestHeaderNames.AppId, testUser.AppId },
                { "Content-Type", "application/json" }
            };

            string jsonBody = $$"""
            {
                "id": "26fc4211-fa5b-442b-9978-fda7b1109a3c",
                "traId": {{testUser.TraId}},
                "name": "{{testUser.Name}}",
                "prefix": "AB",
                "userGroup": {{testUser.UserGroup}},
                "appId": "{{testUser.AppId}}"
            }
            """;

            HttpResponseMessage createUserResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}{RouteTemplates.DtroUsersCreateFromBody}", headers, jsonBody);
            return createUserResponse;
        }
    }
}