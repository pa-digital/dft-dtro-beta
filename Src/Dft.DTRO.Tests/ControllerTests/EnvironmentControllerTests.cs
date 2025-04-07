namespace DfT.DTRO.Tests.Controllers
{
    public class EnvironmentControllerTests
    {
        private readonly Mock<IEnvironmentService> _mockEnvironmentService;
        private readonly Mock<ILogger<ApplicationController>> _mockLogger;
        private readonly Mock<LoggingExtension> _mockLoggingExtension;
        private readonly EnvironmentController _controller;

        public EnvironmentControllerTests()
        {
            _mockEnvironmentService = new Mock<IEnvironmentService>();
            _mockLogger = new Mock<ILogger<ApplicationController>>();
            _mockLoggingExtension = new Mock<LoggingExtension>();
            _controller = new EnvironmentController(
                _mockEnvironmentService.Object,
                _mockLogger.Object,
                _mockLoggingExtension.Object
            );
        }

        [Fact]
        public async Task CanRequestProductionAccessReturnsOkWhenServiceReturnsTrue()
        {
            string email = "user@test.com";
            _mockEnvironmentService.Setup(s => s.CanRequestProductionAccess(email)).ReturnsAsync(true);

            var result = await _controller.CanRequestProductionAccess(email);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task CanRequestProductionAccessReturnsInternalServerErrorOnException()
        {
            string email = "user@test.com";
            _mockEnvironmentService
                .Setup(s => s.CanRequestProductionAccess(email))
                .ThrowsAsync(new Exception("Something went wrong"));

            var result = await _controller.CanRequestProductionAccess(email);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            var errorResponse = Assert.IsType<ApiErrorResponse>(objectResult.Value);
            Assert.Equal("Internal Server Error", errorResponse.Message);
            Assert.Contains("Something went wrong", errorResponse.Error);
        }

        [Fact]
        public async Task RequestProductionAccessReturnsOkWhenServiceSucceeds()
        {
            string email = "user@test.com";
            _mockEnvironmentService.Setup(s => s.RequestProductionAccess(email)).Returns(Task.CompletedTask);

            var result = await _controller.RequestProductionAccess(email);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RequestProductionAccessReturnsInternalServerErrorOnException()
        {
            string email = "user@test.com";
            _mockEnvironmentService
                .Setup(s => s.RequestProductionAccess(email))
                .ThrowsAsync(new Exception("Database is down"));

            var result = await _controller.RequestProductionAccess(email);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            var errorResponse = Assert.IsType<ApiErrorResponse>(objectResult.Value);
            Assert.Equal("Internal Server Error", errorResponse.Message);
            Assert.Contains("Database is down", errorResponse.Error);
        }
    }
}
