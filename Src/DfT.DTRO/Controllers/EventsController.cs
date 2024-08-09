using DfT.DTRO.Enums;

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

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="eventSearchService">An <see cref="IEventSearchService"/> instance.</param>
    /// <param name="metricsService">An <see cref="IMetricsService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{EventsController}"/> instance.</param>
    public EventsController(
        IEventSearchService eventSearchService,
        IMetricsService metricsService,
        ILogger<EventsController> logger)
    {
        _eventSearchService = eventSearchService;
        _metricsService = metricsService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieve all data store events
    /// </summary>
    /// <param name="ta">TRA identification retrieve is for.</param>
    /// <param name="search">A search query object</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Search result.</returns>
    [HttpPost("/events")]
    [FeatureGate(FeatureNames.DtroRead)]
    [ValidateModelState]
    [SwaggerResponse(statusCode: 200, description: "Successfully received the event list")]
    [SwaggerResponse(statusCode: 400, description: "The request was malformed.")]
    public async Task<ActionResult<DtroEventSearchResult>> Events([FromHeader(Name = "TA")][Required] int? ta, [FromBody] DtroEventSearch search)
    {
        try
        {
            DtroEventSearchResult response = await _eventSearchService.SearchAsync(search);
            await _metricsService.IncrementMetric(MetricType.Event, ta);
            _logger.LogInformation($"'{nameof(Events)}' method called using TRA Id: '{ta}' and body '{search}'");
            return Ok(response);
        }
        catch (InvalidOperationException err)
        {
            _logger.LogError(err.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, ta);
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}
