public class UserControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UserController _controller;
    private readonly string _email;

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UserController(_mockUserService.Object);
        _email = "user@test.com";
    }

    [Fact]
    public async Task DeleteUserReturnsNoContentWhenUserDeletedSuccessfully()
    {
        var userId = Guid.NewGuid();
        _mockUserService.Setup(s => s.DeleteUser(_email, userId)).Returns(Task.CompletedTask);

        var result = await _controller.DeleteUser(_email, userId);
        var noContentResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, noContentResult.StatusCode);
    }

    [Fact]
    public async Task DeleteUserReturnsBadRequestWhenFormatExceptionIsThrown()
    {
        var userId = Guid.NewGuid();
        _mockUserService.Setup(s => s.DeleteUser(_email, It.IsAny<Guid>())).Throws<FormatException>();

        var result = await _controller.DeleteUser(_email, userId);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task DeleteUserReturnsServerErrorWhenInvalidOperationExceptionIsThrown()
    {
        var userId = Guid.NewGuid();
        _mockUserService.Setup(s => s.DeleteUser(_email, It.IsAny<Guid>()))
            .Throws(new InvalidOperationException("User not found"));

        var result = await _controller.DeleteUser(_email, userId);
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task DeleteUserReturnsServerErrorWhenUnexpectedExceptionIsThrown()
    {
        var userId = Guid.NewGuid();
        _mockUserService.Setup(s => s.DeleteUser(_email, It.IsAny<Guid>()))
            .Throws(new Exception("Unexpected error"));

        var result = await _controller.DeleteUser(_email, userId);
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }
}