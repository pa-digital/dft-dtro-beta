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
    private readonly IAppIdMapperService _appIdMapperService;
    private readonly LoggingExtension _loggingExtension;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="eventSearchService">An <see cref="IEventSearchService"/> instance.</param>
    /// <param name="metricsService">An <see cref="IMetricsService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{EventsController}"/> instance.</param>
    /// <param name="loggingExtension">An <see cref="LoggingExtension"/> instance.</param>
    public EventsController(
        IEventSearchService eventSearchService,
        IMetricsService metricsService,
          IAppIdMapperService appIdMapperService,
        ILogger<EventsController> logger,
         LoggingExtension loggingExtension)
    {
        _eventSearchService = eventSearchService;
        _metricsService = metricsService;
        _appIdMapperService = appIdMapperService;
        _logger = logger;
        _loggingExtension = loggingExtension;
    }

    /// <summary>
    /// Retrieve all data store events
    /// </summary>
    /// <param name="appId">AppId identification a D-TRO is being submitted for.</param>
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
    public async Task<ActionResult<DtroEventSearchResult>> Events([FromHeader(Name = "x-app-id")][Required] Guid appId, [FromBody] DtroEventSearch search)
    {
        try
        {
            appId = await _appIdMapperService.GetAppId(HttpContext);
            DtroEventSearchResult response = await _eventSearchService.SearchAsync(search);
            await _metricsService.IncrementMetric(MetricType.Event, appId);
            _logger.LogInformation($"'{nameof(Events)}' method called '{search}'");
            _loggingExtension.LogInformation(
                nameof(Events),
                "/events",
                $"'{nameof(Events)}' method called");
            return Ok(response);
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(nameof(Events), "/events", $"Not Found: {nfex.Message}", nfex.StackTrace);
            return NotFound(new ApiErrorResponse("Not Found", nfex.Message));
        }
        catch (InvalidOperationException ioex)
        {
            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(nameof(Events), "/events", $"Bad Request: {ioex.Message}", ioex.StackTrace);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(nameof(Events), "/events", $"Unexpected Null value was found: {anex.Message}", anex.StackTrace);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(Events),
                "/events",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                nameof(Events),
                "/events",
                "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Unable to save record(s) to the database."));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, appId);
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(Events), "/events", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }
}
