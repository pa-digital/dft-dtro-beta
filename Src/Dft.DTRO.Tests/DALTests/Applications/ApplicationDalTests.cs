namespace Dft.DTRO.Tests.DALTests.Applications;

public class ApplicationDalTests : IDisposable
{
    private readonly DtroContext _context;
    private readonly ApplicationDal _applicationDal;

    public ApplicationDalTests()
    {
        var options = new DbContextOptionsBuilder<DtroContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new DtroContext(options);

        SeedDatabase();
        _applicationDal = new ApplicationDal(_context);
    }

    private void SeedDatabase()
    {
        var user0 = new User { Id = Guid.NewGuid(), Email = "admin@test.com", IsCentralServiceOperator = true };
        var user1 = new User { Id = Guid.NewGuid(), Email = "user@test.com" };
        var user2 = new User { Id = Guid.NewGuid(), Email = "anotheruser@test.com" };

        var tra1 = new TrafficRegulationAuthority { Id = Guid.NewGuid(), Name = "TRA1" };
        var tra2 = new TrafficRegulationAuthority { Id = Guid.NewGuid(), Name = "TRA2" };

        var typeA = new ApplicationType { Id = Guid.NewGuid(), Name = "TypeA" };
        var typeB = new ApplicationType { Id = Guid.NewGuid(), Name = "TypeB" };

        var purposeA = new ApplicationPurpose { Id = Guid.NewGuid(), Description = "PurposeA" };
        var purposeB = new ApplicationPurpose { Id = Guid.NewGuid(), Description = "PurposeB" };
        var purposeC = new ApplicationPurpose { Id = Guid.NewGuid(), Description = "PurposeC" };

        _context.Users.AddRange(user0, user1, user2);
        _context.TrafficRegulationAuthorities.AddRange(tra1, tra2);
        _context.ApplicationTypes.AddRange(typeA, typeB);
        _context.ApplicationPurposes.AddRange(purposeA, purposeB);
        _context.SaveChanges();

        _context.Applications.AddRange(new List<Application>
        {
            new Application
            {
                Id = Guid.NewGuid(),
                Nickname = "TestApp1",
                User = user1,
                ApplicationType = typeA,
                TrafficRegulationAuthority = tra1,
                Purpose = purposeA
            },
            new Application
            {
                Id = Guid.NewGuid(),
                Nickname = "TestApp2",
                User = user2,
                ApplicationType = typeB,
                TrafficRegulationAuthority = tra2,
                Purpose = purposeB
            },
            new Application
            {
                Id = Guid.NewGuid(),
                Nickname = "AnotherApp",
                User = user1,
                ApplicationType = typeA,
                TrafficRegulationAuthority = tra1,
                Purpose = purposeC
            },
            new Application
            {
                Id = Guid.NewGuid(),
                Nickname = "Yet Another App",
                User = user0,
                ApplicationType = typeA,
                TrafficRegulationAuthority = tra1,
                Status = "pending"
            }
        });

        _context.SaveChanges();
    }

    [Fact]
    public void CheckApplicationNameDoesNotExistShouldReturnFalseWhenNameExists()
    {
        bool result = _applicationDal.CheckApplicationNameDoesNotExist("TestApp1");
        Assert.False(result);
    }

    [Fact]
    public void CheckApplicationNameDoesNotExistShouldReturnTrueWhenNameDoesNotExist()
    {
        bool result = _applicationDal.CheckApplicationNameDoesNotExist("NonExistentApp");
        Assert.True(result);
    }

    [Fact]
    public void GetApplicationUserShouldReturnUserEmailWhenAppExists()
    {
        var app = _context.Applications.First();
        var appGuid = app.Id;

        string userEmail = _applicationDal.GetApplicationUser(appGuid);
        Assert.Equal(app.User.Email, userEmail);
    }

    [Fact]
    public void GetApplicationUserShouldReturnNullWhenAppDoesNotExist()
    {
        string userEmail = _applicationDal.GetApplicationUser(Guid.NewGuid());
        Assert.Null(userEmail);
    }

    [Fact]
    public void GetApplicationDetailsShouldReturnApplicationDetailsWhenAppExists()
    {
        var app = _context.Applications.Include(application => application.Purpose).First();
        var appId = app.Id.ToString();

        var details = _applicationDal.GetApplicationDetails(appId);
        Assert.NotNull(details);
        Assert.Equal(app.Nickname, details.Name);
        Assert.Equal(app.Purpose.Description, details.Purpose);
    }

    [Fact]
    public void GetApplicationDetailsShouldReturnNullWhenInvalidAppId()
    {
        var details = _applicationDal.GetApplicationDetails("invalid-guid");
        Assert.Null(details);
    }

    [Fact]
    public void GetApplicationListShouldReturnApplicationForUserWithSingleApp()
    {
        var apps = _applicationDal.GetApplicationList("anotheruser@test.com");
        Assert.Single(apps);
        Assert.Equal("TestApp2", apps[0].Name);
    }

    [Fact]
    public void GetApplicationListShouldReturnApplicationsForUserWithMultipleApps()
    {
        var apps = _applicationDal.GetApplicationList("user@test.com");
        Assert.Equal(2, apps.Count);
    }

    [Fact]
    public void GetApplicationList_ShouldReturnEmptyListWhenNoAppsForUser()
    {
        var apps = _applicationDal.GetApplicationList("nonexistent@test.com");
        Assert.Empty(apps);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public void GetPendingApplications_ShouldReturnEmptyWhenNoAppsForUsers()
    {
        var apps = _applicationDal.GetApplicationList("nonexistent@test.com");
        Assert.Empty(apps);
    }

    [Fact]
    public void GetPendingApplications_ShouldReturnPendingApplicationForUserAdminUser()
    {
        var request = new PaginatedRequest { Page = 1, PageSize = 1 };
        var apps = _applicationDal.GetPendingApplications(request);
        Assert.Equal(1, apps.TotalCount);
    }
}