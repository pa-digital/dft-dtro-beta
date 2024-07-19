using Newtonsoft.Json;

namespace DfT.DTRO.Controllers;

[ApiController]
[Consumes("application/json")]
[Produces("application/json")]

public class SchemasController : ControllerBase
{
    private readonly ISchemaTemplateService _schemaTemplateService;
    private readonly IRequestCorrelationProvider _correlationProvider;
    private readonly ILogger<SchemasController> _logger;

    public SchemasController(
        ISchemaTemplateService schemaTemplateService,
        IRequestCorrelationProvider correlationProvider,
        ILogger<SchemasController> logger)
    {
        _schemaTemplateService = schemaTemplateService;
        _correlationProvider = correlationProvider;
        _logger = logger;
    }

    [HttpGet]
    [Route("/v1/schemas/versions")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> GetVersions()
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

    [HttpGet]
    [Route("/v1/schemas")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> Get()
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

    [HttpGet]
    [Route("/v1/schemas/{version}")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> GetByVersion(string version)
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

    [HttpGet]
    [Route("/v1/schemas/{id:guid}")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> GetById(Guid id)
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

    [HttpPost]
    [Route("/v1/schemas/createFromFile/{version}")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
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

    [HttpPost]
    [Route("/v1/schemas/createFromBody/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
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

    [HttpPut]
    [Route("/v1/schemas/updateFromFile/{version}")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
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

    [HttpPut]
    [Route("/v1/schemas/updateFromBody/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
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

    [HttpPatch]
    [Route("/v1/schemas/activate/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
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

    [HttpPatch]
    [Route("/v1/schemas/deactivate/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
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