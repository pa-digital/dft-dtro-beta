﻿using DfT.DTRO.Models.SwaCode;

namespace Dft.DTRO.Tests.CodeiumTests.Tra.Controller;

public class TraControllerCreateFromBodyTests
{
    private readonly Mock<IDtroUserService> _traServiceMock;
    private readonly Mock<ILogger<DtroUserController>> _loggerMock;
    private readonly DtroUserController _controller;

    public TraControllerCreateFromBodyTests()
    {
        _traServiceMock = new Mock<IDtroUserService>();
        _loggerMock = new Mock<ILogger<DtroUserController>>();
        _controller = new DtroUserController(_traServiceMock.Object, _loggerMock.Object);
    }


    [Fact]
    public async Task CreateFromBody_ReturnsCreated_WithGuidResponse()
    {
        // Arrange
        var request = new DtroUserRequest { TraId = 1 };
        var response = new GuidResponse { Id = Guid.NewGuid() };
        _traServiceMock.Setup(service => service.SaveDtroUserAsync(request)).ReturnsAsync(response);

        // Act
        var result = await _controller.CreateFromBody(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal(response, createdResult.Value);
    }

    [Fact]
    public async Task CreateFromBody_ReturnsBadRequest_WhenInvalidOperationExceptionIsThrown()
    {
        // Arrange
        var request = new DtroUserRequest { TraId = 1 };
        _traServiceMock.Setup(service => service.SaveDtroUserAsync(request)).ThrowsAsync(new InvalidOperationException("Invalid operation"));

        // Act
        var result = await _controller.CreateFromBody(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
    }

    [Fact]
    public async Task CreateFromBody_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var request = new DtroUserRequest { TraId = 1 };
        _traServiceMock.Setup(service => service.SaveDtroUserAsync(request)).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.CreateFromBody(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.IsType<ApiErrorResponse>(statusCodeResult.Value);
    }
}
