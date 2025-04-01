using DfT.DTRO.Consts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Oauth;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Enums;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions
{
    public static class HttpExtensions
    {
        public static async Task AddValidHeadersForEnvironmentAsync(this Dictionary<string, string> headers, UserGroup userGroup, String appId)
        {
            switch (EnvironmentName)
            {
                case EnvironmentType.Local:
                    headers.Add("App-Id", appId);
                    break;
                default:
                    string accessToken = await GetAccessToken(userGroup);
                    headers.Add("Authorization", $"Bearer {accessToken}");
                    break;
            }
        }
    }

}