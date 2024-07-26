namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for capturing TRAs
/// </summary>
[Tags("Tra")]
[ApiController]
public class TraController : ControllerBase
{
    private readonly ITraService _traService;
    private readonly ILogger<TraController> _logger;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="traService">An <see cref="ITraService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{TraController}"/> instance.</param>
    public TraController(
        ITraService traService,
        ILogger<TraController> logger)
    {
        _traService = traService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieve TRA IDs
    /// </summary>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>A list of TRA IDs</returns>
    [HttpGet("/v1/swaCodes")]
    [FeatureGate(RequirementType.Any, FeatureNames.DtroRead, FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 200, description: "Tra swa codes retrieved successfully.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]

    public async Task<ActionResult<List<SwaCodeResponse>>> GetSwaCodes()
    {
        try
        {
            List<SwaCodeResponse> swaCodeResponses = await _traService.GetUiFormattedSwaCodeAsync() ?? new List<SwaCodeResponse>();
            _logger.LogInformation($"'{nameof(GetSwaCodes)}' method called");
            return Ok(swaCodeResponses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}
