using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DfT.DTRO.Attributes;
using DfT.DTRO.FeatureManagement;
using DfT.DTRO.Models.DtroDtos;
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
    private readonly IRequestCorrelationProvider _correlationProvider;
    private readonly ILogger<DTROsController> _logger;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="dtroService">An <see cref="IDtroService"/> instance.</param>
    /// <param name="correlationProvider">An <see cref="IRequestCorrelationProvider"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{DTROsController}"/> instance.</param>
    public DTROsController(
        IDtroService dtroService,
        IRequestCorrelationProvider correlationProvider,
        ILogger<DTROsController> logger)
    {
        _dtroService = dtroService;
        _correlationProvider = correlationProvider;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new DTRO.
    /// </summary>
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
    public async Task<IActionResult> CreateDtroFromFile(IFormFile file)
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
                var dtroSubmit = JsonConvert.DeserializeObject<DtroSubmit>(fileContent);

                _logger.LogInformation("[{method}] Creating DTRO", "dtro.create");
                var response = await _dtroService.SaveDtroAsJsonAsync(dtroSubmit, _correlationProvider.CorrelationId);
                return CreatedAtAction(nameof(GetDtroById), new { id = response.Id }, response);
            }
        }
        catch (DtroValidationException err)
        {
            return BadRequest(err);
        }
        catch (NotFoundException nfex)
        {
            return NotFound(new ApiErrorResponse("DTRO", nfex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing CreateSchema request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Updates an existing dtro.
    /// </summary>
    /// <remarks>
    /// The payload requires a dtro which will replace the dtro with the quoted dtro version.
    /// </remarks>
    /// <param name="id">The existing dtro id.</param>
    /// <param name="file">The replacement dtro.</param>
    /// <response code="200">Ok.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>Id of the updated dtro.</returns>
    [HttpPost]
    [Route("/v1/dtros/update/{id:guid}")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(ValueCountLimit = 1)]
    [ValidateModelState]
    [FeatureGate(FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    public async Task<IActionResult> UpdateDtroFromFile(Guid id, IFormFile file)
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
                var dtroSubmit = JsonConvert.DeserializeObject<DtroSubmit>(fileContent);

                _logger.LogInformation("[{method}] Updating dtro with dtro version {dtroVersion}", "dtro.update", id.ToString());
                var response = await _dtroService.TryUpdateDtroAsJsonAsync(id, dtroSubmit, _correlationProvider.CorrelationId);
                return Ok(response);
            }
        }
        catch (NotFoundException)
        {
            return NotFound(new ApiErrorResponse("Dtro version", "Dtro version not found"));
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new ApiErrorResponse("Bad Request", err.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing UpdateDtro request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Creates a new DTRO.
    /// </summary>
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
    public async Task<IActionResult> CreateDtro([FromBody] DtroSubmit dtroSubmit)
    {
        try
        {
            _logger.LogInformation("[{method}] Creating DTRO", "dtro.create");
            var response = await _dtroService.SaveDtroAsJsonAsync(dtroSubmit, _correlationProvider.CorrelationId);
            return CreatedAtAction(nameof(GetDtroById), new { id = response.Id }, response);
        }
        catch (DtroValidationException err)
        {
            return BadRequest(err);
        }
        catch (NotFoundException nfex)
        {
            return NotFound(new ApiErrorResponse("DTRO", nfex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing CreateSchema request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Updates an existing DTRO.
    /// </summary>
    /// <param name="id">The ID of the DTRO to update.</param>
    /// <param name="dtroSubmit">A DTRO submission that satisfies the schema for the model version being submitted.</param>
    /// <remarks>
    /// The payload requires a full DTRO which will replace the TRO with the quoted ID.
    /// </remarks>
    /// <response code="200">Ok.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Not found.</response>
    /// <returns>Id of the updated DTRO.</returns>
    [HttpPut]
    [Route("/v1/dtros/{id:guid}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 200, type: typeof(DtroResponse), description: "Okay")]
    public async Task<IActionResult> UpdateDtro([FromRoute] Guid id, [FromBody] DtroSubmit dtroSubmit)
    {
        try
        {
            _logger.LogInformation("[{method}] Updating DTRO with ID {dtroId}", "dtro.update", id);
            var guidResponse = await _dtroService.TryUpdateDtroAsJsonAsync(id, dtroSubmit, _correlationProvider.CorrelationId);
            return Ok(guidResponse);
        }
        catch (DtroValidationException err)
        {
            return BadRequest(err);
        }
        catch (NotFoundException nfex)
        {
            return NotFound(new ApiErrorResponse("DTRO", nfex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing CreateSchema request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Gets a DTRO by ID.
    /// </summary>
    /// <param name="id">The ID of the DTRO to retrieve.</param>
    /// <response code="200">Ok.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet]
    [Route("/v1/dtros/{id:guid}")]
    [FeatureGate(RequirementType.Any, FeatureNames.DtroRead, FeatureNames.DtroWrite)]
    public async Task<IActionResult> GetDtroById(Guid id)
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
            _logger.LogError(ex, "An error occurred while processing GetDtroById request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }

    /// <summary>
    /// Marks a DTRO as deleted.
    /// </summary>
    /// <param name="id">Id of the DTRO.</param>
    /// <response code="204">Okay.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("/v1/dtros/{id:guid}")]
    [FeatureGate(FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 204, description: "Successfully deleted the DTRO.")]
    [SwaggerResponse(statusCode: 404, description: "Could not find a DTRO with the specified id.")]
    public async Task<IActionResult> DeleteDtro(Guid id)
    {
        try
        {
            await _dtroService.DeleteDtroAsync(id);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound(new ApiErrorResponse("Not found", "Dtro not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing DeleteDtro request.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}