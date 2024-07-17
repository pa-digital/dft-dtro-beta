namespace DfT.DTRO.Controllers;

[Tags("Events")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventSearchService _eventSearchService;
    private readonly IMetricsService _metricsService;
    private readonly ILogger<EventsController> _logger;

    public EventsController(
        IEventSearchService eventSearchService,
        IMetricsService metricsService,
        ILogger<EventsController> logger)
    {
        _eventSearchService = eventSearchService;
        _metricsService = metricsService;
        _logger = logger;
    }

    [HttpPost("/v1/events")]
    [FeatureGate(FeatureNames.DtroRead)]
    [ValidateModelState]
    [SwaggerResponse(statusCode: 200, description: "Successfully received the event list")]
    [SwaggerResponse(statusCode: 400, description: "The request was malformed.")]
    public async Task<ActionResult<DtroEventSearchResult>> Events([FromHeader(Name = "TA")][Required] int? ta, [FromBody] DtroEventSearch search)
    {
        try
        {
            var response = await _eventSearchService.SearchAsync(search);
            await _metricsService.IncrementMetric(MetricType.Event, ta);
            return Ok(response);
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, ta);
            _logger.LogError(ex, "An error occurred while processing GetById request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}
