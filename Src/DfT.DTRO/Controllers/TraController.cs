namespace DfT.DTRO.Controllers;

[Tags("Tra")]
[ApiController]
public class TraController : ControllerBase
{
    private readonly ITraService _traService;
    private readonly ILogger<TraController> _logger;

    public TraController(ITraService traService, ILogger<TraController> logger)
    {
        _traService = traService;
        _logger = logger;
    }

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

    [HttpGet("/v1/SearchSwaCodes/{partialName}")]
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

    [HttpPost]
    [Route("/v1/swaCodes/createFromBody")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
    [SwaggerResponse(201, type: typeof(GuidResponse), description: "Created")]
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

    [HttpPut]
    [Route("/v1/swaCodes/updateFromBody")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
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

    [HttpPatch]
    [Route("/v1/swaCodes/activate/{traId}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
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

    [HttpPatch]
    [Route("/v1/swaCodes/deactivate/{traId}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
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
