﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DfT.DTRO.Tests.CodeiumTests.Integration;

[ExcludeFromCodeCoverage]
public class DTROsController_Codeium_Tests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Mock<IDtroService> _mockDtroService;
    private readonly Mock<IRequestCorrelationProvider> _correlationProviderMock;
    private readonly DTROsController _controller;
    private readonly WebApplicationFactory<Program> _factory;

    private const string ValidDtroJsonPath = "../../../../../examples/D-TROs/3.2.0/valid-new-x.json";
    private readonly DtroSubmit _dtroSubmit;
    private readonly int? _taForTest = 1585;

    public DTROsController_Codeium_Tests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _mockDtroService = new Mock<IDtroService>();
        _correlationProviderMock = new Mock<IRequestCorrelationProvider>();
        Mock<ILogger<DTROsController>> loggerMock = new();
        Mock<IMetricsService> metricsMock = new();

        metricsMock.Setup(x => x.IncrementMetric(It.IsAny<MetricType>(), _taForTest)).ReturnsAsync(true);


        _controller = new DTROsController(_mockDtroService.Object, metricsMock.Object,
            _correlationProviderMock.Object, loggerMock.Object);

        _factory = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(_mockDtroService.Object);
            services.AddSingleton(_correlationProviderMock.Object);
            services.AddSingleton(loggerMock.Object);
        }));

        string json = File.ReadAllText(ValidDtroJsonPath);
        ExpandoObject? dtroData = JsonConvert.DeserializeObject<ExpandoObject>
            (json, new ExpandoObjectConverter());

        _dtroSubmit = new DtroSubmit
        {
            SchemaVersion = new SchemaVersion(3, 1, 2),
            Data = dtroData
        };

    }

    [Fact]
    public async Task CreateDtro_Returns_OK()
    {
        var response = new GuidResponse { Id = Guid.NewGuid() };
        _mockDtroService.Setup(x => x.SaveDtroAsJsonAsync(_dtroSubmit, It.IsAny<string>(), _taForTest)).ReturnsAsync(response);

        var result = await _controller.CreateFromBody(_taForTest, _dtroSubmit);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdAtActionResult.StatusCode);
        Assert.Equal(nameof(_controller.GetById), createdAtActionResult.ActionName);
        Assert.Equal(response.Id, createdAtActionResult.RouteValues?["id"]);
        Assert.Equal(response, createdAtActionResult.Value);
    }

    [Fact]
    public async Task CreateDtro_ValidationException_ReturnsBadRequest()
    {
        var dtroBadSubmit = new DtroSubmit();
        _mockDtroService.Setup(s => s.SaveDtroAsJsonAsync(dtroBadSubmit, _correlationProviderMock.Object.CorrelationId, _taForTest))
            .ThrowsAsync(new DtroValidationException());

        var response = await _controller.CreateFromBody(_taForTest, dtroBadSubmit);

        Assert.IsType<BadRequestObjectResult>(response);
        _mockDtroService.Verify(s => s.SaveDtroAsJsonAsync(dtroBadSubmit, _correlationProviderMock.Object.CorrelationId, _taForTest), Times.Once);
    }

    [Fact]
    public async Task CreateDtro_UnexpectedException_ReturnsInternalServerError()
    {
        var dtroBadSubmit = new DtroSubmit();
        _mockDtroService.Setup(s => s.SaveDtroAsJsonAsync(dtroBadSubmit, _correlationProviderMock.Object.CorrelationId, _taForTest))
            .ThrowsAsync(new Exception("Unexpected error"));

        var response = await _controller.CreateFromBody(_taForTest, dtroBadSubmit);

        Assert.IsType<ObjectResult>(response);
        var objectResult = (ObjectResult)response;
        Assert.Equal(500, objectResult.StatusCode);

        _mockDtroService.Verify(s => s.SaveDtroAsJsonAsync(dtroBadSubmit, _correlationProviderMock.Object.CorrelationId, _taForTest), Times.Once);
    }

    [Fact]
    public async Task UpdateDtro_ReturnsOk_ForExistingDtro()
    {
        _mockDtroService.Setup(s => s.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), _taForTest))
            .Returns(Task.FromResult(new GuidResponse()));

        var result = await _controller.UpdateFromBody(_taForTest, Guid.NewGuid(), _dtroSubmit);

        Assert.IsType<OkObjectResult>(result);
        _mockDtroService.Verify(s => s.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), _taForTest), Times.Once);
    }

    [Fact]
    public async Task UpdateDtro_ReturnsBadRequest_ForInvalidDtro()
    {
        _mockDtroService.Setup(s => s.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), _taForTest))
            .ThrowsAsync(new DtroValidationException());

        var dtroBadSubmit = new DtroSubmit
        { SchemaVersion = new("3.1.2"), Data = new() };

        var result = await _controller.UpdateFromBody(_taForTest, Guid.NewGuid(), dtroBadSubmit);

        Assert.IsType<BadRequestObjectResult>(result);
        _mockDtroService.Verify(s => s.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), _taForTest), Times.Once);
    }

    [Fact]
    public async Task UpdateDtro_ReturnsNotFound_ForNonExistentDtro()
    {
        _mockDtroService.Setup(s => s.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), _taForTest))
            .ThrowsAsync(new NotFoundException());

        var dtroBadSubmit = new DtroSubmit
        { SchemaVersion = new("3.1.2"), Data = new() };

        var result = await _controller.UpdateFromBody(_taForTest, Guid.NewGuid(), dtroBadSubmit);

        Assert.IsType<NotFoundObjectResult>(result);
        _mockDtroService.Verify(s => s.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), _taForTest), Times.Once);
    }

    [Fact]
    public async Task UpdateDtro_ReturnsInternalServerError_ForUnexpectedException()
    {
        _mockDtroService.Setup(s => s.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), _taForTest))
            .ThrowsAsync(new Exception());

        var dtroBadSubmit = new DtroSubmit
        { SchemaVersion = new("3.1.2"), Data = new() };

        var result = await _controller.UpdateFromBody(_taForTest, Guid.NewGuid(), dtroBadSubmit) as ObjectResult;

        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, result?.StatusCode);

        _mockDtroService.Verify(s => s.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), _taForTest), Times.Once);
    }

    [Fact]
    public async Task GetDtroById_ExistingDtro_ReturnsDtro()
    {
        var dtroId = Guid.NewGuid();
        var dtroResponse = new DtroResponse();
        _mockDtroService.Setup(s => s.GetDtroByIdAsync(dtroId)).ReturnsAsync(dtroResponse);

        var result = await _controller.GetById(dtroId) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(dtroResponse, result?.Value);
        _mockDtroService.Verify(s => s.GetDtroByIdAsync(dtroId), Times.Once);
    }

    [Fact]
    public async Task GetDtroById_NonExistingDtro_ReturnsNotFound()
    {
        _mockDtroService.Setup(s => s.GetDtroByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<DtroResponse?>(null));

        var response = await _controller.GetById(Guid.NewGuid()) as NotFoundResult;

        Assert.Null(response);
        _mockDtroService.Verify(s => s.GetDtroByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task GetDtroById_Exception_ReturnsInternalServerError()
    {
        _mockDtroService.Setup(s => s.GetDtroByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception());

        var result = await _controller.GetById(Guid.NewGuid()) as ObjectResult;

        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, result?.StatusCode);
        _mockDtroService.Verify(s => s.GetDtroByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task DeleteDtro_ValidId_ReturnsNoContent()
    {
        _mockDtroService.Setup(s => s.DeleteDtroAsync(It.IsAny<int?>(), It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(Task.FromResult(true));

        var response = await _controller.Delete(It.IsAny<int?>(), Guid.NewGuid()) as NoContentResult;

        Assert.NotNull(response);
        Assert.IsType<NoContentResult>(response);
        Assert.Equal(204, response?.StatusCode);
    }
}