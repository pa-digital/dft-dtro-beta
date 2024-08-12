namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for capturing Rule Templates
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
    /// <param name="ruleTemplateService">An <see cref="IRuleTemplateService"/> instance.</param>
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
    /// Get rule template versions.
    /// </summary>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>List of rule template versions.</returns>
    [HttpGet]
    [Route("/rules/versions")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    public async Task<IActionResult> GetVersions()
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

    /// <summary>
    /// Get rule templates.
    /// </summary>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>List of rule templates.</returns>
    [HttpGet]
    [Route("/rules")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    public async Task<IActionResult> Get()
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

    /// <summary>
    /// Get rule template by its rule version
    /// </summary>
    /// <param name="version">Rule version by which rule template to retrieve.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Rule template.</returns>
    [HttpGet]
    [Route("/rules/{version}")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    public async Task<IActionResult> GetByVersion(string version)
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

    /// <summary>
    /// Get rule template by its ID
    /// </summary>
    /// <param name="id">ID of the rule template to retrieve.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Rule template.</returns>
    [HttpGet]
    [Route("/rules/{id:guid}")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    public async Task<IActionResult> GetById(Guid id)
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

    /// <summary>
    /// Create rule template
    /// </summary>
    /// <param name="version">Schema version the rule template is created for.</param>
    /// <param name="file">JSON file containing a full rule template.</param>
    /// <response code="201">Created.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the rule template.</returns>
    [HttpPost]
    [Route("/rules/createFromFile/{version}")]
    [FeatureGate(FeatureNames.Admin)]
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

    /// <summary>
    /// Update an existing rule template
    /// </summary>
    /// <param name="version">Schema version the rule template is updated for.</param>
    /// <param name="file">JSON file containing a full rule template.</param>
    /// <response code="201">Created.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the rule template.</returns>
    [HttpPut]
    [Route("/rules/updateFromFile/{version}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
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