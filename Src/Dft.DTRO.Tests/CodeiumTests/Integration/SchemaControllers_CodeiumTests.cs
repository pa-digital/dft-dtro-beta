using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Net;
using DfT.DTRO.Controllers;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.Errors;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.SharedResponse;
using DfT.DTRO.RequestCorrelation;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DfT.DTRO.Tests.CodeiumTests.Integration;

[ExcludeFromCodeCoverage]
public class SchemaController_Codeium_Tests : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly Mock<ISchemaTemplateService> _mockSchemaTemplateService;

    private readonly Mock<IRequestCorrelationProvider> _mockCorrelationProvider;

    private readonly Mock<ILogger<SchemasController>> _mockLogger;

    private readonly WebApplicationFactory<Program> _factory;

    private readonly SchemasController _controller;

    private readonly string json = File.ReadAllText("./SchemaJsonExamples/v3.2.0/example-3.2.0.json");

    private readonly string schemaVersion = "3.2.0";

    private readonly string badschemaVersion = "0.0.0";

    private readonly ExpandoObject? schemaTemplate;


    public SchemaController_Codeium_Tests(WebApplicationFactory<Program> factory)
    {
        _mockSchemaTemplateService = new Mock<ISchemaTemplateService>();
        _mockCorrelationProvider = new Mock<IRequestCorrelationProvider>();
        _mockLogger = new Mock<ILogger<SchemasController>>();
        _controller = new SchemasController(_mockSchemaTemplateService.Object,
            _mockCorrelationProvider.Object, _mockLogger.Object);

        schemaTemplate = new ExpandoObject();
        try
        {
            schemaTemplate = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());
        }
        catch (JsonSerializationException ex)
        {
            throw new InvalidOperationException("Failed to deserialize JSON", ex);
        }

        _factory = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(_mockSchemaTemplateService.Object);
            services.AddSingleton(_mockCorrelationProvider.Object);
            services.AddSingleton(_mockLogger.Object);

        }));
    }

    [Fact]
    public async Task GetSchemas_ReturnsOk_WhenTemplatesExist()
    {
        // Arrange
        var templates = new List<SchemaTemplateResponse>
        {
            new SchemaTemplateResponse(),
            new SchemaTemplateResponse()
        };

        _mockSchemaTemplateService.Setup(service => service.GetSchemaTemplatesAsync())
            .ReturnsAsync(templates);

        // Act
        var result = await _controller.Get() as OkObjectResult;

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(templates, result?.Value);
    }

    [Fact]
    public async Task GetSchemas_ReturnsEmptyList_WhenNoTemplatesExist()
    {
        // Arrange
        var templates = new List<SchemaTemplateResponse>();
        _mockSchemaTemplateService.Setup(service => service.GetSchemaTemplatesAsync())
            .ReturnsAsync(templates);

        // Act
        var result = await _controller.Get() as OkObjectResult;

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(templates, result?.Value);

    }

    [Fact]
    public async Task GetSchemas_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        _mockSchemaTemplateService.Setup(service => service.GetSchemaTemplatesAsync())
            .Throws(new Exception());

        // Act
        var result = await _controller.Get() as ObjectResult;

        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equal("500", result?.StatusCode.ToString());
    }

    [Fact]
    public async Task GetSchemaVersion_ReturnsOkResult()
    {
        // Arrange
        var template = new SchemaTemplateResponse
        {
            SchemaVersion = new SchemaVersion(schemaVersion),
            Template = new ExpandoObject(),
            IsActive = true

        };
        _mockSchemaTemplateService.Setup(x => x.GetSchemaTemplateAsync(schemaVersion))
            .Returns(Task.FromResult(template));

        // Act
        var result = await _controller.GetByVersion(schemaVersion) as OkObjectResult;
        var response = result?.StatusCode.ToString();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal("200", response);
        _mockSchemaTemplateService.Verify(s => s.GetSchemaTemplateAsync(schemaVersion), Times.Once);
    }

    [Fact]
    public async Task GetSchema_ReturnsNotFoundResult()
    {
        // Arrange
        _mockSchemaTemplateService.Setup(x => x.GetSchemaTemplateAsync(badschemaVersion))
            .Throws(new NotFoundException());

        // Act
        var result = await _controller.GetByVersion(badschemaVersion) as NotFoundObjectResult;

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetSchema_ReturnsBadRequestResult()
    {
        // Arrange
        _mockSchemaTemplateService.Setup(x => x.GetSchemaTemplateAsync(It.IsAny<SchemaVersion>()))
            .Throws(new InvalidOperationException("Invalid operation"));

        // Act
        var result = await _controller.GetByVersion(badschemaVersion) as BadRequestObjectResult;

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetSchema_ReturnsInternalServerErrorResult()
    {
        // Arrange
        _mockSchemaTemplateService.Setup(x => x.GetSchemaTemplateAsync(It.IsAny<SchemaVersion>()))
            .Throws(new Exception("Something went wrong"));

        // Act
        var result = await _controller.GetByVersion(badschemaVersion) as ObjectResult;

        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, result?.StatusCode);
    }

    [Fact]
    public async Task GetSchemaById_ValidId_ReturnsOk()
    {
        // Arrange
        Guid schemaId = Guid.NewGuid();
        var expectedSchema = new SchemaTemplate
        {
            Id = schemaId
        };
        _mockSchemaTemplateService.Setup(s => s.GetSchemaTemplateByIdAsync(schemaId))
            .Returns(Task.FromResult(new SchemaTemplateResponse()));

        // Act
        var result = await _controller.GetById(schemaId) as OkObjectResult;

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<SchemaTemplateResponse>(result?.Value);
        Assert.Equal("200", result?.StatusCode.ToString());
    }

    [Fact]
    public async Task GetSchemaById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        Guid schemaId = Guid.NewGuid();
        _mockSchemaTemplateService.Setup(s => s.GetSchemaTemplateByIdAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new NotFoundException());

        // Act
        var result = await _controller.GetById(schemaId) as ObjectResult;

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("404", result?.StatusCode.ToString());
    }

    [Fact]
    public async Task GetSchemaById_InvalidOperationException_ReturnsBadRequest()
    {
        // Arrange
        var schemaId = Guid.NewGuid();
        _mockSchemaTemplateService.Setup(s => s.GetSchemaTemplateByIdAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act
        var result = await _controller.GetById(schemaId) as BadRequestObjectResult;

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("400", result?.StatusCode.ToString());

    }

    [Fact]
    public async Task GetSchemaById_UnexpectedException_ReturnsInternalServerError()
    {
        // Arrange
        var schemaId = Guid.NewGuid();
        _mockSchemaTemplateService.Setup(s => s.GetSchemaTemplateByIdAsync(schemaId))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetById(schemaId) as ObjectResult;

        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, result?.StatusCode);

    }

    [Fact]
    public async Task CreateSchema_ReturnsCreated()
    {
        // Arrange
        Guid guidId = Guid.NewGuid();
        _mockSchemaTemplateService.Setup(mock => mock.SaveSchemaTemplateAsJsonAsync("3.2.0", schemaTemplate,
                _mockCorrelationProvider.Object.CorrelationId))
            .ReturnsAsync(new GuidResponse
            {
                Id = guidId
            });

        // Act
        var result = await _controller.CreateFromBodyByVersion(schemaVersion, schemaTemplate) as CreatedAtActionResult;

        // Assert

        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("CreateFromFileByVersion", result?.ActionName);
        Assert.Equal(guidId, result?.RouteValues?["id"]);
        Assert.IsType<GuidResponse>(result?.Value);
    }

    [Fact]
    public async Task CreateSchema_ThrowsInvalidOperationException_ReturnsBadRequest()
    {
        // Arrange
        var body = new ExpandoObject();
        _mockSchemaTemplateService.Setup(mock => mock.SaveSchemaTemplateAsJsonAsync(badschemaVersion, body, _mockCorrelationProvider.Object.CorrelationId))
            .Throws(new InvalidOperationException("Invalid schema"));

        // Act
        var result = await _controller.CreateFromBodyByVersion(badschemaVersion, body) as BadRequestObjectResult;

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<ApiErrorResponse>(result?.Value);
        var apiErrorResponse = result?.Value as ApiErrorResponse;
        Assert.Equal("Bad Request", apiErrorResponse?.Message);
    }

    [Fact]
    public async Task CreateSchema_ThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var body = new ExpandoObject();
        _mockSchemaTemplateService.Setup(mock => mock.SaveSchemaTemplateAsJsonAsync(badschemaVersion,
                body, _mockCorrelationProvider.Object.CorrelationId))
            .Throws(new Exception("Unexpected error"));

        // Act
        var result = await _controller.CreateFromBodyByVersion(badschemaVersion, body) as ObjectResult;

        // Assert
        Assert.IsType<ObjectResult>(result);

    }
    [Fact]
    public async Task UpdateSchema_ReturnsOk_ValidSchemaVersionAndBody()
    {
        // Arrange
        var body = new ExpandoObject();
        var guidResponse = new GuidResponse();

        _mockSchemaTemplateService.Setup(s => s.UpdateSchemaTemplateAsJsonAsync
                (schemaVersion, body, It.IsAny<string>()))
            .ReturnsAsync(guidResponse);

        // Act
        var result = await _controller.UpdateFromBodyByVersion(schemaVersion, body) as ObjectResult;

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(guidResponse, result?.Value);
        _mockSchemaTemplateService.Verify(s => s.UpdateSchemaTemplateAsJsonAsync
            (schemaVersion, body, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateSchema_ReturnsNotFound_SchemaVersionDoesNotExist()
    {
        // Arrange
        var body = new ExpandoObject();
        _mockSchemaTemplateService.Setup(s => s.UpdateSchemaTemplateAsJsonAsync
                (badschemaVersion, body, It.IsAny<string>()))
            .ThrowsAsync(new NotFoundException());

        // Act
        var result = await _controller.UpdateFromBodyByVersion(badschemaVersion, body) as ObjectResult;

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        _mockSchemaTemplateService.Verify(s => s.UpdateSchemaTemplateAsJsonAsync
            (badschemaVersion, body, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateSchema_ReturnsBadRequest_InvalidSchemaVersionOrBody()
    {
        // Arrange
        var body = new ExpandoObject();
        _mockSchemaTemplateService.Setup(s => s.UpdateSchemaTemplateAsJsonAsync(badschemaVersion, body, It.IsAny<string>()))
            .ThrowsAsync(new InvalidOperationException("Invalid schema version or body"));

        // Act
        var result = await _controller.UpdateFromBodyByVersion(badschemaVersion, body) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ApiErrorResponse>(result?.Value);
        _mockSchemaTemplateService.Verify(s => s.UpdateSchemaTemplateAsJsonAsync(badschemaVersion, body, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateSchema_ReturnsInternalServerError_UnexpectedException()
    {
        // Arrange
        var version = "1.0.0";
        var body = new ExpandoObject();
        _mockSchemaTemplateService.Setup(s => s.UpdateSchemaTemplateAsJsonAsync
            (version, body, It.IsAny<string>())).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.UpdateFromBodyByVersion(version, body) as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(500, result?.StatusCode);

    }

    [Fact]
    public async Task ActivateSchema_SchemaTemplateServiceActivates_ReturnsOk()
    {
        // Arrange
        var version = "1.0.0";

        _mockSchemaTemplateService.Setup(s => s.ActivateSchemaTemplateAsync(version))
            .ReturnsAsync(new GuidResponse());

        // Act
        var result = await _controller.ActivateByVersion(version);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task ActivateSchema_ThrowsNotFoundException_ReturnsNotFound()
    {
        // Arrange
        _mockSchemaTemplateService.Setup(s => s.ActivateSchemaTemplateAsync(badschemaVersion))
            .ThrowsAsync(new NotFoundException());

        // Act
        var result = await _controller.ActivateByVersion(badschemaVersion) as ObjectResult;

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task ActivateSchema_ThrowsInvalidOperationException_ReturnsBadRequest()
    {
        // Arrange

        _mockSchemaTemplateService.Setup(s => s.ActivateSchemaTemplateAsync(badschemaVersion))
            .ThrowsAsync(new InvalidOperationException("Some error"));


        // Act
        var result = await _controller.ActivateByVersion(badschemaVersion) as BadRequestObjectResult;

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeActivateSchema_WhenSchemaTemplateServiceSucceeds_ReturnsOk()
    {
        // Arrange
        var version = "1.0.0";

        _mockSchemaTemplateService.Setup(s => s.DeActivateSchemaTemplateAsync(version))
            .ReturnsAsync(new GuidResponse());

        // Act
        var result = await _controller.DeactivateByVersion(version);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeActivateSchema_ThrowsNotFoundException_ReturnsNotFound()
    {
        // Arrange
        _mockSchemaTemplateService.Setup(s => s.DeActivateSchemaTemplateAsync(badschemaVersion))
            .ThrowsAsync(new NotFoundException());

        // Act
        var result = await _controller.DeactivateByVersion(badschemaVersion) as ObjectResult;

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeActivateSchema_ThrowsInvalidOperationException_ReturnsBadRequest()
    {
        // Arrange
        var version = "1.0.0";

        _mockSchemaTemplateService.Setup(s => s.DeActivateSchemaTemplateAsync(version))
            .ThrowsAsync(new InvalidOperationException("Invalid version"));

        // Act
        var result = await _controller.DeactivateByVersion(version) as BadRequestObjectResult;

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var errorResponse = Assert.IsType<ApiErrorResponse>(result?.Value);
        Assert.Equal("Bad Request", errorResponse.Message);
    }

    [Fact]
    public async Task DeActivateSchema_WhenSchemaTemplateServiceThrowsUnexpectedException_ReturnsInternalServerError()
    {
        // Arrange
        var version = "1.0.0";

        _mockSchemaTemplateService.Setup(s => s.DeActivateSchemaTemplateAsync(version))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _controller.DeactivateByVersion(version) as ObjectResult;

        // Assert
        Assert.IsType<ObjectResult>(result);
        var errorResponse = Assert.IsType<ApiErrorResponse>(result?.Value);
        Assert.Equal((int)HttpStatusCode.InternalServerError, result?.StatusCode);
        Assert.Equal("Internal Server Error", errorResponse.Message);
    }

}