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
    private readonly IEmailService _emailService;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="authService">An <see cref="IAuthService"/> instance.</param>
    /// <param name="emailService">An <see cref="IEmailService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{AuthController}"/> instance.</param>
    public AuthController(IAuthService authService, IEmailService emailService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _emailService = emailService;
        _logger = logger;
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
            var authToken = await _authService.GetToken(authTokenInput);
            _logger.LogInformation($"'{nameof(GetToken)}' method called", RouteTemplates.AuthGetToken);
            return Ok(authToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, RouteTemplates.AuthGetToken);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Get Refresh token
    /// </summary>
    /// <param name="email">Developer email linked to access token.</param>
    /// <param name="apigeeDeveloperApp">Parameters passed by body</param>
    /// <returns>A refresh token</returns>
    [HttpPost(RouteTemplates.AuthRefreshSecrets)]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish, FeatureNames.Consumer)]
    [SwaggerResponse(statusCode: 500, description: "Internal Server Error")]
    [SwaggerResponse(statusCode: 200, description: "Ok")]
    public IActionResult RefreshSecrets([FromHeader(Name = RequestHeaderNames.Email)][Required] string email, [FromBody]ApigeeDeveloperApp apigeeDeveloperApp)
    {
        try
        {
            //TODO: This will be changed once the api endpoint is going to be implemented.
            var response = _emailService.SendEmail(email, new ApigeeDeveloperApp(){Name = "Test" });
            if (string.IsNullOrEmpty(response.id))
            {
                throw new EmailSendException();
            }

            _logger.LogInformation($"'{nameof(RefreshSecrets)}' method called", RouteTemplates.AuthRefreshSecrets);
            return Ok(apigeeDeveloperApp);
        }
        catch (EmailSendException esex)
        {
            _logger.LogError(esex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {esex.Message}"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, RouteTemplates.AuthRefreshSecrets);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }
}