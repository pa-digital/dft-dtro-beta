using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DfT.DTRO.Attributes;
using DfT.DTRO.FeatureManagement;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.DtroHistory;
using DfT.DTRO.Models.Errors;
using DfT.DTRO.Models.SharedResponse;
using DfT.DTRO.RequestCorrelation;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for capturing DTROs.
/// </summary>
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]

public class DTROsController : ControllerBase
{
    private readonly IDtroService _dtroService;
    private readonly IMetricsService _metricsService;
    private readonly IRequestCorrelationProvider _correlationProvider;
    private readonly ILogger<DTROsController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DTROsController"/> class.
    /// Default constructor.
    /// </summary>
    /// <param name="dtroService">An <see cref="IDtroService"/> instance.</param>
    /// <param name="metricsService">An <see cref="IMetricsService"/> instance.</param>
    /// <param name="correlationProvider">An <see cref="IRequestCorrelationProvider"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{DTROsController}"/> instance.</param>
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

    /// <summary>
    /// Creates a new DTRO.
    /// </summary>
    /// <param name="ta">Traffic Authority that is creating this D-TRO</param>
    /// <param name="file">The new Dtro.</param>
    /// <response code="201">Created.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Id of the DTRO.</returns>
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

    /// <summary>
    /// Updates an existing dtro.
    /// </summary>
    /// <remarks>
    /// The payload requires a dtro, which will replace the dtro with the quoted dtro version.
    /// </remarks>
    /// <param name="ta">Traffic Authority that is creating this D-TRO</param>
    /// <param name="id">The existing dtro id.</param>
    /// <param name="file">The replacement dtro.</param>
    /// <response code="200">Ok.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Id of the updated dtro.</returns>
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

    /// <summary>
    /// Creates a new DTRO.
    /// </summary>
    /// <param name="ta">Traffic Authority that is creating this D-TRO</param>
    /// <param name="dtroSubmit">A DTRO submission that satisfies the schema for the model version being submitted.</param>
    /// <response code="201">Created.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Id of the DTRO.</returns>
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

    /// <summary>
    /// Updates an existing DTRO.
    /// </summary>
    /// <param name="ta">Traffic Authority that is creating this D-TRO</param>
    /// <param name="id">The ID of the DTRO to update.</param>
    /// <param name="dtroSubmit">A DTRO submission that satisfies the schema for the model version being submitted.</param>
    /// <remarks>
    /// The payload requires a full DTRO which will replace the TRO with the quoted ID.
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Not found.</response>
    /// <returns>ID of the updated DTRO.</returns>
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

    /// <summary>
    /// Gets a DTRO by ID.
    /// </summary>
    /// <param name="id">The ID of the DTRO to retrieve.</param>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
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

    /// <summary>
    /// Marks a DTRO as deleted.
    /// </summary>
    /// <param name="ta">Traffic Authority that is creating this D-TRO</param>
    /// <param name="id">Id of the DTRO.</param>
    /// <response code="204">Okay.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
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

    /// <summary>
    /// Get D-TRO source history.
    /// </summary>
    /// <param name="dtroId">The D-TRO ID reference.</param>
    /// <response code="200">OK</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
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

    /// <summary>
    /// Get D-TRO provision history.
    /// </summary>
    /// <param name="dtroId">The D-TRO ID reference.</param>
    /// <response code="200">OK</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
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

    /// <summary>
    /// Assign a DTRO to anoth TRA.
    /// </summary>
    /// <param name="id">The D-TRO ID reference.</param>
    /// <param name="assignToTraId">Id of tra to which ownership is to be assigned.</param>
    /// <response code="204">Okay.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
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
        catch (NotFoundException)
        {
            return NotFound(new ApiErrorResponse("Not found", "Dtro not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while processing {nameof(AssignOwnership)} request.request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}