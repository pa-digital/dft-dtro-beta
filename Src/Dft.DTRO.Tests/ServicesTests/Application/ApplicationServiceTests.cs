namespace Dft.DTRO.Tests.ServicesTests.Application;

public class ApplicationServiceTests
{
    private readonly Mock<IApplicationDal> _applicationDalMock;
    private readonly Mock<IApigeeAppRepository> _apigeeAppRepositoryMock;
    private readonly ApplicationService _applicationService;

    public ApplicationServiceTests()
    {
        _applicationDalMock = new Mock<IApplicationDal>();
        _apigeeAppRepositoryMock = new Mock<IApigeeAppRepository>();
        _applicationService = new ApplicationService(_applicationDalMock.Object, _apigeeAppRepositoryMock.Object);
    }

    [Fact]
    public async Task ValidateAppBelongsToUserShouldReturnTrueWhenUserMatches()
    {
        string userId = "user@test.com";
        string appId = Guid.NewGuid().ToString();
        _applicationDalMock
            .Setup(dal => dal.GetApplicationUser(Guid.Parse(appId)))
            .ReturnsAsync(userId);

        bool result = await _applicationService.ValidateAppBelongsToUser(userId, appId);
        Assert.True(result);
    }

    [Fact]
    public async Task ValidateAppBelongsToUserShouldReturnFalseWhenUserDoesNotMatch()
    {
        string userId = "user@test.com";
        string differentUserId = "another@test.com";
        string appId = Guid.NewGuid().ToString();
        _applicationDalMock
            .Setup(dal => dal.GetApplicationUser(Guid.Parse(appId)))
            .ReturnsAsync(differentUserId);

        bool result = await _applicationService.ValidateAppBelongsToUser(userId, appId);
        Assert.False(result);
    }

    [Fact]
    public async Task ValidateApplicationNameShouldReturnTrueWhenNameDoesNotAlreadyExist()
    {
        string appName = "TestApp";
        _applicationDalMock
            .Setup(dal => dal.CheckApplicationNameDoesNotExist(appName))
            .ReturnsAsync(true);

        bool result = await _applicationService.ValidateApplicationName(appName);
        Assert.True(result);
    }

    [Fact]
    public async Task ValidateApplicationNameShouldReturnFalseWhenNameAlreadyExists()
    {
        string appName = "ExistingApp";
        _applicationDalMock
            .Setup(dal => dal.CheckApplicationNameDoesNotExist(appName))
            .ReturnsAsync(false);

        bool result = await _applicationService.ValidateApplicationName(appName);
        Assert.False(result);
    }

    [Fact]
    public async Task GetApplicationDetailsShouldReturnApplicationDetails()
    {
        Guid appGuid = Guid.NewGuid();
        string appId = appGuid.ToString();
        var expectedDetails = new ApplicationDetailsDto { AppId = appGuid, Name = "Test", Purpose = "Test" };
        _applicationDalMock
            .Setup(dal => dal.GetApplicationDetails(appId))
            .ReturnsAsync(expectedDetails);

        var result = await _applicationService.GetApplicationDetails(appId);
        Assert.Equal(expectedDetails, result);
    }

    [Fact]
    public async Task GetApplicationListShouldReturnListOfApplications()
    {
        string userId = "user@test.com";
        var expectedList = new List<ApplicationListDto> {
            new ApplicationListDto{ Id = Guid.NewGuid(), Name = "Test", Tra = "Test", Type = "Test" },
            new ApplicationListDto{ Id = Guid.NewGuid(), Name = "Another Test", Tra = "test TRA", Type = "Publish" }
        };
        _applicationDalMock
            .Setup(dal => dal.GetApplicationList(userId))
            .ReturnsAsync(expectedList);

        var result = await _applicationService.GetApplicationList(userId);
        Assert.Equal(expectedList, result);
    }

    [Fact]
    public async Task ActivateApplicationByIdSuccessfulActivationReturnsTrue()
    {
        string validAppId = Guid.NewGuid().ToString();
        _applicationDalMock
            .Setup(dal => dal.ActivateApplicationById(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        var result = await _applicationService.ActivateApplicationById(validAppId);
        Assert.True(result);
    }

    [Fact]
    public async Task ActivateApplicationByIdActivationFailsReturnsFalse()
    {
        string validAppId = Guid.NewGuid().ToString();
        _applicationDalMock
            .Setup(dal => dal.ActivateApplicationById(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        var result = await _applicationService.ActivateApplicationById(validAppId);
        Assert.False(result);
    }

    [Fact]
    public async Task ActivateApplicationByIdUnexpectedExceptionThrowsException()
    {
        string validAppId = Guid.NewGuid().ToString();
        _applicationDalMock
            .Setup(dal => dal.ActivateApplicationById(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception("Unexpected error"));

        var ex = await Assert.ThrowsAsync<Exception>(() => _applicationService.ActivateApplicationById(validAppId));
        Assert.Equal("Unexpected error", ex.Message);
    }
}