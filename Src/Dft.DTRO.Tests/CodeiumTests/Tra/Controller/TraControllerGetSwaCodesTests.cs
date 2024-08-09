using DfT.DTRO.Models.SwaCode;

namespace Dft.DTRO.Tests.CodeiumTests.Tra.Controller;

public class TraControllerGetSwaCodesTests
{
    private readonly Mock<ITraService> _traServiceMock;
    private readonly Mock<ILogger<TraController>> _loggerMock;
    private readonly TraController _controller;

    public TraControllerGetSwaCodesTests()
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
}
