using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.Consts;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.TestConfig;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Extensions;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Enums;

namespace DfT.DTRO.ApiTests.ApiTests.Helpers.DataEntities
{
    public static class DtroUsers
    {
        public static async Task DeleteExistingUsersAsync(TestUser testUser)
        {
            List<string> userIds = await GetAllUserIdsAsync(testUser);
            if (userIds.Count > 0)
            {
                HttpResponseMessage usersDeletionResponse = await DeleteUsersAsync(userIds, testUser);
                string usersDeletionResponseJson = await usersDeletionResponse.Content.ReadAsStringAsync();
                Assert.True(HttpStatusCode.OK == usersDeletionResponse.StatusCode,
                    $"Response JSON:\n\n{usersDeletionResponseJson}");
            }
        }

        public static async Task<HttpResponseMessage> GetAllUsersAsync(TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            await headers.AddValidHeadersForEnvironment(testUser);

            HttpResponseMessage getAllUsersResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{BaseUri}{RouteTemplates.DtroUsersBase}", headers);
            return getAllUsersResponse;
        }

        private static async Task<List<string>> GetAllUserIdsAsync(TestUser testUser)
        {
            HttpResponseMessage usersGetAllResponse = await GetAllUsersAsync(testUser);
            string usersGetAllResponseJson = await usersGetAllResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == usersGetAllResponse.StatusCode,
                $"Response JSON:\n\n{usersGetAllResponseJson}");

            string responseJson = await usersGetAllResponse.Content.ReadAsStringAsync();

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
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            headers.Add("Content-Type", "application/json");

            await headers.AddValidHeadersForEnvironment(testUser);

            JObject requestBody = new JObject { ["ids"] = JArray.FromObject(ids) };

            HttpResponseMessage deleteUsersResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Delete, $"{BaseUri}{RouteTemplates.DtroUsersDeleteRedundant}", headers, requestBody.ToString(Formatting.None));
            return deleteUsersResponse;
        }

        public static async Task<HttpResponseMessage> CreateUserAsync(TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            headers.Add("Content-Type", "application/json");

            await headers.AddValidHeadersForEnvironment(testUser);

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

        public static async Task CreateUserForDataSetUpAsync(TestUser publisher)
        {
            HttpResponseMessage userCreationResponse = await DtroUsers.CreateUserAsync(publisher);
            string userCreationResponseJson = await userCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == userCreationResponse.StatusCode,
                $"Response JSON:\n\n{userCreationResponseJson}");
        }

        public static string GetTraId(TestUserType testUserType)
        {
            switch (testUserType)
            {
                case TestUserType.Publisher1:
                    return TestConfig.PublisherTraId1;
                case TestUserType.Publisher2:
                    return TestConfig.PublisherTraId2;
                case TestUserType.Consumer:
                case TestUserType.Admin:
                    return null;
                default:
                    throw new Exception($"{testUserType} not found!");
            }
        }
    }
}