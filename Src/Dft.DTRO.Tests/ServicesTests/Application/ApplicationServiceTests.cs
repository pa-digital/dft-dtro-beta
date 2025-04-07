namespace Dft.DTRO.Tests.ServicesTests.Application;

public class ApplicationServiceTests
{
    private readonly Mock<IApplicationDal> _applicationDalMock;
    private readonly Mock<IApigeeAppRepository> _apigeeAppRepositoryMock;
    private readonly Mock<ITraDal> _traDalMock;
    private readonly Mock<IUserDal> _userDalMock;
    private readonly ApplicationService _applicationService;
    private readonly DtroContext _dtroContext;
    private readonly Mock<IDtroUserDal> _dtroUserDalMock;
    private readonly string _email;

    public ApplicationServiceTests()
    {
        _applicationDalMock = new Mock<IApplicationDal>();
        _apigeeAppRepositoryMock = new Mock<IApigeeAppRepository>();
        _traDalMock = new Mock<ITraDal>();
        _userDalMock = new Mock<IUserDal>();
        _dtroUserDalMock = new Mock<IDtroUserDal>();

        var options = new DbContextOptionsBuilder<DtroContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dtroContext = new DtroContext(options);

        _applicationService = new ApplicationService(
            _applicationDalMock.Object,
            _apigeeAppRepositoryMock.Object,
            _traDalMock.Object,
            _userDalMock.Object,
            _dtroContext,
            _dtroUserDalMock.Object
        );
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
    public async Task GetApplicationShouldReturnApplication()
    {
        Guid appId = Guid.NewGuid();
        var expectedDetails = new ApplicationDetailsDto { AppId = appId, Name = "Test", Purpose = "Test" };
        
        var expectedDeveloperApp = new ApigeeDeveloperApp()
        {
            AppId = appId.ToString(),
            Name = "Test",
            CreatedAt = 0, 
            LastModifiedAt = 0, 
            Status = "approved", 
            DeveloperId = "dev-123",
            Credentials = new List<ApigeeDeveloperAppCredential>
            {
                new ApigeeDeveloperAppCredential
                {
                    ConsumerKey = "key-123",
                    ConsumerSecret = "secret-456",
                    ExpiresAt = -1,
                    IssuedAt = 0,
                    Status = "approved"
                }
            }
        };
        
        var expectedResponse = new ApplicationResponse
        {
            AppId = appId,
            Name = "Test",
            Purpose = "Test",
            SwaCode = 123,
            CreatedAt = 0, 
            LastModifiedAt = 0, 
            Status = "approved", 
            DeveloperId = "dev-123",
            Credentials = new List<AppCredential>
            {
                new AppCredential
                {
                    ConsumerKey = "key-123",
                    ConsumerSecret = "secret-456",
                    ExpiresAt = -1,
                    IssuedAt = 0,
                    Status = "approved"
                }
            }
        };
        
        _applicationDalMock
            .Setup(dal => dal.GetApplicationDetails(appId))
            .ReturnsAsync(expectedDetails);
        
        _apigeeAppRepositoryMock
            .Setup(repository => repository.GetApp(_email, "Test"))
            .ReturnsAsync(expectedDeveloperApp);

        var result = await _applicationService.GetApplication(_email, appId);
        Assert.Equal(expectedResponse.AppId, result.AppId);
    }

    [Fact]
    public async Task GetApplicationsShouldReturnApplications()
    {
        var expectedList = new List<ApplicationListDto> {
            new ApplicationListDto{ Id = Guid.NewGuid(), Name = "Test", Tra = "Test", Type = "Test" },
            new ApplicationListDto{ Id = Guid.NewGuid(), Name = "Another Test", Tra = "test TRA", Type = "Publish" }
        };
        _applicationDalMock
            .Setup(dal => dal.GetApplicationList(_email))
            .ReturnsAsync(expectedList);

        var result = await _applicationService.GetApplications(_email);
        Assert.Equal(expectedList, result);
    }
    
    [Fact]
    public async Task GetInactiveApplicationsShouldReturnInactiveApplications()
    {
        
        var request = new PaginatedRequest { Page = 1, PageSize = 1 };

        var expectedList = new List<ApplicationInactiveListDto> {
            new() { TraName = "TraName", Type = "Type", UserEmail = "UserEmail", Username = "Username" },
            new() { TraName = "TraName2", Type = "Type2", UserEmail = "UserEmail2", Username = "Username2" }
        };
        
        var paginatedResult = new PaginatedResult<ApplicationInactiveListDto>(expectedList, 2);

        var paginatedResponse = new PaginatedResponse<ApplicationInactiveListDto>(expectedList, 1, 2);

        _applicationDalMock
            .Setup(dal => dal.GetInactiveApplications(request))
            .ReturnsAsync(paginatedResult);

        var result = await _applicationService.GetInactiveApplications(request);
        Assert.Equal(paginatedResponse.TotalCount, result.TotalCount);
    }

    [Fact]
    public async Task ActivateApplicationByIdSuccessfulActivationReturnsTrue()
    {
        Guid appId = Guid.NewGuid();
        
        var expectedDetails = new ApplicationDetailsDto { AppId = appId, Name = "Test", Purpose = "Test" };
        
        var expectedDeveloperApp = new ApigeeDeveloperApp()
        {
            AppId = appId.ToString(),
            Name = "Test",
            CreatedAt = 0, 
            LastModifiedAt = 0, 
            Status = "approved", 
            DeveloperId = "dev-123",
            Credentials = new List<ApigeeDeveloperAppCredential>
            {
                new ApigeeDeveloperAppCredential
                {
                    ConsumerKey = "key-123",
                    ConsumerSecret = "secret-456",
                    ExpiresAt = -1,
                    IssuedAt = 0,
                    Status = "approved"
                }
            }
        };
        
        _applicationDalMock
            .Setup(dal => dal.GetApplicationDetails(appId))
            .ReturnsAsync(expectedDetails);
        
        _applicationDalMock
            .Setup(dal => dal.ActivateApplicationById(It.IsAny<Guid>()))
            .ReturnsAsync(true);
        
        _applicationDalMock
            .Setup(dal => dal.ActivateApplicationById(It.IsAny<Guid>()))
            .ReturnsAsync(true);
        
        _apigeeAppRepositoryMock
            .Setup(repository => repository.UpdateAppStatus(_email, "Test", "approve"))
            .ReturnsAsync(expectedDeveloperApp);

        var result = await _applicationService.ActivateApplicationById(_email, appId);
        Assert.True(result);
    }

    [Fact]
    public async Task ActivateApplicationByIdActivationFailsReturnsFalse()
    {
        
        Guid appId = Guid.NewGuid();
        
        var expectedDetails = new ApplicationDetailsDto { AppId = appId, Name = "Test", Purpose = "Test" };

        var expectedDeveloperApp = new ApigeeDeveloperApp()
        {
            AppId = appId.ToString(),
            Name = "Test",
            CreatedAt = 0, 
            LastModifiedAt = 0, 
            Status = "approved", 
            DeveloperId = "dev-123",
            Credentials = new List<ApigeeDeveloperAppCredential>
            {
                new ApigeeDeveloperAppCredential
                {
                    ConsumerKey = "key-123",
                    ConsumerSecret = "secret-456",
                    ExpiresAt = -1,
                    IssuedAt = 0,
                    Status = "approved"
                }
            }
        };
        
        _applicationDalMock
            .Setup(dal => dal.GetApplicationDetails(appId))
            .ReturnsAsync(expectedDetails);
        
        _applicationDalMock
            .Setup(dal => dal.ActivateApplicationById(It.IsAny<Guid>()))
            .ReturnsAsync(false);
        
        _apigeeAppRepositoryMock
            .Setup(repository => repository.UpdateAppStatus(_email, "Test", "approve"))
            .ReturnsAsync(expectedDeveloperApp);

        var result = await _applicationService.ActivateApplicationById(_email, appId);
        Assert.False(result);
    }

    [Fact]
    public async Task ActivateApplicationByIdUnexpectedExceptionThrowsException()
    {
        Guid appId = Guid.NewGuid();
        
        var expectedDetails = new ApplicationDetailsDto { AppId = appId, Name = "Test", Purpose = "Test" };

        var expectedDeveloperApp = new ApigeeDeveloperApp()
        {
            AppId = appId.ToString(),
            Name = "Test",
            CreatedAt = 0, 
            LastModifiedAt = 0, 
            Status = "approved", 
            DeveloperId = "dev-123",
            Credentials = new List<ApigeeDeveloperAppCredential>
            {
                new ApigeeDeveloperAppCredential
                {
                    ConsumerKey = "key-123",
                    ConsumerSecret = "secret-456",
                    ExpiresAt = -1,
                    IssuedAt = 0,
                    Status = "approved"
                }
            }
        };
        
        _applicationDalMock
            .Setup(dal => dal.GetApplicationDetails(appId))
            .ReturnsAsync(expectedDetails);
        
        _applicationDalMock
            .Setup(dal => dal.ActivateApplicationById(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception("Unexpected error"));
        
        _apigeeAppRepositoryMock
            .Setup(repository => repository.UpdateAppStatus(_email, "Test", "approve"))
            .ReturnsAsync(expectedDeveloperApp);

        var ex = await Assert.ThrowsAsync<Exception>(() => _applicationService.ActivateApplicationById(_email, appId));
        Assert.Equal("Unexpected error", ex.Message);
    }
}