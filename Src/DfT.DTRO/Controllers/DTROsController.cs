using System.Data;
using DfT.DTRO.Extensions.Exceptions;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.Errors;
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
                new LoggingExtension.Builder()
                    .WithLogType(LogType.INFO)
                    .WithMethodCalledFrom(nameof(CreateFromFile))
                    .WithEndpoint("/dtros/createFromFile")
                    .WithMessage($"'{nameof(CreateFromFile)}' method called using appId: '{appId}' and file '{file.Name}'")
                    .Build()
                    .PrintToConsole();
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
        }
        catch (DtroValidationException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);
            DtroValidationExceptionResponse dtroValidationExceptionResponse = ex.MapToResponse();

            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(CreateFromFile))
                .WithEndpoint("/dtros/createFromFile")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return BadRequest(dtroValidationExceptionResponse.Beautify());
        }
        catch (NotFoundException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(CreateFromFile))
                .WithEndpoint("/dtros/createFromFile")
                .WithMessage(" not found")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return NotFound(new ApiErrorResponse(" not found", ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(CreateFromFile))
                .WithEndpoint("/dtros/createFromFile")
                .WithMessage("Bad Request")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ex.Message));
        }
        catch (ArgumentNullException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(CreateFromFile))
                .WithEndpoint("/dtros/createFromFile")
                .WithMessage("Unexpected Null value was found")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ex.Message));
        }
        catch (OperationCanceledException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(CreateFromFile))
                .WithEndpoint("/dtros/createFromFile")
                .WithMessage("operation to the database was unexpectedly canceled")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: operation to the database was unexpectedly canceled."));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, appId);
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(CreateFromFile))
                .WithEndpoint("/dtros/createFromFile")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occured: {ex.Message}"));
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
    [Route("/dtros/updateFromFile/{id:guid}")]
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
                new LoggingExtension.Builder()
                    .WithLogType(LogType.INFO)
                    .WithMethodCalledFrom(nameof(UpdateFromFile))
                    .WithEndpoint($"/dtros/UpdateFromFile/{dtroId}")
                    .WithMessage($"'{nameof(UpdateFromFile)}' method called using appId: '{appId}', unique identifier: '{dtroId}' and file '{file.Name}'")
                    .Build()
                    .PrintToConsole();
                return Ok(response);
            }
        }
        catch (DtroValidationException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);
            DtroValidationExceptionResponse dtroValidationExceptionResponse = ex.MapToResponse();

            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(UpdateFromFile))
                .WithEndpoint($"/dtros/UpdateFromFile/{dtroId}")
                .WithMessage($"TRO invaild: {dtroValidationExceptionResponse.Beautify()}")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return BadRequest(dtroValidationExceptionResponse.Beautify());
        }
        catch (NotFoundException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(UpdateFromFile))
                .WithEndpoint($"/dtros/UpdateFromFile/{dtroId}")
                .WithMessage("TRO not found")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return NotFound(new ApiErrorResponse("TRO not found", ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(UpdateFromFile))
                .WithEndpoint($"/dtros/UpdateFromFile/{dtroId}")
                .WithMessage("Bad Request")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ex.Message));
        }
        catch (DataException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(UpdateFromFile))
                .WithEndpoint($"/dtros/UpdateFromFile/{dtroId}")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return NotFound(new ApiErrorResponse("", ex.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, appId);

            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(UpdateFromFile))
                .WithEndpoint($"/dtros/UpdateFromFile/{dtroId}")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occured: {ex.Message}"));
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
            new LoggingExtension.Builder()
                .WithLogType(LogType.INFO)
                .WithMethodCalledFrom(nameof(CreateFromBody))
                .WithEndpoint("/dtros/createFromBody")
                .WithMessage($"'{nameof(CreateFromBody)}' method called using appId: '{appId}' and body '{dtroSubmit}'")
                .Build()
                .PrintToConsole();
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (DtroValidationException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);
            DtroValidationExceptionResponse dtroValidationExceptionResponse = ex.MapToResponse();

            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(CreateFromBody))
                .WithEndpoint($"/dtros/createFromBody")
                .WithMessage($"TRO invaild: {dtroValidationExceptionResponse.Beautify()}")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);

            return BadRequest(dtroValidationExceptionResponse.Beautify());
        }
        catch (NotFoundException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(CreateFromBody))
                .WithEndpoint($"/dtros/createFromBody")
                .WithMessage("Dtro not found")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return NotFound(new ApiErrorResponse("Dtro not found", ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(CreateFromBody))
                .WithEndpoint($"/dtros/createFromBody")
                .WithMessage("Bad Request")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ex.Message));
        }
        catch (ArgumentNullException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(CreateFromBody))
                .WithEndpoint("/dtros/createFromBody")
                .WithMessage("Unexpected Null value was found")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ex.Message));
        }
        catch (OperationCanceledException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(CreateFromBody))
                .WithEndpoint("/dtros/createFromBody")
                .WithMessage("operation to the database was unexpectedly canceled")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: operation to the database was unexpectedly canceled."));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, appId);

            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(CreateFromBody))
                .WithEndpoint($"/dtros/createFromBody")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occured: {ex.Message}"));
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
    [Route("/dtros/updateFromBody/{id:guid}")]
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
            new LoggingExtension.Builder()
                .WithLogType(LogType.INFO)
                .WithMethodCalledFrom(nameof(UpdateFromBody))
                .WithEndpoint($"/dtros/updateFromBody/{dtroId}")
                .WithMessage($"'{nameof(UpdateFromBody)}' method called using appId: '{appId}', unique identifier: '{dtroId}' and body: '{dtroSubmit}'")
                .Build()
                .PrintToConsole();
            return Ok(guidResponse);
        }
        catch (DtroValidationException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);
            DtroValidationExceptionResponse dtroValidationExceptionResponse = ex.MapToResponse();

            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(UpdateFromBody))
                .WithEndpoint($"/dtros/updateFromBody/{dtroId}")
                .WithMessage($"TRO invaild: {dtroValidationExceptionResponse.Beautify()}")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return BadRequest(dtroValidationExceptionResponse.Beautify());
        }
        catch (NotFoundException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(UpdateFromBody))
                .WithEndpoint($"/dtros/updateFromBody/{dtroId}")
                .WithMessage($"TRO '{dtroId}' not found")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return NotFound(new ApiErrorResponse($"TRO '{dtroId}' not found", ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(UpdateFromBody))
                .WithEndpoint($"/dtros/updateFromBody/{dtroId}")
                .WithMessage("Bad Request")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ex.Message));
        }
        catch (DataException ex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, appId);

            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(UpdateFromBody))
                .WithEndpoint($"/dtros/updateFromBody/{dtroId}")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return NotFound(new ApiErrorResponse("", ex.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, appId);

            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(UpdateFromBody))
                .WithEndpoint($"/dtros/updateFromBody/{dtroId}")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occured: {ex.Message}"));
        }
    }

    /// <summary>
    /// Gets a D-TRO by its ID
    /// </summary>
    /// <param name="dtroId">ID of the D-TRO to retrieve.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>D-TRO object.</returns>
    [HttpGet]
    [Route("/dtros/{id:guid}")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    public async Task<IActionResult> GetById(Guid dtroId)
    {
        try
        {
            DtroResponse dtroResponse = await _dtroService.GetDtroByIdAsync(dtroId);
            _logger.LogInformation($"'{nameof(GetById)}' method called using '{dtroId}' unique identifier");
            new LoggingExtension.Builder()
                .WithLogType(LogType.INFO)
                .WithMethodCalledFrom(nameof(GetById))
                .WithEndpoint($"/dtros/{dtroId}")
                .WithMessage($"'{nameof(GetById)}' method called using '{dtroId}' unique identifier")
                .Build()
                .PrintToConsole();
            return Ok(dtroResponse);
        }
        catch (NotFoundException ex)
        {
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(GetById))
                .WithEndpoint($"/dtros/{dtroId}")
                .WithMessage($"TRO '{dtroId}' not found")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return NotFound(new ApiErrorResponse($"TRO '{dtroId}' not found", ex.Message));
        }
        catch (Exception ex)
        {
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(GetById))
                .WithEndpoint($"/dtros/{dtroId}")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occured: {ex.Message}"));
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
    [HttpDelete("/dtros/{id:guid}")]
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
            new LoggingExtension.Builder()
                .WithLogType(LogType.INFO)
                .WithMethodCalledFrom(nameof(Delete))
                .WithEndpoint($"/dtros/{dtroId}")
                .WithMessage($"'{nameof(Delete)}' method called using appId: '{appId}' and unique identifier '{dtroId}'")
                .Build()
                .PrintToConsole();
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(Delete))
                .WithEndpoint($"/dtros/{dtroId}")
                .WithMessage($"TRO '{dtroId}' not found")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return NotFound(new ApiErrorResponse($"TRO '{dtroId}' not found", ex.Message));
        }
        catch (ArgumentNullException ex)
        {
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(Delete))
                .WithEndpoint($"/dtros/{dtroId}")
                .WithMessage("Unexpected Null value was found")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ex.Message));
        }
        catch (OperationCanceledException ex)
        {
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(Delete))
                .WithEndpoint($"/dtros/{dtroId}")
                .WithMessage("operation to the database was unexpectedly canceled")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: operation to the database was unexpectedly canceled."));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, appId);
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(Delete))
                .WithEndpoint($"/dtros/{dtroId}")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occured: {ex.Message}"));
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
            new LoggingExtension.Builder()
                .WithLogType(LogType.INFO)
                .WithMethodCalledFrom(nameof(GetSourceHistory))
                .WithEndpoint($"/dtros/sourceHistory/{dtroId}")
                .WithMessage($"'{nameof(GetSourceHistory)}' method called using unique identifier '{dtroId}'")
                .Build()
                .PrintToConsole();
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(GetSourceHistory))
                .WithEndpoint($"/dtros/sourceHistory/{dtroId}")
                .WithMessage("Dtro History not found")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return NotFound(new ApiErrorResponse("Dtro History not found.", ex.Message));
        }
        catch (Exception ex)
        {
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(GetSourceHistory))
                .WithEndpoint($"/dtros/sourceHistory/{dtroId}")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occured: {ex.Message}"));
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
            new LoggingExtension.Builder()
                .WithLogType(LogType.INFO)
                .WithMethodCalledFrom(nameof(GetProvisionHistory))
                .WithEndpoint($"/dtros/provisionHistory/{dtroId}")
                .WithMessage($"'{nameof(GetProvisionHistory)}' method called using unique identifier '{dtroId}'")
                .Build()
                .PrintToConsole();
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(GetProvisionHistory))
                .WithEndpoint($"/dtros/provisionHistory/{dtroId}")
                .WithMessage("Dtro History not found")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return NotFound(new ApiErrorResponse("Dtro History not found.", ex.Message));
        }
        catch (Exception ex)
        {
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(GetProvisionHistory))
                .WithEndpoint($"/dtros/provisionHistory/{dtroId}")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occured: {ex.Message}"));
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
    [HttpPost("/dtros/Ownership/{id:guid}/{assignToTraId:guid}")]
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
            new LoggingExtension.Builder()
                .WithLogType(LogType.INFO)
                .WithMethodCalledFrom(nameof(AssignOwnership))
                .WithEndpoint($"/dtros/Ownership/{dtroId}/{assignToTraId}")
                .WithMessage($"'{nameof(AssignOwnership)}' method called using appId '{appId}', unique identifier '{dtroId}' and new assigned TRA Id '{assignToTraId}'")
                .Build()
                .PrintToConsole();
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(AssignOwnership))
                .WithEndpoint($"/dtros/Ownership/{dtroId}/{assignToTraId}")
                .WithMessage("Dtro History not found")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return NotFound(new ApiErrorResponse("Not found", ex.Message));
        }
        catch (ArgumentNullException ex)
        {
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(AssignOwnership))
                .WithEndpoint($"/dtros/Ownership/{dtroId}/{assignToTraId}")
                .WithMessage("Unexpected Null value was found")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ex.Message));
        }
        catch (OperationCanceledException ex)
        {
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(AssignOwnership))
                .WithEndpoint($"/dtros/Ownership/{dtroId}/{assignToTraId}")
                .WithMessage("operation to the database was unexpectedly canceled")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: operation to the database was unexpectedly canceled."));
        }
        catch (Exception ex)
        {
            new LoggingExtension.Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(nameof(AssignOwnership))
                .WithEndpoint($"/dtros/Ownership/{dtroId}/{assignToTraId}")
                .WithExceptionMessage(ex.Message)
                .Build()
                .PrintToConsole();
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occured: {ex.Message}"));
        }
    }
}