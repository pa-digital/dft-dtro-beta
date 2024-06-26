﻿using DfT.DTRO.Models.DtroEvent;
using DfT.DTRO.Services.Mapping;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DfT.DTRO.Services;

/// <summary>
/// An implementation of <see cref="IEventSearchService"/>
/// </summary>
public class EventSearchService : IEventSearchService
{
    private readonly IDtroService _dtroService;
    private readonly IDtroMappingService _dtroMappingService;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="dtroService">An <see cref="IDtroService"/> instance.</param>
    /// <param name="dtroMappingService">An <see cref="IDtroMappingService"/> instance.</param>
    public EventSearchService(IDtroService dtroService, IDtroMappingService dtroMappingService)
    {
        _dtroService = dtroService;
        _dtroMappingService = dtroMappingService;
    }

    /// <inheritdoc/>
    public async Task<DtroEventSearchResult> SearchAsync(DtroEventSearch search)
    {
        if (search.Since is not null && search.Since > DateTime.Now)
        {
            throw new InvalidOperationException("The datetime for the since field cannot be in the future.");
        }

        var searchRes = await _dtroService.FindDtrosAsync(search);

        var events = _dtroMappingService.MapToEvents(searchRes).ToList();

        var paginatedEvents = events
            .Skip((search.Page.Value - 1) * search.PageSize.Value)
            .Take(search.PageSize.Value)
            .ToList();

        var res = new DtroEventSearchResult
        {
            TotalCount = events.Count(),
            Events = paginatedEvents,
            Page = search.Page.Value,
            PageSize = Math.Min(search.PageSize.Value, paginatedEvents.Count)
        };

        return res;
    }
}