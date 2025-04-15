using Microsoft.AspNetCore.Http;
using DfT.DTRO.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DfT.DTRO.Models.TwoFactorAuth;

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
    private readonly ITwoFactorAuthService _twoFactorAuthService;
    private readonly IAuthHelper _authHelper;
    private readonly ILogger<AuthController> _logger;
    private readonly LoggingExtension _loggingExtension;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="authService">An <see cref="IAuthService"/> instance.</param>
    /// /// <param name="userDal">An <see cref="IUserDal"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{AuthController}"/> instance.</param>
    /// <param name="loggingExtension">An <see cref="LoggingExtension"/> instance.</param>
    public AuthController(IAuthService authService, IUserDal userDal, ITwoFactorAuthService twoFactorAuthService, IAuthHelper authHelper, ILogger<AuthController> logger, LoggingExtension loggingExtension)
    {
        _authService = authService;
        _userDal = userDal;
        _twoFactorAuthService = twoFactorAuthService;
        _authHelper = authHelper;
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
    [Authorize]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish, FeatureNames.Consumer)]
    [SwaggerResponse(statusCode: 500, description: "Internal Server Error")]
    [SwaggerResponse(statusCode: 200, description: "Ok")]
    public async Task<IActionResult> VerifyToken()
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value;
        var user = await _userDal.GetUserFromEmail(username);
        return Ok(new { isAdmin = user.IsCentralServiceOperator });
    }

    /// <summary>
    /// Authenticate user credentials against the database
    /// </summary>
    [HttpPost(RouteTemplates.AuthenticateUser)]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish, FeatureNames.Consumer)]
    [SwaggerResponse(statusCode: 200, description: "Ok")]
    [SwaggerResponse(statusCode: 401, description: "Unauthorized")]
    [SwaggerResponse(statusCode: 500, description: "Internal Server Error")]
    public async Task<IActionResult> AuthenticateUser([FromBody] AuthTokenInput authTokenInput)
    {
        try
        {
            bool isAuthenticated = await _authService.AuthenticateUser(authTokenInput.Username, authTokenInput.Password);
            if (!isAuthenticated)
            {
                return Unauthorized();
            }

            var tfa = await _twoFactorAuthService.GenerateTwoFactorAuthCode(authTokenInput.Username);
            // TODO: email code to user

            return Ok(new { token = tfa.Token.ToString() });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Validate two factor auth code
    /// </summary>
    [HttpPost(RouteTemplates.ValidateTwoFactorAuthCode)]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish, FeatureNames.Consumer)]
    [SwaggerResponse(statusCode: 200, description: "Ok")]
    [SwaggerResponse(statusCode: 401, description: "Unauthorized")]
    [SwaggerResponse(statusCode: 500, description: "Internal Server Error")]
    public async Task<IActionResult> ValidateTwoFactorAuthCode([FromBody] TwoFactorAuthInput tfaInput)
    {
        try
        {
            TwoFactorAuthentication tfa = await _twoFactorAuthService.VerifyTwoFactorAuthCode(tfaInput.Token, tfaInput.Code);

            if (tfa == null)
            {
                return BadRequest(new { message = "Invalid code or code has expired" });
            }

            string token = _authHelper.GenerateJwtToken(tfa.User.Email);
            await _twoFactorAuthService.DeleteTwoFactorAuthCodeById(tfa.Id);

            DateTime expiryTime = _authHelper.GetJwtExpiration(token);
            Response.Cookies.Append("jwtToken", token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
                Expires = expiryTime
            });

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }
}