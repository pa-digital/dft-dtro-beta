using DfT.DTRO.FeatureManagement;
using DfT.DTRO.Models.Metrics;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace DfT.DTRO.Controllers;

[Tags("Metrics")]
[ApiController]
public class MetricsController : ControllerBase
{
    private readonly IMetricsService _metricsService;
    private readonly ILogger<MetricsController> _logger;

    public MetricsController(
        IMetricsService metricsService,
        ILogger<MetricsController> logger)
    {
        _metricsService = metricsService;
        _logger = logger;
    }

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

    [HttpPost("/v1/metricsForTra")]
    [FeatureGate(RequirementType.Any, FeatureNames.DtroRead, FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 200, description: "Metrics retrieved successfully.")]
    [SwaggerResponse(statusCode: 400, description: "Dates Incorrect.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]

    public async Task<ActionResult<MetricSummary>> GetMetricsForTra([FromBody] MetricRequest metricRequest)
    {
        try
        {
            var metrics = await _metricsService.GetMetrics(metricRequest);
            if (metrics == null)
            {
                metrics = new MetricSummary();
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
