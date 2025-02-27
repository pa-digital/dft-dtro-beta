using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities;

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
            HttpResponseMessage createRuleResponse = await Rules.CreateRuleSetFromFileAsync(testUser);
            Assert.Equal(HttpStatusCode.Created, createRuleResponse.StatusCode);
            await Schemas.CreateAndActivateSchemaAsync(testUser);
        }
    }
}