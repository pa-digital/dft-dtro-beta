public class UserServiceTests
{
    private readonly Mock<IUserDal> _mockUserDal;
    private readonly Mock<IApigeeDeveloperRepository> _apigeeDeveloperRepositoryMock;
    private readonly UserService _userService;
    private readonly string _email;

    public UserServiceTests()
    {
        _mockUserDal = new Mock<IUserDal>();
        _apigeeDeveloperRepositoryMock = new Mock<IApigeeDeveloperRepository>();
        _userService = new UserService(_mockUserDal.Object, _apigeeDeveloperRepositoryMock.Object);
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
}