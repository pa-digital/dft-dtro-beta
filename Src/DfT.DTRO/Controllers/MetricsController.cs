using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DfT.DTRO.Attributes;
using DfT.DTRO.FeatureManagement;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.Errors;
using DfT.DTRO.Models.Metrics;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller implementation that allows users to obtain metrics.
/// </summary>
[Tags("Metrics")]
[ApiController]
public class MetricsController : ControllerBase
{
    private readonly IMetricsService _metricsService;
    private readonly ILogger<MetricsController> _logger;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="metricsService">An <see cref="IMetricsService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{MetricsController}"/> instance.</param>
    public MetricsController(
        IMetricsService metricsService,
        ILogger<MetricsController> logger)
    {
        _metricsService = metricsService;
        _logger = logger;
    }

    /// <summary>
    /// Endpoint for checking if the API is up and running.
    /// </summary>
    /// <response code="200">Ok.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>True.</returns>
    [HttpGet("/v1/healthApi")]
    [FeatureGate(RequirementType.Any, FeatureNames.DtroRead, FeatureNames.DtroWrite)]
    [ValidateModelState]
    [SwaggerResponse(statusCode: 200, description: "The API is up and running.")]
    [SwaggerResponse(statusCode: 500, description: "Api server error.")]
    public ActionResult<bool> HealthApi()
    {
        try
        {
            return Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing ApiHealth request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Check passing the TRA ID in the header.
    /// </summary>
    /// <param name="ta">The TRA ID.</param>
    /// <response code="200">Ok.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("/v1/healthTraId")]
    [FeatureGate(RequirementType.Any, FeatureNames.DtroRead, FeatureNames.DtroWrite)]
    [ValidateModelState]
    [SwaggerResponse(statusCode: 200, description: "Successfully received the Tra Id")]
    [SwaggerResponse(statusCode: 404, description: "TRA ID not found in header.")]
    [SwaggerResponse(statusCode: 500, description: "Api server error.")]

    public ActionResult<int?> HealthTraId([FromHeader(Name = "TA")][Required] int? ta)
    {
        try
        {
            if (ta == null)
            {
                return NotFound(new ApiErrorResponse("Not found", "ta not found in header"));
            }
            return Ok(ta);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing TraIdHealth request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Check the database connection.
    /// </summary>
    /// <response code="200">Ok.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("/v1/healthDatabase")]
    [FeatureGate(RequirementType.Any, FeatureNames.DtroRead, FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 200, description: "Database is available.")]
    [SwaggerResponse(statusCode: 404, description: "Database is not available.")]
    [SwaggerResponse(statusCode: 500, description: "Api server error.")]
    public async Task<ActionResult<bool>> HealthDatabase()
    {
        try
        {
            var check = await _metricsService.CheckDataBase();
            if (check)
            {
                return Ok(true);
            }
            else
            {
                return NotFound(new ApiErrorResponse("Not found", "Database is not available"));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing HealthDatabase request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "Database is not available"));
        }
    }

    /// <summary>
    /// Gets metrics for a specific TRA within a date range.
    /// </summary>
    /// <param name="metricRequest">The Tra Id , date from and date to.</param>
    /// <response code="200">Metrics retrieved successfully.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">TRA or metrics not found.</response>
    /// <response code="500">Internal server error.</response>

    [HttpPost("/v1/metricsForTra")]
    [FeatureGate(RequirementType.Any, FeatureNames.DtroRead, FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 200, description: "Metrics retrieved successfully.")]
    [SwaggerResponse(statusCode: 400, description: "Dates Incorrect.")]
    [SwaggerResponse(statusCode: 404, description: "TRA not found.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]

    public async Task<ActionResult<MetricSummary>> GetMetricsForTra([FromBody] MetricRequest metricRequest)
    {
        try
        {
            var metrics = await _metricsService.GetMetrics(metricRequest);
            if (metrics == null)
            {
                return NotFound(new ApiErrorResponse("Not found", "Metrics not found for the specified TRA and date range"));
            }
            return Ok(metrics);
        }
        catch (ArgumentException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving metrics for TRA.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}
