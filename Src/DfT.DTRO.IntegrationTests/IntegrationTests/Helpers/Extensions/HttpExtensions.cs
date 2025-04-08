using DfT.DTRO.Consts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Enums;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions
{
    public static class HttpExtensions
    {
        public static Task AddValidHeadersForEnvironment(this Dictionary<string, string> headers, TestUser testUser)
        {
            switch (EnvironmentName)
            {
                case EnvironmentType.Local:
                    headers.Add("App-Id", testUser.AppId);
                    break;
                default:
                    headers.Add("Authorization", $"Bearer {testUser.AccessToken}");
                    break;
            }

            return Task.CompletedTask;
        }
    }

}