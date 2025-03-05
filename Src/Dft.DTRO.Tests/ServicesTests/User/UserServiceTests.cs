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
        string userId = Guid.NewGuid().ToString();
        Guid expectedGuid = Guid.Parse(userId);

        await _userService.DeleteUser(userId);
        _mockUserDal.Verify(dal => dal.DeleteUser(expectedGuid), Times.Once);
    }

    [Fact]
    public async Task DeleteUserThrowsFormatExceptionWhenUserIdIsInvalid()
    {
        string invalidUserId = "invalid-guid";
        await Assert.ThrowsAsync<FormatException>(() => _userService.DeleteUser(invalidUserId));
    }
}
