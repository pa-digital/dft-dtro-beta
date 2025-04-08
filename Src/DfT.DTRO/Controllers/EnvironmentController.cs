namespace DfT.DTRO.Controllers;

[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Tags("Applications")]
public class EnvironmentController : ControllerBase
{
    private readonly IEnvironmentService _environmentService;
    private readonly ILogger<ApplicationController> _logger;
    private readonly LoggingExtension _loggingExtension;

    public EnvironmentController(IEnvironmentService environmentService, ILogger<ApplicationController> logger, LoggingExtension loggingExtension)
    {
        _environmentService = environmentService;
        _logger = logger;
        _loggingExtension = loggingExtension;
    }

    /// <summary>
    /// Determine if user can request access to the production environment
    /// </summary>
    /// <param name="email">Developer email linked to access token.</param>
    /// <returns>bool</returns>
    [HttpGet(RouteTemplates.CanRequestProductionAccess)]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish, FeatureNames.Consumer)]
    [SwaggerResponse(statusCode: 200, description: "Ok")]
    [SwaggerResponse(statusCode: 500, description: "Internal Server Error")]
    public async Task<IActionResult> CanRequestProductionAccess([FromHeader(Name = RequestHeaderNames.Email)][Required] string email)
    {
        try
        {
            bool canRequest = await _environmentService.CanRequestProductionAccess(email);
            _logger.LogInformation($"'{nameof(CanRequestProductionAccess)}' method called ");
            _loggingExtension.LogInformation(nameof(CanRequestProductionAccess), RouteTemplates.CanRequestProductionAccess, $"'{nameof(CanRequestProductionAccess)}' method called.");
            return Ok(canRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(CanRequestProductionAccess), RouteTemplates.CanRequestProductionAccess, "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Request production access for user
    /// </summary>
    /// <param name="email">Developer email linked to access token.</param>
    /// <returns>bool</returns>
    [HttpPost(RouteTemplates.RequestProductionAccess)]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish, FeatureNames.Consumer)]
    [SwaggerResponse(statusCode: 200, description: "Ok")]
    [SwaggerResponse(statusCode: 500, description: "Internal Server Error")]
    public async Task<IActionResult> RequestProductionAccess([FromHeader(Name = RequestHeaderNames.Email)][Required] string email)
    {
        try
        {
            await _environmentService.RequestProductionAccess(email);
            _logger.LogInformation($"'{nameof(RequestProductionAccess)}' method called ");
            _loggingExtension.LogInformation(nameof(RequestProductionAccess), RouteTemplates.RequestProductionAccess, $"'{nameof(RequestProductionAccess)}' method called.");
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(RequestProductionAccess), RouteTemplates.RequestProductionAccess, "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }
}