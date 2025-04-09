using System.Threading;
using DfT.DTRO.ApiTests.ApiTests.Helpers.Enums;
using Xunit;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.TestConfig;

namespace DfT.DTRO.ApiTests.ApiTests.Helpers
{
    public abstract class BaseTest : IAsyncLifetime
    {
        private static readonly Task _setUpBeforeTestRunAsync = SetUpBeforeTestRunAsync();
        private static TestUser AdminTestUser { get; set; }

        private static async Task SetUpBeforeTestRunAsync()
        {
            Console.WriteLine("Do once before each test run...");
            AdminTestUser = await TestUsers.GetUser(TestUserType.Admin);

            FileHelper.DeleteFilesInDirectory(PathToDtroExamplesTempDirectory);

            // Only hit /count end point locally for the time being - because it's not yet been exposed via Apigee
            if (EnvironmentName == EnvironmentType.Local)
            {
                await Dtros.PrintDtroCountAsync(AdminTestUser, "DTRO count before any data is deleted or any tests run");
            }

            // Only delete data on local machine
            if (EnvironmentName == EnvironmentType.Local)
            {
                await DataSetUp.ClearAllDataAsync(AdminTestUser);
                await DataSetUp.CreateRulesAndSchema(AdminTestUser);
                await Dtros.PrintDtroCountAsync(AdminTestUser, "DTRO count after data is deleted (locally) and before tests are run");
            }
            else
            // Only create rules and schema on dev, test, etc., if they don't already exist
            {
                await DataSetUp.CreateRulesAndSchemaIfDoNotExist(AdminTestUser);
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

            // Only hit /count end point locally for the time being - because it's not yet been exposed via Apigee
            if (EnvironmentName == EnvironmentType.Local)
            {
                await Dtros.PrintDtroCountAsync(AdminTestUser, "DTRO count after each test");
            }

            // To avoid hitting the rate limit in dev / test / integration, we need to wait before executing the next test
            if (EnvironmentName != EnvironmentType.Local)
            {
                Thread.Sleep(10000);
            }

            await Task.CompletedTask;
        }
    }
}