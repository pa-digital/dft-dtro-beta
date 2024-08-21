using DfT.DTRO.Models.SystemConfig;

namespace DfT.DTRO.Controllers;

[ApiController]
public class SystemConfigController : ControllerBase
{
    private readonly ISystemConfigService _systemConfigService;
    private readonly ILogger<SystemConfigController> _logger;

    public SystemConfigController(ISystemConfigService systemConfigService, ILogger<SystemConfigController> logger)
    {
        _systemConfigService = systemConfigService;
        _logger = logger;
    }

    [HttpGet("/systemConfig")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 200, description: "System name retrieved successfully.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<ActionResult<SystemConfigResponse>> GetSystemConfig()
    {
        try
        {
            var res = await _systemConfigService.GetSystemConfigAsync();
            _logger.LogInformation($"'{nameof(GetSystemConfig)}' method called");
            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    [HttpPut]
    [Route("/systemConfig/updateFromBody")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    [SwaggerResponse(statusCode: 400, description: "Bad Request.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<IActionResult> UpdateFromBody([FromBody] SystemConfigRequest body)
    {
        try
        {
            var response = await _systemConfigService.UpdateSystemConfigAsync(body);
            _logger.LogInformation($"'{nameof(UpdateFromBody)}' method called");
            return Ok(response);
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Config record", "Config record not found"));
        }
        catch (InvalidOperationException err)
        {
            _logger.LogError(err.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}
