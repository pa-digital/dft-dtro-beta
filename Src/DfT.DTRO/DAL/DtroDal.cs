namespace DfT.DTRO.DAL;

/// <summary>
/// Implementation of the <see cref="IDtroDal" /> service.
/// </summary>
[ExcludeFromCodeCoverage]
public class DtroDal : IDtroDal
{
    private readonly DtroContext _dtroContext;
    private readonly ISpatialProjectionService _projectionService;
    private readonly IDtroMappingService _dtroMappingService;
    private readonly IRedisCache _dtroCache;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="dtroContext"><see cref="DtroContext"/> database context.</param>
    /// <param name="projectionService"><see cref="ISpatialProjectionService"/> service.</param>
    /// <param name="dtroMappingService"><see cref="IDtroMappingService"/> service.</param>
    /// <param name="dtroCache"><see cref="IRedisCache"/> service.</param>
    public DtroDal(DtroContext dtroContext, ISpatialProjectionService projectionService, IDtroMappingService dtroMappingService, IRedisCache dtroCache)
    {
        _dtroContext = dtroContext;
        _projectionService = projectionService;
        _dtroMappingService = dtroMappingService;
        _dtroCache = dtroCache;
    }

    ///<inheritdoc cref="IDtroService"/>
    public async Task<bool> SoftDeleteDtroAsync(Guid id, DateTime? deletionTime)
    {
        var existing = await _dtroContext.Dtros.FindAsync(id);

        if (existing is null || existing.Deleted)
        {
            return false;
        }

        existing.Deleted = true;
        existing.DeletionTime = deletionTime;

        await _dtroCache.RemoveDtro(id);
        await _dtroCache.RemoveDtroIfExists(id);
        await _dtroContext.SaveChangesAsync();

        return true;
    }

    ///<inheritdoc cref="IDtroDal"/>
    public async Task<bool> DtroExistsAsync(Guid id)
    {
        return await _dtroContext.Dtros.AnyAsync(it => it.Id == id && !it.Deleted);
    }

    ///<inheritdoc cref="IDtroDal"/>
    public async Task<int> DtroCountForSchemaAsync(SchemaVersion schemaVersion)
    {
        return await _dtroContext.Dtros.CountAsync(it => it.SchemaVersion == schemaVersion);
    }

    ///<inheritdoc cref="IDtroDal"/>
    public async Task<IEnumerable<Models.DataBase.DTRO>> GetDtrosAsync(GetAllQueryParameters parameters)
    {
        var cachedDtros = await _dtroCache.GetDtros();
        if (cachedDtros.Any())
        {
            return cachedDtros;
        }

        var dtrosQuery = _dtroContext.Dtros.Where(d => !d.Deleted);

        dtrosQuery = parameters.TraCode != null &&
                     parameters.TraCode != 0
            ? dtrosQuery
                .Where(dtro => !dtro.Deleted)
                .Where(dtro => parameters.TraCode.Equals(dtro.TrafficAuthorityOwnerId) ||
                               parameters.TraCode.Equals(dtro.TrafficAuthorityCreatorId))
            : dtrosQuery.Where(dtro => !dtro.Deleted);

        if (parameters.StartDate.HasValue)
        {
            dtrosQuery = dtrosQuery
                .Where(dtro => dtro.Created >= parameters.StartDate.Value.ToDateTimeTruncated());
        }

        if (parameters.EndDate.HasValue)
        {
            dtrosQuery = dtrosQuery
                .Where(dtro => dtro.Created <= parameters.EndDate.Value.ToDateTimeTruncated());
        }

        var dtros = await dtrosQuery.ToListAsync();

        await _dtroCache.CacheDtros(dtros);

        return dtros;
    }

    ///<inheritdoc cref="IDtroDal"/>
    public async Task<Models.DataBase.DTRO> GetDtroByIdAsync(Guid id)
    {
        var cachedDtro = await _dtroCache.GetDtro(id);
        if (cachedDtro is not null)
        {
            return cachedDtro;
        }

        var dtro = await _dtroContext.Dtros.FindAsync(id);

        if (dtro is null || dtro.Deleted)
        {
            throw new NotFoundException($"Dtro '{id}' has either been deleted or cannot be found.");
        }

        await _dtroCache.CacheDtro(dtro);
        return dtro;
    }

    ///<inheritdoc cref="IDtroDal"/>
    public async Task<GuidResponse> SaveDtroAsJsonAsync(DtroSubmit dtroSubmit, string correlationId)
    {
        var dtro = new Models.DataBase.DTRO();
        var response = new GuidResponse();
        dtro.Id = response.Id;

        dtro.SchemaVersion = dtroSubmit.SchemaVersion;


        dtro.Data = dtroSubmit.Data;
        var utcNow = DateTime.UtcNow.ToDateTimeTruncated();

        dtro.LastUpdated = utcNow;
        dtro.Created = utcNow;
        dtro.LastUpdatedCorrelationId = correlationId;
        dtro.CreatedCorrelationId = dtro.LastUpdatedCorrelationId;

        try
        {
            _dtroMappingService.InferIndexFields(ref dtro);

            await _dtroContext.Dtros.AddAsync(dtro);

            await _dtroContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Issue with D-TRO record persistence: {ex.Message}");
        }

        return response;
    }

    ///<inheritdoc cref="IDtroDal"/>
    public async Task<bool> TryUpdateDtroAsJsonAsync(Guid guid, DtroSubmit dtroSubmit, string correlationId)
    {
        try
        {
            await UpdateDtroAsJsonAsync(guid, dtroSubmit, correlationId);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    ///<inheritdoc cref="IDtroDal"/>
    public async Task UpdateDtroAsJsonAsync(Guid id, DtroSubmit dtroSubmit, string correlationId)
    {
        if (await _dtroContext.Dtros.FindAsync(id) is not { } existing || existing.Deleted)
        {
            throw new InvalidOperationException($"There is no DTRO with Id {id}");
        }

        existing.SchemaVersion = dtroSubmit.SchemaVersion;
        existing.Data = dtroSubmit.Data;

        var lastUpdated = DateTime.UtcNow.ToDateTimeTruncated();
        existing.LastUpdated = lastUpdated;
        existing.LastUpdatedCorrelationId = correlationId;

        _dtroContext.Entry(existing).Property(e => e.Data).IsModified = true;

        _dtroMappingService.InferIndexFields(ref existing);
        await _dtroCache.RemoveDtro(id);
        await _dtroContext.SaveChangesAsync();
    }

    ///<inheritdoc cref="IDtroDal"/>
    public async Task AssignDtroOwnership(Guid id, int assignToTraId, string correlationId)
    {
        if (await _dtroContext.Dtros.FindAsync(id) is not { } existing || existing.Deleted)
        {
            throw new InvalidOperationException($"There is no DTRO with Id {id}");
        }

        _dtroMappingService.SetOwnership(ref existing, assignToTraId);
        _dtroMappingService.SetSourceActionType(ref existing, SourceActionType.Amendment);

        var lastUpdated = DateTime.UtcNow.ToDateTimeTruncated();
        existing.LastUpdated = lastUpdated;
        existing.LastUpdatedCorrelationId = correlationId;

        _dtroMappingService.InferIndexFields(ref existing);
        await _dtroCache.RemoveDtro(id);

        _dtroContext.Entry(existing).Property(e => e.Data).IsModified = true;

        await _dtroContext.SaveChangesAsync();
    }

    ///<inheritdoc cref="IDtroDal"/>
    public async Task<PaginatedResult<Models.DataBase.DTRO>> FindDtrosAsync(DtroSearch search)
    {
        IQueryable<Models.DataBase.DTRO> result = _dtroContext.Dtros;
        var expressionsToDisjunct = new List<Expression<Func<Models.DataBase.DTRO, bool>>>();

        foreach (var query in search.Queries)
        {
            var expressionsToConjunct = new List<Expression<Func<Models.DataBase.DTRO, bool>>>();

            if (query.DeletionTime is { } deletionTime)
            {
                deletionTime = DateTime.SpecifyKind(deletionTime, DateTimeKind.Utc).ToDateTimeTruncated();
                expressionsToConjunct.Add(it => it.DeletionTime >= deletionTime);
            }
            else
            {
                expressionsToConjunct.Add(it => !it.Deleted);
            }

            if (query.TraCreator is { } traCreator)
            {
                expressionsToConjunct.Add(it => it.TrafficAuthorityCreatorId == traCreator);
            }

            if (query.CurrentTraOwner is { } currentTraOwner)
            {
                expressionsToConjunct.Add(it => it.TrafficAuthorityOwnerId == currentTraOwner);
            }

            if (query.PublicationTime is { } publicationTime)
            {
                publicationTime = DateTime.SpecifyKind(publicationTime, DateTimeKind.Utc).ToDateTimeTruncated();
                expressionsToConjunct.Add(it => it.Created >= publicationTime);
            }

            if (query.ModificationTime is { } modificationTime)
            {
                modificationTime = DateTime.SpecifyKind(modificationTime, DateTimeKind.Utc).ToDateTimeTruncated();
                expressionsToConjunct.Add(it => it.LastUpdated >= modificationTime);
            }

            if (query.TroName is not null)
            {
                expressionsToConjunct.Add(it => it.TroName.ToLower().Contains(query.TroName.ToLower()));
            }

            if (query.VehicleType is not null)
            {
                expressionsToConjunct.Add(it => it.VehicleTypes.Contains(query.VehicleType));
            }

            if (query.RegulationType is not null)
            {
                expressionsToConjunct.Add(it => it.RegulationTypes.Contains(query.RegulationType));
            }

            if (query.RegulatedPlaceType is not null)
            {
                expressionsToConjunct.Add(it => it.RegulatedPlaceTypes.Contains(query.RegulatedPlaceType));
            }

            if (query.OrderReportingPoint is not null)
            {
                expressionsToConjunct.Add(it => it.OrderReportingPoints.Contains(query.OrderReportingPoint));
            }

            //if (query.Location is not null)
            //{
            //    BoundingBox boundingBox = query.Location.Format switch
            //    {
            //        "wgs84Epsg4326" => _projectionService.Wgs84ToOsgb36(query.Location.Bbox),
            //        "osgb36Epsg27700" or "wkt" => query.Location.Bbox,
            //        _ => new()
            //    };

            //    expressionsToConjunct.Add(it => DatabaseMethods.Overlaps(boundingBox, it.Location));
            //}

            if (query.RegulationStart is not null)
            {
                var value = query.RegulationStart.Value;

                Expression<Func<Models.DataBase.DTRO, bool>> expr = query.RegulationStart.Operator switch
                {
                    ComparisonOperator.Equal => (it) => it.RegulationStart == value,
                    ComparisonOperator.LessThan => (it) => it.RegulationStart < value,
                    ComparisonOperator.LessThanOrEqual => (it) => it.RegulationStart <= value,
                    ComparisonOperator.GreaterThan => (it) => it.RegulationStart > value,
                    ComparisonOperator.GreaterThanOrEqual => (it) => it.RegulationStart >= value,
                    _ => throw new InvalidOperationException("Unsupported comparison operator.")
                };

                expressionsToConjunct.Add(expr);
            }

            if (query.RegulationEnd is not null)
            {
                var value = query.RegulationEnd.Value;

                Expression<Func<Models.DataBase.DTRO, bool>> expr = query.RegulationEnd.Operator switch
                {
                    ComparisonOperator.Equal => (it) => it.RegulationEnd == value,
                    ComparisonOperator.LessThan => (it) => it.RegulationEnd < value,
                    ComparisonOperator.LessThanOrEqual => (it) => it.RegulationEnd <= value,
                    ComparisonOperator.GreaterThan => (it) => it.RegulationEnd > value,
                    ComparisonOperator.GreaterThanOrEqual => (it) => it.RegulationEnd >= value,
                    _ => throw new InvalidOperationException("Unsupported comparison operator.")
                };

                expressionsToConjunct.Add(expr);
            }

            if (!expressionsToConjunct.Any())
            {
                continue;
            }

            expressionsToDisjunct.Add(expressionsToConjunct.AllOf());
        }

        IQueryable<Models.DataBase.DTRO> dataQuery = expressionsToDisjunct.Any()
            ? result
                .Where(expressionsToDisjunct.AnyOf())
            : result;

        IQueryable<Models.DataBase.DTRO> paginatedQuery = dataQuery
            .OrderBy(it => it.Created)
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize);

        return new PaginatedResult<Models.DataBase.DTRO>(await paginatedQuery.ToListAsync(), await dataQuery.CountAsync());
    }

    ///<inheritdoc cref="IDtroDal"/>
    public async Task<List<Models.DataBase.DTRO>> FindDtrosAsync(DtroEventSearch search)
    {
        IQueryable<Models.DataBase.DTRO> result = _dtroContext.Dtros;

        var expressionsToConjunct = new List<Expression<Func<Models.DataBase.DTRO, bool>>>();

        if (search.DeletionTime is { } deletionTime)
        {
            deletionTime = DateTime.SpecifyKind(deletionTime, DateTimeKind.Utc);

            expressionsToConjunct.Add(it => it.DeletionTime >= deletionTime);
        }

        if (search.TraCreator is { } traCreator)
        {
            expressionsToConjunct.Add(it => it.TrafficAuthorityCreatorId == traCreator);
        }

        if (search.CurrentTraOwner is { } currentTraOwner)
        {
            expressionsToConjunct.Add(it => it.TrafficAuthorityOwnerId == currentTraOwner);
        }

        if (search.Since is { } publicationTime)
        {
            publicationTime = DateTime.SpecifyKind(publicationTime, DateTimeKind.Utc);

            expressionsToConjunct.Add(it =>
                it.Created >= publicationTime ||
                it.LastUpdated >= publicationTime ||
                (it.DeletionTime != null && it.DeletionTime >= publicationTime));
        }

        if (search.ModificationTime is { } modificationTime)
        {
            modificationTime = DateTime.SpecifyKind(modificationTime, DateTimeKind.Utc);

            expressionsToConjunct.Add(it => it.LastUpdated >= modificationTime);
        }

        if (search.TroName is not null)
        {
            expressionsToConjunct.Add(it => it.TroName.ToLower().Contains(search.TroName.ToLower()));
        }

        if (search.VehicleType is not null)
        {
            expressionsToConjunct.Add(it => it.VehicleTypes.Contains(search.VehicleType));
        }

        if (search.RegulationType is not null)
        {
            expressionsToConjunct.Add(it => it.RegulationTypes.Contains(search.RegulationType));
        }

        if (search.RegulatedPlaceType is not null)
        {
            expressionsToConjunct.Add(it => it.RegulatedPlaceTypes.Contains(search.RegulatedPlaceType));
        }

        if (search.OrderReportingPoint is not null)
        {
            expressionsToConjunct.Add(it => it.OrderReportingPoints.Contains(search.OrderReportingPoint));
        }

        //if (search.Location is not null)
        //{
        //    var boundingBox =
        //        search.Location.Format != "osgb36Epsg27700"
        //            ? _projectionService.Wgs84ToOsgb36(search.Location.Bbox)
        //            : search.Location.Bbox;

        //    expressionsToConjunct.Add(it => DatabaseMethods.Overlaps(boundingBox, it.Location));
        //}

        if (search.RegulationStart is not null)
        {
            var value = search.RegulationStart.Value;

            Expression<Func<Models.DataBase.DTRO, bool>> expr = search.RegulationStart.Operator switch
            {
                ComparisonOperator.Equal => it => it.RegulationStart == value,
                ComparisonOperator.LessThan => it => it.RegulationStart < value,
                ComparisonOperator.LessThanOrEqual => it => it.RegulationStart <= value,
                ComparisonOperator.GreaterThan => it => it.RegulationStart > value,
                ComparisonOperator.GreaterThanOrEqual => it => it.RegulationStart >= value,
                var _ => throw new InvalidOperationException("Unsupported comparison operator.")
            };

            expressionsToConjunct.Add(expr);
        }

        if (search.RegulationEnd is not null)
        {
            var value = search.RegulationEnd.Value;

            Expression<Func<Models.DataBase.DTRO, bool>> expr = search.RegulationEnd.Operator switch
            {
                ComparisonOperator.Equal => (it) => it.RegulationEnd == value,
                ComparisonOperator.LessThan => (it) => it.RegulationEnd < value,
                ComparisonOperator.LessThanOrEqual => (it) => it.RegulationEnd <= value,
                ComparisonOperator.GreaterThan => (it) => it.RegulationEnd > value,
                ComparisonOperator.GreaterThanOrEqual => (it) => it.RegulationEnd >= value,
                var _ => throw new InvalidOperationException("Unsupported comparison operator.")
            };

            expressionsToConjunct.Add(expr);
        }

        if (!expressionsToConjunct.Any())
        {
            return await result.OrderBy(it => it.Id).ToListAsync();
        }

        var sqlQuery = result
            .Where(expressionsToConjunct.AllOf())
            .OrderBy(it => it.Id);

        return await sqlQuery.ToListAsync();
    }

    ///<inheritdoc cref="IDtroDal"/>
    public async Task<bool> DeleteDtroAsync(Guid id, DateTime? deletionTime = null)
    {
        var dtro = await _dtroContext.Dtros.FindAsync(id);

        if (dtro is null)
        {
            return false;
        }

        _dtroContext.Dtros.Remove(dtro);
        await _dtroContext.SaveChangesAsync();
        return true;
    }
}