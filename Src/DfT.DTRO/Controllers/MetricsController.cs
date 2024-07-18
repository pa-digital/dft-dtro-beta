﻿namespace DfT.DTRO.Controllers;

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
            _logger.LogInformation($"'{nameof(HealthApi)}' method called");
            return Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
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
            _logger.LogInformation($"'{nameof(HealthTraId)}' method called using TRA Id: '{ta}'");
            return Ok(ta);
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Not found", "ta not found in header"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
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
            bool check = await _metricsService.CheckDataBase();
            if (check)
            {
                _logger.LogInformation($"'{nameof(HealthDatabase)}' method called");
                return Ok(true);
            }

            _logger.LogInformation($"'{nameof(HealthDatabase)}' method called");
            return NotFound(new ApiErrorResponse("Not found", "Database is not available"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
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
            MetricSummary metrics = await _metricsService.GetMetrics(metricRequest) ?? new MetricSummary();
            _logger.LogInformation($"'{nameof(GetMetricsForTra)}' method called using TRA Id '{metricRequest.TraId}'");
            return Ok(metrics);
        }
        catch (ArgumentException err)
        {
            _logger.LogError(err.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _logger.LogError(ex, "An error occurred while retrieving metrics for TRA.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}
