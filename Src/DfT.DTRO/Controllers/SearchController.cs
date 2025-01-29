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
    private readonly ILogger<SearchController> _logger;
    private readonly IAppIdMapperService _appIdMapperService;
    private readonly LoggingExtension _loggingExtension;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="searchService">An <see cref="ISearchService"/> instance.</param>
    /// <param name="metricsService">An <see cref="IMetricsService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{SearchController}"/> intance.</param>
    /// <param name="loggingExtension">An <see cref="LoggingExtension"/> instance.</param>
    public SearchController(
        ISearchService searchService,
        IMetricsService metricsService,
        IAppIdMapperService appIdMapperService,
        ILogger<SearchController> logger,
        LoggingExtension loggingExtension)
    {
        _searchService = searchService;
        _metricsService = metricsService;
        _appIdMapperService = appIdMapperService;
        _logger = logger;
        _loggingExtension = loggingExtension;
    }

    /// <summary>
    /// Finds existing D-TROs that match the required criteria.
    /// </summary>
    /// <param name="appId">AppId identification.</param>
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
    public async Task<ActionResult<PaginatedResponse<DtroSearchResult>>> SearchDtros([FromHeader(Name = "x-app-id")][Required] Guid appId, [FromBody] DtroSearch body)
    {
        try
        {
            appId = await _appIdMapperService.GetAppId(HttpContext);
            var response = await _searchService.SearchAsync(body);
            await _metricsService.IncrementMetric(MetricType.Search, appId);
            _logger.LogInformation($"'{nameof(SearchDtros)}' method called and body '{body}'");
            _loggingExtension.LogInformation(
                nameof(SearchDtros),
                "/search",
                $"'{nameof(SearchDtros)}' method called and body '{body}'");
            return Ok(response);
        }
        catch (InvalidOperationException ioex)
        {
            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(nameof(SearchDtros), "/search", "Bad Request", ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(nameof(SearchDtros), "/search", "Unexpected Null value was found", anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, appId);
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(SearchDtros), "/search", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occured: {ex.Message}"));
        }
    }
}