using DfT.DTRO.Consts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;

public static class UserExtensions
{
    public static async Task CreateUserForDataSetUpAsync(this TestUser publisher)
    {
        HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
        Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);
    }
}