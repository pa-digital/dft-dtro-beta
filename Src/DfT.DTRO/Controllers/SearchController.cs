using ILogger = Google.Apis.Logging.ILogger;

namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for searching D-TROs
/// </summary>
[Tags("Search")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;
    private readonly IMetricsService _metricsService;
    private readonly IXappIdMapperService _appIdMapperService;

    private readonly ILogger _logger;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="searchService">An <see cref="ISearchService"/> instance.</param>
    /// <param name="metricsService">An <see cref="IMetricsService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{SearchController}"/> intance.</param>
    public SearchController(
        ISearchService searchService,
        IMetricsService metricsService,
        IXappIdMapperService appIdMapperService, ILogger logger)
    {
        _searchService = searchService;
        _metricsService = metricsService;
        _appIdMapperService = appIdMapperService;
        _logger = logger;
    }

    /// <summary>
    /// Finds existing D-TROs that match the required criteria.
    /// </summary>
    /// <param name="body">A D-TRO object search criteria.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>D-TRO matching searching request.</returns>
    [HttpPost]
    [Route("/search")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Consumer)]
    [SwaggerResponse(200, type: typeof(PaginatedResponse<DtroSearchResult>), description: "Ok")]
    [SwaggerResponse(statusCode: 400, description: "Bad Request.")]
    [SwaggerResponse(statusCode: 500, description: "Internal Server Error.")]
    public async Task<ActionResult<PaginatedResponse<DtroSearchResult>>> SearchDtros([FromHeader(Name = "x-app-id")][Required] Guid xAppId, [FromBody] DtroSearch body)
    {
        _logger.Info($"Entering method: {nameof(SearchDtros)}");
        try
        {
            xAppId = await _appIdMapperService.GetXappId(HttpContext);
            PaginatedResponse<DtroSearchResult> response = await _searchService.SearchAsync(body);
            _logger.Info($"{string.Join(",", response)}");
            await _metricsService.IncrementMetric(MetricType.Search, xAppId);
            return Ok(response);
        }
        catch (InvalidOperationException err)
        {
            _logger.Error(err.Message);
            _logger.Info(err.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, xAppId);
            _logger.Error(ex.Message);
            _logger.Info(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}