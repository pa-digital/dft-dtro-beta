using DfT.DTRO.Extensions;
using DfT.DTRO.FeatureManagement;
using DfT.DTRO.Models.Filtering;
using DfT.DTRO.Models.Pagination;
using DfT.DTRO.Models.Search;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement.Mvc;

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
            _logger.LogInformation("[{method}] Searching DTROs with criteria {searchCriteria}", "dtro.search", body.ToIndentedJsonString());
            var response = await _searchService.SearchAsync(body);
            await _metricsService.IncrementMetric(MetricType.Search, ta);
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