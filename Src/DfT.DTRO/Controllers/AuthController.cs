using Microsoft.AspNetCore.Http;
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
    private readonly IUserDal _userDal;
    private readonly ILogger<AuthController> _logger;
    private readonly LoggingExtension _loggingExtension;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="authService">An <see cref="IAuthService"/> instance.</param>
    /// /// <param name="userDal">An <see cref="IUserDal"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{AuthController}"/> instance.</param>
    /// <param name="loggingExtension">An <see cref="LoggingExtension"/> instance.</param>
    public AuthController(IAuthService authService, IUserDal userDal, ILogger<AuthController> logger, LoggingExtension loggingExtension)
    {
        _authService = authService;
        _userDal = userDal;
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

            DateTimeOffset issuedAt = DateTimeOffset.FromUnixTimeMilliseconds(authToken.IssuedAt);
            DateTimeOffset expiresAt = issuedAt.AddSeconds(authToken.ExpiresIn);
            string expiryEpochTime = expiresAt.ToUnixTimeSeconds().ToString();

            Response.Cookies.Append("access_token", $"{authToken.AccessToken}:{expiryEpochTime}",
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
                Expires = expiresAt.UtcDateTime
            });

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(GetToken), RouteTemplates.AuthGetToken, "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Verify token is not expired
    /// </summary>
    [HttpPost(RouteTemplates.AuthVerifyToken)]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish, FeatureNames.Consumer)]
    [SwaggerResponse(statusCode: 500, description: "Internal Server Error")]
    [SwaggerResponse(statusCode: 200, description: "Ok")]
    public async Task<IActionResult> VerifyToken([FromHeader(Name = RequestHeaderNames.Email)][Required] string email)
    {
        User user = await _userDal.GetUserFromEmail(email);
        bool isAdmin = user.IsCentralServiceOperator;
        var value = Request.Cookies["access_token"];
        string[] parts = value.Split(':');
        string accessToken = parts[0];
        long expiryEpochTime = long.Parse(parts[1]);

        if (string.IsNullOrEmpty(accessToken))
        {
            return Unauthorized();
        }

        bool isValid = DateTimeOffset.Now.ToUnixTimeSeconds() < expiryEpochTime;

        if (isValid)
        {
            return Ok(new { isAdmin = isAdmin });
        }

        return Unauthorized();
    }
}