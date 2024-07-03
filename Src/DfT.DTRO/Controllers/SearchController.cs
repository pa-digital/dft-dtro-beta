using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DfT.DTRO.Attributes;
using DfT.DTRO.Extensions;
using DfT.DTRO.FeatureManagement;
using DfT.DTRO.Models.Errors;
using DfT.DTRO.Models.Filtering;
using DfT.DTRO.Models.Pagination;
using DfT.DTRO.Models.Search;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for searching DTROs.
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
    /// The default constructor.
    /// </summary>
    /// <param name="searchService">An <see cref="ISearchService"/> instance.</param>
    /// <param name="metricsService">An <see cref="IMetricsService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{SearchController}"/> instance.</param>
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
    /// Finds existing DTROs that match the requested criteria.
    /// </summary>
    /// <param name="body">A DTRO search criteria object.</param>
    /// <response code="200">Ok.</response>
    /// <response code="400">Bad request.</response>
    /// <returns>Dtros matching search criteria.</returns>
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