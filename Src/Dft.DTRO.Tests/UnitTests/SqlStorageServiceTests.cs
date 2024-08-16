namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class SqlStorageServiceTests : IDisposable
{
    private readonly DtroContext _context;
    private readonly DfT.DTRO.Models.DataBase.DTRO _deletedDtro;

    private readonly Guid _deletedDtroKey = Guid.NewGuid();
    private readonly DfT.DTRO.Models.DataBase.DTRO _dtroWithCreationDate;
    private readonly Guid _dtroWithCreationDateKey = Guid.NewGuid();
    private readonly Guid _dtroWithHa1234Key = Guid.NewGuid();
    private readonly DfT.DTRO.Models.DataBase.DTRO _dtroWithModificationTime;
    private readonly Guid _dtroWithModificationTimeKey = Guid.NewGuid();
    private readonly DfT.DTRO.Models.DataBase.DTRO _dtroWithName;
    private readonly Guid _dtroWithNameKey = Guid.NewGuid();
    private readonly DfT.DTRO.Models.DataBase.DTRO _dtroWithOrderReportingPoint;
    private readonly Guid _dtroWithOrderReportingPointKey = Guid.NewGuid();
    private readonly DfT.DTRO.Models.DataBase.DTRO _dtroWithRegulationTypes;
    private readonly Guid _dtroWithRegulationTypesKey = Guid.NewGuid();
    private readonly DfT.DTRO.Models.DataBase.DTRO _dtroWithTa1234;
    private readonly DfT.DTRO.Models.DataBase.DTRO _dtroWithVehicleTypes;
    private readonly Guid _dtroWithVehicleTypesKey = Guid.NewGuid();

    private readonly DfT.DTRO.Models.DataBase.DTRO _existingDtro;
    private readonly Guid _existingDtroKey = Guid.NewGuid();

    private readonly Mock<IDtroMappingService> _mappingServiceMock = new();
    private readonly ISpatialProjectionService _spatialProjectionService;

    public SqlStorageServiceTests()
    {
        IHost? host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(opt =>
                opt.AddJsonFile("./Configurations/appsettings.SqlStorageServiceTests.json", true))
            .ConfigureServices(
                (host, services) => services.AddStorage(host.Configuration))
            .Build();

        _context = host.Services.GetService<DtroContext>();

        _context.Database.Migrate();

        _spatialProjectionService = new Proj4SpatialProjectionService();

        _existingDtro = new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = _existingDtroKey,
            SchemaVersion = new SchemaVersion("3.2.0"),
            Data = new ExpandoObject()
        };

        _deletedDtro = new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = _deletedDtroKey,
            SchemaVersion = new SchemaVersion("3.1.2"),
            Data = new ExpandoObject(),
            Deleted = true,
            DeletionTime = DateTime.SpecifyKind(new DateTime(2023, 07, 23), DateTimeKind.Utc)
        };

        _dtroWithTa1234 = new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = _dtroWithHa1234Key,
            SchemaVersion = new SchemaVersion("3.1.2"),
            Data = new ExpandoObject(),
            TrafficAuthorityCreatorId = 1234
        };

        _dtroWithCreationDate = new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = _dtroWithCreationDateKey,
            SchemaVersion = new SchemaVersion("3.1.2"),
            Data = new ExpandoObject(),
            Created = DateTime.SpecifyKind(new DateTime(2023, 07, 22), DateTimeKind.Utc)
        };

        _dtroWithModificationTime = new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = _dtroWithModificationTimeKey,
            SchemaVersion = new SchemaVersion("3.1.2"),
            Data = new ExpandoObject(),
            LastUpdated = DateTime.SpecifyKind(new DateTime(2023, 07, 22), DateTimeKind.Utc)
        };

        _dtroWithName = new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = _dtroWithNameKey,
            SchemaVersion = new SchemaVersion("3.1.2"),
            Data = new ExpandoObject(),
            TroName = "this is a test name"
        };

        _dtroWithVehicleTypes = new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = _dtroWithVehicleTypesKey,
            SchemaVersion = new SchemaVersion("3.1.2"),
            Data = new ExpandoObject(),
            VehicleTypes = new List<string> { "taxi" }
        };

        _dtroWithRegulationTypes = new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = _dtroWithRegulationTypesKey,
            SchemaVersion = new SchemaVersion("3.1.2"),
            Data = new ExpandoObject(),
            RegulationTypes = new List<string> { "test-regulation-type" }
        };

        _dtroWithOrderReportingPoint = new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = _dtroWithOrderReportingPointKey,
            SchemaVersion = new SchemaVersion("3.1.2"),
            Data = new ExpandoObject(),
            OrderReportingPoints = new List<string> { "test-orp" }
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
    public async Task GetDtro_ReturnsNull_ForNonExistedDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        DfT.DTRO.Models.DataBase.DTRO? result = await sut.GetDtroByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetDtro_ReturnsValue_ForExistingDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        DfT.DTRO.Models.DataBase.DTRO? result = await sut.GetDtroByIdAsync(_existingDtroKey);

        Assert.NotNull(result);
        Assert.Same(_existingDtro, result);
    }

    [Fact]
    public async Task SoftDeleteDtro_ReturnsFalse_ForNonexistentDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        bool result = await sut.SoftDeleteDtroAsync(Guid.NewGuid(), DateTime.UtcNow);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteDtro_ReturnsFalse_ForNonExistingDtro()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        bool actual = await sut.DeleteDtroAsync(Guid.NewGuid());

        Assert.False(actual);
    }

    [Fact]
    public async Task SoftDeleteDtro_ReturnsFalse_ForDeletedDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        bool result = await sut.SoftDeleteDtroAsync(_deletedDtroKey, DateTime.UtcNow);

        Assert.False(result);
    }


    [Fact]
    public async Task SoftDeleteDtro_ReturnsTrue_OnSuccessfulDelete()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        bool result = await sut.SoftDeleteDtroAsync(_existingDtroKey, DateTime.UtcNow);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteDtro_ReturnsTrue_OnSuccessfulDelete()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        bool actual = await sut.DeleteDtroAsync(_existingDtroKey);

        Assert.True(actual);
    }

    [Fact]
    public async Task TryUpdateDtro_ReturnsFalse_ForNonexistentDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        Guid newValueKey = Guid.NewGuid();

        DtroSubmit dtro = new() { SchemaVersion = new SchemaVersion("3.1.2"), Data = new ExpandoObject() };

        bool result = await sut.TryUpdateDtroAsJsonAsync(newValueKey, dtro, "xyz");

        Assert.False(result);
    }

    [Fact]
    public async Task TryUpdateDtro_ReturnsFalse_ForDeletedDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        DtroSubmit dtro = new() { SchemaVersion = new SchemaVersion("3.1.2"), Data = new ExpandoObject() };

        bool result = await sut.TryUpdateDtroAsJsonAsync(_deletedDtroKey, dtro, "xyz");

        Assert.False(result);
    }

    [Fact]
    public async Task TryUpdateDtro_ReturnsTrue_ForExistingDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());


        DtroSubmit dtro = new() { SchemaVersion = new SchemaVersion("3.1.2"), Data = new ExpandoObject() };

        bool result = await sut.TryUpdateDtroAsJsonAsync(_existingDtroKey, dtro, "xyz");

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateDtro_Throws_ForNonexistentDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        DtroSubmit newValue = new() { SchemaVersion = new SchemaVersion("3.1.2"), Data = new ExpandoObject() };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            sut.UpdateDtroAsJsonAsync(Guid.NewGuid(), newValue, "xyz"));
    }

    [Fact]
    public async Task UpdateDtro_Throws_ForDeletedDtros()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        DtroSubmit newValue = new() { SchemaVersion = new SchemaVersion("3.1.2"), Data = new ExpandoObject() };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            sut.UpdateDtroAsJsonAsync(_deletedDtroKey, newValue, "xyz"));
    }

    [Fact]
    public async Task FindDtros_ReturnsOnlyNotDeleted_ByDefault()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        PaginatedResult<DfT.DTRO.Models.DataBase.DTRO>? result =
            await sut.FindDtrosAsync(new DtroSearch
            {
                Page = 1,
                PageSize = 10,
                Queries = new List<SearchQuery> { new() }
            });

        Assert.DoesNotContain(_deletedDtro, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsDeleted_WhenDeletionTimeInQuery()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        PaginatedResult<DfT.DTRO.Models.DataBase.DTRO>? result = await sut.FindDtrosAsync(new DtroSearch
        {
            Page = 1,
            PageSize = 10,
            Queries = new List<SearchQuery> { new() { DeletionTime = new DateTime(2023, 07, 22) } }
        });

        Assert.Contains(_deletedDtro, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsDeletedAfterDeletionTime_WhenDeletionTimeInQuery()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        PaginatedResult<DfT.DTRO.Models.DataBase.DTRO>? result = await sut.FindDtrosAsync(new DtroSearch
        {
            Page = 1,
            PageSize = 10,
            Queries = new List<SearchQuery> { new() { DeletionTime = new DateTime(2023, 07, 24) } }
        });

        Assert.DoesNotContain(_deletedDtro, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsOnlyDtrosWithSpecifiedTrafficAuthorityId()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        PaginatedResult<DfT.DTRO.Models.DataBase.DTRO>? result = await sut.FindDtrosAsync(new DtroSearch
        {
            Page = 1,
            PageSize = 10,
            Queries = new List<SearchQuery> { new() { TraCreator = 1234 } }
        });

        Assert.Equal(1, result.Results.Count);
        Assert.Contains(_dtroWithTa1234, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsOnlyDtrosAfterSpecifiedPublicationTime()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        PaginatedResult<DfT.DTRO.Models.DataBase.DTRO>? result = await sut.FindDtrosAsync(new DtroSearch
        {
            Page = 1,
            PageSize = 10,
            Queries = new List<SearchQuery> { new() { PublicationTime = new DateTime(2023, 07, 21) } }
        });

        Assert.Equal(1, result.Results.Count);
        Assert.Contains(_dtroWithCreationDate, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsOnlyDtrosAfterSpecifiedModificationTime()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        PaginatedResult<DfT.DTRO.Models.DataBase.DTRO>? result = await sut.FindDtrosAsync(new DtroSearch
        {
            Page = 1,
            PageSize = 10,
            Queries = new List<SearchQuery> { new() { ModificationTime = new DateTime(2023, 07, 21) } }
        });

        Assert.Equal(1, result.Results.Count);
        Assert.Contains(_dtroWithModificationTime, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsOnlyDtrosContainingSpecifiedStringInName()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        PaginatedResult<DfT.DTRO.Models.DataBase.DTRO>? result = await sut.FindDtrosAsync(new DtroSearch
        {
            Page = 1,
            PageSize = 10,
            Queries = new List<SearchQuery> { new() { TroName = "test" } }
        });

        Assert.Equal(1, result.Results.Count);
        Assert.Contains(_dtroWithName, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsOnlyDtrosContainingSpecifiedVehicleType()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        PaginatedResult<DfT.DTRO.Models.DataBase.DTRO>? result = await sut.FindDtrosAsync(new DtroSearch
        {
            Page = 1,
            PageSize = 10,
            Queries = new List<SearchQuery> { new() { VehicleType = "taxi" } }
        });

        Assert.Equal(1, result.Results.Count);
        Assert.Contains(_dtroWithVehicleTypes, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsOnlyDtrosContainingSpecifiedRegulationTypes()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        PaginatedResult<DfT.DTRO.Models.DataBase.DTRO>? result = await sut.FindDtrosAsync(new DtroSearch
        {
            Page = 1,
            PageSize = 10,
            Queries = new List<SearchQuery> { new() { RegulationType = "test-regulation-type" } }
        });

        Assert.Equal(1, result.Results.Count);
        Assert.Contains(_dtroWithRegulationTypes, result.Results);
    }

    [Fact]
    public async Task FindDtros_ReturnsOnlyDtrosContainingSpecifiedOrderReportingPoint()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        PaginatedResult<DfT.DTRO.Models.DataBase.DTRO>? result = await sut.FindDtrosAsync(new DtroSearch
        {
            Page = 1,
            PageSize = 10,
            Queries = new List<SearchQuery> { new() { OrderReportingPoint = "test-orp" } }
        });

        Assert.Equal(1, result.Results.Count);
        Assert.Contains(_dtroWithOrderReportingPoint, result.Results);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsDeleted_ByDefault()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        List<DfT.DTRO.Models.DataBase.DTRO>? result =
            await sut.FindDtrosAsync(new DtroEventSearch { Page = 1, PageSize = 10 });

        Assert.Contains(_deletedDtro, result);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsDeleted_WhenDeletionTimeInQuery()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        List<DfT.DTRO.Models.DataBase.DTRO>? result =
            await sut.FindDtrosAsync(new DtroEventSearch
            {
                Page = 1,
                PageSize = 10,
                DeletionTime = new DateTime(2023, 07, 22)
            });

        Assert.Single(result);
        Assert.Contains(_deletedDtro, result);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsDeletedAfterDeletionTime_WhenDeletionTimeInQuery()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        List<DfT.DTRO.Models.DataBase.DTRO>? result =
            await sut.FindDtrosAsync(new DtroEventSearch
            {
                Page = 1,
                PageSize = 10,
                DeletionTime = new DateTime(2023, 07, 24)
            });

        Assert.DoesNotContain(_deletedDtro, result);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsOnlyDtrosWithSpecifiedTrafficAuthorityId()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        List<DfT.DTRO.Models.DataBase.DTRO>? result =
            await sut.FindDtrosAsync(new DtroEventSearch { Page = 1, PageSize = 10, TraCreator = 1234 });

        Assert.Single(result);
        Assert.Contains(_dtroWithTa1234, result);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsOnlyDtrosAfterSpecifiedPublicationTime()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        List<DfT.DTRO.Models.DataBase.DTRO>? result =
            await sut.FindDtrosAsync(
                new DtroEventSearch { Page = 1, PageSize = 10, Since = new DateTime(2023, 07, 21) });

        Assert.Single(result);
        Assert.Contains(_dtroWithCreationDate, result);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsOnlyDtrosAfterSpecifiedModificationTime()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        List<DfT.DTRO.Models.DataBase.DTRO>? result = await sut.FindDtrosAsync(
            new DtroEventSearch { Page = 1, PageSize = 10, ModificationTime = new DateTime(2023, 07, 21) });

        Assert.Single(result);
        Assert.Contains(_dtroWithModificationTime, result);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsOnlyDtrosContainingSpecifiedStringInName()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        List<DfT.DTRO.Models.DataBase.DTRO>? result =
            await sut.FindDtrosAsync(new DtroEventSearch { Page = 1, PageSize = 10, TroName = "test" });

        Assert.Single(result);
        Assert.Contains(_dtroWithName, result);
    }

    //TO-DO fix test
    //[Fact]
    //public async Task FindDtrosForEvents_ReturnsOnlyDtrosContainingSpecifiedVehicleType()
    //{
    //    DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

    //    var result = await sut.FindDtrosAsync(new DtroEventSearch { Page = 1, PageSize = 10, VehicleType = "taxi" });

    //    Assert.Single(result);
    //    Assert.Contains(_dtroWithVehicleTypes, result);
    //}

    [Fact]
    public async Task FindDtrosForEvents_ReturnsOnlyDtrosContainingSpecifiedRegulationTypes()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        List<DfT.DTRO.Models.DataBase.DTRO>? result =
            await sut.FindDtrosAsync(new DtroEventSearch
            {
                Page = 1,
                PageSize = 10,
                RegulationType = "test-regulation-type"
            });

        Assert.Single(result);
        Assert.Contains(_dtroWithRegulationTypes, result);
    }

    [Fact]
    public async Task FindDtrosForEvents_ReturnsOnlyDtrosContainingSpecifiedOrderReportingPoint()
    {
        DtroDal sut = new(_context, _spatialProjectionService, _mappingServiceMock.Object, new NoopCache());

        List<DfT.DTRO.Models.DataBase.DTRO>? result =
            await sut.FindDtrosAsync(new DtroEventSearch { Page = 1, PageSize = 10, OrderReportingPoint = "test-orp" });

        Assert.Single(result);
        Assert.Contains(_dtroWithOrderReportingPoint, result);
    }
}