using DfT.DTRO.Models.SwaCode;

namespace Dft.DTRO.Tests.CodeiumTests.Tra.Controller;

public class TraControllerDeactivateTests
{
    private readonly Mock<ITraService> _traServiceMock;
    private readonly Mock<ILogger<TraController>> _loggerMock;
    private readonly TraController _controller;

    public TraControllerDeactivateTests()
    {
        _traServiceMock = new Mock<ITraService>();
        _loggerMock = new Mock<ILogger<TraController>>();
        _controller = new TraController(_traServiceMock.Object, _loggerMock.Object);
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
