namespace DfT.DTRO.Controllers;

[Tags("Tra")]
[ApiController]
public class TraController : ControllerBase
{
    private readonly ITraService _traService;
    private readonly ILogger<TraController> _logger;

    public TraController(
        ITraService traService,
        ILogger<TraController> logger)
    {
        _traService = traService;
        _logger = logger;
    }

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
