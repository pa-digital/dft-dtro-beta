namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for capturing Dtro Users
/// </summary>
[Tags("DtroUser")]
[ApiController]
public class DtroUserController : ControllerBase
{
    private readonly IDtroUserService _dtroUserService;
    private readonly ILogger<DtroUserController> _logger;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="dtroUserService">An <see cref="IDtroUserService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{DtroUserController}"/> instance.</param>
    public DtroUserController(
        IDtroUserService dtroUserService,
        ILogger<DtroUserController> logger)
    {
        _dtroUserService = dtroUserService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieve All Dtro Users
    /// </summary>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>A list of Dtro Users</returns>
    [HttpGet("/dtroUsers")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 200, description: "Dtro Users retrieved successfully.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<ActionResult<List<DtroUserResponse>>> GetDtroUsers()
    {
        try
        {
            List<DtroUserResponse> dtroUserResponses = await _dtroUserService.GetAllDtroUsersAsync() ?? new List<DtroUserResponse>();
            _logger.LogInformation($"'{nameof(GetDtroUsers)}' method called");
            return Ok(dtroUserResponses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Find the existing Dtro Users that matches the requested criteria.
    /// </summary>
    /// <param name="partialName">Partial name of the Dtro User</param>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>A List of Dtro Users</returns>
    [HttpGet("/dtroUsers/search/{partialName}")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 200, description: "Dtro User retrieved successfully.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<ActionResult<List<DtroUserResponse>>> SearchDtroUsers(string partialName)
    {
        try
        {
            List<DtroUserResponse> dtroUserResponses = await _dtroUserService.SearchDtroUsers(partialName) ?? new List<DtroUserResponse>();
            _logger.LogInformation($"'{nameof(SearchDtroUsers)}' method called");
            return Ok(dtroUserResponses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Create a new Dtro user entity.
    /// </summary>
    /// <param name="body">Object containing the dtro Users details.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the submitted dtro User.</returns>
    [HttpPost]
    [Route("/dtroUsers/createFromBody")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(201, type: typeof(GuidResponse), description: "Created")]
    [SwaggerResponse(statusCode: 400, description: "Bad Request.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<IActionResult> CreateFromBody(DtroUserRequest body)
    {
        try
        {
            var response = await _dtroUserService.SaveDtroUserAsync(body);
            _logger.LogInformation($"'{nameof(CreateFromBody)}' method called using ID '{body.Id}'");
            return CreatedAtAction(nameof(CreateFromBody), new { id = response.Id }, response);
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

    /// <summary>
    /// Update an existing Dtro User.
    /// </summary>
    /// <param name="body">Object containing the Dtro User details.</param>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the submitted DtroUser.</returns>
    [HttpPut]
    [Route("/dtroUsers/updateFromBody")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    [SwaggerResponse(statusCode: 404, description: "Not Found.")]
    [SwaggerResponse(statusCode: 400, description: "Bad Request.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<IActionResult> UpdateFromBody([FromBody] DtroUserRequest body)
    {
        try
        {
            GuidResponse response = await _dtroUserService.UpdateDtroUserAsync(body);
            _logger.LogInformation($"'{nameof(UpdateFromBody)}' method called using ID '{body.Id}'");
            return Ok(response);
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Not Found", "User not found"));
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

    /// <summary>
    /// Retrieve a Dtro User
    /// </summary>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>A Dtro User</returns>
    [HttpGet("/dtroUsers/{dtroUserId:guid}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    [SwaggerResponse(statusCode: 404, description: "Not Found.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<ActionResult<DtroUserResponse>> GetDtroUser(Guid dtroUserId)
    {
        try
        {
            DtroUserResponse dtroUserResponses = await _dtroUserService.GetDtroUserAsync(dtroUserId) ?? new DtroUserResponse();
            _logger.LogInformation($"'{nameof(GetDtroUser)}' method called");
            return Ok(dtroUserResponses);
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Not Found", "User not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Delete Dtro users by ids
    /// </summary>
    /// <param name="ids">List of D-TRO User ID, comma separated</param>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>True if successful, False otherwise</returns>
    [HttpDelete("/dtroUsers/redundant")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Ok")]
    [SwaggerResponse(statusCode: 404, description: "Not Found.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<ActionResult<bool>> DeleteDtroUsers([FromBody] DeleteDtroUsersRequest request)
    {
        try
        {
//            List<Guid> userIds = request.Ids.Split(',').Select(id => new Guid(id)).ToList();
            if (request.Ids == null || !request.Ids.Any())
            {
                return BadRequest(new ApiErrorResponse("Bad Request", "No user IDs provided."));
            }
            bool state = await _dtroUserService.DeleteDtroUsersAsync(request.Ids);
            _logger.LogInformation($"Method '{nameof(DeleteDtroUsers)}' called at {DateTime.Now:g}");
            return Ok(state);
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Unknown User", "User not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

}
