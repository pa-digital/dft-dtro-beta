using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Enums;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public abstract class BaseTest : IAsyncLifetime
    {
        private static readonly Task _setUpBeforeTestRunAsync = SetUpBeforeTestRunAsync();
        private static TestUser UserWithAllPermissions { get; set; }

        private static async Task SetUpBeforeTestRunAsync()
        {
            Console.WriteLine("Do once before each test run...");
            UserWithAllPermissions = TestUsers.GetUserForTest(UserGroup.All);
            HttpResponseMessage userCreationResponse = await DtroUsers.CreateUserAsync(UserWithAllPermissions);

            string userCreationResponseJson = await userCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == userCreationResponse.StatusCode,
                $"Response JSON:\n\n{userCreationResponseJson}");

            FileHelper.DeleteFilesInDirectory(PathToDtroExamplesTempDirectory);

            if (EnvironmentName == EnvironmentType.Local)
            {
                await DataSetUp.ClearAllDataAsync(UserWithAllPermissions);
                await DataSetUp.CreateRulesAndSchema(UserWithAllPermissions);
            }
            else
            // Only create rules and schema on dev, test, etc., if they don't already exist
            {
                await DataSetUp.CreateRulesAndSchemaIfDoNotExist(UserWithAllPermissions);
            }
        }

        protected BaseTest()
        {
            _setUpBeforeTestRunAsync.Wait();
        }

        public async Task InitializeAsync()
        {
            // If necessary, put logic here to execute before each test
        }

        public Task DisposeAsync()
        {
            Console.WriteLine("Global async teardown after each test...");
            return Task.CompletedTask;
        }
    }
}