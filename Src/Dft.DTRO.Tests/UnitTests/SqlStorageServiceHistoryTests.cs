using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using DfT.DTRO.DAL;
using DfT.DTRO.Extensions.DependencyInjection;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.SchemaTemplate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dft.DTRO.Tests;

[ExcludeFromCodeCoverage]
public class SqlStorageServiceHistoryTests : IDisposable
{
    private readonly DtroContext _context;

    private readonly DTROHistory _newDtroHistory;
    private readonly DTROHistory _partialAmendmentDtroHistory;
    private readonly DTROHistory _noChangeDtroHistory;

    public SqlStorageServiceHistoryTests()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(opt =>
                opt.AddJsonFile("./Configurations/appsettings.SqlStorageServiceTests.json", true))
            .ConfigureServices(
                (host, services) => services.AddPostgresDtroContext(host.Configuration))
            .Build();

        _context = host.Services.GetRequiredService<DtroContext>();

        _context.Database.Migrate();

        _newDtroHistory = new DTROHistory
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow,
            Deleted = false,
            DeletionTime = null,
            SchemaVersion = new SchemaVersion("3.2.0"),
            Data = new ExpandoObject(){}
        };

        _context.DtroHistories.Add(_newDtroHistory);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _context.DtroHistories.RemoveRange(_context.DtroHistories);
        _context.SaveChanges();
    }

    [Fact]
    public async Task SaveDtrosToHistoryTable_ReturnsTrue_ForExistingHistoricDtros()
    {
        IDtroHistoryDal sut = new DtroHistoryDal(_context);
        
        var actual = await sut.SaveDtroInHistoryTable(_newDtroHistory);

        Assert.True(actual);
    }

    [Fact]
    public async Task SaveDtrosToHistoryTable_ReturnsFalse_ForExistingHistoricDtros()
    {
        IDtroHistoryDal sut = new DtroHistoryDal(_context);

        DTROHistory newDtroHistory = new()
        {
            Id = Guid.NewGuid(),
            SchemaVersion = new SchemaVersion("3.2.0"), 
            Data = new ExpandoObject()
        };
        var actual = await sut.SaveDtroInHistoryTable(newDtroHistory);

        Assert.False(actual);
    }

    [Fact]
    public async Task GetDtroSourceHistory_Returns_ListOfDtroHistory()
    {
        IDtroHistoryDal sut = new DtroHistoryDal(_context);

        List<DTROHistory>? actual = await sut.GetDtroSourceHistory(It.IsAny<string>());
        Assert.NotNull(actual);
        Assert.True(actual.Any());
    }
}