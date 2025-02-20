using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities;

namespace DfT.DTRO.IntegrationTests.IntegrationTests
{
    public class SendDtroIntegrationTests : BaseTest
    {
        [Fact]
        public async Task CreateDtroShouldBeSavedCorrectly()
        {
            var userResponse = await DtroUsers.CreateUserAsync(User.Publisher);
            Assert.Equal(HttpStatusCode.Created, userResponse.StatusCode);

            var dtroResponse = await Dtros.CreateDtroAsync(User.Publisher, $"JSON-{TestConfig.SchemaVersionUnderTest}-example-Derbyshire 2024 DJ388 partial.json");
            Assert.Equal(HttpStatusCode.Created, dtroResponse.StatusCode);
        }
    }
}