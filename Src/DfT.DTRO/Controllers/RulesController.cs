using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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

namespace DfT.DTRO.Controllers;

[ApiController]
[Consumes("application/json")]
[Produces("application/json")]

public class RulesController : ControllerBase
{
    private readonly IRuleTemplateService _ruleTemplateService;
    private readonly IRequestCorrelationProvider _correlationProvider;
    private readonly ILogger<RulesController> _logger;

    public RulesController(
        IRuleTemplateService ruleTemplateService,
        IRequestCorrelationProvider correlationProvider,
        ILogger<RulesController> logger)
    {
        _ruleTemplateService = ruleTemplateService;
        _correlationProvider = correlationProvider;
        _logger = logger;
    }

    [HttpGet]
    [Route("/v1/rules/versions")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> GetVersions()
    {
        try
        {
            var versions = await _ruleTemplateService.GetRuleTemplatesVersionsAsync();
            return Ok(versions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing GetVersions request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    [HttpGet]
    [Route("/v1/rules")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> Get()
    {
        try
        {
            var templates = await _ruleTemplateService.GetRuleTemplatesAsync();
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing GetByVersion request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    [HttpGet]
    [Route("/v1/rules/{version}")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> GetByVersion(string version)
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
            _logger.LogError(ex, "An error occurred while processing GetByVersion request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    [HttpGet]
    [Route("/v1/rules/{id:guid}")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> GetById(Guid id)
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
            _logger.LogError(ex, "An error occurred while processing GetById request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    [HttpPost]
    [Route("/v1/rules/createFromFile/{version}")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
    public async Task<IActionResult> CreateFromFile(string version, IFormFile file)
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
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing CreateFromFile request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    [HttpPut]
    [Route("/v1/rules/updateFromFile/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.SchemaWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
    public async Task<IActionResult> UpdateFromFile(string version, IFormFile file)
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
            _logger.LogError(ex, "An error occurred while processing UpdateFromFile request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}