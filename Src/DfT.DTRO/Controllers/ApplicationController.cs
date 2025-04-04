using DfT.DTRO.Models.Applications;

namespace DfT.DTRO.Controllers;

[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Tags("Applications")]
public class ApplicationController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly ILogger<ApplicationController> _logger;
    private readonly LoggingExtension _loggingExtension;

    public ApplicationController(IApplicationService applicationService, ILogger<ApplicationController> logger, LoggingExtension loggingExtension)
    {
        _applicationService = applicationService;
        _logger = logger;
        _loggingExtension = loggingExtension;
    }

    /// <summary>
    /// Create App
    /// </summary>
    /// <param name="email">Developer email linked to access token.</param>
    /// <param name="appInput">Properties passed by body</param>
    /// <returns>created app</returns>
    [HttpPost(RouteTemplates.ApplicationsCreate)]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish, FeatureNames.Consumer)]
    [SwaggerResponse(statusCode: 500, description: "Internal Server Error")]
    [SwaggerResponse(statusCode: 200, description: "Ok")]
    public async Task<IActionResult> CreateApplication([FromHeader(Name = RequestHeaderNames.Email)][Required] string email, [FromBody] AppInput appInput)
    {
        try
        {
            App app = await _applicationService.CreateApplication(email, appInput);
            _logger.LogInformation($"'{nameof(CreateApplication)}' method called ");
            _loggingExtension.LogInformation(nameof(CreateApplication), RouteTemplates.ApplicationsCreate, $"'{nameof(CreateApplication)}' method called.");
            return Ok(app);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(CreateApplication), RouteTemplates.ApplicationsCreate, "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Activates an application by app ID
    /// </summary>
    /// <param name="email">Developer email linked to access token.</param>
    /// <response code="200">Valid application ID</response>
    /// <response code="400">Invalid or empty parameters, or no matching application</response>
    /// <response code="500">Invalid operation or other exception</response>
    [HttpPost(RouteTemplates.ActivateApplication)]
    [FeatureGate(FeatureNames.ReadOnly)]
    public async Task<IActionResult> ActivateApplication([FromHeader(Name = RequestHeaderNames.Email)][Required] string email, [FromRoute] Guid appId)
    {
        try
        {
            bool appBelongsToUser = await _applicationService.ValidateAppBelongsToUser(email, appId);
            if (!appBelongsToUser)
            {
                return Forbid();
            }

            bool result = await _applicationService.ActivateApplicationById(email, appId);
            return Ok(new { id = appId, status = "Active" });

        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = "Invalid input parameters", error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { message = "An error occurred while trying to activate application", error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }

    /// <summary>
    /// Validates if the Application Name is available.
    /// </summary>
    /// <param name="parameters"></param>
    /// <response code="200">Valid or invalid application name</response>
    /// <response code="400">Invalid or empty parameters</response>
    /// <response code="500">Invalid operation or other exception</response>
    [HttpGet(RouteTemplates.ValidateApplicationName)]
    [FeatureGate(FeatureNames.ReadOnly)]
    public async Task<IActionResult> ValidateApplicationName([FromQuery] ApplicationNameQueryParameters parameters)
    {
        try
        {
            if (parameters == null || string.IsNullOrEmpty(parameters.Name))
            {
                return BadRequest(new { message = "Application name is required" });
            }

            string appName = parameters.Name;
            var result = await _applicationService.ValidateApplicationName(appName);
            return Ok(new { isValid = result, message = result ? "Application name available" : "Application name already in use" });

        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = "Invalid input parameters", error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { message = "An error occurred while validating the application name", error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves application details by ID
    /// </summary>
    /// <param name="email">Developer email linked to access token.</param>
    /// <response code="200">Valid application ID</response>
    /// <response code="400">Invalid or empty parameters, or no matching application</response>
    /// <response code="500">Invalid operation or other exception</response>
    [HttpGet(RouteTemplates.ApplicationsFindById)]
    [FeatureGate(FeatureNames.ReadOnly)]
    public async Task<IActionResult> FindApplicationById([FromHeader(Name = RequestHeaderNames.Email)][Required] string email, [FromRoute] Guid appId)
    {
        try
        {
            bool appBelongsToUser = await _applicationService.ValidateAppBelongsToUser(email, appId);
            if (!appBelongsToUser)
            {
                return Forbid();
            }
            var result = await _applicationService.GetApplication(email, appId);
            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = "Invalid input parameters", error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching application details", error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves application list for user
    /// </summary>
    /// <param name="email">Developer email linked to access token.</param>
    /// <response code="200">Valid user ID</response>
    /// <response code="400">Invalid or empty parameters, or no matching user</response>
    /// <response code="500">Invalid operation or other exception</response>
    [HttpGet(RouteTemplates.ApplicationsFindAll)]
    [FeatureGate(FeatureNames.ReadOnly)]
    public async Task<IActionResult> FindApplications([FromHeader(Name = RequestHeaderNames.Email)][Required] string email)
    {
        try
        {
            var result = await _applicationService.GetApplications(email);
            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = "Invalid input parameters", error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching the applications", error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }
    
    /// <summary>
    /// Retrieves application list for user
    /// </summary>
    /// <response code="200">Valid user ID</response>
    /// <response code="400">Invalid or empty parameters, or no matching user</response>
    /// <response code="500">Invalid operation or other exception</response>
    [HttpGet(RouteTemplates.ApplicationsFindAllInactive)]
    [FeatureGate(FeatureNames.ReadOnly)]
    public async Task<ActionResult<PaginatedResponse<ApplicationInactiveListDto>>> FindInactiveApplications([FromQuery] PaginatedRequest paginatedRequest)
    {
        try
        {
            var result = await _applicationService.GetInactiveApplications(paginatedRequest);
            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = "Invalid input parameters", error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching the applications", error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }
}