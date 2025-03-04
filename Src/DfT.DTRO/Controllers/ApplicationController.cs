using DfT.DTRO.Models.Applications;

namespace DfT.DTRO.Controllers;

[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Tags("Applications")]
[TokenValidation]
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
    /// <param name="accessToken">Access token.</param>
    /// <param name="appInput">Properties passed by body</param>
    /// <returns>created app</returns>
    [HttpPost(RouteTemplates.ApplicationsCreate)]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish, FeatureNames.Consumer)]
    [SwaggerResponse(statusCode: 500, description: "Internal Server Error")]
    [SwaggerResponse(statusCode: 200, description: "Ok")]
    public async Task<IActionResult> CreateApplication([FromBody] AppInput appInput)
    {
        try
        {
            App app = await _applicationService.CreateApplication(appInput);
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
    /// Validates if the Application Name is available.
    /// </summary>
    /// <param name="parameters"></param>
    /// <response code="200">Valid or invalid application name</response>
    /// <response code="400">Invalid or empty parameters</response>
    /// <response code="500">Invalid operation or other exception</response>
    [HttpPost(RouteTemplates.ValidateApplicationName)]
    [FeatureGate(FeatureNames.ReadOnly)]
    public IActionResult ValidateApplicationName([FromBody] ApplicationNameQueryParameters parameters)
    {
        try
        {
            if (parameters == null || string.IsNullOrEmpty(parameters.Name))
            {
                return BadRequest(new { message = "Application name is required" });
            }

            string appName = parameters.Name;
            var result = _applicationService.ValidateApplicationName(appName);
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
    /// <param name="parameters"></param>
    /// <response code="200">Valid application ID</response>
    /// <response code="400">Invalid or empty parameters, or no matching application</response>
    /// <response code="500">Invalid operation or other exception</response>
    [HttpPost(RouteTemplates.GetApplicationDetails)]
    [FeatureGate(FeatureNames.ReadOnly)]
    public IActionResult GetApplicationDetails([FromBody] ApplicationDetailsRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrEmpty(request.appId))
            {
                return BadRequest(new { message = "Application ID is required" });
            }

            string appId = request.appId;
            var userId = HttpContext.Items["UserId"] as string;
            bool appBelongsToUser = _applicationService.ValidateAppBelongsToUser(userId, appId);
            if (!appBelongsToUser) {
                return Forbid();
            }

            var result = _applicationService.GetApplicationDetails(appId);
            if (result != null) {
                // TODO: fetch API key and secret from Apigee
                return Ok(new { name = result.Name, appId = result.AppId, purpose = result.Purpose, apiKey = "thisismyapikey", apiSecret = "thisismyapisecret" });
            }


            return BadRequest(new { message = "No app found for this app ID" });

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
    /// <param name="parameters"></param>
    /// <response code="200">Valid user ID</response>
    /// <response code="400">Invalid or empty parameters, or no matching user</response>
    /// <response code="500">Invalid operation or other exception</response>
    [HttpPost(RouteTemplates.GetApplications)]
    [FeatureGate(FeatureNames.ReadOnly)]
    public IActionResult GetApplications()
    {
        try
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = _applicationService.GetApplicationList(userId);
            if (result != null) {
                return Ok(result);
            }


            return BadRequest(new { message = "No applications found for this user ID" });

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

        /// <summary>
        /// Retrieves pending application list for user
        /// </summary>
        /// <response code="200">Valid user ID</response>
        /// <response code="400">Invalid or empty parameters, or no matching user</response>
        /// <response code="500">Invalid operation or other exception</response>
        [HttpPost(RouteTemplates.GetPendingApplications)]
        [FeatureGate(FeatureNames.Admin)]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedResponse<ApplicationListDto>), description: "Ok")]
        [SwaggerResponse(statusCode: 400, type: typeof(BadRequestObjectResult), description: "No pending applications found for this user ID")]
        [SwaggerResponse(statusCode: 404, type: typeof(ArgumentNullException), description: "Could not found any pending applications")]
        [SwaggerResponse(statusCode: 500, type: typeof(InvalidOperationException), description: "An error occurred while fetching the pending applications")]
        [SwaggerResponse(statusCode: 500, type: typeof(Exception), description: "An unexpected error occurred")]
        public ActionResult<PaginatedResponse<ApplicationListDto>> GetPendingApplications([FromBody] PaginatedRequest request)
        {
            try
            {
                var response = _applicationService.GetPendingApplications(request);
                if (response != null)
                {
                    return Ok(response);
                }

                return BadRequest(new { message = "No pending applications found for this user ID" });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = "Invalid input parameters", error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the pending applications", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
            }
        }
    }
}