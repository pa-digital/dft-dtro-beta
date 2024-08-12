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

    /// <summary>
    /// Default Constructor.
    /// </summary>
    /// <param name="schemaTemplateService">An <see cref="ISchemaTemplateService"/> instance.</param>
    /// <param name="correlationProvider">An <see cref="IRequestCorrelationProvider"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{SchemaController}"/> instance.</param>
    public SchemasController(
        ISchemaTemplateService schemaTemplateService,
        IRequestCorrelationProvider correlationProvider,
        ILogger<SchemasController> logger)
    {
        _schemaTemplateService = schemaTemplateService;
        _correlationProvider = correlationProvider;
        _logger = logger;
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
    [Route("/v1/schemas/versions")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    public async Task<IActionResult> GetVersions()
    {
        try
        {
            List<SchemaTemplateOverview> versions = await _schemaTemplateService.GetSchemaTemplatesVersionsAsync();
            _logger.LogInformation($"'{nameof(GetVersions)}' method called");
            return Ok(versions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
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
    [Route("/v1/schemas")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    public async Task<IActionResult> Get()
    {
        try
        {
            List<SchemaTemplateResponse> templates = await _schemaTemplateService.GetSchemaTemplatesAsync();
            _logger.LogInformation($"'{nameof(Get)}' method called");
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
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
    [Route("/v1/schemas/{version}")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    public async Task<IActionResult> GetByVersion(string version)
    {
        try
        {
            SchemaTemplateResponse result = await _schemaTemplateService.GetSchemaTemplateAsync(version);
            _logger.LogInformation($"'{nameof(GetByVersion)}' method called using version '{version}'");
            return Ok(result);
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Schema version", "Schema version not found"));
        }
        catch (InvalidOperationException err)
        {
            _logger.LogError(err.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _logger.LogError(ex, "An error occurred while processing GetByVersion request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Get schema template by its ID
    /// </summary>
    /// <param name="id">ID of the schema template to retrieve.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Schema template.</returns>
    [HttpGet]
    [Route("/v1/schemas/{id:guid}")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            SchemaTemplateResponse response = await _schemaTemplateService.GetSchemaTemplateByIdAsync(id);
            _logger.LogInformation($"'{nameof(GetById)}' method called using unique identifier '{id}'");
            return Ok(response);
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Schema version", "Schema version not found"));
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
    [Route("/v1/schemas/createFromFile/{version}")]
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
                dynamic expand = JsonConvert.DeserializeObject<ExpandoObject>(fileContent);
                dynamic response = await _schemaTemplateService.SaveSchemaTemplateAsJsonAsync(version, expand, _correlationProvider.CorrelationId);
                _logger.LogInformation($"'{nameof(CreateFromFileByVersion)}' method called using version '{version}' and file '{file.Name}'");
                return CreatedAtAction(nameof(CreateFromFileByVersion), new { id = response.Id }, response);
            }
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
    [Route("/v1/schemas/createFromBody/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(201, type: typeof(GuidResponse), description: "Created")]

    public async Task<IActionResult> CreateFromBodyByVersion(string version, [FromBody] ExpandoObject body)
    {
        try
        {
            GuidResponse response = await _schemaTemplateService.SaveSchemaTemplateAsJsonAsync(version, body, _correlationProvider.CorrelationId);
            _logger.LogInformation($"'{nameof(CreateFromBodyByVersion)}' method called using version '{version}' and body '{body}'");
            return CreatedAtAction(nameof(CreateFromFileByVersion), new { id = response.Id }, response);
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
    [Route("/v1/schemas/updateFromFile/{version}")]
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
                _logger.LogInformation($"'{nameof(UpdateFromBodyByVersion)}' method called using version '{version}' and file '{file.Name}'");
                return Ok(response);
            }
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Schema version", "Schema version not found"));
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
    [Route("/v1/schemas/updateFromBody/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    public async Task<IActionResult> UpdateFromBodyByVersion(string version, [FromBody] ExpandoObject body)
    {
        try
        {
            GuidResponse response = await _schemaTemplateService.UpdateSchemaTemplateAsJsonAsync(version, body, _correlationProvider.CorrelationId);
            _logger.LogInformation($"'{nameof(UpdateFromBodyByVersion)}' method called using version '{version}' and body '{body}'");
            return Ok(response);
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Schema version", "Schema version not found"));
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
    /// Activate schema template 
    /// </summary>
    /// <param name="version">Schema version by which schema template will activate.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the activated schema template.</returns>
    [HttpPatch]
    [Route("/v1/schemas/activate/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    public async Task<IActionResult> ActivateByVersion(string version)
    {
        try
        {
            GuidResponse response = await _schemaTemplateService.ActivateSchemaTemplateAsync(version);
            _logger.LogInformation($"'{nameof(ActivateByVersion)}' method called using version '{version}'");
            return Ok(response);
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Not found", nFex.Message));
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
    /// Deactivate schema template 
    /// </summary>
    /// <param name="version">Schema version by which schema template will deactivate.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the deactivated schema template.</returns>
    [HttpPatch]
    [Route("/v1/schemas/deactivate/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    public async Task<IActionResult> DeactivateByVersion(string version)
    {
        try
        {
            GuidResponse response = await _schemaTemplateService.DeActivateSchemaTemplateAsync(version);
            _logger.LogInformation($"'{nameof(DeactivateByVersion)}' method called using version '{version}'");
            return Ok(response);
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Not found", "Schema version not found"));
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