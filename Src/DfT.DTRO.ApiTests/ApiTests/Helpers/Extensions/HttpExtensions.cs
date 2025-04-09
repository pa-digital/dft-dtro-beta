using DfT.DTRO.Consts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Enums;
using DfT.DTRO.ApiTests.ApiTests.Helpers;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.TestConfig;

namespace DfT.DTRO.ApiTests.ApiTests.Helpers.Extensions
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