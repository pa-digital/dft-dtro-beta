using DfT.DTRO.Models.PortalUser;

namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for front-end user
/// </summary>
[Tags("Users")]
[ApiController]
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
    [Route("/canPublish/{userId}")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 200, description: "Publish permission retrieved successfully.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<ActionResult<List<PortalUserResponse>>> CanPublish([FromRoute] string userId)
    {
        try
        {
            // TODO verify based on token rather than user ID
            var response = await _portalUserService.CanUserPublish(userId);
            _logger.LogInformation($"'{nameof(CanPublish)}' method called");
            _loggingExtension.LogInformation(
                nameof(CanPublish),
                "/dtroUsers",
                $"'{nameof(CanPublish)}' method called");

            Console.WriteLine($"Will look for user {userId}");
            
            return Ok(new  PortalUserResponse {canPublish = response.canPublish});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(CanPublish), "/canPublish", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }
}