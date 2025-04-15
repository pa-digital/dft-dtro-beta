using DfT.DTRO.DAL;
using DfT.DTRO.Models;
using DfT.DTRO.Models.TwoFactorAuth;
using DfT.DTRO.Utilities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Dft.DTRO.Tests.DALTests;

public class TwoFactorAuthDalTests : IDisposable
{
    private readonly DtroContext _context;
    private readonly TwoFactorAuthDal _twoFactorAuthDal;
    private readonly Mock<IAuthHelper> _authHelperMock;

    public TwoFactorAuthDalTests()
    {
        var options = new DbContextOptionsBuilder<DtroContext>()
            .UseInMemoryDatabase(databaseName: "TwoFactorTestDb")
            .Options;

        _context = new DtroContext(options);
        _authHelperMock = new Mock<IAuthHelper>();

        SeedDatabase();

        _twoFactorAuthDal = new TwoFactorAuthDal(_context, _authHelperMock.Object);
    }

    private void SeedDatabase()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "testuser@test.com"
        };

        var tfa = new TwoFactorAuthentication
        {
            Id = Guid.NewGuid(),
            Token = Guid.NewGuid(),
            Code = "999999",
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            User = user,
            UserId = user.Id
        };

        _context.Users.Add(user);
        _context.TwoFactorAuthentication.Add(tfa);
        _context.SaveChanges();
    }

    [Fact]
    public async Task SaveTwoFactorAuthCode_ShouldCreateAndReturnCode()
    {
        var user = _context.Users.First();
        _authHelperMock.Setup(x => x.GenerateTwoFactorCode()).Returns("123456");

        var result = await _twoFactorAuthDal.SaveTwoFactorAuthCode(user);

        Assert.NotNull(result);
        Assert.Equal("123456", result.Code);
        Assert.Equal(user.Id, result.UserId);
        Assert.True(result.ExpiresAt > DateTime.UtcNow);
    }

    [Fact]
    public async Task GetCodeByToken_ShouldReturnCorrectRecord()
    {
        var tfa = _context.TwoFactorAuthentication.Include(x => x.User).First();

        var result = await _twoFactorAuthDal.GetCodeByToken(tfa.Token);

        Assert.NotNull(result);
        Assert.Equal(tfa.Id, result.Id);
        Assert.Equal("999999", result.Code);
        Assert.Equal("testuser@test.com", result.User.Email);
    }

    [Fact]
    public async Task GetCodeByToken_ShouldReturnNullWhenNotFound()
    {
        var result = await _twoFactorAuthDal.GetCodeByToken(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteTwoFactorAuthCodeById_ShouldDeleteRecord()
    {
        var tfa = _context.TwoFactorAuthentication.First();

        await _twoFactorAuthDal.DeleteTwoFactorAuthCodeById(tfa.Id);

        var exists = await _context.TwoFactorAuthentication.AnyAsync(x => x.Id == tfa.Id);
        Assert.False(exists);
    }

    [Fact]
    public async Task DeleteTwoFactorAuthCodeById_ShouldDoNothingIfNotFound()
    {
        var id = Guid.NewGuid();

        var exception = await Record.ExceptionAsync(() => _twoFactorAuthDal.DeleteTwoFactorAuthCodeById(id));

        Assert.Null(exception);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
