namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for managing application-related operations.
/// </summary>
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Tags("Applications")]
public class ApplicationController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly IEmailService _emailService;
    private readonly ILogger<ApplicationController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationController"/> class.
    /// </summary>
    /// <param name="applicationService">Service for application operations.</param>
    /// <param name="logger">Logger instance for logging.</param>
    /// <param name="loggingExtension">Extension for logging operations.</param>
    /// <param name="emailService">Service used for email operations.</param>
    public ApplicationController(IApplicationService applicationService, ILogger<ApplicationController> logger,IEmailService emailService)
    {
        _applicationService = applicationService;
        _logger = logger;
        _emailService = emailService;
    }

    /// <summary>
    /// Create App
    /// </summary>
    /// <param name="email">Developer email linked to access token.</param>
    /// <param name="appInput">Properties passed by body</param>
    /// <returns>Created app</returns>
    [HttpPost(RouteTemplates.ApplicationsCreate)]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish, FeatureNames.Consumer)]
    [SwaggerResponse(statusCode: 500, description: "Internal Server Error")]
    [SwaggerResponse(statusCode: 200, description: "Ok")]
    public async Task<IActionResult> CreateApplication([FromHeader(Name = RequestHeaderNames.Email)][Required] string email, [FromBody] AppInput appInput)
    {
        try
        {
            var app = await _applicationService.CreateApplication(email, appInput);
            if (string.IsNullOrEmpty(app.AppId))
            {
                var response = _emailService.SendEmail(app.Name, email, ApplicationStatusType.Inactive.Status);
                if (string.IsNullOrEmpty(response.id))
                {
                    throw new EmailSendException();
                }
            }

            _logger.LogInformation($"'{nameof(CreateApplication)}' method called ");
            return Ok(app);
        }
        catch (EmailSendException esex)
        {
            _logger.LogError(esex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {esex.Message}"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Activates an application by app ID
    /// </summary>
    /// <param name="email">Developer email linked to access token.</param>
    /// <param name="appId">Application unique identifier.</param>
    /// <response code="200">Valid application ID</response>
    /// <response code="400">Invalid or empty parameters, or no matching application</response>
    /// <response code="500">Invalid operation or other exception.</response>
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

            var isActivated = await _applicationService.ActivateApplicationById(email, appId);
            if (isActivated)
            {
                var app = await _applicationService.GetApplication(email, appId);
                var response = _emailService.SendEmail(app.Name, email, ApplicationStatusType.Active.Status);
                if (string.IsNullOrEmpty(response.id))
                {
                    throw new EmailSendException();
                }

            }
            _logger.LogInformation($"'{nameof(ActivateApplication)}' method called ");
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
        catch (EmailSendException esex)
        {
            _logger.LogError(esex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {esex.Message}"));
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
    public async Task<ActionResult<PaginatedResponse<ApplicationListDto>>> FindApplications([FromHeader(Name = RequestHeaderNames.Email)][Required] string email, [FromQuery] PaginatedRequest paginatedRequest)
    {
        try
        {
            var result = await _applicationService.GetApplications(email, paginatedRequest);
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