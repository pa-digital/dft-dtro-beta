using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities
{
    public static class DtroUsers
    {
        public static async Task DeleteExistingUsersAsync()
        {
            var userIds = await GetAllUserIdsAsync();
            if (userIds.Count > 0)
            {
                var response = await DeleteUsersAsync(userIds);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        public static async Task<HttpResponseMessage> GetAllUsersAsync()
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
            };

            var response = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{TestConfig.BaseUri}/dtroUsers", headers);
            return response;
        }

        public static async Task<List<string>> GetAllUserIdsAsync()
        {
            var response = await GetAllUsersAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            string responseJson = await response.Content.ReadAsStringAsync();

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

        public static async Task<HttpResponseMessage> DeleteUsersAsync(List<string> ids)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", "f553d1ec-a7ca-43d2-b714-60dacbb4d005" },
                { "Content-Type", "application/json" }
            };

            var requestBody = new JObject { ["ids"] = JArray.FromObject(ids) };

            var response = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Delete, $"{TestConfig.BaseUri}/dtroUsers/redundant", headers, requestBody.ToString(Formatting.None));
            return response;
        }

        public static async Task<HttpResponseMessage> CreateUserAsync(UserDetails userDetails)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", userDetails.AppId },
                { "Content-Type", "application/json" }
            };

            var jsonBody = $$"""
            {
                "id": "26fc4211-fa5b-442b-9978-fda7b1109a3c",
                "traId": {{userDetails.TraId}},
                "name": "{{userDetails.Name}}",
                "prefix": "AB",
                "userGroup": {{userDetails.UserGroup}},
                "xAppId": "{{userDetails.AppId}}"
            }
            """;

            var response = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{TestConfig.BaseUri}/dtroUsers/createFromBody", headers, jsonBody);
            return response;
        }
    }
}