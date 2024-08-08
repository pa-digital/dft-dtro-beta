namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for capturing TRAs
/// </summary>
[Tags("Tra")]
[ApiController]
public class TraController : ControllerBase
{
    private readonly ITraService _traService;
    private readonly ILogger<TraController> _logger;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="traService">An <see cref="ITraService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{TraController}"/> instance.</param>
    public TraController(
        ITraService traService,
        ILogger<TraController> logger)
    {
        _traService = traService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieve TRA IDs
    /// </summary>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>A list of TRA IDs</returns>
    [HttpGet("/v1/swaCodes")]
    [FeatureGate(RequirementType.Any, FeatureNames.DtroRead, FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 200, description: "Tra swa codes retrieved successfully.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<ActionResult<List<SwaCodeResponse>>> GetSwaCodes()
    {
        try
        {
            List<SwaCodeResponse> swaCodeResponses = await _traService.GetSwaCodeAsync() ?? new List<SwaCodeResponse>();
            _logger.LogInformation($"'{nameof(GetSwaCodes)}' method called");
            return Ok(swaCodeResponses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Find the existing SWA Codes that matches the requested criteria.
    /// </summary>
    /// <param name="partialName">Partial name of the Street Work Act</param>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>A List of SWA Codes</returns>
    [HttpGet("/v1/swaCodes/search/{partialName}")]
    [FeatureGate(RequirementType.Any, FeatureNames.DtroRead, FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 200, description: "Tra swa codes retrieved successfully.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<ActionResult<List<SwaCodeResponse>>> SearchSwaCodes(string partialName)
    {
        try
        {
            List<SwaCodeResponse> swaCodeResponses = await _traService.SearchSwaCodes(partialName) ?? new List<SwaCodeResponse>();
            _logger.LogInformation($"'{nameof(GetSwaCodes)}' method called");
            return Ok(swaCodeResponses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Create a new Street Work Manager entity.
    /// </summary>
    /// <param name="body">Object containing a full SWA details.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the submitted SWA.</returns>
    [HttpPost]
    [Route("/v1/swaCodes/createFromBody")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
    [SwaggerResponse(201, type: typeof(GuidResponse), description: "Created")]
    [SwaggerResponse(statusCode: 400, description: "Bad Request.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<IActionResult> CreateFromBody(SwaCodeRequest body)
    {
        try
        {
            var response = await _traService.SaveTraAsync(body);
            _logger.LogInformation($"'{nameof(CreateFromBody)}' method called using tra ID '{body.TraId}'");
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
    /// Update an existing Street Work Act.
    /// </summary>
    /// <param name="body">Object containing a full SWA details.</param>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the submitted SWA.</returns>
    [HttpPut]
    [Route("/v1/swaCodes/updateFromBody")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    [SwaggerResponse(statusCode: 404, description: "Not Found.")]
    [SwaggerResponse(statusCode: 400, description: "Bad Request.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<IActionResult> UpdateFromBody([FromBody] SwaCodeRequest body)
    {
        try
        {
            GuidResponse response = await _traService.UpdateTraAsync(body);
            _logger.LogInformation($"'{nameof(UpdateFromBody)}' method called using tra ID '{body.TraId}'");
            return Ok(response);
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("TRA ID", "TRA ID not found"));
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



    [HttpGet("/v1/swaCodes/{traId}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    public async Task<ActionResult<SwaCodeResponse>> GetSwaCode(int traId)
    {
        try
        {
            SwaCodeResponse swaCodeResponses = await _traService.GetSwaCodeAsync(traId) ?? new SwaCodeResponse();
            _logger.LogInformation($"'{nameof(GetSwaCodes)}' method called");
            return Ok(swaCodeResponses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }


    /// <summary>
    /// Activate the Street Work Act.
    /// </summary>
    /// <param name="traId">ID of the SWA by which Street Work Act will be activated.</param>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the activated SWA.</returns>
    [HttpPatch]
    [Route("/v1/swaCodes/activate/{traId:int}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    [SwaggerResponse(statusCode: 404, description: "Not Found.")]
    [SwaggerResponse(statusCode: 400, description: "Bad Request.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<IActionResult> ActivateByTraId(int traId)
    {
        try
        {
            var response = await _traService.ActivateTraAsync(traId);
            _logger.LogInformation($"'{nameof(ActivateByTraId)}' method called using tra ID '{traId}'");
            return Ok(response);
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Not found", "TRA ID not found"));
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
    /// Deactivate the Street Work Act.
    /// </summary>
    /// <param name="traId">ID of the SWA by which Street Work Act will be deactivated.</param>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the deactivated SWA.</returns>
    [HttpPatch]
    [Route("/v1/swaCodes/deactivate/{traId:int}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    [SwaggerResponse(statusCode: 404, description: "Not Found.")]
    [SwaggerResponse(statusCode: 400, description: "Bad Request.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<IActionResult> DeactivateByTraId(int traId)
    {
        try
        {
            var response = await _traService.DeActivateTraAsync(traId);
            _logger.LogInformation($"'{nameof(DeactivateByTraId)}' method called using TRA ID '{traId}'");
            return Ok(response);
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Not found", "TRA ID not found"));
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
