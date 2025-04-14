#nullable enable
using System.Threading.Tasks;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Enums;

namespace DfT.DTRO.ApiTests.ApiTests.Helpers
{
    public static class TestUsers
    {
        public static async Task<TestUser> GetUser(TestUserType testUserType)
        {
            // If we're running the tests on dev / test / integration, the user already exists - we just need get the TRA ID (if relevant) and return an access token
            if (TestConfig.EnvironmentName != EnvironmentType.Local)
            {
                return new TestUser
                {
                    LocalUser = false,
                    AppId = null,
                    AccessToken = await Oauth.GetAccessToken(testUserType),
                    TraId = DtroUsers.GetTraId(testUserType),
                    Name = null,
                    UserGroup = null
                };
            }
            else
            {
                string appId = Guid.NewGuid().ToString();

                string timeNow = DateTime.UtcNow.Ticks.ToString();
                string first9Digits = timeNow.Substring(timeNow.Length - 9);
                int traId = int.Parse("9" + first9Digits.Substring(1)); // Make sure traId doesn't have leading 0

                string name;
                int userGroup;

                switch (testUserType)
                {
                    case TestUserType.Publisher1:
                    case TestUserType.Publisher2:
                        name = "A publisher / TRA";
                        userGroup = 1;
                        break;
                    case TestUserType.Consumer:
                        name = "A consumer";
                        userGroup = 2;
                        break;
                    case TestUserType.Admin:
                        name = "A CSO / admin";
                        userGroup = 3;
                        break;
                    case TestUserType.AllPermissions:
                        name = "User with all permissions";
                        userGroup = 0;
                        break;
                    default:
                        throw new Exception($"{testUserType} is not a valid test user type!");
                }

                Console.WriteLine();
                Console.WriteLine($"App ID: {appId}");
                Console.WriteLine($"TRA ID: {traId}");
                Console.WriteLine($"Group ID: {userGroup}");

                TestUser testUser = new TestUser
                {
                    LocalUser = true,
                    AppId = Guid.NewGuid().ToString(),
                    AccessToken = null,
                    TraId = traId,
                    Name = name,
                    UserGroup = userGroup
                };

                await DtroUsers.CreateUserForDataSetUpAsync(testUser);
                return testUser;
            }
        }
    }

    public class TestUser
    {
        public required bool LocalUser { get; init; }
        public string? AppId { get; init; }
        public string? AccessToken { get; init; }
        public int? TraId { get; init; }
        public string? Name { get; init; }
        public int? UserGroup { get; init; }
    }
}