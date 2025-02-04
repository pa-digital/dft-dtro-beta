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
    [InlineData("EC6298AA-B5F8-4BEB-946D-3C3DA1D828B2", HttpStatusCode.InternalServerError,
        "An error occurred: Middleware, access denied: Dtro user for (ec6298aa-b5f8-4beb-946d-3c3da1d828b2) not found")]
    [InlineData("f553d1ec-a7ca-43d2-b714-60dacbb4d005", HttpStatusCode.OK, "")]
    public async Task Test_GetSchemaVersions(string appId, HttpStatusCode httpStatusCode, string message)
        => await TestGetEndpoint(appId, httpStatusCode, message, $"{_url}/versions");

    [Theory]
    [InlineData("EC6298AA-B5F8-4BEB-946D-3C3DA1D828B2", HttpStatusCode.InternalServerError, "An error occurred: Middleware, access denied: Dtro user for (ec6298aa-b5f8-4beb-946d-3c3da1d828b2) not found")]
    [InlineData("f553d1ec-a7ca-43d2-b714-60dacbb4d005", HttpStatusCode.OK, "")]
    public async Task Test_GetSchemas(string appId, HttpStatusCode httpStatusCode, string message)
        => await TestGetEndpoint(appId, httpStatusCode, message, _url);

    [Theory]
    [InlineData("EC6298AA-B5F8-4BEB-946D-3C3DA1D828B2", HttpStatusCode.InternalServerError, "An error occurred: Middleware, access denied: Dtro user for (ec6298aa-b5f8-4beb-946d-3c3da1d828b2) not found")]
    [InlineData("f553d1ec-a7ca-43d2-b714-60dacbb4d005", HttpStatusCode.OK, "")]
    public async Task Test_GetSchemaByVersion(string appId, HttpStatusCode httpStatusCode, string message)
        => await TestGetEndpoint(appId, httpStatusCode, message, $"{_url}/3.3.0");

    [Theory]
    [InlineData("EC6298AA-B5F8-4BEB-946D-3C3DA1D828B2", HttpStatusCode.InternalServerError, "An error occurred: Middleware, access denied: Dtro user for (ec6298aa-b5f8-4beb-946d-3c3da1d828b2) not found")]
    public async Task Test_PostSchemaByVersion(string appId, HttpStatusCode httpStatusCode, string message)
        => await TestPostEndpoint(appId, httpStatusCode, message, $"{_url}/createFromBody/3.2.3");


    [Theory]
    [InlineData("EC6298AA-B5F8-4BEB-946D-3C3DA1D828B2", HttpStatusCode.InternalServerError, "An error occurred: Middleware, access denied: Dtro user for (ec6298aa-b5f8-4beb-946d-3c3da1d828b2) not found")]
    public async Task Test_DeleteActivatedSchemaByVersion(string appId, HttpStatusCode httpStatusCode, string message)
        => await TestDeleteEndpoint(appId, httpStatusCode, message, $"{_url}/3.2.4");

    [Theory]
    [InlineData("EC6298AA-B5F8-4BEB-946D-3C3DA1D828B2", HttpStatusCode.InternalServerError, "An error occurred: Middleware, access denied: Dtro user for (ec6298aa-b5f8-4beb-946d-3c3da1d828b2) not found")]
    public async Task Test_DeleteDeactivatedSchemaByVersion(string appId, HttpStatusCode httpStatusCode, string message)
        => await TestDeleteEndpoint(appId, httpStatusCode, message, $"{_url}/3.2.3");

    private async Task TestGetEndpoint(string appId, HttpStatusCode httpStatusCode, string message, string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);

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

    private async Task TestDeleteEndpoint(string appId, HttpStatusCode httpStatusCode, string message, string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, url);

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

    private async Task TestPostEndpoint(string appId, HttpStatusCode httpStatusCode, string message, string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        var file = await File.ReadAllTextAsync("../../../TestData/Schemas/schemas-3.2.3.json");
        request.Content = new StringContent(file, Encoding.UTF8, "application/json");
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