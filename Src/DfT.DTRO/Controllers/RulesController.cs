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
using Swashbuckle.AspNetCore.Annotations;
using System;
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

public class RulesController : ControllerBase
{
    private readonly IRuleTemplateService _ruleTemplateService;
    private readonly IRequestCorrelationProvider _correlationProvider;
    private readonly ILogger<RulesController> _logger;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="ruleTemplateService">A <see cref="IRuleTemplateService"/> instance.</param>
    /// <param name="correlationProvider">An <see cref="IRequestCorrelationProvider"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{RulesController}"/> instance.</param>
    public RulesController(
        IRuleTemplateService ruleTemplateService,
        IRequestCorrelationProvider correlationProvider,
        ILogger<RulesController> logger)
    {
        _ruleTemplateService = ruleTemplateService;
        _correlationProvider = correlationProvider;
        _logger = logger;
    }

    /// <summary>
    /// Gets available ruleTemplate versions.
    /// </summary>
    /// <response code="200">Ok.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Rule Versions.</returns>
    [HttpGet]
    [Route("/v1/ruleVersions")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> GetRulesVersions()
    {
        try
        {
            var versions = await _ruleTemplateService.GetRuleTemplatesVersionsAsync();
            return Ok(versions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing GetRulesVersions request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Gets available rule Templates.
    /// </summary>
    /// <response code="200">Ok.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Rules.</returns>
    [HttpGet]
    [Route("/v1/rules")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> GetRules()
    {
        try
        {
            var templates = await _ruleTemplateService.GetRuleTemplatesAsync();
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing GetRules request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Gets a rule by a named identifier.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <response code="200">Ok.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Rule.</returns>
    [HttpGet]
    [Route("/v1/rules/{version}")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> GetRule(string version)
    {
        try
        {
            var result = await _ruleTemplateService.GetRuleTemplateAsync(version);
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
            _logger.LogError(ex, "An error occurred while processing GetRule request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Gets a rule by a identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <response code="200">Ok.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Rule.</returns>
    [HttpGet]
    [Route("/v1/ruleById/{id}")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> GetRuleById(Guid id)
    {
        try
        {
            var response = await _ruleTemplateService.GetRuleTemplateByIdAsync(id);
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
            _logger.LogError(ex, "An error occurred while processing GetRuleById request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Creates a new Rule.
    /// </summary>
    /// <param name="version">The new version.</param>
    /// <param name="file">The new rule.</param>
    /// <response code="201">Created.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Id of the Rule.</returns>
    [HttpPost]
    [Route("/v1/createRuleFromFile/{version}")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
    public async Task<IActionResult> CreateRule(string version, IFormFile file)
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

                var response = await _ruleTemplateService.SaveRuleTemplateAsJsonAsync(version, fileContent, _correlationProvider.CorrelationId);
                return CreatedAtAction(nameof(GetRuleById), new { id = response.Id }, response);
            }
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing CreateRule request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Updates an existing rule.
    /// </summary>
    /// <remarks>
    /// The payload requires a rule which will replace the rule with the quoted schema version.
    /// </remarks>
    /// <param name="version">The existing version.</param>
    /// <param name="file">The replacement rule file.</param>
    /// <response code="200">Ok.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Id of the updated rule.</returns>
    [HttpPut]
    [Route("/v1/updateRuleFromFile/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
    public async Task<IActionResult> UpdateRule(string version, IFormFile file)
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

                _logger.LogInformation("[{method}] Updating rule for schema version {schemaVersion}", "rule.update", version);
                var response = await _ruleTemplateService.UpdateRuleTemplateAsJsonAsync(version, fileContent, _correlationProvider.CorrelationId);
                return Ok(response);
            }
        }
        catch (NotFoundException)
        {
            return NotFound(new ApiErrorResponse("Schema version", "Rule for Schema version not found"));
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing UpdateRule request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}