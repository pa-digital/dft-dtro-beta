using System.Linq;
using System.Threading.Tasks;
using DfT.DTRO.Consts;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using DfT.DTRO.Models.DataBase;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public static class Oauth
    {
        public static async Task<string> GetAccessToken(UserGroup userGroup)
        {
            string clientId;
            string clientSecret;

            switch (userGroup)
            {
                case UserGroup.Admin:
                    clientId = TestConfig.AdminClientId;
                    clientSecret = TestConfig.AdminClientSecret;
                    break;
                case UserGroup.Tra:
                    clientId = TestConfig.PublisherClientId;
                    clientSecret = TestConfig.PublisherClientSecret;
                    break;
                case UserGroup.Consumer:
                    clientId = TestConfig.ConsumerClientId;
                    clientSecret = TestConfig.ConsumerClientSecret;
                    break;
                default:
                    throw new Exception($"{userGroup} not found!");
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