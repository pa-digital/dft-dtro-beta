namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public static class TestUsers
    {
        public static TestUser GenerateUser(UserGroup userGroup)
        {
            string appId = Guid.NewGuid().ToString();
            string timeNow = DateTime.UtcNow.Ticks.ToString();
            string traId = timeNow.Substring(timeNow.Length - 9);
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

            return new TestUser
            {
                AppId = Guid.NewGuid().ToString(),
                TraId = traId,
                Name = name,
                UserGroup = (int)userGroup
            };
        }
    }

    public class TestUser
    {
        public string AppId { get; init; }
        public string TraId { get; init; }
        public string Name { get; init; }
        public int UserGroup { get; init; }
    }
}