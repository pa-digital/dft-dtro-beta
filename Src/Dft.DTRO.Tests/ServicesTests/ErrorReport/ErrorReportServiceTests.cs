using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DfT.DTRO.DAL;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Services;

public class ErrorReportServiceTests
{
    private readonly Mock<IErrorReportDal> errorReportDalMock = new();
    private readonly Mock<IUserDal> userDalMock = new();
    private readonly Mock<IDtroDal> dtroDalMock = new();

    private readonly ErrorReportService service;

    public ErrorReportServiceTests()
    {
        service = new ErrorReportService(
            errorReportDalMock.Object,
            userDalMock.Object,
            dtroDalMock.Object
        );
    }

    [Fact]
    public async Task createErrorReportWithValidDataCallsDalCorrectly()
    {
        var username = "test@example.com";
        var user = new User { Id = Guid.NewGuid(), Email = username };
        var dtroId = Guid.NewGuid();
        var dtro = new DTRO { Id = dtroId };

        var filenames = new List<string> { "file1.txt", "file2.txt" };
        var request = new ErrorReportRequest
        {
            TroId = dtroId.ToString(),
            Tras = new List<string> { "A", "B" },
            RegulationTypes = new List<string> { "Perm" },
            TroTypes = new List<string> { "Temp" },
            Type = "Other",
            OtherType = "Description",
            MoreInformation = "Some details"
        };

        userDalMock.Setup(x => x.GetUserFromEmail(username)).ReturnsAsync(user);
        dtroDalMock.Setup(x => x.GetDtroByIdAsync(dtroId)).ReturnsAsync(dtro);

        await service.CreateErrorReport(username, filenames, request);

        userDalMock.Verify(x => x.GetUserFromEmail(username), Times.Once);
        dtroDalMock.Verify(x => x.GetDtroByIdAsync(dtroId), Times.Once);
        errorReportDalMock.Verify(x => x.CreateErrorReport(
            user,
            dtro,
            request.Tras,
            request.RegulationTypes,
            request.TroTypes,
            request.Type,
            request.OtherType,
            request.MoreInformation,
            filenames
        ), Times.Once);
    }

    [Fact]
    public async Task createErrorReportWithoutTroIdSkipsDtroLookup()
    {
        var username = "test@example.com";
        var user = new User { Id = Guid.NewGuid(), Email = username };

        var filenames = new List<string> { "file1.txt" };
        var request = new ErrorReportRequest
        {
            TroId = null,
            Tras = new List<string>(),
            RegulationTypes = new List<string>(),
            TroTypes = new List<string>(),
            Type = "Missing Sign",
            OtherType = null,
            MoreInformation = "More info"
        };

        userDalMock.Setup(x => x.GetUserFromEmail(username)).ReturnsAsync(user);

        await service.CreateErrorReport(username, filenames, request);

        dtroDalMock.Verify(x => x.GetDtroByIdAsync(It.IsAny<Guid>()), Times.Never);
        errorReportDalMock.Verify(x => x.CreateErrorReport(
            user,
            null,
            request.Tras,
            request.RegulationTypes,
            request.TroTypes,
            request.Type,
            request.OtherType,
            request.MoreInformation,
            filenames
        ), Times.Once);
    }
}
