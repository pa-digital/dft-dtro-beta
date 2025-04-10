public class UserServiceTests
{
    private readonly Mock<IUserDal> _mockUserDal;
    private readonly Mock<IApplicationDal> _mockApplicationDal;
    private readonly Mock<IApigeeDeveloperRepository> _apigeeDeveloperRepositoryMock;
    private readonly UserService _userService;
    private readonly string _email;

    public UserServiceTests()
    {
        _mockUserDal = new Mock<IUserDal>();
        _mockApplicationDal = new Mock<IApplicationDal>();
        _apigeeDeveloperRepositoryMock = new Mock<IApigeeDeveloperRepository>();
        _userService = new UserService(_mockUserDal.Object, _mockApplicationDal.Object, _apigeeDeveloperRepositoryMock.Object);
        _email = "user@test.com";
    }

    [Fact]
    public async Task DeleteUserCallsDalWithParsedGuidWhenUserIdIsValid()
    {
        Guid userId = Guid.NewGuid();

        await _userService.DeleteUser(_email, userId);
        _mockUserDal.Verify(dal => dal.DeleteUser(userId), Times.Once);
        _apigeeDeveloperRepositoryMock.Verify(repository => repository.DeleteDeveloper(_email), Times.Once);
    }

    [Fact]
    public async Task GetUserDetailsReturnsUserDtoWithApplications()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Forename = "Test",
            Surname = "User",
            Email = "user@test.com",
            Created = DateTime.UtcNow
        };

        var applications = new List<ApplicationListDto>
        {
            new ApplicationListDto { Id = Guid.NewGuid(), Name = "App 1" },
            new ApplicationListDto { Id = Guid.NewGuid(), Name = "App 2" }
        };

        _mockUserDal.Setup(dal => dal.GetUserFromId(userId)).ReturnsAsync(user);
        _mockApplicationDal.Setup(dal => dal.GetUserApplications(userId)).ReturnsAsync(applications);

        var result = await _userService.GetUserDetails(userId);

        Assert.Equal(userId, result.Id);
        Assert.Equal("Test User", result.Name);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.Created, result.Created);
        Assert.Equal(applications, result.Applications);
    }

    [Fact]
    public async Task GetUserDetailsThrowsWhenUserNotFound()
    {
        var userId = Guid.NewGuid();
        _mockUserDal.Setup(dal => dal.GetUserFromId(userId)).ThrowsAsync(new ArgumentNullException("user"));

        await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.GetUserDetails(userId));
    }

    [Fact]
    public async Task GetUserDetailsThrowsWhenApplicationFetchFails()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Forename = "Test",
            Surname = "User",
            Email = "user@test.com",
            Created = DateTime.UtcNow
        };

        _mockUserDal.Setup(dal => dal.GetUserFromId(userId)).ReturnsAsync(user);
        _mockApplicationDal.Setup(dal => dal.GetUserApplications(userId)).ThrowsAsync(new InvalidOperationException("DB error"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.GetUserDetails(userId));
    }
}