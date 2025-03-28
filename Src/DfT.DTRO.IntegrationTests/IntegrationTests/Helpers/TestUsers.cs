#nullable enable
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Enums;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public static class TestUsers
    {
        public static TestUser GetUserForTest(UserGroup userGroup)
        {
            // If we're running the tests on dev, test, integration, the user already exists - we just need to return it
            if (TestConfig.EnvironmentName != EnvironmentType.Local && userGroup == UserGroup.Tra)
            {
                return new TestUser
                {
                    AppId = null,
                    TraId = "8888",
                    Name = null,
                    UserGroup = null
                };
            }
            else
            {
                string appId = Guid.NewGuid().ToString();

                string timeNow = DateTime.UtcNow.Ticks.ToString();
                string first9Digits = timeNow.Substring(timeNow.Length - 9);
                string traId = "9" + first9Digits.Substring(1); // Make sure traId doesn't have leading 0

                string name;

                switch (userGroup)
                {
                    case UserGroup.Tra:
                        name = "A publisher / TRA";
                        break;
                    case UserGroup.Consumer:
                        name = "A consumer";
                        break;
                    case UserGroup.Admin:
                        name = "A CSO / admin";
                        break;
                    case UserGroup.All:
                        name = "All permissions";
                        break;
                    default:
                        throw new Exception($"{userGroup} is not a valid user group!");
                }

                Console.WriteLine();
                Console.WriteLine($"App ID: {appId}");
                Console.WriteLine($"TRA ID: {traId}");
                Console.WriteLine($"Group ID: {(int)userGroup}");

                TestUser testUser = new TestUser
                {
                    AppId = Guid.NewGuid().ToString(),
                    TraId = traId,
                    Name = name,
                    UserGroup = (int)userGroup
                };

                DtroUsers.CreateUserForDataSetUpAsync(testUser);
                return testUser;
            }
        }
    }

    public class TestUser
    {
        public string? AppId { get; init; }
        public required string TraId { get; init; }
        public string? Name { get; init; }
        public int? UserGroup { get; init; }
    }
}