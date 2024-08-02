using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.SwaCode;
using Microsoft.EntityFrameworkCore;

namespace DfT.DTRO.DAL;

[ExcludeFromCodeCoverage]
public class SwaCodeDal : ISwaCodeDal
{
    private readonly DtroContext _dtroContext;

    public SwaCodeDal(DtroContext dtroContext) => _dtroContext = dtroContext;

    public async Task<List<SwaCodeResponse>> GetAllCodesAsync() =>
        await _dtroContext.SwaCodes
            .OrderBy(swaCode => swaCode.Name)
            .Select(swaCode => new SwaCodeResponse
            {
                TraId = swaCode.TraId,
                Name = swaCode.Name,
                Prefix = swaCode.Prefix,
                IsAdmin = swaCode.IsAdmin,
                IsActive = swaCode.IsActive
            })
            .ToListAsync();

    public async Task<SwaCodeResponse> GetSwaCodeAsync(int traId)
    {
        var swaCode = await _dtroContext.SwaCodes
            .Where(swaCode => swaCode.TraId == traId)
            .Select(swaCode => new SwaCodeResponse
            {
                TraId = swaCode.TraId,
                Name = swaCode.Name,
                Prefix = swaCode.Prefix,
                IsAdmin = swaCode.IsAdmin,
                IsActive = swaCode.IsActive
            })
            .FirstOrDefaultAsync();

        return swaCode ?? new SwaCodeResponse(); // Return a default instance if no record is found
    }

    public async Task<List<SwaCodeResponse>> SearchSwaCodesAsync(string partialName) =>
     await _dtroContext.SwaCodes
         .Where(swaCode => EF.Functions.Like(swaCode.Name.ToLower(), $"%{partialName.ToLower()}%"))
         .OrderBy(swaCode => swaCode.Name)
         .Select(swaCode => new SwaCodeResponse
         {
             TraId = swaCode.TraId,
             Name = swaCode.Name,
             Prefix = swaCode.Prefix,
             IsAdmin = swaCode.IsAdmin,
             IsActive = swaCode.IsActive
         })
         .ToListAsync();


    public async Task<GuidResponse> ActivateTraAsync(int traId)
    {
        if (!await TraExistsAsync(traId))
        {
            throw new InvalidOperationException($"There is no TRA with ID {traId}");
        }

        var existing = await GetTraAsync(traId);
        existing.IsActive = true;
        await _dtroContext.SaveChangesAsync();
        return new GuidResponse() { Id = existing.Id };
    }

    public async Task<GuidResponse> DeActivateTraAsync(int traId)
    {
        if (!await TraExistsAsync(traId))
        {
            throw new InvalidOperationException($"There is no TRA with ID {traId}");
        }

        var existing = await GetTraAsync(traId);
        existing.IsActive = false;
        await _dtroContext.SaveChangesAsync();
        return new GuidResponse() { Id = existing.Id };
    }

    public async Task<bool> TraExistsAsync(int traId)
    {
        var exists = await _dtroContext.SwaCodes.AnyAsync(it => it.TraId == traId);
        return exists;
    }
    public async Task<SwaCode> GetTraAsync(int traId)
    {
        var ret = await _dtroContext.SwaCodes.FirstOrDefaultAsync(b => b.TraId == traId);
        return ret;
    }

    public async Task<GuidResponse> SaveTraAsync(SwaCodeRequest swaCodeRequest)
    {
        var swaCode = new SwaCode();
        var response = new GuidResponse();

        swaCode.Id = response.Id;
        swaCode.TraId = swaCodeRequest.TraId;
        swaCode.Name = swaCodeRequest.Name;
        swaCode.Prefix = swaCodeRequest.Prefix;
        swaCode.IsAdmin = swaCode.IsAdmin;
        swaCode.IsActive = swaCode.IsActive;

        if (await TraExistsAsync(swaCodeRequest.TraId))
        {
            throw new InvalidOperationException($"There is an existing TRA with Id {swaCodeRequest.TraId}");
        }

        await _dtroContext.SwaCodes.AddAsync(swaCode);

        await _dtroContext.SaveChangesAsync();
        return response;
    }

    public async Task<GuidResponse> UpdateTraAsync(SwaCodeRequest swaCodeRequest)
    {
        if (!await TraExistsAsync(swaCodeRequest.TraId))
        {
            throw new InvalidOperationException($"There is no TRA with ID {swaCodeRequest.TraId}");
        }

        var existing = await GetTraAsync(swaCodeRequest.TraId);
        existing.Name = swaCodeRequest.Name;
        existing.Prefix = swaCodeRequest.Prefix;
        existing.IsAdmin = swaCodeRequest.IsAdmin;
        existing.IsActive = swaCodeRequest.IsActive;

        await _dtroContext.SaveChangesAsync();
        return new GuidResponse() { Id = existing.Id };
    }
}