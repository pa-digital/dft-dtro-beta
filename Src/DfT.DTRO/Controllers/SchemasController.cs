using System.IO;
using System.Text;
using DfT.DTRO.FeatureManagement;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement.Mvc;
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
            var versions = await _schemaTemplateService.GetSchemaTemplatesVersionsAsync();
            return Ok(versions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing GetVersions request.");
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
            var templates = await _schemaTemplateService.GetSchemaTemplatesAsync();
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing GetByVersion request.");
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
            var result = await _schemaTemplateService.GetSchemaTemplateAsync(version);
            return Ok(result);
        }
        catch (NotFoundException)
        {
            return NotFound(new ApiErrorResponse("Schema version", "Schema version not found"));
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
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
            var response = await _schemaTemplateService.GetSchemaTemplateByIdAsync(id);
            return Ok(response);
        }
        catch (NotFoundException)
        {
            return NotFound(new ApiErrorResponse("Schema version", "Schema version not found"));
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing GetById request.");
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
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                string fileContent = Encoding.UTF8.GetString(memoryStream.ToArray());
                dynamic expandon = JsonConvert.DeserializeObject<ExpandoObject>(fileContent);

                _logger.LogInformation("[{method}] Creating schema", "schema.create");
                var response = await _schemaTemplateService.SaveSchemaTemplateAsJsonAsync(version, expandon, _correlationProvider.CorrelationId);
                return CreatedAtAction(nameof(CreateFromFileByVersion), new { id = response.Id }, response);
            }
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing CreateFromFileByVersion request.");
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
            _logger.LogInformation("[{method}] Creating schema", "schema.create");
            var response = await _schemaTemplateService.SaveSchemaTemplateAsJsonAsync(version, body, _correlationProvider.CorrelationId);
            return CreatedAtAction(nameof(CreateFromFileByVersion), new { id = response.Id }, response);
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing CreateFromFileByVersion request.");
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
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                string fileContent = Encoding.UTF8.GetString(memoryStream.ToArray());
                dynamic expandon = JsonConvert.DeserializeObject<ExpandoObject>(fileContent);

                _logger.LogInformation("[{method}] Updating schema with schema version {schemaVersion}", "schema.update", version);
                var response = await _schemaTemplateService.UpdateSchemaTemplateAsJsonAsync(version, expandon, _correlationProvider.CorrelationId);
                return Ok(response);
            }
        }
        catch (NotFoundException)
        {
            return NotFound(new ApiErrorResponse("Schema version", "Schema version not found"));
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing UpdateFromFileByVersion request.");
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
            _logger.LogInformation("[{method}] Updating schema with schema version {schemaVersion}", "schema.update", version);
            var response = await _schemaTemplateService.UpdateSchemaTemplateAsJsonAsync(version, body, _correlationProvider.CorrelationId);
            return Ok(response);
        }
        catch (NotFoundException)
        {
            return NotFound(new ApiErrorResponse("Schema version", "Schema version not found"));
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing UpdateFromFileByVersion request.");
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
            _logger.LogInformation("[{method}] activate schema with schema version {schemaVersion}", "schema.activate", version);
            var response = await _schemaTemplateService.ActivateSchemaTemplateAsync(version);
            return Ok(response);
        }
        catch (NotFoundException exnf)
        {
            return NotFound(new ApiErrorResponse("Not found", exnf.Message));
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing ActivateByVersion request.");
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
            _logger.LogInformation("[{method}] deactivate schema with schema version {schemaVersion}", "schema.deactivate", version);
            var response = await _schemaTemplateService.DeActivateSchemaTemplateAsync(version);
            return Ok(response);
        }
        catch (NotFoundException)
        {
            return NotFound(new ApiErrorResponse("Not found", "Schema version not found"));
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing ActivateByVersion request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}