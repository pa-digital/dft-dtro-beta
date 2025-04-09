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

    [Fact]
    public async Task FindUserDetailsReturnsOkWhenUserFound()
    {
        var userId = Guid.NewGuid();
        var userDto = new UserDto { Id = userId, Name = "Test User" };

        _mockUserService.Setup(s => s.GetUserDetails(userId))
                        .ReturnsAsync(userDto);

        var result = await _controller.FindUserDetails(userId.ToString());
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(userDto, okResult.Value);
    }

    [Fact]
    public async Task FindUserDetailsReturnsBadRequestWhenArgumentNullExceptionIsThrown()
    {
        var userId = Guid.NewGuid();

        _mockUserService.Setup(s => s.GetUserDetails(userId))
                        .ThrowsAsync(new ArgumentNullException("userId"));

        var result = await _controller.FindUserDetails(userId.ToString());
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task FindUserDetailsReturnsServerErrorWhenInvalidOperationExceptionIsThrown()
    {
        var userId = Guid.NewGuid();

        _mockUserService.Setup(s => s.GetUserDetails(userId))
                        .ThrowsAsync(new InvalidOperationException("Something went wrong"));

        var result = await _controller.FindUserDetails(userId.ToString());
        var serverError = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, serverError.StatusCode);
    }

    [Fact]
    public async Task FindUserDetailsReturnsServerErrorWhenUnexpectedExceptionIsThrown()
    {
        var userId = Guid.NewGuid();

        _mockUserService.Setup(s => s.GetUserDetails(userId))
                        .ThrowsAsync(new Exception("Unknown error"));

        var result = await _controller.FindUserDetails(userId.ToString());
        var serverError = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, serverError.StatusCode);
    }

    [Fact]
    public async Task FindUserDetailsReturnsBadRequestWhenUserIdIsInvalid()
    {
        var invalidUserId = "not-a-guid";
        var result = await _controller.FindUserDetails(invalidUserId);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }
}