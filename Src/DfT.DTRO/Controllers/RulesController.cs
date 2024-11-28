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
    private readonly LoggingExtension _loggingExtension;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="ruleTemplateService">An <see cref="IRuleTemplateService"/> instance.</param>
    /// <param name="correlationProvider">An <see cref="IRequestCorrelationProvider"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{RulesController}"/> instance.</param>
    /// <param name="loggingExtension">An <see cref="LoggingExtension"/> instance.</param>
    public RulesController(
        IRuleTemplateService ruleTemplateService,
        IRequestCorrelationProvider correlationProvider,
        ILogger<RulesController> logger,
         LoggingExtension loggingExtension)
    {
        _ruleTemplateService = ruleTemplateService;
        _correlationProvider = correlationProvider;
        _logger = logger;
        _loggingExtension = loggingExtension;
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
            _loggingExtension.LogInformation(
                nameof(GetVersions),
                "/rules/versions",
                $"'{nameof(GetVersions)}' method called");
            return Ok(versions);
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(nameof(GetVersions), "/rules/versions", "Unexpected Null value was found", anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(GetVersions),
                "/rules/versions",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(GetVersions), "/rules/versions", "", ex.Message);
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
            _loggingExtension.LogInformation(
                nameof(Get),
                "/rules",
                $"'{nameof(Get)}' method called");
            return Ok(templates);
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(nameof(Get), "/rules", "Unexpected Null value was found", anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(Get),
                "/rules",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(Get), "/rules", "", ex.Message);
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
            _loggingExtension.LogInformation(
                nameof(GetByVersion),
                $"/rules/{version}",
                $"'{nameof(GetByVersion)}' method called using version '{version}'");
            return Ok(result);
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(nameof(GetByVersion), $"/rules/{version}", $"Schema version '{version}' not found", nfex.Message);
            return NotFound(new ApiErrorResponse("Schema version", "Schema version not found"));
        }
        catch (InvalidOperationException ioex)
        {
            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(nameof(GetByVersion), $"/rules/{version}", "", ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(nameof(GetByVersion), $"/rules/{version}", "Unexpected Null value was found", anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(GetByVersion),
                $"/rules/{version}",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(GetByVersion), $"/rules/{version}", $"An unexpected error occurred: {ex.Message}", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Get rule template by its ID
    /// </summary>
    /// <param name="ruleId">ID of the rule template to retrieve.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Rule template.</returns>
    [HttpGet]
    [Route("/rules/{ruleId:guid}")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    public async Task<IActionResult> GetById(Guid ruleId)
    {
        try
        {
            RuleTemplateResponse response = await _ruleTemplateService.GetRuleTemplateByIdAsync(ruleId);
            _logger.LogInformation($"'{nameof(GetById)}' method called using unique identifier'{ruleId}'");
            _loggingExtension.LogInformation(
                nameof(GetById),
                $"/rules/{ruleId}",
                $"'{nameof(GetById)}' method called using unique identifier'{ruleId}'");
            return Ok(response);
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(nameof(GetById), $"/rules/{ruleId}", "", nfex.Message);
            return NotFound(new ApiErrorResponse("Rules version", "Rules version not found"));
        }
        catch (InvalidOperationException ioex)
        {
            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(nameof(GetById), $"/rules/{ruleId}", "Bad Request", ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(nameof(GetById), $"/rules/{ruleId}", "Unexpected Null value was found", anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(GetById),
                $"/rules/{ruleId}",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(
                nameof(GetById),
                $"/rules/{ruleId}",
                $"An unexpected error occurred: {ex.Message}",
                ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
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
                dynamic response = await _ruleTemplateService.SaveRuleTemplateAsJsonAsync(version, fileContent, _correlationProvider.CorrelationId);
                _logger.LogInformation($"'{nameof(CreateFromFile)}' method called using version '{version}' and file '{file.Name}'");
                _loggingExtension.LogInformation(
                    nameof(CreateFromFile),
                    $"/rules/createFromFile/{version}",
                    $"'{nameof(CreateFromFile)}' method called using version '{version}' and file '{file.Name}'");
                return CreatedAtAction(nameof(CreateFromFile), new { id = response.Id }, response);
            }
        }
        catch (InvalidOperationException ioex)
        {
            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromFile),
                $"/rules/createFromFile/{version}",
                "Bad Request",
                ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (DecoderFallbackException dfex)
        {
            _logger.LogError(dfex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromFile),
                $"/rules/createFromFile/{version}",
                "File decoding failed",
                dfex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", dfex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromFile),
                $"/rules/createFromFile/{version}",
                "Unexpected Null value was found",
                anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromFile),
                $"/rules/createFromFile/{version}",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromFile),
                $"/rules/createFromFile/{version}",
                "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Unable to save record(s) to the database."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing CreateFromFile request.");
            _loggingExtension.LogError(
                nameof(CreateFromFile),
                $"/rules/createFromFile/{version}",
                $"An unexpected error occurred: {ex.Message}",
                ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
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
                _loggingExtension.LogInformation(
                    nameof(UpdateFromFile),
                    $"/rules/updateFromFile/{version}",
                    $"'{nameof(UpdateFromFile)}' method called using version '{version}' and file '{file.Name}'");
                return Ok(response);
            }
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromFile),
                $"/rules/updateFromFile/{version}",
                "Rule for Schema version not found",
                nfex.Message);
            return NotFound(new ApiErrorResponse("Schema version", "Rule for Schema version not found"));
        }
        catch (InvalidOperationException ioex)
        {
            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromFile),
                $"/rules/updateFromFile/{version}",
                "Bad Request",
                ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (DecoderFallbackException dfex)
        {
            _logger.LogError(dfex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromFile),
                $"/rules/updateFromFile/{version}",
                "File decoding failed",
                dfex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", dfex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromFile),
                $"/rules/updateFromFile/{version}",
                "Unexpected Null value was found",
                anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromFile),
                $"/rules/updateFromFile/{version}",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromFile),
                $"/rules/updateFromFile/{version}",
                "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Unable to save record(s) to the database."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(
                nameof(UpdateFromFile),
                $"/rules/updateFromFile/{version}",
                $"An unexpected error occurred: {ex.Message}",
                ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }
}