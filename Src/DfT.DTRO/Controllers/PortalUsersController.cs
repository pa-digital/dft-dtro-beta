using DfT.DTRO.Models.PortalUser;

namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for front-end user
/// </summary>
[ApiController]
[Tags("Users")]
[TokenValidation]
public class PortalUsersController : ControllerBase
{
    private readonly IPortalUserService _portalUserService;
    private readonly ILogger<DtroUserController> _logger;
    private readonly LoggingExtension _loggingExtension;

   
    public PortalUsersController(
        IPortalUserService portalUserService,
        ILogger<DtroUserController> logger,
        LoggingExtension loggingExtension)
    {
        _portalUserService = portalUserService;
        _logger = logger;
        _loggingExtension = loggingExtension;
    }
    
    
    [HttpGet]
    [Route("/canPublish")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 200, description: "Publish permission retrieved successfully.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<ActionResult<List<PortalUserResponse>>> CanPublish()
    {
        try
        {
            var userId = HttpContext.Items["UserId"] as string;
            var response = await _portalUserService.CanUserPublish(userId);
            _logger.LogInformation($"'{nameof(CanPublish)}' method called");
            _loggingExtension.LogInformation(
                nameof(CanPublish),
                "/dtroUsers",
                $"'{nameof(CanPublish)}' method called");

            
            return Ok(new  PortalUserResponse {canPublish = response.canPublish});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(CanPublish), "/canPublish", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    [HttpGet]
    [Route("/userInfo")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 200, description: "User information retrieved successfully.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<ActionResult<List<PortalUserResponse>>> GetUserInfo()
    {
        try
        {
            var userId = HttpContext.Items["UserId"] as string;
            var response = await _portalUserService.GetUserInfo(userId);
            _logger.LogInformation($"'{nameof(GetUserInfo)}' method called");
            _loggingExtension.LogInformation(
                nameof(GetUserInfo),
                "/dtroUsers",
                $"'{nameof(GetUserInfo)}' method called");

            
            return Ok(new UserAppDto()
            {
                UserId = response.UserId,
                User = response.User,
                Apps = response.Apps,
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(GetUserInfo), "/userInfo", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }
}