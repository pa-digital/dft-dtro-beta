using System;

namespace DfT.DTRO.IntegrationTests.IntegrationTests;

public class SchemaIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;
    private readonly string _url;

    public SchemaIntegrationTests(WebApplicationFactory<Startup> factory)
    {
        _client = factory.CreateClient();
        _url = "/schemas";

        using IServiceScope scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DtroContext>();
        dbContext.Database.Migrate();
    }

    [Theory]
    [InlineData("EC6298AA-B5F8-4BEB-946D-3C3DA1D828B2", HttpStatusCode.InternalServerError, "An error occurred: Middleware, access denied: Dtro user for (ec6298aa-b5f8-4beb-946d-3c3da1d828b2) not found")]
    [InlineData("f553d1ec-a7ca-43d2-b714-60dacbb4d005", HttpStatusCode.OK, "")]
    [InlineData("free-text", HttpStatusCode.InternalServerError, "An error occurred: Middleware, access denied: x-App-Id (or x-App-Id-Override) not in header")]
    [InlineData(null, HttpStatusCode.InternalServerError, "Middleware, access denied: x-App-Id")]
    public async Task Test_X_App_Id_Returns_VariousStatusCode(string appId, HttpStatusCode httpStatusCode, string message)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, _url);

        var isValidGuid = Guid.TryParse(appId, out var _);
        if (isValidGuid)
        {
            request.Headers.Add("x-app-id", appId);
        }

        var response = await _client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Equal(httpStatusCode, response.StatusCode);
        Assert.Contains(message, responseBody);
    }
}