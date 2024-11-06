using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DfT.DTRO.Tests.CodeiumTests.Integration;

[ExcludeFromCodeCoverage]
public class SchemaController_Codeium_Tests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string SchemaVersion = "3.2.0";

    private const string BadSchemaVersion = "0.0.0";

    private readonly SchemasController _controller;

    private readonly WebApplicationFactory<Program> _factory;

    private readonly Mock<IRequestCorrelationProvider> _mockCorrelationProvider;

    private readonly Mock<ISchemaTemplateService> _mockSchemaTemplateService;

    private readonly ExpandoObject? _schemaTemplate;

    public SchemaController_Codeium_Tests(WebApplicationFactory<Program> factory)
    {
        _mockSchemaTemplateService = new Mock<ISchemaTemplateService>();
        _mockCorrelationProvider = new Mock<IRequestCorrelationProvider>();
        Mock<ILogger<SchemasController>> mockLogger = new();
        Mock<LoggingExtension.Builder> _mockLoggingBuilder = new Mock<LoggingExtension.Builder>();
        var mockLoggingExtension = new Mock<LoggingExtension>();

        _controller = new SchemasController(_mockSchemaTemplateService.Object,
            _mockCorrelationProvider.Object, mockLogger.Object, mockLoggingExtension.Object);

        _schemaTemplate = new ExpandoObject();
        try
        {
            _schemaTemplate = JsonConvert.DeserializeObject<ExpandoObject>(JsonExample, new ExpandoObjectConverter());
        }
        catch (JsonSerializationException ex)
        {
            throw new InvalidOperationException("Failed to deserialize JSON", ex);
        }

        _factory = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(_mockSchemaTemplateService.Object);
            services.AddSingleton(_mockCorrelationProvider.Object);
            services.AddSingleton(mockLogger.Object);
        }));
    }

    private static string JsonExample =>
        File.ReadAllText("../../../../../examples/Schemas/schemas-3.2.0.json");

    [Fact]
    public async Task GetSchemas_ReturnsOk_WhenTemplatesExist()
    {
        List<SchemaTemplateResponse> templates = new() { new SchemaTemplateResponse(), new SchemaTemplateResponse() };

        _mockSchemaTemplateService.Setup(service => service.GetSchemaTemplatesAsync())
            .ReturnsAsync(templates);

        OkObjectResult? result = await _controller.Get() as OkObjectResult;

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(templates, result.Value);
    }

    [Fact]
    public async Task GetSchemas_ReturnsEmptyList_WhenNoTemplatesExist()
    {
        List<SchemaTemplateResponse> templates = new();
        _mockSchemaTemplateService.Setup(service => service.GetSchemaTemplatesAsync())
            .ReturnsAsync(templates);

        OkObjectResult? result = await _controller.Get() as OkObjectResult;

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(templates, result.Value);
    }

    [Fact]
    public async Task GetSchemas_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        _mockSchemaTemplateService.Setup(service => service.GetSchemaTemplatesAsync())
            .Throws(new Exception());

        ObjectResult? result = await _controller.Get() as ObjectResult;

        Assert.IsType<ObjectResult>(result);
        Assert.Equal("500", result.StatusCode.ToString());
    }

    [Fact]
    public async Task GetSchemaVersion_ReturnsOkResult()
    {
        SchemaTemplateResponse template = new()
        {
            SchemaVersion = new SchemaVersion(SchemaVersion),
            Template = new ExpandoObject(),
            IsActive = true
        };
        _mockSchemaTemplateService.Setup(x => x.GetSchemaTemplateAsync(SchemaVersion))
            .Returns(Task.FromResult(template));

        OkObjectResult? result = await _controller.GetByVersion(SchemaVersion) as OkObjectResult;
        string? response = result?.StatusCode.ToString();

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal("200", response);
        _mockSchemaTemplateService.Verify(s => s.GetSchemaTemplateAsync(SchemaVersion), Times.Once);
    }

    [Fact]
    public async Task GetSchema_ReturnsNotFoundResult()
    {
        _mockSchemaTemplateService.Setup(x => x.GetSchemaTemplateAsync(BadSchemaVersion))
            .Throws(new NotFoundException());

        NotFoundObjectResult? result = await _controller.GetByVersion(BadSchemaVersion) as NotFoundObjectResult;

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetSchema_ReturnsBadRequestResult()
    {
        _mockSchemaTemplateService.Setup(x => x.GetSchemaTemplateAsync(It.IsAny<SchemaVersion>()))
            .Throws(new InvalidOperationException("Invalid operation"));

        BadRequestObjectResult? result = await _controller.GetByVersion(BadSchemaVersion) as BadRequestObjectResult;

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetSchema_ReturnsInternalServerErrorResult()
    {
        _mockSchemaTemplateService.Setup(x => x.GetSchemaTemplateAsync(It.IsAny<SchemaVersion>()))
            .Throws(new Exception("Something went wrong"));

        ObjectResult? result = await _controller.GetByVersion(BadSchemaVersion) as ObjectResult;

        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, result.StatusCode);
    }

    [Fact]
    public async Task GetSchemaById_ValidId_ReturnsOk()
    {
        Guid schemaId = Guid.NewGuid();
        _mockSchemaTemplateService.Setup(s => s.GetSchemaTemplateByIdAsync(schemaId))
            .Returns(Task.FromResult(new SchemaTemplateResponse()));

        OkObjectResult? result = await _controller.GetById(schemaId) as OkObjectResult;

        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<SchemaTemplateResponse>(result.Value);
        Assert.Equal("200", result.StatusCode.ToString());
    }

    [Fact]
    public async Task GetSchemaById_InvalidId_ReturnsNotFound()
    {
        Guid schemaId = Guid.NewGuid();
        _mockSchemaTemplateService.Setup(s => s.GetSchemaTemplateByIdAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new NotFoundException());

        ObjectResult? result = await _controller.GetById(schemaId) as ObjectResult;

        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("404", result.StatusCode.ToString());
    }

    [Fact]
    public async Task GetSchemaById_InvalidOperationException_ReturnsBadRequest()
    {
        Guid schemaId = Guid.NewGuid();
        _mockSchemaTemplateService.Setup(s => s.GetSchemaTemplateByIdAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new InvalidOperationException());

        BadRequestObjectResult? result = await _controller.GetById(schemaId) as BadRequestObjectResult;

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("400", result.StatusCode.ToString());
    }

    [Fact]
    public async Task GetSchemaById_UnexpectedException_ReturnsInternalServerError()
    {
        Guid schemaId = Guid.NewGuid();
        _mockSchemaTemplateService.Setup(s => s.GetSchemaTemplateByIdAsync(schemaId))
            .ThrowsAsync(new Exception());

        ObjectResult? result = await _controller.GetById(schemaId) as ObjectResult;

        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, result.StatusCode);
    }

    [Fact]
    public async Task CreateSchema_ReturnsCreated()
    {
        Guid guidId = Guid.NewGuid();
        _mockSchemaTemplateService.Setup(mock => mock.SaveSchemaTemplateAsJsonAsync("3.2.0", _schemaTemplate,
                _mockCorrelationProvider.Object.CorrelationId))
            .ReturnsAsync(new GuidResponse { Id = guidId });

        CreatedAtActionResult? result =
            await _controller.CreateFromBodyByVersion(SchemaVersion, _schemaTemplate) as CreatedAtActionResult;

        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("CreateFromFileByVersion", result.ActionName);
        Assert.Equal(guidId, result.RouteValues?["id"]);
        Assert.IsType<GuidResponse>(result.Value);
    }

    [Fact]
    public async Task CreateSchema_ThrowsInvalidOperationException_ReturnsBadRequest()
    {
        ExpandoObject body = new ExpandoObject();
        _mockSchemaTemplateService.Setup(mock =>
                mock.SaveSchemaTemplateAsJsonAsync(BadSchemaVersion, body,
                    _mockCorrelationProvider.Object.CorrelationId))
            .Throws(new InvalidOperationException("Invalid schema"));

        BadRequestObjectResult? result =
            await _controller.CreateFromBodyByVersion(BadSchemaVersion, body) as BadRequestObjectResult;

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<ApiErrorResponse>(result.Value);
        ApiErrorResponse? apiErrorResponse = result.Value as ApiErrorResponse;
        Assert.Equal("Bad Request", apiErrorResponse?.Message);
    }

    [Fact]
    public async Task CreateSchema_ThrowsException_ReturnsInternalServerError()
    {
        ExpandoObject body = new ExpandoObject();
        _mockSchemaTemplateService.Setup(mock => mock.SaveSchemaTemplateAsJsonAsync(BadSchemaVersion,
                body, _mockCorrelationProvider.Object.CorrelationId))
            .Throws(new Exception("Unexpected error"));

        ObjectResult? result = await _controller.CreateFromBodyByVersion(BadSchemaVersion, body) as ObjectResult;

        Assert.IsType<ObjectResult>(result);
    }

    [Fact]
    public async Task UpdateSchema_ReturnsOk_ValidSchemaVersionAndBody()
    {
        ExpandoObject body = new ExpandoObject();
        GuidResponse guidResponse = new GuidResponse();

        _mockSchemaTemplateService.Setup(s => s.UpdateSchemaTemplateAsJsonAsync
                (SchemaVersion, body, It.IsAny<string>()))
            .ReturnsAsync(guidResponse);

        ObjectResult? result = await _controller.UpdateFromBodyByVersion(SchemaVersion, body) as ObjectResult;

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(guidResponse, result.Value);
        _mockSchemaTemplateService.Verify(s => s.UpdateSchemaTemplateAsJsonAsync
            (SchemaVersion, body, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateSchema_ReturnsNotFound_SchemaVersionDoesNotExist()
    {
        ExpandoObject body = new ExpandoObject();
        _mockSchemaTemplateService.Setup(s => s.UpdateSchemaTemplateAsJsonAsync
                (BadSchemaVersion, body, It.IsAny<string>()))
            .ThrowsAsync(new NotFoundException());

        ObjectResult? result = await _controller.UpdateFromBodyByVersion(BadSchemaVersion, body) as ObjectResult;

        Assert.IsType<NotFoundObjectResult>(result);
        _mockSchemaTemplateService.Verify(s => s.UpdateSchemaTemplateAsJsonAsync
            (BadSchemaVersion, body, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateSchema_ReturnsBadRequest_InvalidSchemaVersionOrBody()
    {
        ExpandoObject body = new ExpandoObject();
        _mockSchemaTemplateService
            .Setup(s => s.UpdateSchemaTemplateAsJsonAsync(BadSchemaVersion, body, It.IsAny<string>()))
            .ThrowsAsync(new InvalidOperationException("Invalid schema version or body"));

        BadRequestObjectResult? result =
            await _controller.UpdateFromBodyByVersion(BadSchemaVersion, body) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.IsType<ApiErrorResponse>(result.Value);
        _mockSchemaTemplateService.Verify(
            s => s.UpdateSchemaTemplateAsJsonAsync(BadSchemaVersion, body, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateSchema_ReturnsInternalServerError_UnexpectedException()
    {
        string version = "1.0.0";
        ExpandoObject body = new ExpandoObject();
        _mockSchemaTemplateService.Setup(s => s.UpdateSchemaTemplateAsJsonAsync
            (version, body, It.IsAny<string>())).ThrowsAsync(new Exception());

        ObjectResult? result = await _controller.UpdateFromBodyByVersion(version, body) as ObjectResult;

        Assert.NotNull(result);
        Assert.Equal(500, result.StatusCode);
    }

    [Fact]
    public async Task ActivateSchema_SchemaTemplateServiceActivates_ReturnsOk()
    {
        string version = "1.0.0";

        _mockSchemaTemplateService.Setup(s => s.ActivateSchemaTemplateAsync(version))
            .ReturnsAsync(new GuidResponse());

        IActionResult? result = await _controller.ActivateByVersion(version);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task ActivateSchema_ThrowsNotFoundException_ReturnsNotFound()
    {
        _mockSchemaTemplateService.Setup(s => s.ActivateSchemaTemplateAsync(BadSchemaVersion))
            .ThrowsAsync(new NotFoundException());

        ObjectResult? result = await _controller.ActivateByVersion(BadSchemaVersion) as ObjectResult;

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task ActivateSchema_ThrowsInvalidOperationException_ReturnsBadRequest()
    {
        _mockSchemaTemplateService.Setup(s => s.ActivateSchemaTemplateAsync(BadSchemaVersion))
            .ThrowsAsync(new InvalidOperationException("Some error"));


        BadRequestObjectResult? result =
            await _controller.ActivateByVersion(BadSchemaVersion) as BadRequestObjectResult;

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeActivateSchema_WhenSchemaTemplateServiceSucceeds_ReturnsOk()
    {
        string version = "1.0.0";

        _mockSchemaTemplateService.Setup(s => s.DeActivateSchemaTemplateAsync(version))
            .ReturnsAsync(new GuidResponse());

        IActionResult? result = await _controller.DeactivateByVersion(version);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeActivateSchema_ThrowsNotFoundException_ReturnsNotFound()
    {
        _mockSchemaTemplateService.Setup(s => s.DeActivateSchemaTemplateAsync(BadSchemaVersion))
            .ThrowsAsync(new NotFoundException());

        ObjectResult? result = await _controller.DeactivateByVersion(BadSchemaVersion) as ObjectResult;

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeActivateSchema_ThrowsInvalidOperationException_ReturnsBadRequest()
    {
        string version = "1.0.0";

        _mockSchemaTemplateService.Setup(s => s.DeActivateSchemaTemplateAsync(version))
            .ThrowsAsync(new InvalidOperationException("Invalid version"));

        BadRequestObjectResult? result = await _controller.DeactivateByVersion(version) as BadRequestObjectResult;

        Assert.IsType<BadRequestObjectResult>(result);
        ApiErrorResponse errorResponse = Assert.IsType<ApiErrorResponse>(result.Value);
        Assert.Equal("Bad Request", errorResponse.Message);
    }

    [Fact]
    public async Task DeActivateSchema_WhenSchemaTemplateServiceThrowsUnexpectedException_ReturnsInternalServerError()
    {
        string version = "1.0.0";

        _mockSchemaTemplateService.Setup(s => s.DeActivateSchemaTemplateAsync(version))
            .ThrowsAsync(new Exception("Unexpected error"));

        ObjectResult? result = await _controller.DeactivateByVersion(version) as ObjectResult;

        Assert.IsType<ObjectResult>(result);
        ApiErrorResponse errorResponse = Assert.IsType<ApiErrorResponse>(result.Value);
        Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.Equal("Internal Server Error", errorResponse.Message);
    }

    [Fact]
    public async Task DeleteSchema_WhenSchemaTemplateFound_ReturnsOk()
    {
        string version = "1.0.0";

        _mockSchemaTemplateService.Setup(it => it.GetSchemaTemplateAsync(version))
            .ReturnsAsync(() => new SchemaTemplateResponse());

        _mockSchemaTemplateService.Setup(s => s.DeleteSchemaTemplateAsync(version))
            .ReturnsAsync(true);

        IActionResult? result = await _controller.DeleteByVersion(version);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeleteSchema_WhenSchemaTemplateNotFound_ReturnsNotFound()
    {
        _mockSchemaTemplateService.Setup(it => it.GetSchemaTemplateAsync("4.2.2"))
            .ReturnsAsync(() => new SchemaTemplateResponse());

        _mockSchemaTemplateService.Setup(s => s.DeleteSchemaTemplateAsync("4.2.4"))
            .ReturnsAsync(() => false);

        var actual = await _controller.DeleteByVersion(BadSchemaVersion);

        Assert.Equal(404, ((NotFoundObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task DeleteSchema_ThrowsInvalidOperationException_ReturnsBadRequest()
    {
        string version = "1.0.0";

        _mockSchemaTemplateService.Setup(it => it.GetSchemaTemplateAsync(version))
            .ReturnsAsync(() => new SchemaTemplateResponse());

        _mockSchemaTemplateService.Setup(s => s.DeleteSchemaTemplateAsync(version))
            .ThrowsAsync(new InvalidOperationException("Invalid version"));

        BadRequestObjectResult? result = await _controller.DeleteByVersion(version) as BadRequestObjectResult;

        Assert.IsType<BadRequestObjectResult>(result);
        ApiErrorResponse errorResponse = Assert.IsType<ApiErrorResponse>(result.Value);
        Assert.Equal("Bad Request", errorResponse.Message);
    }

    [Fact]
    public async Task DeleteSchema_ThrowsUnexpectedException_ReturnsInternalServerError()
    {
        string version = "1.0.0";

        _mockSchemaTemplateService.Setup(it => it.GetSchemaTemplateAsync(version))
            .ReturnsAsync(() => new SchemaTemplateResponse());

        _mockSchemaTemplateService.Setup(s => s.DeleteSchemaTemplateAsync(version))
            .ThrowsAsync(new Exception("Invalid version"));

        var actual = await _controller.DeleteByVersion(version);


        Assert.Equal(500, ((ObjectResult)actual).StatusCode);
    }
}
