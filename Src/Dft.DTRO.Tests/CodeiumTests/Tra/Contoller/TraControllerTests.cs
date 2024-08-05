using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DfT.DTRO.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dft.DTRO.Tests.CodeiumTests.Tra.Controler;
public class TraControllerTests
{
    private readonly Mock<ITraService> _traServiceMock;
    private readonly Mock<ILogger<TraController>> _loggerMock;
    private readonly TraController _controller;

    public TraControllerTests()
    {
        _traServiceMock = new Mock<ITraService>();
        _loggerMock = new Mock<ILogger<TraController>>();
        _controller = new TraController(_traServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetSwaCodes_ReturnsOk_WithListOfSwaCodeResponse()
    {
        // Arrange
        var swaCodes = new List<SwaCodeResponse> { new SwaCodeResponse() };
        _traServiceMock.Setup(service => service.GetSwaCodeAsync()).ReturnsAsync(swaCodes);

        // Act
        var result = await _controller.GetSwaCodes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(swaCodes, okResult.Value);
    }

    [Fact]
    public async Task GetSwaCodes_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        _traServiceMock.Setup(service => service.GetSwaCodeAsync()).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetSwaCodes();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.IsType<ApiErrorResponse>(statusCodeResult.Value);
    }

    [Fact]
    public async Task SearchSwaCodes_ReturnsOk_WithListOfSwaCodeResponse()
    {
        // Arrange
        var partialName = "test";
        var swaCodes = new List<SwaCodeResponse> { new SwaCodeResponse() };
        _traServiceMock.Setup(service => service.SearchSwaCodes(partialName)).ReturnsAsync(swaCodes);

        // Act
        var result = await _controller.SearchSwaCodes(partialName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(swaCodes, okResult.Value);
    }

    [Fact]
    public async Task SearchSwaCodes_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var partialName = "test";
        _traServiceMock.Setup(service => service.SearchSwaCodes(partialName)).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.SearchSwaCodes(partialName);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.IsType<ApiErrorResponse>(statusCodeResult.Value);
    }

    [Fact]
    public async Task CreateFromBody_ReturnsCreated_WithGuidResponse()
    {
        // Arrange
        var request = new SwaCodeRequest { TraId = 1 };
        var response = new GuidResponse { Id = Guid.NewGuid() };
        _traServiceMock.Setup(service => service.SaveTraAsync(request)).ReturnsAsync(response);

        // Act
        var result = await _controller.CreateFromBody(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal(response, createdResult.Value);
    }

    [Fact]
    public async Task CreateFromBody_ReturnsBadRequest_WhenInvalidOperationExceptionIsThrown()
    {
        // Arrange
        var request = new SwaCodeRequest { TraId = 1 };
        _traServiceMock.Setup(service => service.SaveTraAsync(request)).ThrowsAsync(new InvalidOperationException("Invalid operation"));

        // Act
        var result = await _controller.CreateFromBody(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
    }

    [Fact]
    public async Task CreateFromBody_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var request = new SwaCodeRequest { TraId = 1 };
        _traServiceMock.Setup(service => service.SaveTraAsync(request)).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.CreateFromBody(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.IsType<ApiErrorResponse>(statusCodeResult.Value);
    }

    [Fact]
    public async Task UpdateFromBody_ReturnsOk_WithGuidResponse()
    {
        // Arrange
        var request = new SwaCodeRequest { TraId = 1 };
        var response = new GuidResponse { Id = Guid.NewGuid() };
        _traServiceMock.Setup(service => service.UpdateTraAsync(request)).ReturnsAsync(response);

        // Act
        var result = await _controller.UpdateFromBody(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task UpdateFromBody_ReturnsNotFound_WhenNotFoundExceptionIsThrown()
    {
        // Arrange
        var request = new SwaCodeRequest { TraId = 1 };
        _traServiceMock.Setup(service => service.UpdateTraAsync(request)).ThrowsAsync(new NotFoundException("Not found"));

        // Act
        var result = await _controller.UpdateFromBody(request);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateFromBody_ReturnsBadRequest_WhenInvalidOperationExceptionIsThrown()
    {
        // Arrange
        var request = new SwaCodeRequest { TraId = 1 };
        _traServiceMock.Setup(service => service.UpdateTraAsync(request)).ThrowsAsync(new InvalidOperationException("Invalid operation"));

        // Act
        var result = await _controller.UpdateFromBody(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
    }

    [Fact]
    public async Task UpdateFromBody_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var request = new SwaCodeRequest { TraId = 1 };
        _traServiceMock.Setup(service => service.UpdateTraAsync(request)).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.UpdateFromBody(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.IsType<ApiErrorResponse>(statusCodeResult.Value);
    }

    [Fact]
    public async Task ActivateByTraId_ReturnsOk_WithGuidResponse()
    {
        // Arrange
        var traId = 1;
        var response = new GuidResponse { Id = Guid.NewGuid() };
        _traServiceMock.Setup(service => service.ActivateTraAsync(traId)).ReturnsAsync(response);

        // Act
        var result = await _controller.ActivateByTraId(traId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task ActivateByTraId_ReturnsNotFound_WhenNotFoundExceptionIsThrown()
    {
        // Arrange
        var traId = 1;
        _traServiceMock.Setup(service => service.ActivateTraAsync(traId)).ThrowsAsync(new NotFoundException("Not found"));

        // Act
        var result = await _controller.ActivateByTraId(traId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
    }

    [Fact]
    public async Task ActivateByTraId_ReturnsBadRequest_WhenInvalidOperationExceptionIsThrown()
    {
        // Arrange
        var traId = 1;
        _traServiceMock.Setup(service => service.ActivateTraAsync(traId)).ThrowsAsync(new InvalidOperationException("Invalid operation"));

        // Act
        var result = await _controller.ActivateByTraId(traId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
    }

    [Fact]
    public async Task ActivateByTraId_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var traId = 1;
        _traServiceMock.Setup(service => service.ActivateTraAsync(traId)).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.ActivateByTraId(traId);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.IsType<ApiErrorResponse>(statusCodeResult.Value);
    }

    [Fact]
    public async Task DeactivateByTraId_ReturnsOk_WithGuidResponse()
    {
        // Arrange
        var traId = 1;
        var response = new GuidResponse { Id = Guid.NewGuid() };
        _traServiceMock.Setup(service => service.DeActivateTraAsync(traId)).ReturnsAsync(response);

        // Act
        var result = await _controller.DeactivateByTraId(traId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task DeactivateByTraId_ReturnsNotFound_WhenNotFoundExceptionIsThrown()
    {
        // Arrange
        var traId = 1;
        _traServiceMock.Setup(service => service.DeActivateTraAsync(traId)).ThrowsAsync(new NotFoundException("Not found"));

        // Act
        var result = await _controller.DeactivateByTraId(traId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
    }

    [Fact]
    public async Task DeactivateByTraId_ReturnsBadRequest_WhenInvalidOperationExceptionIsThrown()
    {
        // Arrange
        var traId = 1;
        _traServiceMock.Setup(service => service.DeActivateTraAsync(traId)).ThrowsAsync(new InvalidOperationException("Invalid operation"));

        // Act
        var result = await _controller.DeactivateByTraId(traId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
    }

    [Fact]
    public async Task DeactivateByTraId_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var traId = 1;
        _traServiceMock.Setup(service => service.DeActivateTraAsync(traId)).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.DeactivateByTraId(traId);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.IsType<ApiErrorResponse>(statusCodeResult.Value);
    }
}
