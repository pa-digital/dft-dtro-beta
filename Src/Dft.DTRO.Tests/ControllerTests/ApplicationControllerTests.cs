using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using DfT.DTRO.Controllers;
using DfT.DTRO.Services;
using System;

public class ApplicationControllerTests
{
    private readonly Mock<IApplicationService> _mockApplicationService;
    private readonly ApplicationController _controller;

    public ApplicationControllerTests()
    {
        _mockApplicationService = new Mock<IApplicationService>();
        _controller = new ApplicationController(_mockApplicationService.Object);
    }

    [Fact]
    public void ValidateApplicationNameValidNameReturnsOk()
    {
        var parameters = new ApplicationNameQueryParameters { Name = "ValidAppName" };
        _mockApplicationService.Setup(service => service.ValidateApplicationName("ValidAppName")).Returns(true);
        var result = _controller.ValidateApplicationName(parameters);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public void ValidateApplicationNameInvalidNameReturnsBadRequest()
    {
        var parameters = new ApplicationNameQueryParameters { Name = "" };
        var result = _controller.ValidateApplicationName(parameters);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public void ValidateApplicationNameNoQueryParametersReturnsBadRequest()
    {
        var result = _controller.ValidateApplicationName(null);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public void GetApplicationDetailsValidRequestReturnsOk()
    {
        Guid userGuid = Guid.NewGuid();
        string userId = userGuid.ToString();

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(ctx => ctx.Items["UserId"]).Returns(userId);
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext.Object
        };


        Guid appGuid = Guid.NewGuid();
        string appId = appGuid.ToString();

        var request = new ApplicationDetailsRequest { appId = appId };
        ApplicationDetailsDto mockAppDetails = new ApplicationDetailsDto { Name = "App1", AppId = appGuid, Purpose = "Test" };
        _mockApplicationService.Setup(service => service.GetApplicationDetails(appId)).Returns(mockAppDetails);
        _mockApplicationService.Setup(service => service.ValidateAppBelongsToUser(userId, appId)).Returns(true);

        var result = _controller.GetApplicationDetails(request);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public void GetApplicationDetailsNotUsersAppReturnsForbidden()
    {
        Guid userGuid = Guid.NewGuid();
        string userId = userGuid.ToString();

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(ctx => ctx.Items["UserId"]).Returns(userId);
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext.Object
        };


        Guid appGuid = Guid.NewGuid();
        string appId = appGuid.ToString();

        var request = new ApplicationDetailsRequest { appId = appId };
        ApplicationDetailsDto mockAppDetails = new ApplicationDetailsDto { Name = "App1", AppId = appGuid, Purpose = "Test" };
        _mockApplicationService.Setup(service => service.GetApplicationDetails(appId)).Returns(mockAppDetails);
        _mockApplicationService.Setup(service => service.ValidateAppBelongsToUser(userId, appId)).Returns(false);

        var result = _controller.GetApplicationDetails(request);
        var forbiddenRequestResult = Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public void GetApplicationDetailsInvalidAppIdReturnsBadRequest()
    {

        Guid userGuid = Guid.NewGuid();
        string userId = userGuid.ToString();

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(ctx => ctx.Items["UserId"]).Returns(userId);
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext.Object
        };


        Guid appGuid = Guid.NewGuid();
        string appId = appGuid.ToString();

        var request = new ApplicationDetailsRequest { appId = appId };
        ApplicationDetailsDto mockAppDetails = new ApplicationDetailsDto { Name = "App1", AppId = appGuid, Purpose = "Test" };
        _mockApplicationService.Setup(service => service.GetApplicationDetails(appId)).Returns((ApplicationDetailsDto)null);
        _mockApplicationService.Setup(service => service.ValidateAppBelongsToUser(userId, appId)).Returns(true);

        var result = _controller.GetApplicationDetails(request);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public void GetApplicationsValidUserReturnsOk()
    {
        Guid userGuid = Guid.NewGuid();
        string userId = userGuid.ToString();

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(ctx => ctx.Items["UserId"]).Returns(userId);
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext.Object
        };

        var applicationListDtos = new List<ApplicationListDto>
        {
            new ApplicationListDto { Id = Guid.NewGuid(), Name = "App 1", Type = "Test", Tra = "Test" },
            new ApplicationListDto { Id = Guid.NewGuid(), Name = "App 2", Type = "Test", Tra = "Test" },
            new ApplicationListDto { Id = Guid.NewGuid(), Name = "App 3", Type = "Test", Tra = "Test" }
        };
        _mockApplicationService.Setup(service => service.GetApplicationList(It.IsAny<string>())).Returns(applicationListDtos);
        var result = _controller.GetApplications();
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
}
