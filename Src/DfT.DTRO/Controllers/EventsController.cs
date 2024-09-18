namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for querying the database for data store events (e.g. create, update, delete)
/// </summary>
[Tags("Events")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventSearchService _eventSearchService;
    private readonly IMetricsService _metricsService;
    private readonly ILogger<EventsController> _logger;
    private readonly IXappIdMapperService _appIdMapperService;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="eventSearchService">An <see cref="IEventSearchService"/> instance.</param>
    /// <param name="metricsService">An <see cref="IMetricsService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{EventsController}"/> instance.</param>
    public EventsController(
        IEventSearchService eventSearchService,
        IMetricsService metricsService,
          IXappIdMapperService appIdMapperService,
        ILogger<EventsController> logger)
    {
        _eventSearchService = eventSearchService;
        _metricsService = metricsService;
        _appIdMapperService = appIdMapperService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieve all data store events
    /// </summary>
    /// <param name="search">A search query object</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Search result.</returns>
    [HttpPost("/events")]
    [FeatureGate(FeatureNames.Consumer)]
    [ValidateModelState]
    [SwaggerResponse(statusCode: 200, description: "Successfully received the event list")]
    [SwaggerResponse(statusCode: 400, description: "The request was malformed.")]
    [SwaggerResponse(statusCode: 404, description: "Event(s) not found.")]
    public async Task<ActionResult<DtroEventSearchResult>> Events([FromHeader(Name = "x-app-id")][Required] Guid xAppId, [FromBody] DtroEventSearch search)
    {
        try
        {
            xAppId = await _appIdMapperService.GetXappId(HttpContext);
            DtroEventSearchResult response = await _eventSearchService.SearchAsync(search);
            await _metricsService.IncrementMetric(MetricType.Event, xAppId);
            _logger.LogInformation($"'{nameof(Events)}' method called '{search}'");
            return Ok(response);
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("No Found", nFex.Message));
        }
        catch (InvalidOperationException err)
        {
            _logger.LogError(err.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, xAppId);
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}
