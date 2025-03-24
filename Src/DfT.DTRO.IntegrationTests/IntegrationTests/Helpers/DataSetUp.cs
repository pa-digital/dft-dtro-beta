using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public static class DataSetUp
    {
        public static async Task ClearAllDataAsync(TestUser testUser)
        {
            await DtroUsers.DeleteExistingUsersAsync(testUser);
            Rules.DeleteExistingRules();
            await Schemas.DeleteExistingSchemasAsync(testUser);
            Dtros.DeleteExistingDtros();
        }

        public static async Task CreateRulesAndSchema(TestUser testUser)
        {
            string[] schemaFiles = FileHelper.GetFileNames(PathToSchemaExamplesDirectory);
            string[] schemaVersions = Schemas.GetSchemaVersions(schemaFiles);

            foreach (string schemaVersion in schemaVersions)
            {
                HttpResponseMessage createRuleResponse = await Rules.CreateRuleSetFromFileAsync(schemaVersion, testUser);
                Assert.Equal(HttpStatusCode.Created, createRuleResponse.StatusCode);
                await Schemas.CreateAndActivateSchemaAsync(schemaVersion, testUser);
            }
        }

        public static async Task CreateRulesAndSchemaIfDoNotExist(TestUser testUser)
        {
            string[] schemaFiles = FileHelper.GetFileNames(PathToSchemaExamplesDirectory);
            string[] schemaVersions = Schemas.GetSchemaVersions(schemaFiles);

            foreach (string schemaVersion in schemaVersions)
            {
                HttpResponseMessage getRulesResponse = await Rules.GetRuleSetAsync(schemaVersion, testUser);
                if (getRulesResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    HttpResponseMessage createRuleResponse = await Rules.CreateRuleSetFromFileAsync(schemaVersion, testUser);
                    Assert.Equal(HttpStatusCode.Created, createRuleResponse.StatusCode);
                }

                HttpResponseMessage getSchemaResponse = await Schemas.GetSchemaAsync(schemaVersion, testUser);
                if (getSchemaResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    HttpResponseMessage createSchemaResponse = await Schemas.CreateSchemaFromFileAsync(schemaVersion, testUser);
                    Assert.Equal(HttpStatusCode.Created, createSchemaResponse.StatusCode);
                }
                HttpResponseMessage activateSchemaResponse = await Schemas.ActivateSchemaAsync(schemaVersion, testUser);
                Assert.Equal(HttpStatusCode.OK, activateSchemaResponse.StatusCode);
            }
        }
    }
}