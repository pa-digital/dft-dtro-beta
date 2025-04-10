using System.Linq;
using System.Threading.Tasks;
using DfT.DTRO.Consts;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Enums;
using DfT.DTRO.ApiTests.ApiTests.Helpers.JsonHelpers;
using DfT.DTRO.Models.DataBase;

namespace DfT.DTRO.ApiTests.ApiTests.Helpers
{
    public static class Oauth
    {
        public static async Task<string> GetAccessToken(TestUserType testUserType)
        {
            string clientId;
            string clientSecret;

            switch (testUserType)
            {
                case TestUserType.Publisher1:
                    clientId = TestConfig.PublisherClientId1;
                    clientSecret = TestConfig.PublisherClientSecret1;
                    break;
                case TestUserType.Publisher2:
                    clientId = TestConfig.PublisherClientId2;
                    clientSecret = TestConfig.PublisherClientSecret2;
                    break;
                case TestUserType.Consumer:
                    clientId = TestConfig.ConsumerClientId;
                    clientSecret = TestConfig.ConsumerClientSecret;
                    break;
                case TestUserType.Admin:
                    clientId = TestConfig.AdminClientId;
                    clientSecret = TestConfig.AdminClientSecret;
                    break;
                default:
                    throw new Exception($"{testUserType} not found!");
            }

            string credentials = $"{clientId}:{clientSecret}";
            string base64EncodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            headers.Add("Content-Type", "application/x-www-form-urlencoded");
            headers.Add("Authorization", $"Basic {base64EncodedCredentials}");

            KeyValuePair<string, string> formUrlEncodedBody = new KeyValuePair<string, string>("grant_type", "client_credentials");

            // Make sure to set printCurl to false so we don't leak any secrets
            HttpResponseMessage oauthResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{TestConfig.BaseUri}/oauth-generator", headers, formUrlEncodedBody: formUrlEncodedBody, printCurl: false);
            string responseJson = await oauthResponse.Content.ReadAsStringAsync();
            string token = JsonMethods.GetValueAtJsonPath(responseJson, "access_token").ToString();
            return token;
        }
    }
}