using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DfT.DTRO.DAL;
using DfT.DTRO.Models.DataBase;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DfT.DTRO.Tests.DAL
{
    public class ErrorReportDalTests
    {
        private DtroContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<DtroContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DtroContext(options);
        }

        [Fact]
        public async Task CreateErrorReportSavesReportCorrectly()
        {
            var context = GetInMemoryContext();
            var dal = new ErrorReportDal(context);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com"
            };

            var dtro = new DTRO.Models.DataBase.DTRO
            {
                Id = Guid.NewGuid(),
                SchemaVersion = "3.4.0",
                Data = new ExpandoObject()
            };

            context.Users.Add(user);
            context.Dtros.Add(dtro);
            await context.SaveChangesAsync();

            var tras = new List<string> { "TRA1", "TRA2" };
            var regulationTypes = new List<string> { "Permanent" };
            var troTypes = new List<string> { "A", "B" };
            var files = new List<string> { "file1.txt", "file2.pdf" };

            await dal.CreateErrorReport(
                user,
                dtro,
                tras,
                regulationTypes,
                troTypes,
                "Other",
                "Some other type",
                "This is additional info",
                files
            );

            var report = await context.ErrorReport.FirstOrDefaultAsync();
            Assert.NotNull(report);
            Assert.Equal(user.Id, report.UserId);
            Assert.Equal(dtro.Id, report.TroId);
            Assert.Equal("Other", report.Type);
            Assert.Equal("Some other type", report.OtherType);
            Assert.Equal("This is additional info", report.MoreInformation);
            Assert.Equal(string.Join(",", files), report.FilePaths);
            Assert.Equal(tras, report.Tras);
            Assert.Equal(regulationTypes, report.RegulationTypes);
            Assert.Equal(troTypes, report.TroTypes);
        }

        [Fact]
        public async Task CreateErrorReportHandlesNullOptionalFields()
        {
            var context = GetInMemoryContext();
            var dal = new ErrorReportDal(context);

            var user = new User { Id = Guid.NewGuid(), Email = "test@example.com" };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            await dal.CreateErrorReport(
                user,
                null,
                new List<string>(),
                new List<string>(),
                new List<string>(),
                "Missing Info",
                null,
                "Minimal input",
                null
            );

            var report = await context.ErrorReport.FirstOrDefaultAsync();
            Assert.NotNull(report);
            Assert.Equal(user.Id, report.UserId);
            Assert.Null(report.TroId);
            Assert.Equal("Missing Info", report.Type);
            Assert.Null(report.OtherType);
            Assert.Equal("Minimal input", report.MoreInformation);
            Assert.Null(report.FilePaths);
        }
    }
}
