using System.Diagnostics.CodeAnalysis;
using DfT.DTRO.Caching;
using DfT.DTRO.DAL;
using DfT.DTRO.Extensions.DependencyInjection;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.DtroEvent;
using DfT.DTRO.Models.Filtering;
using DfT.DTRO.Services;
using DfT.DTRO.Services.Conversion;
using DfT.DTRO.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dft.DTRO.Tests;

[ExcludeFromCodeCoverage]
public class SqlStorageServiceTests : IDisposable
{
    private readonly DtroContext _context;
    private readonly ISpatialProjectionService _spatialProjectionService;

    private readonly Guid _deletedDtroKey = Guid.NewGuid();
    private readonly Guid _existingDtroKey = Guid.NewGuid();
    private readonly Guid _dtroWithHa1234Key = Guid.NewGuid();
    private readonly Guid _dtroWithCreationDateKey = Guid.NewGuid();
    private readonly Guid _dtroWithModificationTimeKey = Guid.NewGuid();
    private readonly Guid _dtroWithNameKey = Guid.NewGuid();
    private readonly Guid _dtroWithVehicleTypesKey = Guid.NewGuid();
    private readonly Guid _dtroWithRegulationTypesKey = Guid.NewGuid();
    private readonly Guid _dtroWithOrderReportingPointKey = Guid.NewGuid();

    private readonly DfT.DTRO.Models.DataBase.DTRO _existingDtro;   
    private readonly DfT.DTRO.Models.DataBase.DTRO _deletedDtro;
    private readonly DfT.DTRO.Models.DataBase.DTRO _dtroWithTa1234;
    private readonly DfT.DTRO.Models.DataBase.DTRO _dtroWithCreationDate;
    private readonly DfT.DTRO.Models.DataBase.DTRO _dtroWithModificationTime;
    private readonly DfT.DTRO.Models.DataBase.DTRO _dtroWithName;
    private readonly DfT.DTRO.Models.DataBase.DTRO _dtroWithVehicleTypes;
    private readonly DfT.DTRO.Models.DataBase.DTRO _dtroWithRegulationTypes;
    private readonly DfT.DTRO.Models.DataBase.DTRO _dtroWithOrderReportingPoint;

    private readonly Mock<IDtroMappingService> _mappingServiceMock = new();

    public SqlStorageServiceTests()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(opt =>
                opt.AddJsonFile("./Configurations/appsettings.SqlStorageServiceTests.json", true))
            .ConfigureServices(
                (host, services) => services.AddPostgresDtroContext(host.Configuration))
            .Build();

        //_context = host.Services.GetRequiredService<DfT.DTRO.DAL.DtroContext>();

        _context = host.Services.GetService<DtroContext>();

        _context.Database.Migrate();

        _spatialProjectionService = new Proj4SpatialProjectionService();

        _existingDtro = new()
        {
            Id = _existingDtroKey,
            SchemaVersion = new("3.2.0"),
            Data = new(),
        };

        _deletedDtro = new()
        {
            Id = _deletedDtroKey,
            SchemaVersion = new("3.1.2"),
            Data = new(),
            Deleted = true,
            DeletionTime = DateTime.SpecifyKind(new DateTime(2023, 07, 23), DateTimeKind.Utc),
        };

        _dtroWithTa1234 = new()
        {
            Id = _dtroWithHa1234Key,
            SchemaVersion = new("3.1.2"),
            Data = new(),
            TrafficAuthorityCreatorId = 1234
        };

        _dtroWithCreationDate = new()
        {
            Id = _dtroWithCreationDateKey,
            SchemaVersion = new("3.1.2"),
            Data = new(),
            Created = DateTime.SpecifyKind(new DateTime(2023, 07, 22), DateTimeKind.Utc)
        };

        _dtroWithModificationTime = new()
        {
            Id = _dtroWithModificationTimeKey,
            SchemaVersion = new("3.1.2"),
            Data = new(),
            LastUpdated = DateTime.SpecifyKind(new DateTime(2023, 07, 22), DateTimeKind.Utc)
        };

        _dtroWithName = new()
        {
            Id = _dtroWithNameKey,
            SchemaVersion = new("3.1.2"),
            Data = new(),
            TroName = "this is a test name"
        };

        _dtroWithVehicleTypes = new()
        {
            Id = _dtroWithVehicleTypesKey,
            SchemaVersion = new("3.1.2"),
            Data = new(),
            VehicleTypes = new List<string>() { "taxi" }
        };

        _dtroWithRegulationTypes = new()
        {
            Id = _dtroWithRegulationTypesKey,
            SchemaVersion = new("3.1.2"),
            Data = new(),
            RegulationTypes = new List<string>() { "test-regulation-type" }
        };

        _dtroWithOrderReportingPoint = new()
        {
            Id = _dtroWithOrderReportingPointKey,
            SchemaVersion = new("3.1.2"),
            Data = new(),
            OrderReportingPoints = new List<string>() { "test-orp" }
        };

        _context.Dtros.Add(_existingDtro);
        _context.Dtros.Add(_deletedDtro);
        _context.Dtros.Add(_dtroWithTa1234);
        _context.Dtros.Add(_dtroWithCreationDate);
        _context.Dtros.Add(_dtroWithModificationTime);
        _context.Dtros.Add(_dtroWithName);
        _context.Dtros.Add(_dtroWithVehicleTypes);
        _context.Dtros.Add(_dtroWithRegulationTypes);
        _context.Dtros.Add(_dtroWithOrderReportingPoint);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _context.Dtros.RemoveRange(_context.Dtros);
        _context.SaveChanges();
    }

    [Fact]
    public async Task DtroExists_ReturnsFalse_ForNonexistentDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        bool result = await sut.DtroExistsAsync(Guid.NewGuid());

        Assert.False(result);
    }

    [Fact]
    public async Task DtroExists_ReturnsFalse_ForDeletedDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        bool result = await sut.DtroExistsAsync(_deletedDtroKey);

        Assert.False(result);
    }

    [Fact]
    public async Task DtroExists_ReturnsTrue_ForExistingDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        bool result = await sut.DtroExistsAsync(_existingDtroKey);

        Assert.True(result);
    }

    [Fact]
    public async Task GetDtro_ReturnsNull_ForNonexistendDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.GetDtroByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetDtro_ReturnsValue_ForExistingDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.GetDtroByIdAsync(_existingDtroKey);

        Assert.NotNull(result);
        Assert.Same(_existingDtro, result);
    }

    [Fact]
    public async Task SoftDeleteDtro_ReturnsFalse_ForNonexistentDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.SoftDeleteDtroAsync(Guid.NewGuid());

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteDtro_ReturnsFalse_ForNonExistingDtro()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var actual = await sut.DeleteDtroAsync(Guid.NewGuid());

        Assert.False(actual);
    }

    [Fact]
    public async Task SoftDeleteDtro_ReturnsFalse_ForDeletedDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.SoftDeleteDtroAsync(_deletedDtroKey);

        Assert.False(result);
    }
    

    [Fact]
    public async Task SoftDeleteDtro_ReturnsTrue_OnSuccessfulDelete()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.SoftDeleteDtroAsync(_existingDtroKey);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteDtro_ReturnsTrue_OnSuccessfulDelete()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var actual = await sut.DeleteDtroAsync(_existingDtroKey);

        Assert.True(actual);
    }

    [Fact]
    public async Task TryUpdateDtro_ReturnsFalse_ForNonexistentDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var newValueKey = Guid.NewGuid();

        var dtro = new DtroSubmit
        {
            SchemaVersion = new("3.1.2"),
            Data = new(),
        };

        var result = await sut.TryUpdateDtroAsJsonAsync(newValueKey, dtro, "xyz");

        Assert.False(result);
    }

    [Fact]
    public async Task TryUpdateDtro_ReturnsFalse_ForDeletedDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var dtro = new DtroSubmit
        {
            SchemaVersion = new("3.1.2"),
            Data = new(),
        };

        var result = await sut.TryUpdateDtroAsJsonAsync(_deletedDtroKey, dtro, "xyz");

        Assert.False(result);
    }

    [Fact]
    public async Task TryUpdateDtro_ReturnsTrue_ForExistingDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());


        var dtro = new DtroSubmit
        {
            SchemaVersion = new("3.1.2"),
            Data = new()
        };

        var result = await sut.TryUpdateDtroAsJsonAsync(_existingDtroKey, dtro, "xyz");

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateDtro_Throws_ForNonexistentDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var newValue = new DtroSubmit()
        {
            SchemaVersion = new("3.1.2"),
            Data = new(),
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.UpdateDtroAsJsonAsync(Guid.NewGuid(), newValue, "xyz"));
    }

    [Fact]
    public async Task UpdateDtro_Throws_ForDeletedDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var newValue = new DtroSubmit()
        {
            SchemaVersion = new("3.1.2"),
            Data = new(),
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.UpdateDtroAsJsonAsync(_deletedDtroKey, newValue, "xyz"));
    }

    [Fact]
    public async Task FindDtros_ReturnsOnlyNotDeleted_ByDefault()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroSearch { Page = 1, PageSize = 10, Queries = new List<SearchQuery> { new() } });

        Assert.DoesNotContain(_deletedDtro, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsDeleted_WhenDeletionTimeInQuery()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroSearch { Page = 1, PageSize = 10, Queries = new List<SearchQuery> { new() { DeletionTime = new(2023, 07, 22) } } });

        Assert.Contains(_deletedDtro, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsDeletedAfterDeletionTime_WhenDeletionTimeInQuery()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroSearch { Page = 1, PageSize = 10, Queries = new List<SearchQuery> { new() { DeletionTime = new(2023, 07, 24) } } });

        Assert.DoesNotContain(_deletedDtro, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsOnlyDtrosWithSpecifiedTrafficAuthorityId()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroSearch { Page = 1, PageSize = 10, Queries = new List<SearchQuery> { new() { TraCreator = 1234 } } });

        Assert.Equal(1, result.Results.Count);
        Assert.Contains(_dtroWithTa1234, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsOnlyDtrosAfterSpecifiedPublicationTime()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroSearch { Page = 1, PageSize = 10, Queries = new List<SearchQuery> { new() { PublicationTime = new DateTime(2023, 07, 21) } } });

        Assert.Equal(1, result.Results.Count);
        Assert.Contains(_dtroWithCreationDate, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsOnlyDtrosAfterSpecifiedModificationTime()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroSearch { Page = 1, PageSize = 10, Queries = new List<SearchQuery> { new() { ModificationTime = new DateTime(2023, 07, 21) } } });

        Assert.Equal(1, result.Results.Count);
        Assert.Contains(_dtroWithModificationTime, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsOnlyDtrosContainingSpecifiedStringInName()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroSearch { Page = 1, PageSize = 10, Queries = new List<SearchQuery> { new() { TroName = "test" } } });

        Assert.Equal(1, result.Results.Count);
        Assert.Contains(_dtroWithName, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsOnlyDtrosContainingSpecifiedVehicleType()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroSearch { Page = 1, PageSize = 10, Queries = new List<SearchQuery> { new() { VehicleType = "taxi" } } });

        Assert.Equal(1, result.Results.Count);
        Assert.Contains(_dtroWithVehicleTypes, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsOnlyDtrosContainingSpecifiedRegulationTypes()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroSearch { Page = 1, PageSize = 10, Queries = new List<SearchQuery> { new() { RegulationType = "test-regulation-type" } } });

        Assert.Equal(1, result.Results.Count);
        Assert.Contains(_dtroWithRegulationTypes, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsOnlyDtrosContainingSpecifiedOrderReportingPoint()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroSearch { Page = 1, PageSize = 10, Queries = new List<SearchQuery> { new() { OrderReportingPoint = "test-orp" } } });

        Assert.Equal(1, result.Results.Count);
        Assert.Contains(_dtroWithOrderReportingPoint, result.Results);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsDeleted_ByDefault()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroEventSearch { Page = 1, PageSize = 10 });

        Assert.Contains(_deletedDtro, result);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsDeleted_WhenDeletionTimeInQuery()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroEventSearch { Page = 1, PageSize = 10, DeletionTime = new(2023, 07, 22) });

        Assert.Single(result);
        Assert.Contains(_deletedDtro, result);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsDeletedAfterDeletionTime_WhenDeletionTimeInQuery()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroEventSearch { Page = 1, PageSize = 10, DeletionTime = new(2023, 07, 24) });

        Assert.DoesNotContain(_deletedDtro, result);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsOnlyDtrosWithSpecifiedTrafficAuthorityId()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroEventSearch { Page = 1, PageSize = 10, TraCreator = 1234 });

        Assert.Single(result);
        Assert.Contains(_dtroWithTa1234, result);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsOnlyDtrosAfterSpecifiedPublicationTime()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroEventSearch { Page = 1, PageSize = 10, Since = new DateTime(2023, 07, 21) });

        Assert.Single(result);
        Assert.Contains(_dtroWithCreationDate, result);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsOnlyDtrosAfterSpecifiedModificationTime()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroEventSearch { Page = 1, PageSize = 10, ModificationTime = new DateTime(2023, 07, 21) });

        Assert.Single(result);
        Assert.Contains(_dtroWithModificationTime, result);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsOnlyDtrosContainingSpecifiedStringInName()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroEventSearch { Page = 1, PageSize = 10, TroName = "test" });

        Assert.Single(result);
        Assert.Contains(_dtroWithName, result);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsOnlyDtrosContainingSpecifiedVehicleType()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroEventSearch { Page = 1, PageSize = 10, VehicleType = "taxi" });

        Assert.Single(result);
        Assert.Contains(_dtroWithVehicleTypes, result);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsOnlyDtrosContainingSpecifiedRegulationTypes()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroEventSearch { Page = 1, PageSize = 10, RegulationType = "test-regulation-type" });

        Assert.Single(result);
        Assert.Contains(_dtroWithRegulationTypes, result);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsOnlyDtrosContainingSpecifiedOrderReportingPoint()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        var result = await sut.FindDtrosAsync(new DtroEventSearch { Page = 1, PageSize = 10, OrderReportingPoint = "test-orp" });

        Assert.Single(result);
        Assert.Contains(_dtroWithOrderReportingPoint, result);
    }
}