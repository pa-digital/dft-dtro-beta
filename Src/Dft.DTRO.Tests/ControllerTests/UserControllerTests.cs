public class UserControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UserController(_mockUserService.Object);
    }

    [Fact]
    public async Task DeleteUserReturnsNoContentWhenUserDeletedSuccessfully()
    {
        var request = new UserDeleteRequest { Id = Guid.NewGuid().ToString() };
        _mockUserService.Setup(s => s.DeleteUser(request.Id)).Returns(Task.CompletedTask);

        var result = await _controller.DeleteUser(request);
        var noContentResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, noContentResult.StatusCode);
    }

    [Fact]
    public async Task DeleteUserReturnsBadRequestWhenRequestIsNull()
    {
        var result = await _controller.DeleteUser(null);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task DeleteUserReturnsBadRequestWhenUserIdIsEmpty()
    {
        var request = new UserDeleteRequest { Id = "" };

        var result = await _controller.DeleteUser(request);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task DeleteUserReturnsBadRequestWhenFormatExceptionIsThrown()
    {
        var request = new UserDeleteRequest { Id = "invalid-guid" };
        _mockUserService.Setup(s => s.DeleteUser(It.IsAny<string>())).Throws<FormatException>();

        var result = await _controller.DeleteUser(request);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task DeleteUserReturnsServerErrorWhenInvalidOperationExceptionIsThrown()
    {
        var request = new UserDeleteRequest { Id = Guid.NewGuid().ToString() };
        _mockUserService.Setup(s => s.DeleteUser(It.IsAny<string>()))
            .Throws(new InvalidOperationException("User not found"));

        var result = await _controller.DeleteUser(request);
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task DeleteUserReturnsServerErrorWhenUnexpectedExceptionIsThrown()
    {
        var request = new UserDeleteRequest { Id = Guid.NewGuid().ToString() };
        _mockUserService.Setup(s => s.DeleteUser(It.IsAny<string>()))
            .Throws(new Exception("Unexpected error"));

        var result = await _controller.DeleteUser(request);
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }
}