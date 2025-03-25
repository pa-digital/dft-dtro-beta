using DfT.DTRO.Consts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;

public static class UserExtensions
{
    public static async Task CreateUserForDataSetUpAsync(this TestUser publisher)
    {
        HttpResponseMessage userCreationResponse = await DtroUsers.CreateUserAsync(publisher);
        string userCreationResponseJson = await userCreationResponse.Content.ReadAsStringAsync();
        Assert.True(HttpStatusCode.Created == userCreationResponse.StatusCode,
            $"Response JSON:\n\n{userCreationResponseJson}");
    }
}