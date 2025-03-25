using DfT.DTRO.Models.Pagination;

namespace Dft.DTRO.Tests.DALTests
{
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
            var user1 = new User { Id = Guid.NewGuid(), Email = "user@test.com" };
            var user2 = new User { Id = Guid.NewGuid(), Email = "anotheruser@test.com" };

            var tra1 = new TrafficRegulationAuthority { Id = Guid.NewGuid(), Name = "TRA1" };
            var tra2 = new TrafficRegulationAuthority { Id = Guid.NewGuid(), Name = "TRA2" };

            var typeA = new ApplicationType { Id = Guid.NewGuid(), Name = "TypeA" };
            var typeB = new ApplicationType { Id = Guid.NewGuid(), Name = "TypeB" };

            var statusA = new ApplicationStatus { Id = Guid.NewGuid(), Status = "Active" };
            var statusB = new ApplicationStatus { Id = Guid.NewGuid(), Status = "Inactive" };

            _context.Users.AddRange(user1, user2);
            _context.TrafficRegulationAuthorities.AddRange(tra1, tra2);
            _context.ApplicationTypes.AddRange(typeA, typeB);
            _context.ApplicationStatus.AddRange(statusA, statusB);
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
                    Purpose = "PurposeA",
                    Status = statusB
                },
                new Application
                {
                    Id = Guid.NewGuid(),
                    Nickname = "TestApp2",
                    User = user2,
                    ApplicationType = typeB,
                    TrafficRegulationAuthority = tra2,
                    Purpose = "PurposeB",
                    Status = statusB
                },
                new Application
                {
                    Id = Guid.NewGuid(),
                    Nickname = "AnotherApp",
                    User = user1,
                    ApplicationType = typeA,
                    TrafficRegulationAuthority = tra1,
                    Purpose = "PurposeC",
                    Status = statusB
                },
            });

            _context.SaveChanges();
        }

        [Fact]
        public async Task CheckApplicationNameDoesNotExistShouldReturnFalseWhenNameExists()
        {
            bool result = await _applicationDal.CheckApplicationNameDoesNotExist("TestApp1");
            Assert.False(result);
        }

        [Fact]
        public async Task CheckApplicationNameDoesNotExistShouldReturnTrueWhenNameDoesNotExist()
        {
            bool result = await _applicationDal.CheckApplicationNameDoesNotExist("NonExistentApp");
            Assert.True(result);
        }

        [Fact]
        public async Task GetApplicationUserShouldReturnUserEmailWhenAppExists()
        {
            var app = _context.Applications.First();
            var appGuid = app.Id;

            string userEmail = await _applicationDal.GetApplicationUser(appGuid);
            Assert.Equal(app.User.Email, userEmail);
        }

        [Fact]
        public async Task GetApplicationUserShouldReturnNullWhenAppDoesNotExist()
        {
            string userEmail = await _applicationDal.GetApplicationUser(Guid.NewGuid());
            Assert.Null(userEmail);
        }

        [Fact]
        public async Task GetApplicationDetailsShouldReturnApplicationDetailsWhenAppExists()
        {
            var app = _context.Applications.First();
            var appId = app.Id;

            var details = await _applicationDal.GetApplicationDetails(appId);
            Assert.NotNull(details);
            Assert.Equal(app.Nickname, details.Name);
        }

        [Fact]
        public async Task GetApplicationDetailsShouldReturnNullWhenInvalidAppId()
        {
            Guid appId = Guid.NewGuid();
            var details = await _applicationDal.GetApplicationDetails(appId);
            Assert.Null(details);
        }

        [Fact]
        public async Task GetApplicationListShouldReturnApplicationForUserWithSingleApp()
        {
            var apps = await _applicationDal.GetApplicationList("anotheruser@test.com");
            Assert.Single(apps);
            Assert.Equal("TestApp2", apps[0].Name);
        }

        [Fact]
        public async Task GetApplicationListShouldReturnApplicationsForUserWithMultipleApps()
        {
            var apps = await _applicationDal.GetApplicationList("user@test.com");
            Assert.Equal(2, apps.Count);
        }

        [Fact]
        public async Task GetApplicationList_ShouldReturnEmptyListWhenNoAppsForUser()
        {
            var apps = await _applicationDal.GetApplicationList("nonexistent@test.com");
            Assert.Empty(apps);
        }

        [Fact]
        public async Task ActivateApplicationByIdApplicationNotFoundReturnsFalse()
        {
            var appGuid = Guid.NewGuid(); // Non-existing GUID
            var result = await _applicationDal.ActivateApplicationById(appGuid);
            Assert.False(result);
        }

        [Fact]
        public async Task ActivateApplicationByIdApplicationStatusUpdatedReturnsTrue()
        {
            Application application = _context.Applications.First();
            ApplicationStatus activeStatus = await _context.ApplicationStatus.FirstOrDefaultAsync(a => a.Status == "Active");
            ApplicationStatus inactiveStatus = await _context.ApplicationStatus.FirstOrDefaultAsync(a => a.Status == "Inactive");
            Assert.Equal(application.Status, inactiveStatus);

            var result = await _applicationDal.ActivateApplicationById(application.Id);
            Assert.True(result);
            application = _context.Applications.First();
            Assert.Equal(application.Status, activeStatus);
        }

        [Fact]
        public async Task ActivateApplicationReturnsFalseWHenActiveStatusIsNull()
        {
            Application application = _context.Applications.First();
            var activeStatuses = await _context.ApplicationStatus
                .Where(a => a.Status == "Active").ToListAsync();

            _context.ApplicationStatus.RemoveRange(activeStatuses);
            await _context.SaveChangesAsync();

            var result = await _applicationDal.ActivateApplicationById(application.Id);
            Assert.False(result);
        }

        [Fact]
        public async Task GetInactiveApplications_ShouldReturnInactiveApplicationForUserAdminUser()
        {
            var request = new PaginatedRequest { Page = 1, PageSize = 1 };
            var apps = await _applicationDal.GetInactiveApplications(request);
            Assert.Equal(1, apps.TotalCount);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}