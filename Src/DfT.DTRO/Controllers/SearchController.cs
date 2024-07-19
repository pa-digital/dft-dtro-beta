namespace DfT.DTRO.Controllers;

[Tags("Search")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;
    private readonly IMetricsService _metricsService;
    private readonly ILogger<SearchController> _logger;

    public SearchController(
        ISearchService searchService,
        IMetricsService metricsService,
        ILogger<SearchController> logger)
    {
        _searchService = searchService;
        _metricsService = metricsService;
        _logger = logger;
    }

    [HttpPost]
    [Route("/v1/search")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.DtroRead)]
    [SwaggerResponse(200, type: typeof(PaginatedResponse<DtroSearchResult>), description: "Ok")]
    public async Task<ActionResult<PaginatedResponse<DtroSearchResult>>> SearchDtros([FromHeader(Name = "TA")][Required] int? ta, [FromBody] DtroSearch body)
    {
        try
        {
            PaginatedResponse<DtroSearchResult> response = await _searchService.SearchAsync(body);
            await _metricsService.IncrementMetric(MetricType.Search, ta);
            _logger.LogInformation($"'{nameof(SearchDtros)}' method called using TRA Id: '{ta}' and body '{body}'");
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