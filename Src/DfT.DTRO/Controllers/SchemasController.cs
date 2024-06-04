using DfT.DTRO.Attributes;
using DfT.DTRO.FeatureManagement;
using DfT.DTRO.Models.Errors;
using DfT.DTRO.Models.SharedResponse;
using DfT.DTRO.RequestCorrelation;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DfT.DTRO.Controllers;

/// <summary>
/// Prototype controller for sourcing data model information.
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
    /// Default constructor.
    /// </summary>
    /// <param name="schemaTemplateService">A <see cref="ISchemaTemplateService"/> instance.</param>
    /// <param name="correlationProvider">An <see cref="IRequestCorrelationProvider"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{SchemasController}"/> instance.</param>
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
    /// Gets available schemaTemplate versions.
    /// </summary>
    /// <response code="200">Ok.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Schema Versions.</returns>
    [HttpGet]
    [Route("/v1/schemasversions")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> GetSchemasVersions()
    {
        try
        {
            var versions = await _schemaTemplateService.GetSchemaTemplatesVersionsAsync();
            return Ok(versions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing GetSchemasVersions request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Gets available schemas Templates.
    /// </summary>
    /// <response code="200">Ok.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Schemas.</returns>
    [HttpGet]
    [Route("/v1/schemas")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> GetSchemas()
    {
        try
        {
            var templates = await _schemaTemplateService.GetSchemaTemplatesAsync();
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing GetSchemas request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Gets a schema by a named identifier.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <response code="200">Ok.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Schema.</returns>
    [HttpGet]
    [Route("/v1/schemas/{version}")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> GetSchema(string version)
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
            _logger.LogError(ex, "An error occurred while processing GetSchema request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Gets a schema by a identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <response code="200">Ok.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Schema.</returns>
    [HttpGet]
    [Route("/v1/schemaById/{id}")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> GetSchemaById(Guid id)
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
            _logger.LogError(ex, "An error occurred while processing GetSchemaById request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Creates a new Schema.
    /// </summary>
    /// <param name="version">The new version.</param>
    /// <param name="file">The new Schema.</param>
    /// <response code="201">Created.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Id of the Schema.</returns>
    [HttpPost]
    [Route("/v1/createSchemaFromFile/{version}")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
    public async Task<IActionResult> CreateSchema(string version, IFormFile file)
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
                return CreatedAtAction(nameof(CreateSchema), new { id = response.Id }, response);
            }
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing CreateSchema request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Creates a new Schema.
    /// </summary>
    /// <param name="version">The new version.</param>
    /// <param name="body">The new schema.</param>
    /// <response code="201">Created.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Id of the Schema.</returns>
    [HttpPost]
    [Route("/v1/create/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
    [SwaggerResponse(201, type: typeof(GuidResponse), description: "Created")]

    public async Task<IActionResult> CreateSchema(string version, [FromBody] ExpandoObject body)
    {
        try
        {
            _logger.LogInformation("[{method}] Creating schema", "schema.create");
            var response = await _schemaTemplateService.SaveSchemaTemplateAsJsonAsync(version, body, _correlationProvider.CorrelationId);
            return CreatedAtAction(nameof(CreateSchema), new { id = response.Id }, response);
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing CreateSchema request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Updates an existing schema.
    /// </summary>
    /// <remarks>
    /// The payload requires a schema which will replace the schema with the quoted schema version.
    /// </remarks>
    /// <param name="version">The existing version.</param>
    /// <param name="file">The replacement schema.</param>
    /// <response code="200">Ok.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Id of the updated schema.</returns>
    [HttpPost]
    [Route("/v1/updateSchemaFromFile/{version}")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    public async Task<IActionResult> UpdateSchema(string version, IFormFile file)
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
            _logger.LogError(ex, "An error occurred while processing UpdateSchema request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Updates an existing schema.
    /// </summary>
    /// <remarks>
    /// The payload requires a schema which will replace the schema with the quoted schema version.
    /// </remarks>
    /// <param name="version">The existing version.</param>
    /// <param name="body">The replacement schema.</param>
    /// <response code="200">Ok.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Id of the updated schema.</returns>
    [HttpPost]
    [Route("/v1/update/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    public async Task<IActionResult> UpdateSchema(string version, [FromBody] ExpandoObject body)
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
            _logger.LogError(ex, "An error occurred while processing UpdateSchema request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Activates an existing schema.
    /// </summary>
    /// <param name="version">The existing version to activate.</param>
    /// <response code="200">Ok.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Id of the activated schema.</returns>
    [HttpPatch]
    [Route("/v1/activate/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    public async Task<IActionResult> ActivateSchema(string version)
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
            _logger.LogError(ex, "An error occurred while processing ActivateSchema request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// DeActivates an existing schema.
    /// </summary>
    /// <param name="version">The existing version to deactivate.</param>
    /// <response code="200">Ok.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Id of the deactivated schema.</returns>
    [HttpPatch]
    [Route("/v1/deactivate/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    public async Task<IActionResult> DeActivateSchema(string version)
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
            _logger.LogError(ex, "An error occurred while processing ActivateSchema request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}