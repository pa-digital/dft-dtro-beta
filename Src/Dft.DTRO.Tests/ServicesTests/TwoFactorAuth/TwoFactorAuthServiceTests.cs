using DfT.DTRO.Models.TwoFactorAuth;
using DfT.DTRO.Services;
using Moq;
using Xunit;

public class TwoFactorAuthServiceTests
{
    private readonly Mock<ITwoFactorAuthDal> _mockTwoFactorAuthDal;
    private readonly Mock<IUserDal> _mockUserDal;
    private readonly TwoFactorAuthService _sut;

    public TwoFactorAuthServiceTests()
    {
        _mockTwoFactorAuthDal = new Mock<ITwoFactorAuthDal>();
        _mockUserDal = new Mock<IUserDal>();
        _sut = new TwoFactorAuthService(_mockTwoFactorAuthDal.Object, _mockUserDal.Object);
    }

    [Fact]
    public async Task GenerateTwoFactorAuthCodeReturnsValidTfa()
    {
        var username = "user@test.com";
        var user = new User { Email = username };
        var tfa = new TwoFactorAuthentication
        {
            Token = Guid.NewGuid(),
            Code = "123456",
            ExpiresAt = DateTime.UtcNow.AddMinutes(10)
        };

        _mockUserDal.Setup(x => x.GetUserFromEmail(username)).ReturnsAsync(user);
        _mockTwoFactorAuthDal.Setup(x => x.SaveTwoFactorAuthCode(user)).ReturnsAsync(tfa);

        var result = await _sut.GenerateTwoFactorAuthCode(username);

        Assert.NotNull(result);
        Assert.Equal(tfa.Token, result.Token);
        Assert.Equal(tfa.Code, result.Code);
        _mockUserDal.Verify(x => x.GetUserFromEmail(username), Times.Once);
        _mockTwoFactorAuthDal.Verify(x => x.SaveTwoFactorAuthCode(user), Times.Once);
    }

    [Fact]
    public async Task VerifyTwoFactorAuthCodeReturnsTfaIfCodeIsValid()
    {
        var token = Guid.NewGuid().ToString();
        var code = "123456";
        var tfa = new TwoFactorAuthentication
        {
            Token = Guid.Parse(token),
            Code = code,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10)
        };

        _mockTwoFactorAuthDal.Setup(x => x.GetCodeByToken(tfa.Token)).ReturnsAsync(tfa);

        var result = await _sut.VerifyTwoFactorAuthCode(token, code);

        Assert.NotNull(result);
        Assert.Equal(tfa.Token, result.Token);
        Assert.Equal(tfa.Code, result.Code);
        _mockTwoFactorAuthDal.Verify(x => x.GetCodeByToken(tfa.Token), Times.Once);
    }

    [Fact]
    public async Task VerifyTwoFactorAuthCodeReturnsNullIfCodeIsInvalid()
    {
        var token = Guid.NewGuid().ToString();
        var code = "123456";
        var tfa = new TwoFactorAuthentication
        {
            Token = Guid.Parse(token),
            Code = "654321",
            ExpiresAt = DateTime.UtcNow.AddMinutes(10)
        };

        _mockTwoFactorAuthDal.Setup(x => x.GetCodeByToken(tfa.Token)).ReturnsAsync(tfa);

        var result = await _sut.VerifyTwoFactorAuthCode(token, code);

        Assert.Null(result);
        _mockTwoFactorAuthDal.Verify(x => x.GetCodeByToken(tfa.Token), Times.Once);
    }

    [Fact]
    public async Task VerifyTwoFactorAuthCodeReturnsNullIfCodeHasExpired()
    {
        var token = Guid.NewGuid().ToString();
        var code = "123456";
        var tfa = new TwoFactorAuthentication
        {
            Token = Guid.Parse(token),
            Code = code,
            ExpiresAt = DateTime.UtcNow.AddMinutes(-10)
        };

        _mockTwoFactorAuthDal.Setup(x => x.GetCodeByToken(tfa.Token)).ReturnsAsync(tfa);

        var result = await _sut.VerifyTwoFactorAuthCode(token, code);

        Assert.Null(result);
        _mockTwoFactorAuthDal.Verify(x => x.GetCodeByToken(tfa.Token), Times.Once);
    }

    [Fact]
    public async Task DeleteTwoFactorAuthCodeByIdCallsDeleteMethod()
    {
        var tfaId = Guid.NewGuid();

        _mockTwoFactorAuthDal.Setup(x => x.DeleteTwoFactorAuthCodeById(tfaId)).Returns(Task.CompletedTask);

        await _sut.DeleteTwoFactorAuthCodeById(tfaId);

        _mockTwoFactorAuthDal.Verify(x => x.DeleteTwoFactorAuthCodeById(tfaId), Times.Once);
    }
}
