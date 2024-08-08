using DfT.DTRO.Models.SwaCode;

namespace Dft.DTRO.Tests.CodeiumTests.Tra.Controller;

public class TraControllerSearchSwaCodesTests
{
    private readonly Mock<ITraService> _traServiceMock;
    private readonly Mock<ILogger<TraController>> _loggerMock;
    private readonly TraController _controller;

    public TraControllerSearchSwaCodesTests()
    {
        _traServiceMock = new Mock<ITraService>();
        _loggerMock = new Mock<ILogger<TraController>>();
        _controller = new TraController(_traServiceMock.Object, _loggerMock.Object);
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
}
