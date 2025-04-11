using DfT.DTRO.Consts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Consts;
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
                    headers.Add(HttpHeaderKeys.AppId, testUser.AppId);
                    break;
                default:
                    headers.Add(HttpHeaderKeys.Authorization, $"Bearer {testUser.AccessToken}");
                    break;
            }

            return Task.CompletedTask;
        }
    }

}