namespace DfT.DTRO.Tests.Services
{
    public class EnvironmentServiceTests
    {
        private readonly Mock<IApplicationDal> _mockApplicationDal;
        private readonly Mock<IUserDal> _mockUserDal;
        private readonly EnvironmentService _service;

        public EnvironmentServiceTests()
        {
            _mockApplicationDal = new Mock<IApplicationDal>();
            _mockUserDal = new Mock<IUserDal>();
            _service = new EnvironmentService(_mockApplicationDal.Object, _mockUserDal.Object);
        }

        [Fact]
        public async Task CanRequestProductionAccessReturnsTrueWhenUserHasApplicationsAndHasNotRequestedAccess()
        {
            string email = "user@test.com";
            var user = new User { Id = Guid.NewGuid() };

            _mockUserDal.Setup(x => x.GetUserFromEmail(email)).ReturnsAsync(user);
            _mockApplicationDal.Setup(x => x.GetUserApplicationsCount(user.Id)).ReturnsAsync(3);
            _mockUserDal.Setup(x => x.HasUserRequestedProductionAccess(user.Id)).ReturnsAsync(false);

            bool result = await _service.CanRequestProductionAccess(email);
            Assert.True(result);
        }

        [Fact]
        public async Task CanRequestProductionAccessReturnsFalseWhenUserHasNoApplications()
        {
            string email = "user@test.com";
            var user = new User { Id = Guid.NewGuid() };

            _mockUserDal.Setup(x => x.GetUserFromEmail(email)).ReturnsAsync(user);
            _mockApplicationDal.Setup(x => x.GetUserApplicationsCount(user.Id)).ReturnsAsync(0);
            _mockUserDal.Setup(x => x.HasUserRequestedProductionAccess(user.Id)).ReturnsAsync(false);

            bool result = await _service.CanRequestProductionAccess(email);
            Assert.False(result);
        }

        [Fact]
        public async Task CanRequestProductionAccessReturnsFalseWhenUserAlreadyRequestedAccess()
        {
            string email = "user@test.com";
            var user = new User { Id = Guid.NewGuid() };

            _mockUserDal.Setup(x => x.GetUserFromEmail(email)).ReturnsAsync(user);
            _mockApplicationDal.Setup(x => x.GetUserApplicationsCount(user.Id)).ReturnsAsync(2);
            _mockUserDal.Setup(x => x.HasUserRequestedProductionAccess(user.Id)).ReturnsAsync(true);

            bool result = await _service.CanRequestProductionAccess(email);
            Assert.False(result);
        }

        [Fact]
        public async Task RequestProductionAccessCallsRequestProductionAccessOnUserDal()
        {
            string email = "user@test.com";
            var user = new User { Id = Guid.NewGuid() };

            _mockUserDal.Setup(x => x.GetUserFromEmail(email)).ReturnsAsync(user);

            await _service.RequestProductionAccess(email);
            _mockUserDal.Verify(x => x.RequestProductionAccess(user.Id), Times.Once);
        }
    }
}
