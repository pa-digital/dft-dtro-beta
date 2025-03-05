namespace Dft.DTRO.Tests.CodeiumTests.Users.Controller;

public class UserControllerSearchSwaCodesTests
{
    private readonly Mock<IDtroUserService> _traServiceMock;
    private readonly Mock<ILogger<DtroUserController>> _loggerMock;
    private readonly DtroUserController _controller;
    private readonly Mock<LoggingExtension.Builder> _mockLoggingBuilder;
    private readonly Mock<LoggingExtension> _mockLoggingExtension;

    public UserControllerSearchSwaCodesTests()
    {
        _traServiceMock = new Mock<IDtroUserService>();
        _loggerMock = new Mock<ILogger<DtroUserController>>();
        _mockLoggingBuilder = new Mock<LoggingExtension.Builder>();
        _mockLoggingExtension = new Mock<LoggingExtension>();
        _controller = new DtroUserController(_traServiceMock.Object, _loggerMock.Object, _mockLoggingExtension.Object);
    }

    [Fact]
    public async Task SearchSwaCodes_ReturnsOk_WithListOfSwaCodeResponse()
    {
        // Arrange
        var partialName = "test";
        var swaCodes = new List<DtroUserResponse> { new DtroUserResponse() };
        _traServiceMock.Setup(service => service.SearchDtroUsers(partialName)).ReturnsAsync(swaCodes);

        // Act
        var result = await _controller.SearchDtroUsers(partialName);

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
        _traServiceMock.Setup(service => service.SearchDtroUsers(partialName)).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.SearchDtroUsers(partialName);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.IsType<ApiErrorResponse>(statusCodeResult.Value);
    }
}
