using DfT.DTRO.Models.SystemConfig;

namespace DfT.DTRO.Controllers;

[ApiController]
public class SystemConfigController : ControllerBase
{
    private readonly ISystemConfigService _systemConfigService;
    private readonly ILogger<SystemConfigController> _logger;
    private readonly IXappIdMapperService _appIdMapperService;

    public SystemConfigController(ISystemConfigService systemConfigService, IXappIdMapperService appIdMapperService, ILogger<SystemConfigController> logger)
    {
        _systemConfigService = systemConfigService;
        _appIdMapperService = appIdMapperService;
        _logger = logger;
    }

    [HttpGet("/systemConfig")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 200, description: "System name retrieved successfully.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<ActionResult<SystemConfigResponse>> GetSystemConfig([FromHeader(Name = "x-app-id")][Required] Guid xAppId)
    {
        try
        {
            xAppId = await _appIdMapperService.GetXappId(HttpContext);
            var res = await _systemConfigService.GetSystemConfigAsync(xAppId);
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
