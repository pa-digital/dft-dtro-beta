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
        ILogger<DTROsController> logger)
    {
        _dtroService = dtroService;
        _metricsService = metricsService;
        _correlationProvider = correlationProvider;
        _logger = logger;
    }

    /// <summary>
    /// Create a new D-TRO
    /// </summary>
    /// <param name="ta">TRA identification a D-TRO is being submitted for.</param>
    /// <param name="file">JSON file containing a full D-TRO details.</param>
    /// <response code="201">Created.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the submitted D-TRO</returns>
    [HttpPost]
    [Route("/v1/dtros/createFromFile")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
    public async Task<IActionResult> CreateFromFile([FromHeader(Name = "TA")][Required] int? ta, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty");
        }

        try
        {
            using (MemoryStream memoryStream = new())
            {
                await file.CopyToAsync(memoryStream);
                string fileContent = Encoding.UTF8.GetString(memoryStream.ToArray());
                DtroSubmit dtroSubmit = JsonConvert.DeserializeObject<DtroSubmit>(fileContent);
                GuidResponse response = await _dtroService.SaveDtroAsJsonAsync(dtroSubmit, _correlationProvider.CorrelationId, ta);
                await _metricsService.IncrementMetric(MetricType.Submission, ta);
                _logger.LogInformation($"'{nameof(CreateFromFile)}' method called using TRA Id: '{ta}' and file '{file.Name}'");
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
        }
        catch (DtroValidationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            _logger.LogError(err.Message);
            return BadRequest(err);
        }
        catch (NotFoundException nFex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("DTRO", nFex.Message));
        }
        catch (InvalidOperationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            _logger.LogError(err.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, ta);
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Update an existing D-TRO
    /// </summary>
    /// <param name="ta">TRA identification a D-TRO is being updated for.</param>
    /// <param name="id">ID of the D-TRO to update.</param>
    /// <param name="file">JSON file containing a full D-TRO details</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the submitted D-TRO</returns>
    [HttpPut]
    [Route("/v1/dtros/updateFromFile/{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
    [ValidateModelState]
    [FeatureGate(FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    public async Task<IActionResult> UpdateFromFile([FromHeader(Name = "TA")][Required] int? ta, Guid id, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty");
        }

        try
        {
            using (MemoryStream memoryStream = new())
            {
                await file.CopyToAsync(memoryStream);
                string fileContent = Encoding.UTF8.GetString(memoryStream.ToArray());
                DtroSubmit dtroSubmit = JsonConvert.DeserializeObject<DtroSubmit>(fileContent);
                GuidResponse response = await _dtroService.TryUpdateDtroAsJsonAsync(id, dtroSubmit, _correlationProvider.CorrelationId, ta);
                await _metricsService.IncrementMetric(MetricType.Submission, ta);
                _logger.LogInformation($"'{nameof(CreateFromFile)}' method called using TRA Id: '{ta}', unique identifier: '{id}' and file: '{file.Name}'");
                return Ok(response);
            }
        }
        catch (DtroValidationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            _logger.LogError(err.Message);
            return BadRequest(err);
        }
        catch (NotFoundException nFex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("DTRO", nFex.Message));
        }
        catch (InvalidOperationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            _logger.LogError(err.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, ta);
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
    [Route("/v1/dtros/createFromBody")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.DtroWrite)]
    [SwaggerResponse(201, type: typeof(GuidResponse), description: "Created")]
    public async Task<IActionResult> CreateFromBody([FromHeader(Name = "TA")][Required] int? ta, [FromBody] DtroSubmit dtroSubmit)
    {
        try
        {
            GuidResponse response = await _dtroService.SaveDtroAsJsonAsync(dtroSubmit, _correlationProvider.CorrelationId, ta);
            await _metricsService.IncrementMetric(MetricType.Submission, ta);
            _logger.LogInformation($"'{nameof(CreateFromFile)}' method called using TRA Id: '{ta}' and body '{dtroSubmit}'");
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (DtroValidationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            _logger.LogError(err.Message);
            return BadRequest(err);
        }
        catch (NotFoundException nFex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("DTRO", nFex.Message));
        }
        catch (InvalidOperationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            _logger.LogError(err.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, ta);
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
    [Route("/v1/dtros/updateFromBody/{id:guid}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(DtroResponse), description: "Okay")]
    public async Task<IActionResult> UpdateFromBody([FromHeader(Name = "TA")][Required] int? ta, [FromRoute] Guid id, [FromBody] DtroSubmit dtroSubmit)
    {
        try
        {
            GuidResponse guidResponse = await _dtroService.TryUpdateDtroAsJsonAsync(id, dtroSubmit, _correlationProvider.CorrelationId, ta);
            await _metricsService.IncrementMetric(MetricType.Submission, ta);
            _logger.LogInformation($"'{nameof(CreateFromFile)}' method called using TRA Id: '{ta}', unique identifier: '{id}' and body: '{dtroSubmit}'");
            return Ok(guidResponse);
        }
        catch (DtroValidationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            _logger.LogError(err.Message);
            return BadRequest(err);
        }
        catch (NotFoundException nFex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("DTRO", nFex.Message));
        }
        catch (InvalidOperationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            _logger.LogError(err.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, ta);
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
    [Route("/v1/dtros/{id:guid}")]
    [FeatureGate(RequirementType.Any, FeatureNames.DtroRead, FeatureNames.DtroWrite)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            DtroResponse dtroResponse = await _dtroService.GetDtroByIdAsync(id);
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
    [HttpDelete("/v1/dtros/{id:guid}")]
    [FeatureGate(FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 204, description: "Successfully deleted the DTRO.")]
    [SwaggerResponse(statusCode: 404, description: "Could not find a DTRO with the specified id.")]
    public async Task<IActionResult> Delete([FromHeader(Name = "TA")][Required] int? ta, Guid id)
    {
        try
        {
            await _dtroService.DeleteDtroAsync(ta, id);
            _logger.LogInformation($"'{nameof(Delete)}' method called using TRA Id: '{ta}' and unique identifier '{id}'");
            return NoContent();
        }
        catch (NotFoundException nFex)
        {
            await _metricsService.IncrementMetric(MetricType.Deletion, ta);
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Not found", "Dtro not found"));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, ta);
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
    [Route("/v1/dtros/sourceHistory/{dtroId:guid}")]
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
    [Route("/v1/dtros/provisionHistory/{dtroId:guid}")]
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
    [HttpPost("v1/dtros/Ownership/{id:guid}/{assignToTraId:int}")]
    [FeatureGate(FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 201, description: "Successfully assigned the DTRO.")]
    [SwaggerResponse(statusCode: 404, description: "Could not find a DTRO with the specified id.")]
    public async Task<IActionResult> AssignOwnership([FromHeader(Name = "TA")][Required] int? ta, Guid id, int assignToTraId)
    {
        try
        {
            await _dtroService.AssignOwnershipAsync(id, ta, assignToTraId, _correlationProvider.CorrelationId);
            _logger.LogInformation($"'{nameof(AssignOwnership)}' method called using TRA Id '{ta}', unique identifier '{id}' and new assigned TRA Id '{assignToTraId}'");
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