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
            List<RuleTemplateOverview> versions = await _ruleTemplateService.GetRuleTemplatesVersionsAsync();
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
    [Route("/v1/rules")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> Get()
    {
        try
        {
            List<RuleTemplateResponse> templates = await _ruleTemplateService.GetRuleTemplatesAsync();
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
    [Route("/v1/rules/{version}")]
    [FeatureGate(FeatureNames.SchemasRead)]
    public virtual async Task<IActionResult> GetByVersion(string version)
    {
        try
        {
            RuleTemplateResponse result = await _ruleTemplateService.GetRuleTemplateAsync(version);
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
            RuleTemplateResponse response = await _ruleTemplateService.GetRuleTemplateByIdAsync(id);
            _logger.LogInformation($"'{nameof(GetById)}' method called using unique identifier'{id}'");
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
            using (MemoryStream memoryStream = new())
            {
                await file.CopyToAsync(memoryStream);
                string fileContent = Encoding.UTF8.GetString(memoryStream.ToArray());
                GuidResponse response = await _ruleTemplateService.SaveRuleTemplateAsJsonAsync(version, fileContent, _correlationProvider.CorrelationId);
                _logger.LogInformation($"'{nameof(CreateFromFile)}' method called using version '{version}' and file '{file.Name}'");
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
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
            using (MemoryStream memoryStream = new())
            {
                await file.CopyToAsync(memoryStream);
                string fileContent = Encoding.UTF8.GetString(memoryStream.ToArray());
                GuidResponse response = await _ruleTemplateService.UpdateRuleTemplateAsJsonAsync(version, fileContent, _correlationProvider.CorrelationId);
                _logger.LogInformation($"'{nameof(UpdateFromFile)}' method called using version '{version}' and file '{file.Name}'");
                return Ok(response);
            }
        }
        catch (NotFoundException nFex)
        {
            _logger.LogError(nFex.Message);
            return NotFound(new ApiErrorResponse("Schema version", "Rule for Schema version not found"));
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