namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public abstract class BaseTest : IAsyncLifetime
    {
        public async Task InitializeAsync()
        {
            // Runs before each test (inheriting classes)
            Console.WriteLine("Global async setup before each test...");
            await DataSetUp.ClearAllDataAsync();

            // Create rules and schema
            // TODO: Only run once before each test run (not before each test)?
            await DataSetUp.CreateRulesAndSchema();
        }

        public Task DisposeAsync()
        {
            Console.WriteLine("Global async teardown after each test...");
            return Task.CompletedTask;
        }
    }
}
