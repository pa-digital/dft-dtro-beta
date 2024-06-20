using System.Net;
using DfT.DTRO;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.Errors;
using DfT.DTRO.Models.SharedResponse;
using DfT.DTRO.Services;
using DfT.DTRO.Services.Conversion;
using DfT.DTRO.Services.Mapping;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Dft.DTRO.Tests.IntegrationTests;

public class DTROsControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private const string ValidDtroJsonPath = "./DtroJsonDataExamples/v3.1.1/proper-data.json";
    private const string ValidComplexDtroJsonPath = "./DtroJsonDataExamples/v3.1.2/3.1.2-valid-complex-dtro.json";
    private const string ValidComplexDtroJsonPathV320 = "./DtroJsonDataExamples/v3.2.0/valid-new-x.json";
    private const string InvalidDtroJsonPath = "./DtroJsonDataExamples/v3.1.1/provision-empty.json";

    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IDtroService> _mockDtroService;
    private readonly DtroMappingService _dtroMappingService;
    public DTROsControllerTests(WebApplicationFactory<Program> factory)
    {
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();

        _dtroMappingService = new DtroMappingService(configuration, new Proj4SpatialProjectionService());
        _mockDtroService = new Mock<IDtroService>(MockBehavior.Strict);

        _factory = factory
            .WithWebHostBuilder(builder => builder
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(_mockDtroService.Object);
                }));
    }

    [Theory]
    [InlineData(ValidComplexDtroJsonPath, "3.1.2")]
    [InlineData(ValidComplexDtroJsonPathV320, "3.2.0")]
    public async Task Post_DtroIsValid_CreatesDtroAndReturnsDtroId(string dtroPath, string version)
    {
        _mockDtroService.Setup(mock => mock.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(), null))
            .ReturnsAsync(new GuidResponse { Id = Guid.NewGuid() });

        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("ta", "0");

        var payload = await CreateDtroJsonPayload(dtroPath, version);

        HttpResponseMessage response = await client.PostAsync("/v1/dtros/createFromBody", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        GuidResponse? data = JsonConvert.DeserializeObject<GuidResponse>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(data);
        Assert.IsType<Guid>(data!.Id);

        _mockDtroService.Verify(mock => mock.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(), null), Times.Once);
    }

    [Fact]
    public async Task Post_SchemaDoesNotExist_ReturnsNotFoundError()
    {
        HttpClient client = _factory.CreateClient();

        _mockDtroService.Setup(mock => mock.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(), null))
            .Throws((new NotFoundException()));

        var payload = await CreateDtroJsonPayload(ValidDtroJsonPath, "0.0.0");
        HttpResponseMessage response = await client.PostAsync("/v1/dtros", payload);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_DtroIsInvalid_ReturnsValidationError()
    {
        _mockDtroService.Setup(mock => mock.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(), null))
            .Throws((new DtroValidationException()));
        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("ta", "0");

        var payload = await CreateDtroJsonPayload(InvalidDtroJsonPath, "3.1.1", false);
        HttpResponseMessage response = await client.PostAsync("/v1/dtros/createFromBody", payload);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        ApiErrorResponse? data = JsonConvert.DeserializeObject<ApiErrorResponse>(await response.Content.ReadAsStringAsync());

        Assert.NotNull(data);
        Assert.Equal("Dtro Validation Failure", data!.Message);
    }

    [Fact]
    public async Task Put_DtroIsValid_UpdatesDtroAndReturnsDtroId()
    {
        Guid dtroId = Guid.NewGuid();
        _mockDtroService.Setup(mock => mock.TryUpdateDtroAsJsonAsync
            (It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), null)).ReturnsAsync(new GuidResponse());
        HttpClient client = _factory.CreateClient();
        
        client.DefaultRequestHeaders.Add("ta", "0");

        var payload = await CreateDtroJsonPayload(ValidDtroJsonPath, "3.1.1");
        HttpResponseMessage response = await client.PutAsync($"/v1/dtros/updateFromBody{dtroId}", payload);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        GuidResponse? data = JsonConvert.DeserializeObject<GuidResponse>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(data);
        Assert.IsType<Guid>(data!.Id);
        _mockDtroService.Verify(mock => mock.TryUpdateDtroAsJsonAsync
            (It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), null), Times.Once);
    }

    [Fact]
    public async Task Put_DtroDoesNotExist_ReturnsNotFoundError()
    {
        Guid notExistingDtroId = Guid.NewGuid();
        _mockDtroService.Setup(mock => mock.TryUpdateDtroAsJsonAsync
            (It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), null)).Throws(new NotFoundException());


        HttpClient client = _factory.CreateClient();

        var payload = await CreateDtroJsonPayload(ValidDtroJsonPath, "3.1.1");
        HttpResponseMessage response = await client.PutAsync($"/v1/dtros/{notExistingDtroId}", payload);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Put_DtroIsInvalid_ReturnsBAdRequest()
    {
        Guid dtroId = Guid.NewGuid();
        _mockDtroService.Setup(mock => mock.TryUpdateDtroAsJsonAsync
            (It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), null)).Throws(new DtroValidationException());
        HttpClient client = _factory.CreateClient();

        var payload = await CreateDtroJsonPayload(InvalidDtroJsonPath, "3.1.1", false);
        HttpResponseMessage response = await client.PutAsync($"/v1/dtros/updateFromBody{dtroId}", payload);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_DtroExists_ReturnsDtro()
    {
        var sampleDtro = await CreateDtroObject(ValidDtroJsonPath);
        var sampleDtroResponse = _dtroMappingService.MapToDtroResponse(sampleDtro);

        _mockDtroService.Setup(mock => mock.GetDtroByIdAsync
            (It.Is(sampleDtro.Id, EqualityComparer<Guid>.Default))).Returns(Task.FromResult(sampleDtroResponse));

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync($"/v1/dtros/{sampleDtro.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_DtroDoesNotExist_ReturnsNotFoundError()
    {
        Guid dtroId = Guid.NewGuid();
        _mockDtroService.Setup(mock => mock.GetDtroByIdAsync(It.Is(dtroId, EqualityComparer<Guid>.Default)))
            .Throws(new NotFoundException());

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync($"/v1/dtros/{dtroId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_DtroExists_ReturnsDtro()
    {
        Guid dtroId = Guid.NewGuid();
        _mockDtroService.Setup(mock => mock.DeleteDtroAsync
            (It.Is(dtroId, EqualityComparer<Guid>.Default), It.IsAny<DateTime?>())).Returns(Task.FromResult(true));

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.DeleteAsync($"/v1/dtros/{dtroId}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_DtroDoesNotExist_ReturnsNotFoundError()
    {
        Guid dtroId = Guid.NewGuid();
        _mockDtroService.Setup(mock => mock.DeleteDtroAsync
            (It.Is(dtroId, EqualityComparer<Guid>.Default), It.IsAny<DateTime?>())).Throws(new NotFoundException());
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.DeleteAsync($"/v1/dtros/{dtroId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}