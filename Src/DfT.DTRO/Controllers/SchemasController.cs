using Newtonsoft.Json;

namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for capturing Schema Template
/// </summary>
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]

public class SchemasController : ControllerBase
{
    private readonly ISchemaTemplateService _schemaTemplateService;
    private readonly IRequestCorrelationProvider _correlationProvider;
    private readonly ILogger<SchemasController> _logger;
    private readonly LoggingExtension _loggingExtension;

    /// <summary>
    /// Default Constructor.
    /// </summary>
    /// <param name="schemaTemplateService">An <see cref="ISchemaTemplateService"/> instance.</param>
    /// <param name="correlationProvider">An <see cref="IRequestCorrelationProvider"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{SchemaController}"/> instance.</param>
    /// <param name="loggingExtension">An <see cref="LoggingExtension"/> instance.</param>
    public SchemasController(
        ISchemaTemplateService schemaTemplateService,
        IRequestCorrelationProvider correlationProvider,
        ILogger<SchemasController> logger,
         LoggingExtension loggingExtension)
    {
        _schemaTemplateService = schemaTemplateService;
        _correlationProvider = correlationProvider;
        _logger = logger;
        _loggingExtension = loggingExtension;
    }

    /// <summary>
    /// Get schema template versions.
    /// </summary>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>List of schema template versions.</returns>
    [HttpGet]
    [Route("/schemas/versions")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    public async Task<IActionResult> GetVersions()
    {
        try
        {
            List<SchemaTemplateOverview> versions = await _schemaTemplateService.GetSchemaTemplatesVersionsAsync();
            _logger.LogInformation($"'{nameof(GetVersions)}' method called");
            _loggingExtension.LogInformation(
                nameof(GetVersions),
                "/schemas/versions",
                $"'{nameof(GetVersions)}' method called");
            return Ok(versions);
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(nameof(GetVersions), "/schemas/versions", "Unexpected Null value was found", anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(GetVersions),
                "/schemas/versions",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(
                nameof(GetVersions),
                $"/schemas/versions",
                $"An unexpected error occurred: {ex.Message}",
                ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Get schema templates.
    /// </summary>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>List of schema templates.</returns>
    [HttpGet]
    [Route("/schemas")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    public async Task<IActionResult> Get()
    {
        try
        {
            List<SchemaTemplateResponse> templates = await _schemaTemplateService.GetSchemaTemplatesAsync();
            _logger.LogInformation($"'{nameof(Get)}' method called");
            _loggingExtension.LogInformation(
                nameof(Get),
                "/schemas",
                $"'{nameof(Get)}' method called");
            return Ok(templates);
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(nameof(Get), "/schemas", "Unexpected Null value was found", anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(Get),
                "/schemas",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(
                nameof(Get),
                $"/schemas",
                $"An unexpected error occurred: {ex.Message}",
                ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Get schema template by its schema version
    /// </summary>
    /// <param name="version">Schema version by which schema template to retrieve.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Schema template.</returns>
    [HttpGet]
    [Route("/schemas/{version}")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    public async Task<IActionResult> GetByVersion(string version)
    {
        try
        {
            SchemaTemplateResponse result = await _schemaTemplateService.GetSchemaTemplateAsync(version);
            _logger.LogInformation($"'{nameof(GetByVersion)}' method called using version '{version}'");
            _loggingExtension.LogInformation(
                nameof(GetByVersion),
                $"/schemas/{version}",
                $"'{nameof(GetByVersion)}' method called using version '{version}'");
            return Ok(result);
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(nameof(GetByVersion), $"/schemas/{version}", $"Schema version '{version}' not found", nfex.Message);
            return NotFound(new ApiErrorResponse("Schema version", "Schema version not found"));
        }
        catch (InvalidOperationException ioex)
        {
            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(nameof(GetByVersion), $"/schemas/{version}", "", ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(nameof(GetByVersion), $"/schemas/{version}", "Unexpected Null value was found", anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(GetByVersion), $"/schemas/{version}",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(
                nameof(GetByVersion),
                $"/schemas/{version}",
                $"An unexpected error occurred: {ex.Message}",
                ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Get schema template by its ID
    /// </summary>
    /// <param name="schemaId">ID of the schema template to retrieve.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Schema template.</returns>
    [HttpGet]
    [Route("/schemas/{schemaId:guid}")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    public async Task<IActionResult> GetById(Guid schemaId)
    {
        try
        {
            SchemaTemplateResponse response = await _schemaTemplateService.GetSchemaTemplateByIdAsync(schemaId);
            _logger.LogInformation($"'{nameof(GetById)}' method called using unique identifier '{schemaId}'");
            _loggingExtension.LogInformation(
                nameof(GetById),
                $"/schemas/{schemaId}",
                $"'{nameof(GetById)}' method called using unique identifier'{schemaId}'");
            return Ok(response);
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(nameof(GetById), $"/schemas/{schemaId}", "", nfex.Message);
            return NotFound(new ApiErrorResponse("Schema version", "Schema version not found"));
        }
        catch (InvalidOperationException ioex)
        {
            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(nameof(GetById), $"/schemas/{schemaId}", "Bad Request", ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(nameof(GetById), $"/schemas/{schemaId}", "Unexpected Null value was found", anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(GetById), $"/schemas/{schemaId}",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(GetById),
                $"/schemas/{schemaId}", $"An unexpected error occurred: {ex.Message}", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Create schema template
    /// </summary>
    /// <param name="version">Schema version the schema template is created for.</param>
    /// <param name="file">JSON file containing a full schema template.</param>
    /// <response code="201">Created.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the schema template.</returns>
    [HttpPost]
    [Route("/schemas/createFromFile/{version}")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
    [FeatureGate(FeatureNames.Admin)]
    public async Task<IActionResult> CreateFromFileByVersion(string version, IFormFile file)
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
                var payload = JsonConvert.DeserializeObject<ExpandoObject>(fileContent);
                dynamic response = await _schemaTemplateService.SaveSchemaTemplateAsJsonAsync(version, payload, _correlationProvider.CorrelationId);
                _logger.LogInformation($"'{nameof(CreateFromFileByVersion)}' method called using version '{version}' and file '{file.Name}'");
                _loggingExtension.LogInformation(
                    nameof(CreateFromFileByVersion),
                    $"/schemas/createFromFile/{version}",
                    $"'{nameof(CreateFromFileByVersion)}' method called using version '{version}' and file '{file.Name}'");
                return CreatedAtAction(nameof(CreateFromFileByVersion), new { id = response.Id }, response);
            }
        }
        catch (InvalidOperationException ioex)
        {
            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(
                    nameof(CreateFromFileByVersion),
                    $"/schemas/createFromFile/{version}", "Bad Request", ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (DecoderFallbackException dfex)
        {
            _logger.LogError(dfex.Message);
            _loggingExtension.LogError(
                    nameof(CreateFromFileByVersion),
                    $"/schemas/createFromFile/{version}", "File decoding failed", dfex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", dfex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(
                    nameof(CreateFromFileByVersion),
                    $"/schemas/createFromFile/{version}", "Unexpected Null value was found", anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                    nameof(CreateFromFileByVersion),
                    $"/schemas/createFromFile/{version}",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                    nameof(CreateFromFileByVersion),
                    $"/schemas/createFromFile/{version}",
                "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Unable to save record(s) to the database."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing CreateFromFile request.");
            _loggingExtension.LogError(
                    nameof(CreateFromFileByVersion),
                    $"/schemas/createFromFile/{version}",
                    $"An unexpected error occurred: {ex.Message}",
                    ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Create schema template
    /// </summary>
    /// <param name="version">Schema version the schema template is created for.</param>
    /// <param name="body">Object containing a full schema template.</param>
    /// <response code="201">Created.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the schema template.</returns>
    [HttpPost]
    [Route("/schemas/createFromBody/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(201, type: typeof(GuidResponse), description: "Created")]

    public async Task<IActionResult> CreateFromBodyByVersion(string version, [FromBody] ExpandoObject body)
    {
        try
        {
            GuidResponse response = await _schemaTemplateService.SaveSchemaTemplateAsJsonAsync(version, body, _correlationProvider.CorrelationId);
            _logger.LogInformation($"'{nameof(CreateFromBodyByVersion)}' method called using version '{version}' and body '{body}'");
            _loggingExtension.LogInformation(
                nameof(CreateFromBodyByVersion),
                $"/schemas/createFromBody/{version}",
                $"'{nameof(CreateFromBodyByVersion)}' method called using version '{version}' and body '{body}'");
            return CreatedAtAction(nameof(CreateFromFileByVersion), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ioex)
        {
            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBodyByVersion),
                $"/schemas/createFromBody/{version}",
                "Unexpected Null value was found",
                ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBodyByVersion),
                $"/schemas/createFromBody/{version}",
                "Unexpected Null value was found",
                anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBodyByVersion),
                $"/schemas/createFromBody/{version}",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBodyByVersion),
                $"/schemas/createFromBody/{version}",
                "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Unable to save record(s) to the database."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBodyByVersion),
                $"/schemas/createFromBody/{version}",
                $"An unexpected error occurred: {ex.Message}",
                ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Update an existing schema template.
    /// </summary>
    /// <param name="version">Schema version to be used for updating schema template.</param>
    /// <param name="file">JSON file containing a full schema template.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the updated schema template.</returns>
    [HttpPut]
    [Route("/schemas/updateFromFile/{version}")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    public async Task<IActionResult> UpdateFromFileByVersion(string version, IFormFile file)
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
                dynamic expand = JsonConvert.DeserializeObject<ExpandoObject>(fileContent);
                dynamic response = await _schemaTemplateService.UpdateSchemaTemplateAsJsonAsync(version, expand, _correlationProvider.CorrelationId);
                _logger.LogInformation($"'{nameof(UpdateFromFileByVersion)}' method called using version '{version}' and file '{file.Name}'");
                _loggingExtension.LogInformation(
                    nameof(UpdateFromFileByVersion),
                    $"/schemas/updateFromFile/{version}",
                    $"'{nameof(UpdateFromFileByVersion)}' method called using version '{version}' and file '{file.Name}'");
                return Ok(response);
            }
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromFileByVersion),
                $"/schemas/updateFromFile/{version}",
                "Rule for Schema version not found",
                nfex.Message);
            return NotFound(new ApiErrorResponse("Schema version", "Rule for Schema version not found"));
        }
        catch (InvalidOperationException ioex)
        {
            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromFileByVersion),
                $"/schemas/updateFromFile/{version}",
                "Bad Request",
                ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (DecoderFallbackException dfex)
        {
            _logger.LogError(dfex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromFileByVersion),
                $"/schemas/updateFromFile/{version}",
                "File decoding failed",
                dfex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", dfex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromFileByVersion),
                $"/schemas/updateFromFile/{version}",
                "Unexpected Null value was found",
                anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromFileByVersion),
                $"/schemas/updateFromFile/{version}",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromFileByVersion),
                $"/schemas/updateFromFile/{version}",
                "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Unable to save record(s) to the database."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromFileByVersion),
                $"/schemas/updateFromFile/{version}",
                $"An unexpected error occurred: {ex.Message}",
                ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Update an existing schema template.
    /// </summary>
    /// <param name="version">Schema version to be used for updating schema template.</param>
    /// <param name="body">Object containing a full schema template.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the updated schema template.</returns>
    [HttpPut]
    [Route("/schemas/updateFromBody/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    public async Task<IActionResult> UpdateFromBodyByVersion(string version, [FromBody] ExpandoObject body)
    {
        try
        {
            GuidResponse response = await _schemaTemplateService.UpdateSchemaTemplateAsJsonAsync(version, body, _correlationProvider.CorrelationId);
            _logger.LogInformation($"'{nameof(UpdateFromBodyByVersion)}' method called using version '{version}' and body '{body}'");
            _loggingExtension.LogInformation(
                nameof(UpdateFromBodyByVersion),
                $"/schemas/updateFromBody/{version}",
                $"'{nameof(UpdateFromBodyByVersion)}' method called using version '{version}' and body '{body}'");
            return Ok(response);
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromBodyByVersion),
                $"/schemas/updateFromBody/{version}",
                "Schema version not found",
                nfex.Message);
            return NotFound(new ApiErrorResponse("Schema version", "Schema version not found"));
        }
        catch (InvalidOperationException ioex)
        {
            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromBodyByVersion),
                $"/schemas/updateFromBody/{version}",
                "Unexpected Null value was found",
                ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromBodyByVersion),
                $"/schemas/updateFromBody/{version}",
                "Unexpected Null value was found",
                anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromBodyByVersion),
                $"/schemas/updateFromBody/{version}",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromBodyByVersion),
                $"/schemas/updateFromBody/{version}",
                "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Unable to save record(s) to the database."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromBodyByVersion),
                $"/schemas/updateFromBody/{version}",
                $"An unexpected error occurred: {ex.Message}",
                ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Activate schema template 
    /// </summary>
    /// <param name="version">Schema version by which schema template will activate.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the activated schema template.</returns>
    [HttpPatch]
    [Route("/schemas/activate/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    public async Task<IActionResult> ActivateByVersion(string version)
    {
        try
        {
            GuidResponse response = await _schemaTemplateService.ActivateSchemaTemplateAsync(version);
            _logger.LogInformation($"'{nameof(ActivateByVersion)}' method called using version '{version}'");
            _loggingExtension.LogInformation(
                nameof(ActivateByVersion),
                $"/schemas/activate/{version}",
                $"'{nameof(ActivateByVersion)}' method called using version '{version}'");
            return Ok(response);
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(
               nameof(ActivateByVersion),
                $"/schemas/activate/{version}",
                $"An unexpected error occurred: {nfex.Message}",
                nfex.Message);
            return NotFound(new ApiErrorResponse("Not found", nfex.Message));
        }
        catch (InvalidOperationException ioex)
        {
            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(
                nameof(ActivateByVersion),
                $"/schemas/activate/{version}",
                $"An unexpected error occurred: {ioex.Message}",
                ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(
                nameof(ActivateByVersion),
                $"/schemas/activate/{version}",
                $"An unexpected error occurred: {ex.Message}",
                ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Deactivate schema template 
    /// </summary>
    /// <param name="version">Schema version by which schema template will deactivate.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the deactivated schema template.</returns>
    [HttpPatch]
    [Route("/schemas/deactivate/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    public async Task<IActionResult> DeactivateByVersion(string version)
    {
        try
        {
            GuidResponse response = await _schemaTemplateService.DeActivateSchemaTemplateAsync(version);
            _logger.LogInformation($"'{nameof(DeactivateByVersion)}' method called using version '{version}'");
            _loggingExtension.LogInformation(
                nameof(DeactivateByVersion),
                $"/schemas/deactivate/{version}",
                $"'{nameof(DeactivateByVersion)}' method called using version '{version}'");
            return Ok(response);
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(
               nameof(DeactivateByVersion),
                $"/schemas/deactivate/{version}",
                $"An unexpected error occurred: {nfex.Message}",
                nfex.Message);
            return NotFound(new ApiErrorResponse("Not found", "Schema version not found"));
        }
        catch (InvalidOperationException ioex)
        {
            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(
                nameof(DeactivateByVersion),
                $"/schemas/deactivate/{version}",
                $"An unexpected error occurred: {ioex.Message}",
                ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(
                nameof(DeactivateByVersion),
                $"/schemas/deactivate/{version}",
                $"An unexpected error occurred: {ex.Message}",
                ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Delete schema template
    /// </summary>
    /// <param name="version">Schema template version by which schema template will delete.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Schema not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true"/> if schema template
    /// with specified schema version is deleted. Otherwise <see langword="false"/>
    /// </returns>
    [HttpDelete]
    [Route("/schemas/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Successfully deleted schema template.")]
    [SwaggerResponse(statusCode: 404, description: "Could not find the schema template with the specified version.")]
    [SwaggerResponse(statusCode: 400, description: "Bad Request.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<IActionResult> DeleteByVersion(string version)
    {
        _logger.LogTrace($"'{nameof(DeleteByVersion)}' method called.");
        try
        {
            var response = await _schemaTemplateService.GetSchemaTemplateAsync(version);
            if (response == null)
            {
                _logger.LogError($"Schema template with version '{version}' not found.");
                _loggingExtension.LogError(
                    nameof(DeleteByVersion),
                    $"/schemas/{version}",
                    $"Schema template with version '{version}' not found.",
                    "");
                return NotFound(new ApiErrorResponse("Schema not found.", $"Schema template with version '{version}' not found."));
            }

            var isDeleted = await _schemaTemplateService.DeleteSchemaTemplateAsync(version);
            if (!isDeleted)
            {
                _logger.LogError($"Schema template with version '{version}' couldn't be deleted. Make sure is deactivated first.");
                _loggingExtension.LogError(
                    nameof(DeleteByVersion),
                    $"/schemas/{version}",
                    $"Schema template with version '{version}' couldn't be deleted. Make sure is deactivated first.",
                    "");
                return BadRequest(new ApiErrorResponse("Schema not deleted", $"Schema template with version '{version}' couldn't be deleted. Make sure is deactivated first."));
            }

            _logger.LogInformation($"Schema template with version '{version}' deleted.");
            _loggingExtension.LogInformation(
                nameof(DeleteByVersion),
                $"/schemas/{version}",
                $"Schema template with version '{version}' deleted.");
            return Ok(true);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(
                nameof(DeleteByVersion),
                $"/schemas/{version}",
                "Bad Request",
                ex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(nameof(DeleteByVersion),
                $"/schemas/{version}", "Unexpected Null value was found", anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(DeleteByVersion),
                $"/schemas/{version}",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                nameof(DeleteByVersion),
                $"/schemas/{version}",
                "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Unable to update record(s) to the database."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(DeleteByVersion), $"/schemas/{version}", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occured: {ex.Message}"));
        }
    }
}

