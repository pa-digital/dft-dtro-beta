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
using System;
using System.Threading.Tasks;

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
    private readonly ILogger<SearchController> _logger;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="searchService">An <see cref="ISearchService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{SearchController}"/> instance.</param>
    public SearchController(
        ISearchService searchService,
        ILogger<SearchController> logger)
    {
        _searchService = searchService;
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
    public async Task<ActionResult<PaginatedResponse<DtroSearchResult>>> SearchDtros([FromBody] DtroSearch body)
    {
        try
        {
            _logger.LogInformation("[{method}] Searching DTROs with criteria {searchCriteria}", "dtro.search", body.ToIndentedJsonString());
            var response = await _searchService.SearchAsync(body);
            return Ok(response);
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing GetSchemaById request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    [HttpPost]
    [Route("/v1/search2")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.DtroRead)]
    [SwaggerResponse(200, type: typeof(PaginatedResponse<DtroSearchResult>), description: "Ok")]
    public async Task<ActionResult<PaginatedResponse<DtroSearchResult>>> SearchDtros2([FromBody] DtroSearch2 body)
    {
        try
        {
            _logger.LogInformation("[{method}] Searching DTROs with criteria {searchCriteria}", "dtro.search", body.ToIndentedJsonString());
            var response = await _searchService.SearchAsync(new DtroSearch());
            return Ok(response);
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing GetSchemaById request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}