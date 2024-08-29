namespace Dft.DTRO.Tests.CodeiumTests.Tra.Controller;

public class TraControllerGetSwaCodesTests
{
    private readonly Mock<IDtroUserService> _traServiceMock;
    private readonly Mock<ILogger<DtroUserController>> _loggerMock;
    private readonly DtroUserController _controller;

    public TraControllerGetSwaCodesTests()
    {
        _traServiceMock = new Mock<IDtroUserService>();
        _loggerMock = new Mock<ILogger<DtroUserController>>();
        _controller = new DtroUserController(_traServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetSwaCodes_ReturnsOk_WithListOfSwaCodeResponse()
    {
        // Arrange
        var swaCodes = new List<DtroUserResponse> { new DtroUserResponse() };
        _traServiceMock.Setup(service => service.GetAllDtroUsersAsync()).ReturnsAsync(swaCodes);

        // Act
        var result = await _controller.GetDtroUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(swaCodes, okResult.Value);
    }

    [Fact]
    public async Task GetSwaCodes_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        _traServiceMock.Setup(service => service.GetAllDtroUsersAsync()).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetDtroUsers();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.IsType<ApiErrorResponse>(statusCodeResult.Value);
    }
}
