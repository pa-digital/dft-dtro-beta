namespace Dft.DTRO.Tests.ServicesTests.Application;

public class ApplicationServiceTests
{
    private readonly Mock<IApplicationDal> _applicationDalMock;
    private readonly Mock<IApigeeAppRepository> _apigeeAppRepositoryMock;
    private readonly ApplicationService _applicationService;
    private readonly string _email;

    public ApplicationServiceTests()
    {
        _applicationDalMock = new Mock<IApplicationDal>();
        _apigeeAppRepositoryMock = new Mock<IApigeeAppRepository>();
        _applicationService = new ApplicationService(_applicationDalMock.Object, _apigeeAppRepositoryMock.Object);
        _email = "user@test.com";
    }

    [Fact]
    public async Task ValidateAppBelongsToUserShouldReturnTrueWhenUserMatches()
    {
        Guid appId = Guid.NewGuid();
        _applicationDalMock
            .Setup(dal => dal.GetApplicationUser(appId))
            .ReturnsAsync(_email);

        bool result = await _applicationService.ValidateAppBelongsToUser(_email, appId);
        Assert.True(result);
    }

    [Fact]
    public async Task ValidateAppBelongsToUserShouldReturnFalseWhenUserDoesNotMatch()
    {
        string differentEmail = "another@test.com";
        Guid appId = Guid.NewGuid();
        _applicationDalMock
            .Setup(dal => dal.GetApplicationUser(appId))
            .ReturnsAsync(differentEmail);

        bool result = await _applicationService.ValidateAppBelongsToUser(_email, appId);
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
        Guid appId = Guid.NewGuid();
        var expectedDetails = new ApplicationDetailsDto { AppId = appId, Name = "Test", Purpose = "Test" };
        _applicationDalMock
            .Setup(dal => dal.GetApplicationDetails(appId))
            .ReturnsAsync(expectedDetails);

        var result = await _applicationService.GetApplicationDetails(appId);
        Assert.Equal(expectedDetails, result);
    }

    [Fact]
    public async Task GetApplicationListShouldReturnListOfApplications()
    {
        var expectedList = new List<ApplicationListDto> {
            new ApplicationListDto{ Id = Guid.NewGuid(), Name = "Test", Tra = "Test", Type = "Test" },
            new ApplicationListDto{ Id = Guid.NewGuid(), Name = "Another Test", Tra = "test TRA", Type = "Publish" }
        };
        _applicationDalMock
            .Setup(dal => dal.GetApplicationList(_email))
            .ReturnsAsync(expectedList);

        var result = await _applicationService.GetApplicationList(_email);
        Assert.Equal(expectedList, result);
    }

    [Fact]
    public async Task ActivateApplicationByIdSuccessfulActivationReturnsTrue()
    {
        Guid appId = Guid.NewGuid();
        _applicationDalMock
            .Setup(dal => dal.ActivateApplicationById(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        var result = await _applicationService.ActivateApplicationById(appId);
        Assert.True(result);
    }

    [Fact]
    public async Task ActivateApplicationByIdActivationFailsReturnsFalse()
    {
        Guid appId = Guid.NewGuid();
        _applicationDalMock
            .Setup(dal => dal.ActivateApplicationById(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        var result = await _applicationService.ActivateApplicationById(appId);
        Assert.False(result);
    }

    [Fact]
    public async Task ActivateApplicationByIdUnexpectedExceptionThrowsException()
    {
        Guid appId = Guid.NewGuid();
        _applicationDalMock
            .Setup(dal => dal.ActivateApplicationById(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception("Unexpected error"));

        var ex = await Assert.ThrowsAsync<Exception>(() => _applicationService.ActivateApplicationById(appId));
        Assert.Equal("Unexpected error", ex.Message);
    }
}