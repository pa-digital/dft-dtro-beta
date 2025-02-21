using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public static class DataSetUp
    {
        public static async Task ClearAllDataAsync()
        {
            await DtroUsers.DeleteExistingUsersAsync();
            Rules.DeleteExistingRules();
            await Schemas.DeleteExistingSchemasAsync();
            Dtros.DeleteExistingDtros();
        }
        public static async Task CreateRulesAndSchema()
        {
            var createRuleResponse = await Rules.CreateRuleSetFromFileAsync();
            Assert.Equal(HttpStatusCode.Created, createRuleResponse.StatusCode);
            await Schemas.CreateAndActivateSchemaAsync();
        }
    }
}