using static DfT.DTRO.ApiTests.ApiTests.Helpers.TestConfig;

namespace DfT.DTRO.ApiTests.ApiTests.Helpers
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
                HttpResponseMessage ruleCreationResponse = await Rules.CreateRuleSetFromFileAsync(schemaVersion, testUser);
                string ruleCreationResponseJson = await ruleCreationResponse.Content.ReadAsStringAsync();
                Assert.True(HttpStatusCode.Created == ruleCreationResponse.StatusCode,
                    $"Response JSON:\n\n{ruleCreationResponseJson}");

                await Schemas.CreateAndActivateSchemaAsync(schemaVersion, testUser);
            }
        }

        public static async Task CreateRulesAndSchemaIfDoNotExist(TestUser testUser)
        {
            string[] schemaFiles = FileHelper.GetFileNames(PathToSchemaExamplesDirectory);
            string[] schemaVersions = Schemas.GetSchemaVersions(schemaFiles);

            foreach (string schemaVersion in schemaVersions)
            {
                HttpResponseMessage rulesGetResponse = await Rules.GetRuleSetAsync(schemaVersion, testUser);
                if (rulesGetResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    HttpResponseMessage ruleCreationResponse = await Rules.CreateRuleSetFromFileAsync(schemaVersion, testUser);
                    string dtroCreationResponseJson = await ruleCreationResponse.Content.ReadAsStringAsync();
                    Assert.True(HttpStatusCode.Created == ruleCreationResponse.StatusCode,
                        $"Response JSON:\n\n{dtroCreationResponseJson}");
                }

                HttpResponseMessage getSchemaResponse = await Schemas.GetSchemaAsync(schemaVersion, testUser);
                if (getSchemaResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    HttpResponseMessage schemaCreationResponse = await Schemas.CreateSchemaFromFileAsync(schemaVersion, testUser);
                    string schemaCreationResponseJson = await schemaCreationResponse.Content.ReadAsStringAsync();
                    Assert.True(HttpStatusCode.Created == schemaCreationResponse.StatusCode,
                        $"Response JSON:\n\n{schemaCreationResponseJson}");
                }

                HttpResponseMessage schemaActivationResponse = await Schemas.ActivateSchemaAsync(schemaVersion, testUser);
                string schemaActivationResponseJson = await schemaActivationResponse.Content.ReadAsStringAsync();
                Assert.True(HttpStatusCode.OK == schemaActivationResponse.StatusCode,
                    $"Response JSON:\n\n{schemaActivationResponseJson}");
            }
        }
    }
}