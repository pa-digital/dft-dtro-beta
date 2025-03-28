using System.Linq;
using System.Threading.Tasks;
using DfT.DTRO.Consts;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using DfT.DTRO.Models.DataBase;
using DotNetEnv;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public static class Oauth
    {
        public static async Task<string> GetAccessToken(UserGroup userGroup)
        {
            // string envFilePath = $"{TestConfig.PathToProjectDirectory}/docker/dev/.env";

            // string[] lines = File.ReadAllLines(envFilePath);
            // string clientIdLine = lines.First(line => line.StartsWith("CLIENT_ID="));
            // string clientId = clientIdLine.Split('=')[1];
            // string clientSecretLine = lines.First(line => line.StartsWith("CLIENT_SECRET="));
            // string clientSecret = clientSecretLine.Split('=')[1];

            string clientId;
            string clientSecret;

            switch (userGroup)
            {
                case UserGroup.Admin:
                    clientId = FileHelper.GetValueFromDotEnv("CLIENT_ID_CSO_DEV");
                    clientSecret = FileHelper.GetValueFromDotEnv("CLIENT_SECRET_CSO_DEV");
                    break;
                case UserGroup.Tra:
                    clientId = FileHelper.GetValueFromDotEnv("CLIENT_ID_PUBLISHER_DEV");
                    clientSecret = FileHelper.GetValueFromDotEnv("CLIENT_SECRET_PUBLISHER_DEV");
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

            HttpResponseMessage oauthResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{TestConfig.BaseUri}/oauth-generator", headers, formUrlEncodedBody: formUrlEncodedBody);
            string responseJson = await oauthResponse.Content.ReadAsStringAsync();
            string token = JsonMethods.GetValueAtJsonPath(responseJson, "access_token").ToString();
            return token;
        }
    }
}