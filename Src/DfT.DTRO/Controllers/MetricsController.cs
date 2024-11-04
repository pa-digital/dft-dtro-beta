namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for capturing metric
/// </summary>
[Tags("Metrics")]
[ApiController]
public class MetricsController : ControllerBase
{
    private readonly IMetricsService _metricsService;
    private readonly ILogger<MetricsController> _logger;
    private readonly LoggingExtension _loggingExtension;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="metricsService">An <see cref="IMetricsService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{MetricsController}"/> instance.</param>
    public MetricsController(
        IMetricsService metricsService,
        ILogger<MetricsController> logger,
         LoggingExtension loggingExtension)
    {
        _metricsService = metricsService;
        _logger = logger;
        _loggingExtension = loggingExtension;
    }

    /// <summary>
    /// Check API health.
    /// </summary>
    /// <response code="200">OK.</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("/healthApi")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    [ValidateModelState]
    [SwaggerResponse(statusCode: 200, description: "The API is up and running.")]
    [SwaggerResponse(statusCode: 500, description: "Api server error.")]
    public ActionResult<bool> HealthApi()
    {
        try
        {
            _logger.LogInformation($"'{nameof(HealthApi)}' method called");
            new LoggingExtension.Builder()
                    .WithLogType(LogType.INFO)
                    .WithMethodCalledFrom("healthApi")
                    .WithEndpoint("/healthApi")
                    .WithMessage($"'{nameof(HealthApi)}' method called")
                    .Build()
                    .PrintToConsole();
            return Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Check database health.
    /// </summary>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("/healthDatabase")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
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

    /// <summary>
    /// Get metrics for Dtro User
    /// </summary>
    /// <param name="metricRequest">Object containing a metric request.</param>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Metric summary</returns>
    [HttpPost("/metricsForDtroUser")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 200, description: "Metrics retrieved successfully.")]
    [SwaggerResponse(statusCode: 400, description: "Dates Incorrect.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]

    public async Task<ActionResult<MetricSummary>> GetMetricsForDtroUser([FromBody] MetricRequest metricRequest)
    {
        try
        {
            MetricSummary metrics = await _metricsService.GetMetrics(metricRequest) ?? new MetricSummary();
            _logger.LogInformation($"'{nameof(GetMetricsForDtroUser)}' method called using DtroUserId '{metricRequest.DtroUserId}'");
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

    /// <summary>
    /// Get full metrics for Dtro User
    /// </summary>
    /// <param name="metricRequest">Object containing a metric request.</param>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Metric summary</returns>
    [HttpPost("/fullMetricsForDtroUser")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 200, description: "Metrics retrieved successfully.")]
    [SwaggerResponse(statusCode: 400, description: "Dates Incorrect.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]

    public async Task<ActionResult<List<FullMetricSummary>>> GetFullMetricsForDtroUser([FromBody] MetricRequest metricRequest)
    {
        try
        {
            List<FullMetricSummary> metrics = await _metricsService.GetFullMetrics(metricRequest) ?? new List<FullMetricSummary>();
            _logger.LogInformation($"'{nameof(GetMetricsForDtroUser)}' method called using DtroUserId '{metricRequest.DtroUserId}'");
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
