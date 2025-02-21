using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Enums;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public abstract class BaseTest : IAsyncLifetime
    {
        private static readonly Task _setUpBeforeTestRunAsync = SetUpBeforeTestRunAsync();

        private static async Task SetUpBeforeTestRunAsync()
        {
            Console.WriteLine("Do once before each test run...");
            if (EnvironmentName == EnvironmentType.Local)
            {
                await DataSetUp.ClearAllDataAsync();
                await DataSetUp.CreateRulesAndSchema();
            }
            else
            // Only create rules and schema on dev, test, etc., if they don't already exist
            {
                var getRulesResponse = await Rules.GetRuleSetAsync(SchemaVersionUnderTest);
                if (getRulesResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    var createRuleResponse = await Rules.CreateRuleSetFromFileAsync();
                    Assert.Equal(HttpStatusCode.Created, createRuleResponse.StatusCode);
                }

                var getSchemaResponse = await Schemas.GetSchemaAsync(SchemaVersionUnderTest);
                if (getSchemaResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    var createSchemaResponse = await Schemas.CreateSchemaFromFileAsync();
                    Assert.Equal(HttpStatusCode.Created, createSchemaResponse.StatusCode);
                }
                var activateSchemaResponse = await Schemas.ActivateSchemaAsync(SchemaVersionUnderTest);
                Assert.Equal(HttpStatusCode.OK, activateSchemaResponse.StatusCode);
            }
        }

        protected BaseTest()
        {
            _setUpBeforeTestRunAsync.Wait();
        }

        public async Task InitializeAsync()
        {
            Console.WriteLine("Do before before each test...");
            if (EnvironmentName == EnvironmentType.Local)
            {
                await DtroUsers.DeleteExistingUsersAsync();
                Dtros.DeleteExistingDtros();
            }
        }

        public Task DisposeAsync()
        {
            Console.WriteLine("Global async teardown after each test...");
            return Task.CompletedTask;
        }
    }
}
