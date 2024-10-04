using Newtonsoft.Json;

namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for capturing D-TROs
/// </summary>
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]

public class DTROsController : ControllerBase
{
    private readonly IDtroService _dtroService;
    private readonly IMetricsService _metricsService;
    private readonly IRequestCorrelationProvider _correlationProvider;
    private readonly ILogger<DTROsController> _logger;
    private readonly IXappIdMapperService _appIdMapperService;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="dtroService">An <see cref="IDtroService"/> instance.</param>
    /// <param name="metricsService">An <see cref="IMetricsService"/> instance.</param>
    /// <param name="correlationProvider">An <see cref="IRequestCorrelationProvider"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{DTROsController}"/> instance.</param>
    public DTROsController(
         IDtroService dtroService,
         IMetricsService metricsService,
         IRequestCorrelationProvider correlationProvider,
         IXappIdMapperService appIdMapperService,
         ILogger<DTROsController> logger)
    {
        _dtroService = dtroService;
        _metricsService = metricsService;
        _correlationProvider = correlationProvider;
        _appIdMapperService = appIdMapperService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new D-TRO
    /// </summary>
    /// <param name="xAppId">xAppId identification a D-TRO is being submitted for.</param>
    /// <param name="file">JSON file containing a full D-TRO details.</param>
    /// <response code="201">Created.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the submitted D-TRO</returns>
    [HttpPost]
    [Route("/dtros/createFromFile")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
    [FeatureGate(FeatureNames.Publish)]
    public async Task<IActionResult> CreateFromFile([FromHeader(Name = "x-app-id")][Required] Guid xAppId, IFormFile file)
    {


        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty");
        }

        try
        {
            xAppId = await _appIdMapperService.GetXappId(HttpContext);
            using (MemoryStream memoryStream = new())
            {

                await file.CopyToAsync(memoryStream);
                string fileContent = Encoding.UTF8.GetString(memoryStream.ToArray());
                DtroSubmit dtroSubmit = JsonConvert.DeserializeObject<DtroSubmit>(fileContent);
                GuidResponse response = await _dtroService.SaveDtroAsJsonAsync(dtroSubmit, _correlationProvider.CorrelationId, xAppId);
                await _metricsService.IncrementMetric(MetricType.Submission, xAppId);
                _logger.LogInformation($"'{nameof(CreateFromFile)}' method called using xAppId: '{xAppId}' and file '{file.Name}'");
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
        }
        catch (DtroValidationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, xAppId);
            _logger.LogError(err.Message);
            return BadRequest(err.MapToResponse());
        }
        catch (NotFoundException nFex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, xAppId);
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("DTRO", nFex.Message));
        }
        catch (InvalidOperationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, xAppId);
            _logger.LogError(err.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, xAppId);
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Update an existing D-TRO
    /// </summary>
    /// <param name="xAppId">xappid identification a D-TRO is being updated for.</param>
    /// <param name="id">ID of the D-TRO to update.</param>
    /// <param name="file">JSON file containing a full D-TRO details</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the submitted D-TRO</returns>
    [HttpPut]
    [Route("/dtros/updateFromFile/{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    public async Task<IActionResult> UpdateFromFile([FromHeader(Name = "x-app-id")][Required] Guid xAppId, Guid id, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty");
        }

        try
        {
            xAppId = await _appIdMapperService.GetXappId(HttpContext);
            using (MemoryStream memoryStream = new())
            {
                await file.CopyToAsync(memoryStream);
                string fileContent = Encoding.UTF8.GetString(memoryStream.ToArray());
                DtroSubmit dtroSubmit = JsonConvert.DeserializeObject<DtroSubmit>(fileContent);
                GuidResponse response = await _dtroService.TryUpdateDtroAsJsonAsync(id, dtroSubmit, _correlationProvider.CorrelationId, xAppId);
                await _metricsService.IncrementMetric(MetricType.Submission, xAppId);
                _logger.LogInformation($"'{nameof(CreateFromFile)}' method called using x-app-id Id: '{xAppId}', unique identifier: '{id}' and file: '{file.Name}'");
                return Ok(response);
            }
        }
        catch (DtroValidationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, xAppId);
            _logger.LogError(err.Message);
            return BadRequest(err.MapToResponse());
        }
        catch (NotFoundException nFex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, xAppId);
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("DTRO", nFex.Message));
        }
        catch (InvalidOperationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, xAppId);
            _logger.LogError(err.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, xAppId);
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Create a new D-TRO
    /// </summary>
    /// <param name="ta">TRA identification a D-TRO is being submitted for.</param>
    /// <param name="dtroSubmit">Object containing a full D-TRO details.</param>
    /// <response code="201">Created.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the submitted D-TRO</returns>
    [HttpPost]
    [Route("/dtros/createFromBody")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Publish)]
    [SwaggerResponse(201, type: typeof(GuidResponse), description: "Created")]
    public async Task<IActionResult> CreateFromBody([FromHeader(Name = "x-app-id")][Required] Guid xAppId, [FromBody] DtroSubmit dtroSubmit)
    {
        try
        {
            xAppId = await _appIdMapperService.GetXappId(HttpContext);
            GuidResponse response = await _dtroService.SaveDtroAsJsonAsync(dtroSubmit, _correlationProvider.CorrelationId, xAppId);
            await _metricsService.IncrementMetric(MetricType.Submission, xAppId);
            _logger.LogInformation($"'{nameof(CreateFromFile)}' method called using xAppId: '{xAppId}' and body '{dtroSubmit}'");
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (DtroValidationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, xAppId);
            _logger.LogError(err.Message);
            return BadRequest(err.MapToResponse());
        }
        catch (NotFoundException nFex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, xAppId);
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("DTRO", nFex.Message));
        }
        catch (InvalidOperationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, xAppId);
            _logger.LogError(err.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, xAppId);
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Update an existing D-TRO
    /// </summary>
    /// <param name="ta">TRA identification a D-TRO is being updated for.</param>
    /// <param name="id">ID of the D-TRO to update.</param>
    /// <param name="dtroSubmit">Object containing a full D-TRO details.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the submitted D-TRO</returns>
    [HttpPut]
    [Route("/dtros/updateFromBody/{id:guid}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 200, type: typeof(DtroResponse), description: "Ok")]
    public async Task<IActionResult> UpdateFromBody([FromHeader(Name = "x-app-id")][Required] Guid xAppId, [FromRoute] Guid id, [FromBody] DtroSubmit dtroSubmit)
    {
        try
        {
            xAppId = await _appIdMapperService.GetXappId(HttpContext);
            GuidResponse guidResponse = await _dtroService.TryUpdateDtroAsJsonAsync(id, dtroSubmit, _correlationProvider.CorrelationId, xAppId);
            await _metricsService.IncrementMetric(MetricType.Submission, xAppId);
            _logger.LogInformation($"'{nameof(CreateFromFile)}' method called using xAppId: '{xAppId}', unique identifier: '{id}' and body: '{dtroSubmit}'");
            return Ok(guidResponse);
        }
        catch (DtroValidationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, xAppId);
            _logger.LogError(err.Message);
            return BadRequest(err.MapToResponse());
        }
        catch (NotFoundException nFex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, xAppId);
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("DTRO", nFex.Message));
        }
        catch (InvalidOperationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, xAppId);
            _logger.LogError(err.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, xAppId);
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Gets a D-TRO by its ID
    /// </summary>
    /// <param name="id">ID of the D-TRO to retrieve.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>D-TRO object.</returns>
    [HttpGet]
    [Route("/dtros/{id:guid}")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var dtroResponse = await _dtroService.GetDtroByIdAsync(id);
            _logger.LogInformation($"'{nameof(GetById)}' method called using '{id}' unique identifier");
            return Ok(dtroResponse);
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Not found", "Dtro not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Marks a D-TRO as deleted.
    /// </summary>
    /// <param name="ta">TRA identification a D-TRO is being mark as deleted for.</param>
    /// <param name="id">ID of the D-TRO to mark as deleted.</param>
    /// <response code="204">No content.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("/dtros/{id:guid}")]
    [FeatureGate(FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 204, description: "Successfully deleted the DTRO.")]
    [SwaggerResponse(statusCode: 404, description: "Could not find a DTRO with the specified id.")]
    public async Task<IActionResult> Delete([FromHeader(Name = "x-app-id")][Required] Guid xAppId, Guid id)
    {
        try
        {
            xAppId = await _appIdMapperService.GetXappId(HttpContext);
            await _dtroService.DeleteDtroAsync(id);
            await _metricsService.IncrementMetric(MetricType.Deletion, xAppId);
            _logger.LogInformation($"'{nameof(Delete)}' method called using xAppId: '{xAppId}' and unique identifier '{id}'");
            return NoContent();
        }
        catch (NotFoundException nFex)
        {

            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Not found", "Dtro not found"));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, xAppId);
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Retrieve the Source History for existing D-TROs
    /// </summary>
    /// <param name="dtroId">ID of the D-TRO to retrieve source history for.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>List of D-TROs source history.</returns>
    [HttpGet]
    [Route("/dtros/sourceHistory/{dtroId:guid}")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    public async Task<ActionResult<List<DtroHistorySourceResponse>>> GetSourceHistory(Guid dtroId)
    {
        try
        {
            List<DtroHistorySourceResponse> response = await _dtroService.GetDtroSourceHistoryAsync(dtroId);
            _logger.LogInformation($"'{nameof(GetSourceHistory)}' method called using unique identifier '{dtroId}'");
            return Ok(response);
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse(nFex.Message, "Dtro History not found."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Retrieve the Provision History for existing D-TROs
    /// </summary>
    /// <param name="dtroId">ID of the D-TRO to retrieve provision history for.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>List of D-TROs provision history.</returns>
    [HttpGet]
    [Route("/dtros/provisionHistory/{dtroId:guid}")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    public async Task<ActionResult<List<DtroHistoryProvisionResponse>>> GetProvisionHistory(Guid dtroId)
    {
        try
        {
            List<DtroHistoryProvisionResponse> response = await _dtroService.GetDtroProvisionHistoryAsync(dtroId);
            _logger.LogInformation($"'{nameof(GetProvisionHistory)}' method called using unique identifier '{dtroId}'");
            return Ok(response);
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse(nFex.Message, "Dtro History not found."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Assign D-TRO ownership
    /// </summary>
    /// <param name="ta">TRA identification of the D-TRO owner.</param>
    /// <param name="id">ID of the D-TRO to assign.</param>
    /// <param name="assignToTraId">TRA identification of the new D-TRO owner</param>
    /// <response code="201">Created.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost("/dtros/Ownership/{id:guid}/{assignToTraId:guid}")]
    [FeatureGate(FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 201, description: "Successfully assigned the DTRO.")]
    [SwaggerResponse(statusCode: 404, description: "Could not find a DTRO with the specified id.")]
    public async Task<IActionResult> AssignOwnership([FromHeader(Name = "x-app-id")][Required] Guid xAppId, Guid id, Guid assignToTraId)
    {
        try
        {
            xAppId = await _appIdMapperService.GetXappId(HttpContext);
            await _dtroService.AssignOwnershipAsync(id, xAppId, assignToTraId, _correlationProvider.CorrelationId);
            _logger.LogInformation($"'{nameof(AssignOwnership)}' method called using xAppId '{xAppId}', unique identifier '{id}' and new assigned TRA Id '{assignToTraId}'");
            return NoContent();
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Not found", nFex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}