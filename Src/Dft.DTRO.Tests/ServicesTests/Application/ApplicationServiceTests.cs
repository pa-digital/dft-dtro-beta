namespace Dft.DTRO.Tests.ServicesTests.Application;

public class ApplicationServiceTests
{
    private readonly Mock<IApplicationDal> _applicationDalMock;
    private readonly ApplicationService _applicationService;

    public ApplicationServiceTests()
    {
        _applicationDalMock = new Mock<IApplicationDal>();
        Mock<IApigeeAppRepository> mockApigeeRepository = new();
        _applicationService = new ApplicationService(_applicationDalMock.Object, mockApigeeRepository.Object);
    }

    [Fact]
    public void ValidateAppBelongsToUserShouldReturnTrueWhenUserMatches()
    {
        string userId = "user@test.com";
        string appId = Guid.NewGuid().ToString();
        _applicationDalMock
            .Setup(dal => dal.GetApplicationUser(Guid.Parse(appId)))
            .Returns(userId);

        bool result = _applicationService.ValidateAppBelongsToUser(userId, appId);
        Assert.True(result);
    }

    [Fact]
    public void ValidateAppBelongsToUserShouldReturnFalseWhenUserDoesNotMatch()
    {
        string userId = "user@test.com";
        string differentUserId = "another@test.com";
        string appId = Guid.NewGuid().ToString();
        _applicationDalMock
            .Setup(dal => dal.GetApplicationUser(Guid.Parse(appId)))
            .Returns(differentUserId);

        bool result = _applicationService.ValidateAppBelongsToUser(userId, appId);
        Assert.False(result);
    }

    [Fact]
    public void ValidateApplicationNameShouldReturnTrueWhenNameDoesNotAlreadyExist()
    {
        string appName = "TestApp";
        _applicationDalMock
            .Setup(dal => dal.CheckApplicationNameDoesNotExist(appName))
            .Returns(true);

        bool result = _applicationService.ValidateApplicationName(appName);
        Assert.True(result);
    }

    [Fact]
    public void ValidateApplicationNameShouldReturnFalseWhenNameAlreadyExists()
    {
        string appName = "ExistingApp";
        _applicationDalMock
            .Setup(dal => dal.CheckApplicationNameDoesNotExist(appName))
            .Returns(false);

        bool result = _applicationService.ValidateApplicationName(appName);
        Assert.False(result);
    }

    [Fact]
    public void GetApplicationDetailsShouldReturnApplicationDetails()
    {
        Guid appGuid = Guid.NewGuid();
        string appId = appGuid.ToString();
        var expectedDetails = new ApplicationDetailsDto { AppId = appGuid, Name = "Test", Purpose = "Test" };
        _applicationDalMock
            .Setup(dal => dal.GetApplicationDetails(appId))
            .Returns(expectedDetails);

        var result = _applicationService.GetApplicationDetails(appId);
        Assert.Equal(expectedDetails, result);
    }

    [Fact]
    public void GetApplicationListShouldReturnListOfApplications()
    {
        string userId = "user@test.com";
        var expectedList = new List<ApplicationListDto> {
            new ApplicationListDto{ Id = Guid.NewGuid(), Name = "Test", Tra = "Test", Type = "Test" },
            new ApplicationListDto{ Id = Guid.NewGuid(), Name = "Another Test", Tra = "test TRA", Type = "Publish" }
        };
        _applicationDalMock
            .Setup(dal => dal.GetApplicationList(userId))
            .Returns(expectedList);

        var result = _applicationService.GetApplicationList(userId);
        Assert.Equal(expectedList, result);
    }
}