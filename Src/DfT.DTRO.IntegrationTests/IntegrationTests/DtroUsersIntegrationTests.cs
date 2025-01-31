namespace DfT.DTRO.IntegrationTests.IntegrationTests;

public class DtroUsersIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;
    private readonly string _url;

    public DtroUsersIntegrationTests(WebApplicationFactory<Startup> factory)
    {
        _client = factory.CreateClient();
        _url = "/dtroUsers";

        using IServiceScope scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DtroContext>();
        dbContext.Database.Migrate();
    }

    [Fact]
    public async Task Test_No_X_App_Id_Returns_500()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, _url);
        var response = await _client.SendAsync(request);
        string responseBody = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Contains("Middleware, access denied: x-App-Id", responseBody);
    }
}