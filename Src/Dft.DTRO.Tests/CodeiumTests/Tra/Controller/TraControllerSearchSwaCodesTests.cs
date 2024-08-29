namespace Dft.DTRO.Tests.CodeiumTests.Tra.Controller;

public class TraControllerSearchSwaCodesTests
{
    private readonly Mock<IDtroUserService> _traServiceMock;
    private readonly Mock<ILogger<DtroUserController>> _loggerMock;
    private readonly DtroUserController _controller;

    public TraControllerSearchSwaCodesTests()
    {
        _traServiceMock = new Mock<IDtroUserService>();
        _loggerMock = new Mock<ILogger<DtroUserController>>();
        _controller = new DtroUserController(_traServiceMock.Object, _loggerMock.Object);
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
