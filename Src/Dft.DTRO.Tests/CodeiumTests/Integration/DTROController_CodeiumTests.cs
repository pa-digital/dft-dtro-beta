using DfT.DTRO.Controllers;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.Errors;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.SharedResponse;
using DfT.DTRO.RequestCorrelation;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;


namespace DfT.DTRO.Tests.CodeiumTests.Integration
{
    public class DTROsController_Codeium_Tests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly Mock<IDtroService> _mockDtroService;
        private readonly Mock<IRequestCorrelationProvider> _correlationProviderMock;
        private readonly Mock<ILogger<DTROsController>> _loggerMock;
        private readonly DTROsController _controller;

        private readonly WebApplicationFactory<Program> _factory;

        private const string ValidDtroJsonPath = "./DtroJsonDataExamples/3.1.2-valid-complex-dtro.json";
        private readonly DtroSubmit dtroSubmit;


        public DTROsController_Codeium_Tests(WebApplicationFactory<Program> factory)
        {
            _mockDtroService = new Mock<IDtroService>();
            _correlationProviderMock = new Mock<IRequestCorrelationProvider>();
            _loggerMock = new Mock<ILogger<DTROsController>>();
            _controller = new DTROsController(_mockDtroService.Object,
            _correlationProviderMock.Object, _loggerMock.Object);

            _factory = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(_mockDtroService.Object);
                services.AddSingleton(_correlationProviderMock.Object);
                services.AddSingleton(_loggerMock.Object);
            }));

            string json = File.ReadAllText(ValidDtroJsonPath);
            ExpandoObject? dtroData = JsonConvert.DeserializeObject<ExpandoObject>
            (json, new ExpandoObjectConverter());

            dtroSubmit = new DtroSubmit
            {
                SchemaVersion = new SchemaVersion(3, 1, 2),
                Data = dtroData
            };

        }

        [Fact]
        public async Task CreateDtro_Returns_OK()
        {
            // Arrange
            var response = new GuidResponse { Id = Guid.NewGuid() };
            _mockDtroService.Setup(x => x.SaveDtroAsJsonAsync(dtroSubmit, It.IsAny<string>())).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateDtro(dtroSubmit);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
            Assert.Equal(nameof(_controller.GetDtroById), createdAtActionResult.ActionName);
            Assert.Equal(response.Id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(response, createdAtActionResult.Value);
        }

        [Fact]
        public async Task CreateDtro_ValidationException_ReturnsBadRequest()
        {
            // Arrange
            var dtroBadSubmit = new DtroSubmit();
            _mockDtroService.Setup(s => s.SaveDtroAsJsonAsync(dtroBadSubmit, _correlationProviderMock.Object.CorrelationId))
                .ThrowsAsync(new DtroValidationException());

            // Act
            var response = await _controller.CreateDtro(dtroBadSubmit);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response);
            _mockDtroService.Verify(s => s.SaveDtroAsJsonAsync(dtroBadSubmit, _correlationProviderMock.Object.CorrelationId), Times.Once);
        }

        [Fact]
        public async Task CreateDtro_UnexpectedException_ReturnsInternalServerError()
        {
            // Arrange
            var dtroBadSubmit = new DtroSubmit();
            _mockDtroService.Setup(s => s.SaveDtroAsJsonAsync(dtroBadSubmit, _correlationProviderMock.Object.CorrelationId))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var response = await _controller.CreateDtro(dtroBadSubmit);

            // Assert
            Assert.IsType<ObjectResult>(response);
            var objectResult = (ObjectResult)response;
            Assert.Equal(500, objectResult.StatusCode);

            _mockDtroService.Verify(s => s.SaveDtroAsJsonAsync(dtroBadSubmit, _correlationProviderMock.Object.CorrelationId), Times.Once);
        }

        [Fact]
        public async Task UpdateDtro_ReturnsOk_ForExistingDtro()
        {
            // Arrange
            _mockDtroService.Setup(s => s.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>()))
            .Returns(Task.FromResult(new GuidResponse()));

            // Act
            var result = await _controller.UpdateDtro(Guid.NewGuid(), dtroSubmit);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockDtroService.Verify(s => s.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDtro_ReturnsBadRequest_ForInvalidDtro()
        {
            // Arrange
            _mockDtroService.Setup(s => s.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>()))
                .ThrowsAsync(new DtroValidationException());

            var dtroBadSubmit = new DtroSubmit
            { SchemaVersion = new("3.1.2"), Data = new() };

            // Act
            var result = await _controller.UpdateDtro(Guid.NewGuid(), dtroBadSubmit);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            _mockDtroService.Verify(s => s.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDtro_ReturnsNotFound_ForNonExistentDtro()
        {
            // Arrange
            _mockDtroService.Setup(s => s.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>()))
                .ThrowsAsync(new NotFoundException());

            var dtroBadSubmit = new DtroSubmit
            { SchemaVersion = new("3.1.2"), Data = new() };

            // Act
            var result = await _controller.UpdateDtro(Guid.NewGuid(), dtroBadSubmit);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            _mockDtroService.Verify(s => s.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDtro_ReturnsInternalServerError_ForUnexpectedException()
        {
            // Arrange
            _mockDtroService.Setup(s => s.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var dtroBadSubmit = new DtroSubmit
            { SchemaVersion = new("3.1.2"), Data = new() };

            // Act
            var result = await _controller.UpdateDtro(Guid.NewGuid(), dtroBadSubmit) as ObjectResult;

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, result?.StatusCode);

            _mockDtroService.Verify(s => s.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetDtroById_ExistingDtro_ReturnsDtro()
        {
            // Arrange
            var dtroId = Guid.NewGuid();
            var dtroResponse = new DtroResponse();
            _mockDtroService.Setup(s => s.GetDtroByIdAsync(dtroId)).ReturnsAsync(dtroResponse);

            // Act
            var result = await _controller.GetDtroById(dtroId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dtroResponse, result?.Value);
            _mockDtroService.Verify(s => s.GetDtroByIdAsync(dtroId), Times.Once);
        }

        [Fact]
        public async Task GetDtroById_NonExistingDtro_ReturnsNotFound()
        {
            // Arrange
            _mockDtroService.Setup(s => s.GetDtroByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<DtroResponse?>(null));

            // Act
            var response = await _controller.GetDtroById(Guid.NewGuid()) as NotFoundResult;

            // Assert
            Assert.Null(response);
            _mockDtroService.Verify(s => s.GetDtroByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetDtroById_Exception_ReturnsInternalServerError()
        {
            // Arrange
            _mockDtroService.Setup(s => s.GetDtroByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetDtroById(Guid.NewGuid()) as ObjectResult;

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, result?.StatusCode);
            _mockDtroService.Verify(s => s.GetDtroByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDtro_ValidId_ReturnsNoContent()
        {
            // Arrange
            _mockDtroService.Setup(s => s.DeleteDtroAsync(It.IsAny<Guid>(), null)).Returns(Task.FromResult(true));

            // Act
            var response = await _controller.DeleteDtro(Guid.NewGuid()) as NoContentResult;

            // Assert
            Assert.NotNull(response);
            Assert.IsType<NoContentResult>(response);
            Assert.Equal(204, response?.StatusCode);
        }

        [Fact]
        public async Task DeleteDtro_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockDtroService.Setup(s => s.DeleteDtroAsync(It.IsAny<Guid>(), null)).Returns(Task.FromResult(true));

            // Act
            var response = await _controller.DeleteDtro(Guid.NewGuid()) as NoContentResult;

            Assert.NotNull(response);
            Assert.IsType<NoContentResult>(response);
            Assert.Equal(204, response?.StatusCode);

        }

        [Fact]
        public async Task DeleteDtro_ReturnsInternalServerError()
        {
            // Arrange
            _mockDtroService.Setup(s => s.DeleteDtroAsync(It.IsAny<Guid>(), null)).ThrowsAsync(new Exception());

            // Act
            var response = await _controller.DeleteDtro(Guid.NewGuid()) as ObjectResult;

            Assert.NotNull(response);
            Assert.IsType<ObjectResult>(response);
            Assert.Equal(500, response?.StatusCode);
        }

    }
}