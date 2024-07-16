using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DfT.DTRO.Caching;
using DfT.DTRO.DAL;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.DtroEvent;
using DfT.DTRO.Models.Filtering;
using DfT.DTRO.Models.Pagination;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.SharedResponse;
using DfT.DTRO.Services.Conversion;
using DfT.DTRO.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using static DfT.DTRO.Extensions.ExpressionExtensions;

namespace DfT.DTRO.Services;

[ExcludeFromCodeCoverage]
public class DtroDal : IDtroDal
{
    private readonly DtroContext _dtroContext;
    private readonly ISpatialProjectionService _projectionService;
    private readonly IDtroMappingService _dtroMappingService;
    private readonly IRedisCache _dtroCache;

    public DtroDal(DtroContext dtroContext, ISpatialProjectionService projectionService, IDtroMappingService dtroMappingService, IRedisCache dtroCache)
    {
        _dtroContext = dtroContext;
        _projectionService = projectionService;
        _dtroMappingService = dtroMappingService;
        _dtroCache = dtroCache;
    }

    public async Task<bool> SoftDeleteDtroAsync(Guid id, DateTime? deletionTime)
    {
        Models.DataBase.DTRO existing = await _dtroContext.Dtros.FindAsync(id);

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

    public async Task<bool> DtroExistsAsync(Guid id)
    {
        return await _dtroContext.Dtros.AnyAsync(it => it.Id == id && !it.Deleted);
    }

    public async Task<int> DtroCountForSchemaAsync(SchemaVersion schemaVersion)
    {
        return await _dtroContext.Dtros.CountAsync(it => it.SchemaVersion == schemaVersion);
    }

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
            return null;
        }

        await _dtroCache.CacheDtro(dtro);
        return dtro;
    }

    public async Task<GuidResponse> SaveDtroAsJsonAsync(DtroSubmit dtroSubmit, string correlationId)
    {
        var dtro = new Models.DataBase.DTRO();
        var response = new GuidResponse();
        dtro.Id = response.Id;

        dtro.SchemaVersion = dtroSubmit.SchemaVersion;


        dtro.Data = dtroSubmit.Data;
        var now = DateTime.UtcNow;
        dtro.LastUpdated = now;
        dtro.Created = now;
        dtro.LastUpdatedCorrelationId = correlationId;
        dtro.CreatedCorrelationId = dtro.LastUpdatedCorrelationId;

        _dtroMappingService.InferIndexFields(ref dtro);

        await _dtroContext.Dtros.AddAsync(dtro);

        await _dtroContext.SaveChangesAsync();
        return response;
    }

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

    public async Task UpdateDtroAsJsonAsync(Guid id, DtroSubmit dtroSubmit, string correlationId)
    {
        if (await _dtroContext.Dtros.FindAsync(id) is not Models.DataBase.DTRO existing || existing.Deleted)
        {
            throw new InvalidOperationException($"There is no DTRO with Id {id}");
        }

        existing.SchemaVersion = dtroSubmit.SchemaVersion;
        existing.Data = dtroSubmit.Data;

        existing.LastUpdated = DateTime.UtcNow;
        existing.LastUpdatedCorrelationId = correlationId;

        _dtroContext.Entry(existing).Property(e => e.Data).IsModified = true;

        _dtroMappingService.InferIndexFields(ref existing);
        await _dtroCache.RemoveDtro(id);
        await _dtroContext.SaveChangesAsync();
    }

    public async Task AssignDtroOwnership(Guid id, int assignToTraId, string correlationId)
    {
        if (await _dtroContext.Dtros.FindAsync(id) is not Models.DataBase.DTRO existing || existing.Deleted)
        {
            throw new InvalidOperationException($"There is no DTRO with Id {id}");
        }

        _dtroMappingService.SetOwnership(ref existing, assignToTraId);
        _dtroMappingService.SetSourceActionType(ref existing, Enums.SourceActionType.Amendment);
        existing.LastUpdated = DateTime.UtcNow;
        existing.LastUpdatedCorrelationId = correlationId;

        _dtroMappingService.InferIndexFields(ref existing);
        await _dtroCache.RemoveDtro(id);

        _dtroContext.Entry(existing).Property(e => e.Data).IsModified = true;

        await _dtroContext.SaveChangesAsync();
    }

    public async Task<PaginatedResult<Models.DataBase.DTRO>> FindDtrosAsync(DtroSearch search)
    {
        IQueryable<Models.DataBase.DTRO> result = _dtroContext.Dtros;

        var expressionsToDisjunct = new List<Expression<Func<Models.DataBase.DTRO, bool>>>();

        foreach (var query in search.Queries)
        {
            var expressionsToConjunct = new List<Expression<Func<Models.DataBase.DTRO, bool>>>();

            if (query.DeletionTime is DateTime deletionTime)
            {
                deletionTime = DateTime.SpecifyKind(deletionTime, DateTimeKind.Utc);
                expressionsToConjunct.Add(it => it.DeletionTime >= deletionTime);
            }
            else
            {
                expressionsToConjunct.Add(it => !it.Deleted);
            }

            if (query.TraCreator is int traCreator)
            {
                expressionsToConjunct.Add(it => it.TrafficAuthorityCreatorId == traCreator);
            }

            if (query.CurrentTraOwner is int currentTraOwner)
            {
                expressionsToConjunct.Add(it => it.TrafficAuthorityOwnerId == currentTraOwner);
            }

            if (query.PublicationTime is DateTime publicationTime)
            {
                publicationTime = DateTime.SpecifyKind(publicationTime, DateTimeKind.Utc);
                expressionsToConjunct.Add(it => it.Created >= publicationTime);
            }

            if (query.ModificationTime is DateTime modificationTime)
            {
                modificationTime = DateTime.SpecifyKind(modificationTime, DateTimeKind.Utc);
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

            if (query.OrderReportingPoint is not null)
            {
                expressionsToConjunct.Add(it => it.OrderReportingPoints.Contains(query.OrderReportingPoint));
            }

            if (query.Location is not null)
            {
                var bbox =
                    query.Location.Crs != "osgb36Epsg27700"
                        ? _projectionService.Wgs84ToOsgb36(query.Location.Bbox)
                        : query.Location.Bbox;

                expressionsToConjunct.Add(it => DatabaseMethods.Overlaps(bbox, it.Location));
            }

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

            expressionsToDisjunct.Add(AllOf(expressionsToConjunct));
        }

        IQueryable<Models.DataBase.DTRO> dataQuery = expressionsToDisjunct.Any()
            ? result
                .Where(AnyOf(expressionsToDisjunct))
            : result;

        IQueryable<Models.DataBase.DTRO> paginatedQuery = dataQuery
            .OrderBy(it => it.Created)
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize);

        return new PaginatedResult<Models.DataBase.DTRO>(await paginatedQuery.ToListAsync(), await dataQuery.CountAsync());
    }

    public async Task<List<Models.DataBase.DTRO>> FindDtrosAsync(DtroEventSearch search)
    {
        IQueryable<Models.DataBase.DTRO> result = _dtroContext.Dtros;

        var expressionsToConjunct = new List<Expression<Func<Models.DataBase.DTRO, bool>>>();

        if (search.DeletionTime is DateTime deletionTime)
        {
            deletionTime = DateTime.SpecifyKind(deletionTime, DateTimeKind.Utc);

            expressionsToConjunct.Add(it => it.DeletionTime >= deletionTime);
        }

        if (search.TraCreator is int traCreator)
        {
            expressionsToConjunct.Add(it => it.TrafficAuthorityCreatorId == traCreator);
        }

        if (search.CurrentTraOwner is int currentTraOwner)
        {
            expressionsToConjunct.Add(it => it.TrafficAuthorityOwnerId == currentTraOwner);
        }

        if (search.Since is DateTime publicationTime)
        {
            publicationTime = DateTime.SpecifyKind(publicationTime, DateTimeKind.Utc);

            expressionsToConjunct.Add(it => it.Created >= publicationTime);
        }

        if (search.ModificationTime is DateTime modificationTime)
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

        if (search.OrderReportingPoint is not null)
        {
            expressionsToConjunct.Add(it => it.OrderReportingPoints.Contains(search.OrderReportingPoint));
        }

        if (search.Location is not null)
        {
            var bbox =
                search.Location.Crs != "osgb36Epsg27700"
                    ? _projectionService.Wgs84ToOsgb36(search.Location.Bbox)
                    : search.Location.Bbox;

            expressionsToConjunct.Add(it => DatabaseMethods.Overlaps(bbox, it.Location));
        }

        if (search.RegulationStart is not null)
        {
            var value = search.RegulationStart.Value;

            Expression<Func<Models.DataBase.DTRO, bool>> expr = search.RegulationStart.Operator switch
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
                _ => throw new InvalidOperationException("Unsupported comparison operator.")
            };

            expressionsToConjunct.Add(expr);
        }

        if (!expressionsToConjunct.Any())
        {
            return await result.OrderBy(it => it.Id).ToListAsync();
        }

        var sqlQuery = result
            .Where(AllOf(expressionsToConjunct))
            .OrderBy(it => it.Id);

        return await sqlQuery.ToListAsync();
    }

    public async Task<bool> DeleteDtroAsync(Guid id, DateTime? deletionTime = null)
    {
        Models.DataBase.DTRO dtro = await _dtroContext.Dtros.FindAsync(id);

        if (dtro is null)
        {
            return false;
        }

        _dtroContext.Dtros.Remove(dtro);
        await _dtroContext.SaveChangesAsync();
        return true;
    }
}