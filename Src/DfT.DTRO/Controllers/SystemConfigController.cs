using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.SystemConfig;

namespace DfT.DTRO.Controllers;

[ApiController]
public class SystemConfigController : ControllerBase
{
    private readonly ISystemConfigService _systemConfigService;
    private readonly ILogger<SystemConfigController> _logger;
    private readonly IAppIdMapperService _appIdMapperService;
    private readonly LoggingExtension _loggingExtension;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="systemConfigService">An <see cref="ISystemConfigService"/> instance.</param>
    /// <param name="appIdMapperService">An <see cref="IAppIdMapperService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{SystemConfigController}"/> instance.</param>
    /// <param name="loggingExtension">An <see cref="LoggingExtension"/> instance.</param>
    public SystemConfigController(
        ISystemConfigService systemConfigService,
        IAppIdMapperService appIdMapperService,
        ILogger<SystemConfigController> logger,
        LoggingExtension loggingExtension)
    {
        _systemConfigService = systemConfigService;
        _appIdMapperService = appIdMapperService;
        _logger = logger;
        _loggingExtension = loggingExtension;
    }

    /// <summary>
    /// Get System Config details
    /// </summary>
    /// <param name="appId">AppId identification.</param>
    /// <response code="200">System name retrieved successfully.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>SystemConfigResponse</returns>
    [HttpGet("/systemConfig")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 200, description: "System name retrieved successfully.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<ActionResult<SystemConfigResponse>> GetSystemConfig([FromHeader(Name = "x-app-id")][Required] Guid appId)
    {
        try
        {
            appId = await _appIdMapperService.GetAppId(HttpContext);
            var res = await _systemConfigService.GetSystemConfigAsync(appId);
            _logger.LogInformation($"'{nameof(GetSystemConfig)}' method called");
            _loggingExtension.LogInformation(
                nameof(GetSystemConfig),
                "/systemConfig",
                $"'{nameof(GetSystemConfig)}' method called");
            return Ok(res);
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(nameof(GetSystemConfig), "/systemConfig", "Unexpected Null value was found", anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(GetSystemConfig),
                "/systemConfig",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(GetSystemConfig), "/systemConfig", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occured: {ex.Message}"));
        }
    }

    /// <summary>
    /// Update System Config details
    /// </summary>
    /// <param name="body">Object containing System Config details.</param>
    /// <response code="200">ok.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>SystemConfigResponse</returns>
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
            _loggingExtension.LogInformation(
                nameof(UpdateFromBody),
                "/systemConfig/updateFromBody",
                $"'{nameof(UpdateFromBody)}' method called");
            return Ok(response);
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromBody),
                "/systemConfig/updateFromBody",
                "Config record not found",
                nfex.Message);
            return NotFound(new ApiErrorResponse("Config record", "Config record not found"));
        }
        catch (InvalidOperationException ioex)
        {
            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromBody),
                "/systemConfig/updateFromBody",
                "Bad Request",
                ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromBody),
                "/systemConfig/updateFromBody",
                "Unexpected Null value was found",
                anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromBody),
                "/systemConfig/updateFromBody",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromBody),
                "/systemConfig/updateFromBody",
                "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Unable to save record(s) to the database."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(UpdateFromBody), "/systemConfig/updateFromBody", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occured: {ex.Message}"));
        }
    }
}
