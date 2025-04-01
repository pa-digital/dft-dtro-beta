using System.Threading;
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
            UserWithAllPermissions = await TestUsers.GetUser(UserGroup.All);
            HttpResponseMessage userCreationResponse = await DtroUsers.CreateUserAsync(UserWithAllPermissions);

            string userCreationResponseJson = await userCreationResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.Created == userCreationResponse.StatusCode,
                $"Response JSON:\n\n{userCreationResponseJson}");

            FileHelper.DeleteFilesInDirectory(PathToDtroExamplesTempDirectory);

            await Dtros.PrintDtroCountAsync(UserWithAllPermissions, "DTRO count before any data is deleted or any tests run");

            // Only delete data on local machine
            if (EnvironmentName == EnvironmentType.Local)
            {
                await DataSetUp.ClearAllDataAsync(UserWithAllPermissions);
                await DataSetUp.CreateRulesAndSchema(UserWithAllPermissions);
                await Dtros.PrintDtroCountAsync(UserWithAllPermissions, "DTRO count after data is deleted (locally) and before tests are run");
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

        public async Task DisposeAsync()
        {
            Console.WriteLine("Global async teardown after each test...");

            await Dtros.PrintDtroCountAsync(UserWithAllPermissions, "DTRO count after each test");


            // To avoid hitting the rate limit in dev / test / integration, we need to wait before executing the next test
            if (EnvironmentName != EnvironmentType.Local)
            {
                Thread.Sleep(10000);
            }

            await Task.CompletedTask;
        }
    }
}