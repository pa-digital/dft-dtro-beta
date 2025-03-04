namespace Dft.DTRO.Tests.ServicesTests.Application
{
    public class ApplicationServiceTests
    {
        private readonly Mock<IApplicationDal> _applicationDalMock;
        private readonly ApplicationService _applicationService;

        public ApplicationServiceTests()
        {
            _applicationDalMock = new Mock<IApplicationDal>();
            _applicationService = new ApplicationService(_applicationDalMock.Object);
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
}
