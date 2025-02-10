using DfT.DTRO.Models.Tra;

namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for capturing TRAs
/// </summary>
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Route(RouteTemplates.TrasBase)]
[Tags("Tras")]
public class TraController : ControllerBase
{
    private readonly ITraService _traService;
    private readonly ILogger<TraController> _logger;
    private readonly LoggingExtension _loggingExtension;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="traService">An <see cref="ITraService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{TraController}"/> instance.</param>
    /// <param name="loggingExtension">An <see cref="LoggingExtension"/> instance.</param>
    public TraController(ITraService traService, ILogger<TraController> logger, LoggingExtension loggingExtension) {
        _traService = traService;
        _logger = logger;
        _loggingExtension = loggingExtension;
    }

    /// <summary>
    /// Get TRA records
    /// </summary>
    /// <param name="parameters">Properties passed to query by</param>
    /// <returns>A list of TRA active records</returns>
    [HttpGet(RouteTemplates.TrasFindAll)]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish, FeatureNames.Consumer)]
    [SwaggerResponse(statusCode: 500, description: "Internal Server Error")]
    [SwaggerResponse(statusCode: 404, description: "Could not found any TRA records.")]
    [SwaggerResponse(statusCode: 200, description: "Ok")]
    public async Task<IActionResult> FindAll([FromQuery] GetAllTrasQueryParameters parameters)
    {
        try
        {
            IEnumerable<TraFindAllResponse> traFindAllResponse = await _traService.GetTrasAsync(parameters);
            _logger.LogInformation($"'{nameof(FindAll)}' method called ");
            _loggingExtension.LogInformation(
                nameof(FindAll),
                $"{RouteTemplates.TrasBase}{RouteTemplates.TrasFindAll}",
                $"'{nameof(FindAll)}' method called.");
            return Ok(traFindAllResponse);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(FindAll), $"{RouteTemplates.TrasBase}{RouteTemplates.TrasFindAll}", "TRA records not found", ex.Message);
            return NotFound(new ApiErrorResponse("TRA records not found", ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(FindAll), $"{RouteTemplates.TrasBase}{RouteTemplates.TrasFindAll}", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }
}