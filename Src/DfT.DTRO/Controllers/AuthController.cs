using DfT.DTRO.Models.Auth;

namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for capturing Auth
/// </summary>
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Tags("Auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private readonly LoggingExtension _loggingExtension;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="authService">An <see cref="IAuthService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{AuthController}"/> instance.</param>
    /// <param name="loggingExtension">An <see cref="LoggingExtension"/> instance.</param>
    public AuthController(IAuthService authService, ILogger<AuthController> logger, LoggingExtension loggingExtension) {
        _authService = authService;
        _logger = logger;
        _loggingExtension = loggingExtension;
    }

    /// <summary>
    /// Get Auth token
    /// </summary>
    /// <param name="authTokenInput">Properties passed by body</param>
    /// <returns>An auth token</returns>
    [HttpPost(RouteTemplates.AuthGetToken)]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish, FeatureNames.Consumer)]
    [SwaggerResponse(statusCode: 500, description: "Internal Server Error")]
    [SwaggerResponse(statusCode: 200, description: "Ok")]
    public async Task<IActionResult> GetToken([FromBody] AuthTokenInput authTokenInput)
    {
        try
        {
            AuthToken authToken = await _authService.GetToken(authTokenInput);
            _logger.LogInformation($"'{nameof(GetToken)}' method called ");
            _loggingExtension.LogInformation(nameof(GetToken), RouteTemplates.AuthGetToken, $"'{nameof(GetToken)}' method called.");
            return Ok(authToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(GetToken), RouteTemplates.AuthGetToken, "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }
}