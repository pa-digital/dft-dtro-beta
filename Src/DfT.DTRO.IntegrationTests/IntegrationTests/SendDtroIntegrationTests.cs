using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.RequestEndPoints;

namespace DfT.DTRO.IntegrationTests.IntegrationTests
{
    public class SendDtroIntegrationTests : BaseTest
    {
        [Fact]
        public async Task CreateDtroShouldBeSavedCorrectly()
        {
            await DtroUsers.CreateUserAsync(User.Publisher);
            await Dtros.CreateDtroAsync(User.Publisher);
        }
    }
}