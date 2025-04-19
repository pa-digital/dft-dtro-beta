using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DfT.DTRO.Controllers;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DfT.DTRO.Tests.Controllers
{
    public class ErrorReportControllerTests
    {
        private readonly Mock<IErrorReportService> errorReportServiceMock;
        private readonly Mock<IGoogleCloudStorageService> storageServiceMock;
        private readonly ErrorReportController controller;

        public ErrorReportControllerTests()
        {
            errorReportServiceMock = new Mock<IErrorReportService>();
            storageServiceMock = new Mock<IGoogleCloudStorageService>();
            controller = new ErrorReportController(errorReportServiceMock.Object, storageServiceMock.Object);
        }

        [Fact]
        public async Task ErrorReportSubmitReturnsOkWhenValidRequest()
        {
            var fileMock = new Mock<IFormFile>();
            var content = "File content";
            var fileName = "test.txt";

            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(ms.Length);
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Returns(Task.CompletedTask);

            var request = new ErrorReportRequest
            {
                Type = "TestType",
                MoreInformation = "Some info",
                Files = new List<IFormFile> { fileMock.Object }
            };

            storageServiceMock.Setup(s => s.UploadFileAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            var result = await controller.ErrorReportSubmit(request);

            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            storageServiceMock.Verify(s => s.UploadFileAsync(It.IsAny<string>()), Times.Once);
            errorReportServiceMock.Verify(s => s.CreateErrorReport("user@test.com", It.IsAny<List<string>>(), request), Times.Once);
        }

        [Fact]
        public async Task ErrorReportSubmitReturnsBadRequestWhenArgumentNullExceptionThrown()
        {
            var request = new ErrorReportRequest
            {
                Type = "TestType",
                MoreInformation = "Some info",
                Files = new List<IFormFile>()
            };

            errorReportServiceMock
                .Setup(s => s.CreateErrorReport(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<ErrorReportRequest>()))
                .ThrowsAsync(new ArgumentNullException("input"));

            var result = await controller.ErrorReportSubmit(request);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task ErrorReportSubmitReturnsInternalServerErrorWhenUnhandledExceptionThrown()
        {
            var request = new ErrorReportRequest
            {
                Type = "TestType",
                MoreInformation = "Some info",
                Files = new List<IFormFile>()
            };

            errorReportServiceMock
                .Setup(s => s.CreateErrorReport(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<ErrorReportRequest>()))
                .ThrowsAsync(new Exception("Unexpected"));

            var result = await controller.ErrorReportSubmit(request);

            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }
    }
}
