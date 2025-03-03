namespace Dft.DTRO.Tests.ControllerTests;

public class UserControllerTests : IClassFixture<UserControllerTestFixture>
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UserController _controller;

    private static List<UserDto> Dtos =>
        [
            new()
            {
                Id = Guid.NewGuid(),
                Status = "pending",
                Email = "jon.doe@email.com",
                IsCentralServiceOperator = true,
                Created = DateTime.UtcNow.ToString("G"),
                Username = "Jon Doe"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Status = "accepted",
                Email = "jane.doe@email.com",
                Created = DateTime.UtcNow.ToString("G"),
                Username = "Jane Doe"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Status = "revoked",
                Email = "john.blogs@email.com",
                Created = DateTime.UtcNow.ToString("G"),
                Username = "John Blogs"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Status = "rejected",
                Email = "jane.blogs@email.com",
                Created = DateTime.UtcNow.ToString("G"),
                Username = "Jane Blogs"
            }
        ];

    public UserControllerTests(UserControllerTestFixture fixture)
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UserController(_mockUserService.Object)
        {
            ControllerContext = fixture.ControllerContext
        };
    }

    [Fact]
    public void GetUsersReturnsOk()
    {
        var fixture = new UserControllerTestFixture { UserId = "jon.doe@email.com" };
        var request = new UserRequest { UserId = fixture.UserId, Page = 1, PageSize = 50 };
        var response = new PaginatedResponse<UserDto>(Dtos, 1, 4);
        _mockUserService.Setup(userService => userService.GetUsers(request)).Returns(response);

        var actual = _controller.GetUsers(request);
        var result = Assert.IsType<OkObjectResult>(actual.Result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public void GetUsersReturnsBadRequest()
    {
        var fixture = new UserControllerTestFixture { UserId = "wrong.address@email.com" };
        var request = new UserRequest { UserId = fixture.UserId, Page = 1, PageSize = 50 };
        _mockUserService.Setup(userService => userService.GetUsers(request)).Returns(() => null);

        var actual = _controller.GetUsers(request);
        var result = Assert.IsType<BadRequestObjectResult>(actual.Result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public void GetUsersReturnsArgumentNullException()
    {
        var fixture = new UserControllerTestFixture { UserId = "jon.doe@email.com" };
        var request = new UserRequest { UserId = fixture.UserId, Page = 1, PageSize = 50 };
        _mockUserService.Setup(userService => userService.GetUsers(request)).Throws(() => new ArgumentNullException());

        var actual = _controller.GetUsers(request);
        var result = Assert.IsType<BadRequestObjectResult>(actual.Result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public void GetUsersReturnsInvalidOperationException()
    {
        var fixture = new UserControllerTestFixture { UserId = "jon.doe@email.com" };
        var request = new UserRequest { UserId = fixture.UserId, Page = 1, PageSize = 50 };
        _mockUserService.Setup(userService => userService.GetUsers(request)).Throws(() => new InvalidOperationException());

        var actual = _controller.GetUsers(request);
        var result = Assert.IsType<ObjectResult>(actual.Result);
        Assert.Equal(500, result.StatusCode);
    }

    [Fact]
    public void GetUsersReturnsException()
    {
        var fixture = new UserControllerTestFixture { UserId = "jon.doe@email.com" };
        var request = new UserRequest { UserId = fixture.UserId, Page = 1, PageSize = 50 };
        _mockUserService.Setup(userService => userService.GetUsers(request)).Throws(() => new Exception());

        var actual = _controller.GetUsers(request);
        var result = Assert.IsType<ObjectResult>(actual.Result);
        Assert.Equal(500, result.StatusCode);
    }
}