using System.Data;
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
    private readonly IAppIdMapperService _appIdMapperService;
    private readonly LoggingExtension _loggingExtension;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="dtroService">An <see cref="IDtroService"/> instance.</param>
    /// <param name="metricsService">An <see cref="IMetricsService"/> instance.</param>
    /// <param name="correlationProvider">An <see cref="IRequestCorrelationProvider"/> instance.</param>
    /// <param name="appIdMapperService">An <see cref="IAppIdMapperService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{DTROsController}"/> instance.</param>
    /// <param name="loggingExtension">An <see cref="LoggingExtension"/> instance.</param>
    public DTROsController(
         IDtroService dtroService,
         IMetricsService metricsService,
         IRequestCorrelationProvider correlationProvider,
         IAppIdMapperService appIdMapperService,
         ILogger<DTROsController> logger,
         LoggingExtension loggingExtension)
    {
        _dtroService = dtroService;
        _metricsService = metricsService;
        _correlationProvider = correlationProvider;
        _appIdMapperService = appIdMapperService;
        _logger = logger;
        _loggingExtension = loggingExtension;
    }

    /// <summary>
    /// Create a new D-TRO
    /// </summary>
    /// <param name="appId">AppId identification a D-TRO is being submitted for.</param>
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
    public async Task<IActionResult> CreateFromFile([FromHeader(Name = "x-app-id")][Required] Guid appId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty");
        }

        try
        {
            appId = await _appIdMapperService.GetAppId(HttpContext);
            using (MemoryStream memoryStream = new())
            {
                await file.CopyToAsync(memoryStream);
                string fileContent = Encoding.UTF8.GetString(memoryStream.ToArray());
                DtroSubmit dtroSubmit = JsonConvert.DeserializeObject<DtroSubmit>(fileContent);

                GuidResponse response = await _dtroService.SaveDtroAsJsonAsync(dtroSubmit, _correlationProvider.CorrelationId, appId);
                await _metricsService.IncrementMetric(MetricType.Submission, appId);
                _logger.LogInformation($"'{nameof(CreateFromFile)}' method called using appId: '{appId}' and file '{file.Name}'");
                _loggingExtension.LogInformation(
                    nameof(CreateFromFile),
                    "/dtros/createFromFile",
                    $"'{nameof(CreateFromFile)}' method called using appId: '{appId}' and file '{file.Name}'");
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
        }
        catch (DtroValidationException dvex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            DtroValidationExceptionResponse dtroValidationExceptionResponse = dvex.MapToResponse();

            _logger.LogError(dvex.Message);
            _loggingExtension.LogError(nameof(CreateFromFile), "/dtros/createFromFile", "", dvex.Message);
            return BadRequest(dtroValidationExceptionResponse.Beautify());
        }
        catch (NotFoundException nfex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(nameof(CreateFromFile), "/dtros/createFromFile", " not found", nfex.Message);
            return NotFound(new ApiErrorResponse(" not found", nfex.Message));
        }
        catch (InvalidOperationException ioex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(nameof(CreateFromFile), "/dtros/createFromFile", "Bad Request", ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (ArgumentNullException anex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            _logger.LogError(anex.Message);
            _loggingExtension.LogError(nameof(CreateFromFile), "/dtros/createFromFile", "Unexpected Null value was found", anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromFile),
                "/dtros/createFromFile",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred: Operation to the database was unexpectedly canceled."));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromFile),
                "/dtros/createFromFile",
                "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred: Unable to save record(s) to the database."));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, appId);
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(CreateFromFile), "/dtros/createFromFile", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Update an existing D-TRO
    /// </summary>
    /// <param name="appId">AppId identification a D-TRO is being updated for.</param>
    /// <param name="dtroId">ID of the D-TRO to update.</param>
    /// <param name="file">JSON file containing a full D-TRO details</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the submitted D-TRO</returns>
    [HttpPut]
    [Route("/dtros/updateFromFile/{dtroId:guid}")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    public async Task<IActionResult> UpdateFromFile([FromHeader(Name = "x-app-id")][Required] Guid appId, Guid dtroId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty");
        }

        try
        {
            appId = await _appIdMapperService.GetAppId(HttpContext);
            using (MemoryStream memoryStream = new())
            {
                await file.CopyToAsync(memoryStream);
                string fileContent = Encoding.UTF8.GetString(memoryStream.ToArray());
                DtroSubmit dtroSubmit = JsonConvert.DeserializeObject<DtroSubmit>(fileContent);
                GuidResponse response = await _dtroService.TryUpdateDtroAsJsonAsync(dtroId, dtroSubmit, _correlationProvider.CorrelationId, appId);
                await _metricsService.IncrementMetric(MetricType.Submission, appId);
                _logger.LogInformation($"'{nameof(UpdateFromFile)}' method called using x-app-id Id: '{appId}', unique identifier: '{dtroId}' and file: '{file.Name}'");
                _loggingExtension.LogInformation(
                    nameof(UpdateFromFile),
                    $"/dtros/UpdateFromFile/{dtroId}",
                    $"'{nameof(UpdateFromFile)}' method called using appId: '{appId}', unique identifier: '{dtroId}' and file '{file.Name}'");
                return Ok(response);
            }
        }
        catch (DtroValidationException dvex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);
            DtroValidationExceptionResponse dtroValidationExceptionResponse = dvex.MapToResponse();

            _logger.LogError(dvex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromFile),
                $"/dtros/UpdateFromFile/{dtroId}",
                $"TRO invaild: {dtroValidationExceptionResponse.Beautify()}",
                dvex.Message);
            return BadRequest(dtroValidationExceptionResponse.Beautify());
        }
        catch (NotFoundException nfex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(nameof(UpdateFromFile), $"/dtros/UpdateFromFile/{dtroId}", "TRO not found", nfex.Message);
            return NotFound(new ApiErrorResponse("TRO not found", nfex.Message));
        }
        catch (InvalidOperationException ioex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(nameof(UpdateFromFile), $"/dtros/UpdateFromFile/{dtroId}", "Bad Request", ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (DataException dex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            _logger.LogError(dex.Message);
            _loggingExtension.LogError(nameof(UpdateFromFile), $"/dtros/UpdateFromFile/{dtroId}", "", dex.Message);
            return NotFound(new ApiErrorResponse("", dex.Message));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromFile),
                $"/dtros/UpdateFromFile/{dtroId}",
                "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred: Unable to save record(s) to the database."));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, appId);

            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(UpdateFromFile), $"/dtros/UpdateFromFile/{dtroId}", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Create a new D-TRO
    /// </summary>
    /// <param name="appId">AppId identification a D-TRO is being updated for.</param>
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
    public async Task<IActionResult> CreateFromBody([FromHeader(Name = "x-app-id")][Required] Guid appId, [FromBody] DtroSubmit dtroSubmit)
    {
        try
        {
            appId = await _appIdMapperService.GetAppId(HttpContext);
            GuidResponse response = await _dtroService.SaveDtroAsJsonAsync(dtroSubmit, _correlationProvider.CorrelationId, appId);
            await _metricsService.IncrementMetric(MetricType.Submission, appId);
            _logger.LogInformation($"'{nameof(CreateFromBody)}' method called using appId: '{appId}' and body '{dtroSubmit}'");
            _loggingExtension.LogInformation(
                nameof(CreateFromBody),
                "/dtros/createFromBody",
                $"'{nameof(CreateFromBody)}' method called using appId: '{appId}' and body '{dtroSubmit}'");
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (DtroValidationException dvex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);
            DtroValidationExceptionResponse dtroValidationExceptionResponse = dvex.MapToResponse();

            _logger.LogError(dvex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBody),
                "/dtros/createFromBody",
                $"TRO invaild: {dtroValidationExceptionResponse.Beautify()}",
                dvex.Message);
            return BadRequest(dtroValidationExceptionResponse.Beautify());
        }
        catch (NotFoundException nfex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(nameof(CreateFromBody), "/dtros/createFromBody", "Dtro not found", nfex.Message);
            return NotFound(new ApiErrorResponse("Dtro not found", nfex.Message));
        }
        catch (InvalidOperationException ioex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(nameof(CreateFromBody), "/dtros/createFromBody", "Bad Request", ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (ArgumentNullException anex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            _logger.LogError(anex.Message);
            _loggingExtension.LogError(nameof(CreateFromBody), "/dtros/createFromBody", "Unexpected Null value was found", anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBody),
                "/dtros/createFromBody",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred: Operation to the database was unexpectedly canceled."));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBody),
                "/dtros/createFromBody",
                "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred: Unable to save record(s) to the database."));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, appId);

            _logger.LogError(ex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBody),
                "/dtros/createFromBody",
                "",
                ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Update an existing D-TRO
    /// </summary>
    /// <param name="appId">AppId identification a D-TRO is being updated for.</param>
    /// <param name="dtroId">ID of the D-TRO to update.</param>
    /// <param name="dtroSubmit">Object containing a full D-TRO details.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the submitted D-TRO</returns>
    [HttpPut]
    [Route("/dtros/updateFromBody/{dtroId:guid}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 200, type: typeof(DtroResponse), description: "Ok")]
    public async Task<IActionResult> UpdateFromBody([FromHeader(Name = "x-app-id")][Required] Guid appId, [FromRoute] Guid dtroId, [FromBody] DtroSubmit dtroSubmit)
    {
        try
        {
            appId = await _appIdMapperService.GetAppId(HttpContext);
            GuidResponse guidResponse = await _dtroService.TryUpdateDtroAsJsonAsync(dtroId, dtroSubmit, _correlationProvider.CorrelationId, appId);
            await _metricsService.IncrementMetric(MetricType.Submission, appId);
            _logger.LogInformation($"'{nameof(CreateFromFile)}' method called using appId: '{appId}', unique identifier: '{dtroId}' and body: '{dtroSubmit}'");
            _loggingExtension.LogInformation(
                nameof(UpdateFromBody),
                $"/dtros/updateFromBody/{dtroId}",
                $"'{nameof(UpdateFromBody)}' method called using appId: '{appId}', unique identifier: '{dtroId}' and body: '{dtroSubmit}'");
            return Ok(guidResponse);
        }
        catch (DtroValidationException dvex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);
            DtroValidationExceptionResponse dtroValidationExceptionResponse = dvex.MapToResponse();

            _logger.LogError(dvex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromBody),
                $"/dtros/updateFromBody/{dtroId}",
                $"TRO invaild: {dtroValidationExceptionResponse.Beautify()}",
                dvex.Message);
            return BadRequest(dtroValidationExceptionResponse.Beautify());
        }
        catch (NotFoundException nfex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromBody),
                $"/dtros/updateFromBody/{dtroId}",
                $"TRO '{dtroId}' not found",
                nfex.Message);
            return NotFound(new ApiErrorResponse($"TRO not found", nfex.Message));
        }
        catch (InvalidOperationException ioex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(nameof(UpdateFromBody), $"/dtros/updateFromBody/{dtroId}", "Bad Request", ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (DataException dex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            _logger.LogError(dex.Message);
            _loggingExtension.LogError(nameof(UpdateFromBody), $"/dtros/updateFromBody/{dtroId}", "", dex.Message);
            return NotFound(new ApiErrorResponse("", dex.Message));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromBody),
                $"/dtros/updateFromBody/{dtroId}",
                "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred: Unable to save record(s) to the database."));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, appId);

            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(UpdateFromBody), $"/dtros/updateFromBody/{dtroId}", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Get D-TRO records
    /// </summary>
    /// <param name="parameters">Properties passed to query by</param>
    /// <returns>A list of D-TRO active records</returns>
    [HttpGet]
    [Route("/dtros")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish, FeatureNames.Consumer)]
    [SwaggerResponse(statusCode: 404, description: "Could not found any D-TRO records.")]
    [SwaggerResponse(statusCode: 500, description: "Internal Server Error")]
    [SwaggerResponse(statusCode: 200, description: "Ok")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllQueryParameters parameters)
    {
        try
        {
            IEnumerable<DtroResponse> dtroResponses = await _dtroService.GetDtrosAsync(parameters);
            _logger.LogInformation($"'{nameof(GetAll)}' method called ");
            _loggingExtension.LogInformation(
                nameof(GetAll),
                "/dtros",
                $"'{nameof(GetAll)}' method called.");
            return Ok(dtroResponses);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(GetAll), "/dtros", "D-TRO records not found", ex.Message);
            return NotFound(new ApiErrorResponse("D-TRO records not found", ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(GetAll), "/dtros", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
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
            DtroResponse dtroResponse = await _dtroService.GetDtroByIdAsync(id);
            _logger.LogInformation($"'{nameof(GetById)}' method called using '{id}' unique identifier");
            _loggingExtension.LogInformation(
                nameof(GetById),
                $"/dtros/{id}",
                $"'{nameof(GetById)}' method called using '{id}' unique identifier");
            return Ok(dtroResponse);
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(nameof(GetById), $"/dtros/{id}", $"TRO '{id}' not found", nfex.Message);
            return NotFound(new ApiErrorResponse($"TRO '{id}' not found", nfex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(GetById), $"/dtros/{id}", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Marks a D-TRO as deleted.
    /// </summary>
    /// <param name="appId">AppId identification a D-TRO is being updated for.</param>
    /// <param name="dtroId">ID of the D-TRO to mark as deleted.</param>
    /// <response code="204">No content.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("/dtros/{dtroId:guid}")]
    [FeatureGate(FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 204, description: "Successfully deleted the DTRO.")]
    [SwaggerResponse(statusCode: 404, description: "Could not find a DTRO with the specified id.")]
    public async Task<IActionResult> Delete([FromHeader(Name = "x-app-id")][Required] Guid appId, Guid dtroId)
    {
        try
        {
            appId = await _appIdMapperService.GetAppId(HttpContext);
            await _dtroService.DeleteDtroAsync(dtroId);
            await _metricsService.IncrementMetric(MetricType.Deletion, appId);
            _logger.LogInformation($"'{nameof(Delete)}' method called using appId: '{appId}' and unique identifier '{dtroId}'");
            _loggingExtension.LogInformation(
                nameof(Delete),
                $"/dtros/{dtroId}",
                $"'{nameof(Delete)}' method called using appId: '{appId}' and unique identifier '{dtroId}'");
            return NoContent();
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(nameof(Delete), $"/dtros/{dtroId}", $"TRO '{dtroId}' not found", nfex.Message);
            return NotFound(new ApiErrorResponse($"TRO '{dtroId}' not found", nfex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(nameof(Delete), $"/dtros/{dtroId}", "Unexpected Null value was found", anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(Delete),
                $"/dtros/{dtroId}",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred: Operation to the database was unexpectedly canceled."));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                nameof(Delete),
                $"/dtros/{dtroId}",
                "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred: Unable to update record(s) to the database."));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, appId);

            _logger.LogError(ex.Message);
            _loggingExtension.LogError(
                nameof(Delete),
                $"/dtros/{dtroId}",
                "",
                ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
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
            _loggingExtension.LogInformation(
                nameof(GetSourceHistory),
                $"/dtros/sourceHistory/{dtroId}",
                $"'{nameof(GetSourceHistory)}' method called using unique identifier '{dtroId}'");
            return Ok(response);
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(
                nameof(GetSourceHistory),
                $"/dtros/sourceHistory/{dtroId}",
                "History for DTRO not found",
                nfex.StackTrace);
            return NotFound(new ApiErrorResponse("History for DTRO not found.", nfex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(GetSourceHistory), $"/dtros/sourceHistory/{dtroId}", "", ex.StackTrace);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
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
            _loggingExtension.LogInformation(
                nameof(GetProvisionHistory),
                $"/dtros/provisionHistory/{dtroId}",
                $"'{nameof(GetProvisionHistory)}' method called using unique identifier '{dtroId}'");
            return Ok(response);
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(
                nameof(GetProvisionHistory),
                $"/dtros/provisionHistory/{dtroId}",
                "History for DTRO not found",
                nfex.Message);
            return NotFound(new ApiErrorResponse("History for DTRO not found.", nfex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(GetProvisionHistory), $"/dtros/provisionHistory/{dtroId}", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Assign D-TRO ownership
    /// </summary>
    /// <param name="appId">AppId identification a D-TRO is being updated for.</param>
    /// <param name="dtroId">ID of the D-TRO to assign.</param>
    /// <param name="assignToTraId">TRA identification of the new D-TRO owner</param>
    /// <response code="201">Created.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost("/dtros/ownership/{dtroId:guid}/{assignToTraId:guid}")]
    [FeatureGate(FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 201, description: "Successfully assigned the DTRO.")]
    [SwaggerResponse(statusCode: 404, description: "Could not find a DTRO with the specified id.")]
    public async Task<IActionResult> AssignOwnership([FromHeader(Name = "x-app-id")][Required] Guid appId, Guid dtroId, Guid assignToTraId)
    {
        try
        {
            appId = await _appIdMapperService.GetAppId(HttpContext);
            await _dtroService.AssignOwnershipAsync(dtroId, appId, assignToTraId, _correlationProvider.CorrelationId);
            _logger.LogInformation($"'{nameof(AssignOwnership)}' method called using appId '{appId}', unique identifier '{dtroId}' and new assigned TRA Id '{assignToTraId}'");
            _loggingExtension.LogInformation(
                nameof(AssignOwnership),
                $"/dtros/ownership/{dtroId}/{assignToTraId}",
                $"'{nameof(AssignOwnership)}' method called using appId '{appId}', unique identifier '{dtroId}' and new assigned TRA Id '{assignToTraId}'");
            return NoContent();
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(
                nameof(AssignOwnership),
                $"/dtros/ownership/{dtroId}/{assignToTraId}",
                "Dtro History not found",
                nfex.Message);
            return NotFound(new ApiErrorResponse("Not found", nfex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(
                nameof(AssignOwnership),
                $"/dtros/ownership/{dtroId}/{assignToTraId}",
                "Unexpected Null value was found",
                anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(AssignOwnership),
                $"/dtros/ownership/{dtroId}/{assignToTraId}",
                "operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred: operation to the database was unexpectedly canceled."));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                nameof(AssignOwnership),
                $"/dtros/ownership/{dtroId}/{assignToTraId}",
                "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred: Unable to save record(s) to the database."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(AssignOwnership), $"/dtros/ownership/{dtroId}/{assignToTraId}", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }
}