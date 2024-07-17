using System.IO;
using System.Text;
using DfT.DTRO.FeatureManagement;
using DfT.DTRO.Models.DtroHistory;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Newtonsoft.Json;

namespace DfT.DTRO.Controllers;

[ApiController]
[Consumes("application/json")]
[Produces("application/json")]

public class DTROsController : ControllerBase
{
    private readonly IDtroService _dtroService;
    private readonly IMetricsService _metricsService;
    private readonly IRequestCorrelationProvider _correlationProvider;
    private readonly ILogger<DTROsController> _logger;

    public DTROsController(
        IDtroService dtroService,
        IMetricsService metricsService,
        IRequestCorrelationProvider correlationProvider,
        ILogger<DTROsController> logger)
    {
        _dtroService = dtroService;
        _metricsService = metricsService;
        _correlationProvider = correlationProvider;
        _logger = logger;
    }

    [HttpPost]
    [Route("/v1/dtros/createFromFile")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
    public async Task<IActionResult> CreateFromFile([FromHeader(Name = "TA")][Required] int? ta, IFormFile file)
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
                DtroSubmit dtroSubmit = JsonConvert.DeserializeObject<DtroSubmit>(fileContent);

                _logger.LogInformation("[{method}] Creating DTRO", "dtro.create");

                GuidResponse response = await _dtroService.SaveDtroAsJsonAsync(dtroSubmit, _correlationProvider.CorrelationId, ta);

                await _metricsService.IncrementMetric(MetricType.Submission, ta);
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
        }
        catch (DtroValidationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            return BadRequest(err);
        }
        catch (NotFoundException nFex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            return NotFound(new ApiErrorResponse("DTRO", nFex.Message));
        }
        catch (InvalidOperationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, ta);
            _logger.LogError(ex, "An error occurred while processing CreateFromFileByVersion request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    [HttpPut]
    [Route("/v1/dtros/updateFromFile/{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
    [ValidateModelState]
    [FeatureGate(FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    public async Task<IActionResult> UpdateFromFile([FromHeader(Name = "TA")][Required] int? ta, Guid id, IFormFile file)
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
                DtroSubmit dtroSubmit = JsonConvert.DeserializeObject<DtroSubmit>(fileContent);
                _logger.LogInformation("[{method}] Updating dtro with dtro version {dtroVersion}", "dtro.update", id.ToString());
                GuidResponse response = await _dtroService.TryUpdateDtroAsJsonAsync(id, dtroSubmit, _correlationProvider.CorrelationId, ta);
                await _metricsService.IncrementMetric(MetricType.Submission, ta);
                return Ok(response);
            }
        }
        catch (DtroValidationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            return BadRequest(err);
        }
        catch (NotFoundException nfex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            return NotFound(new ApiErrorResponse("DTRO", nfex.Message));
        }
        catch (InvalidOperationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, ta);
            _logger.LogError(ex, "An error occurred while processing UpdateFromBody request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    [HttpPost]
    [Route("/v1/dtros/createFromBody")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.DtroWrite)]
    [SwaggerResponse(201, type: typeof(GuidResponse), description: "Created")]
    public async Task<IActionResult> CreateFromBody([FromHeader(Name = "TA")][Required] int? ta, [FromBody] DtroSubmit dtroSubmit)
    {
        try
        {
            _logger.LogInformation("[{method}] Creating DTRO", "dtro.create");
            GuidResponse response = await _dtroService.SaveDtroAsJsonAsync(dtroSubmit, _correlationProvider.CorrelationId, ta);
            await _metricsService.IncrementMetric(MetricType.Submission, ta);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (DtroValidationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            return BadRequest(err);
        }
        catch (NotFoundException nfex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            return NotFound(new ApiErrorResponse("DTRO", nfex.Message));
        }
        catch (InvalidOperationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, ta);
            _logger.LogError(ex, "An error occurred while processing CreateFromFileByVersion request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    [HttpPut]
    [Route("/v1/dtros/updateFromBody/{id:guid}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(DtroResponse), description: "Okay")]
    public async Task<IActionResult> UpdateFromBody([FromHeader(Name = "TA")][Required] int? ta, [FromRoute] Guid id, [FromBody] DtroSubmit dtroSubmit)
    {
        try
        {
            _logger.LogInformation("[{method}] Updating DTRO with ID {dtroId}", "dtro.update", id);
            GuidResponse guidResponse = await _dtroService.TryUpdateDtroAsJsonAsync(id, dtroSubmit, _correlationProvider.CorrelationId, ta);
            await _metricsService.IncrementMetric(MetricType.Submission, ta);
            return Ok(guidResponse);
        }
        catch (DtroValidationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            return BadRequest(err);
        }
        catch (NotFoundException nfex)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            return NotFound(new ApiErrorResponse("DTRO", nfex.Message));
        }
        catch (InvalidOperationException err)
        {
            await _metricsService.IncrementMetric(MetricType.SubmissionValidationFailure, ta);
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, ta);
            _logger.LogError(ex, "An error occurred while processing CreateFromFileByVersion request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    [HttpGet]
    [Route("/v1/dtros/{id:guid}")]
    [FeatureGate(RequirementType.Any, FeatureNames.DtroRead, FeatureNames.DtroWrite)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var dtroResponse = await _dtroService.GetDtroByIdAsync(id);
            return Ok(dtroResponse);
        }
        catch (NotFoundException)
        {
            return NotFound(new ApiErrorResponse("Not found", "Dtro not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while processing {nameof(GetById)} request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    [HttpDelete("/v1/dtros/{id:guid}")]
    [FeatureGate(FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 204, description: "Successfully deleted the DTRO.")]
    [SwaggerResponse(statusCode: 404, description: "Could not find a DTRO with the specified id.")]
    public async Task<IActionResult> Delete([FromHeader(Name = "TA")][Required] int? ta, Guid id)
    {
        try
        {
            await _dtroService.DeleteDtroAsync(ta, id);
            return NoContent();
        }
        catch (NotFoundException)
        {
            await _metricsService.IncrementMetric(MetricType.Deletion, ta);
            return NotFound(new ApiErrorResponse("Not found", "Dtro not found"));
        }
        catch (Exception ex)
        {
            await _metricsService.IncrementMetric(MetricType.SystemFailure, ta);
            _logger.LogError(ex, $"An error occurred while processing {nameof(Delete)} request.request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    [HttpGet]
    [Route("/v1/dtros/sourceHistory/{dtroId:guid}")]
    public async Task<ActionResult<List<DtroHistorySourceResponse>>> GetSourceHistory(Guid dtroId)
    {
        try
        {
            List<DtroHistorySourceResponse> response = await _dtroService.GetDtroSourceHistoryAsync(dtroId);
            return Ok(response);
        }
        catch (NotFoundException nFex)
        {
            return NotFound(new ApiErrorResponse(nFex.Message, "Dtro History not found."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while processing {nameof(GetSourceHistory)} request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    [HttpGet]
    [Route("/v1/dtros/provisionHistory/{dtroId:guid}")]
    public async Task<ActionResult<List<DtroHistoryProvisionResponse>>> GetProvisionHistory(Guid dtroId)
    {
        try
        {
            List<DtroHistoryProvisionResponse> response = await _dtroService.GetDtroProvisionHistoryAsync(dtroId);
            return Ok(response);
        }
        catch (NotFoundException nFex)
        {

            return NotFound(new ApiErrorResponse(nFex.Message, "Dtro History not found."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while processing {nameof(GetProvisionHistory)} request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    [HttpPost("v1/dtros/Ownership/{id:guid}/{assignToTraId:int}")]
    [FeatureGate(FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 201, description: "Successfully assigned the DTRO.")]
    [SwaggerResponse(statusCode: 404, description: "Could not find a DTRO with the specified id.")]
    public async Task<IActionResult> AssignOwnership([FromHeader(Name = "TA")][Required] int? ta, Guid id, int assignToTraId)
    {
        try
        {
            await _dtroService.AssignOwnershipAsync(id, ta, assignToTraId, _correlationProvider.CorrelationId);
            return NoContent();
        }
        catch (NotFoundException nfex)
        {
            return NotFound(new ApiErrorResponse("Not found", nfex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while processing {nameof(AssignOwnership)} request.request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}