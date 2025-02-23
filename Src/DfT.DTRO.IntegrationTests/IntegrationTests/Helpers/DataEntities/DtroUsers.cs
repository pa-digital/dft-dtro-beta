using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities
{
    public static class DtroUsers
    {
        public static async Task DeleteExistingUsersAsync(TestUser testUser)
        {
            var userIds = await GetAllUserIdsAsync(testUser);
            if (userIds.Count > 0)
            {
                var deleteUsersResponse = await DeleteUsersAsync(userIds, testUser);
                Assert.Equal(HttpStatusCode.OK, deleteUsersResponse.StatusCode);
            }
        }

        public static async Task<HttpResponseMessage> GetAllUsersAsync(TestUser testUser)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId }
            };

            var getAllUsersResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{BaseUri}/dtroUsers", headers);
            return getAllUsersResponse;
        }

        public static async Task<List<string>> GetAllUserIdsAsync(TestUser testUser)
        {
            var getAllUsersResponse = await GetAllUsersAsync(testUser);
            Assert.Equal(HttpStatusCode.OK, getAllUsersResponse.StatusCode);

            string responseJson = await getAllUsersResponse.Content.ReadAsStringAsync();

            JArray jsonArray = JArray.Parse(responseJson);

            var ids = new List<string>();
            foreach (var obj in jsonArray)
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
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId },
                { "Content-Type", "application/json" }
            };

            var requestBody = new JObject { ["ids"] = JArray.FromObject(ids) };

            var deleteUsersResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Delete, $"{BaseUri}/dtroUsers/redundant", headers, requestBody.ToString(Formatting.None));
            return deleteUsersResponse;
        }

        public static async Task<HttpResponseMessage> CreateUserAsync(TestUser testUser)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId },
                { "Content-Type", "application/json" }
            };

            var jsonBody = $$"""
            {
                "id": "26fc4211-fa5b-442b-9978-fda7b1109a3c",
                "traId": {{testUser.TraId}},
                "name": "{{testUser.Name}}",
                "prefix": "AB",
                "userGroup": {{testUser.UserGroup}},
                "xAppId": "{{testUser.AppId}}"
            }
            """;

            var createUserResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}/dtroUsers/createFromBody", headers, jsonBody);
            return createUserResponse;
        }
    }
}