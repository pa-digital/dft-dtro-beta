using System;
using System.Threading.Tasks;
using DfT.DTRO.Attributes;
using DfT.DTRO.FeatureManagement;
using DfT.DTRO.Models.DtroEvent;
using DfT.DTRO.Models.Errors;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller implementation that allows users to query data store events (e.g., D-TROs being created, updated or deleted).
/// </summary>
[Tags("Events")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventSearchService _eventSearchService;
    private readonly ILogger<EventsController> _logger;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="eventSearchService">An <see cref="IEventSearchService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{EventsController}"/> instance.</param>
    public EventsController(
        IEventSearchService eventSearchService,
        ILogger<EventsController> logger)
    {
        _eventSearchService = eventSearchService;
        _logger = logger;
    }

    /// <summary>
    /// Endpoint for querying central data store events (e.g., D-TROs being created, updated or deleted).
    /// </summary>
    /// <param name="search">A search query object.</param>
    /// <response code="200">Ok.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Search Result.</returns>
    [HttpPost("/v1/events")]
    [FeatureGate(FeatureNames.DtroRead)]
    [ValidateModelState]
    [SwaggerResponse(statusCode: 200, description: "Successfully received the event list")]
    [SwaggerResponse(statusCode: 400, description: "The request was malformed.")]
    public async Task<ActionResult<DtroEventSearchResult>> Events([FromBody] DtroEventSearch search)
    {
        try
        {
            var response = await _eventSearchService.SearchAsync(search);
            return Ok(response);
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing GetById request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}
