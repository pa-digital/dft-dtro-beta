using DfT.DTRO.Models.App;

namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for capturing Apps
/// </summary>
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Tags("Apps")]
public class AppsController : ControllerBase
{
    private readonly IAppService _appService;
    private readonly ILogger<AppsController> _logger;
    private readonly LoggingExtension _loggingExtension;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="appService">An <see cref="IAppService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{AppsController}"/> instance.</param>
    /// <param name="loggingExtension">An <see cref="LoggingExtension"/> instance.</param>
    public AppsController(IAppService appService, ILogger<AppsController> logger, LoggingExtension loggingExtension) {
        _appService = appService;
        _logger = logger;
        _loggingExtension = loggingExtension;
    }

    /// <summary>
    /// Create App
    /// </summary>
    /// <param name="accessToken">Access token.</param>
    /// <param name="appInput">Properties passed by body</param>
    /// <returns>created app</returns>
    [HttpPost(RouteTemplates.AppsCreate)]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish, FeatureNames.Consumer)]
    [SwaggerResponse(statusCode: 500, description: "Internal Server Error")]
    [SwaggerResponse(statusCode: 200, description: "Ok")]
    public async Task<IActionResult> CreateApp([FromBody] AppInput appInput)
    {
        try
        {
            App app = await _appService.CreateApp(appInput);
            _logger.LogInformation($"'{nameof(CreateApp)}' method called ");
            _loggingExtension.LogInformation(nameof(CreateApp), RouteTemplates.AppsCreate, $"'{nameof(CreateApp)}' method called.");
            return Ok(app);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(CreateApp), RouteTemplates.AppsCreate, "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }
}