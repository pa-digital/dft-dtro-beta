public class UserServiceTests
{
    private readonly Mock<IUserDal> _mockUserDal;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserDal = new Mock<IUserDal>();
        _userService = new UserService(_mockUserDal.Object);
    }

    [Fact]
    public async Task DeleteUserCallsDalWithParsedGuidWhenUserIdIsValid()
    {
        Guid userId = Guid.NewGuid();

        await _userService.DeleteUser(userId);
        _mockUserDal.Verify(dal => dal.DeleteUser(userId), Times.Once);
    }
}