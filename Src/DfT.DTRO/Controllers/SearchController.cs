using DfT.DTRO.Enums;

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

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="searchService">An <see cref="ISearchService"/> instance.</param>
    /// <param name="metricsService">An <see cref="IMetricsService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{SearchController}"/> intance.</param>
    public SearchController(
        ISearchService searchService,
        IMetricsService metricsService,
        ILogger<SearchController> logger)
    {
        _searchService = searchService;
        _metricsService = metricsService;
        _logger = logger;
    }

    /// <summary>
    /// Finds existing D-TROs that match the required criteria.
    /// </summary>
    /// <param name="ta">TRA identification a D-TRO is search for.</param>
    /// <param name="body">A D-TRO object search criteria.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>D-TRO matching searching request.</returns>
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