using DfT.DTRO.Migrations;
using DfT.DTRO.Models.DataBase;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Drawing;

namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller for capturing Dtro Users
/// </summary>
[Tags("DtroUser")]
[ApiController]
public class DtroUserController : ControllerBase
{
    private readonly IDtroUserService _dtroUserService;
    private readonly ILogger<DtroUserController> _logger;
    private readonly LoggingExtension _loggingExtension;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="dtroUserService">An <see cref="IDtroUserService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{DtroUserController}"/> instance.</param>
    /// <param name="loggingExtension">An <see cref="LoggingExtension"/> instance.</param>
    public DtroUserController(
        IDtroUserService dtroUserService,
        ILogger<DtroUserController> logger,
         LoggingExtension loggingExtension)
    {
        _dtroUserService = dtroUserService;
        _logger = logger;
        _loggingExtension = loggingExtension;
    }

    /// <summary>
    /// Retrieve All Dtro Users
    /// </summary>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>A list of Dtro Users</returns>
    [HttpGet("/dtroUsers")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 200, description: "Dtro Users retrieved successfully.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<ActionResult<List<DtroUserResponse>>> GetDtroUsers()
    {
        try
        {
            List<DtroUserResponse> dtroUserResponses = await _dtroUserService.GetAllDtroUsersAsync() ?? new List<DtroUserResponse>();
            _logger.LogInformation($"'{nameof(GetDtroUsers)}' method called");
            _loggingExtension.LogInformation(
                nameof(GetDtroUsers),
                "/dtroUsers",
                $"'{nameof(GetDtroUsers)}' method called");
            return Ok(dtroUserResponses);
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(nameof(GetDtroUsers), "/dtroUsers", "Unexpected Null value was found", anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(nameof(GetDtroUsers), "/dtroUsers", "Operation to the database was unexpectedly canceled", ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(GetDtroUsers), "/dtroUsers", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Find the existing Dtro Users that matches the requested criteria.
    /// </summary>
    /// <param name="partialName">Partial name of the Dtro User</param>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>A List of Dtro Users</returns>
    [HttpGet("/dtroUsers/search/{partialName}")]
    [FeatureGate(RequirementType.Any, FeatureNames.ReadOnly, FeatureNames.Publish)]
    [SwaggerResponse(statusCode: 200, description: "Dtro User retrieved successfully.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<ActionResult<List<DtroUserResponse>>> SearchDtroUsers(string partialName)
    {
        try
        {
            List<DtroUserResponse> dtroUserResponses = await _dtroUserService.SearchDtroUsers(partialName) ?? new List<DtroUserResponse>();
            _logger.LogInformation($"'{nameof(SearchDtroUsers)}' method called");
            _loggingExtension.LogInformation(
                nameof(SearchDtroUsers),
                $"/dtroUsers/search/{partialName}",
                $"'{nameof(SearchDtroUsers)}' method called for {partialName}");
            return Ok(dtroUserResponses);
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(nameof(SearchDtroUsers), $"/dtroUsers/search/{partialName}", "Unexpected Null value was found", anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(SearchDtroUsers),
                $"/dtroUsers/search/{partialName}",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(SearchDtroUsers), $"/dtroUsers/search/{partialName}", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Create a new Dtro user entity.
    /// </summary>
    /// <param name="body">Object containing the dtro Users details.</param>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the submitted dtro User.</returns>
    [HttpPost]
    [Route("/dtroUsers/createFromBody")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(201, type: typeof(GuidResponse), description: "Created")]
    [SwaggerResponse(statusCode: 400, description: "Bad Request.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<IActionResult> CreateFromBody(DtroUserRequest body)
    {
        try
        {
            var response = await _dtroUserService.SaveDtroUserAsync(body);
            _logger.LogInformation($"'{nameof(CreateFromBody)}' method called using ID '{body.Id}'");
            _loggingExtension.LogInformation(
                nameof(CreateFromBody),
                "/dtroUsers/createFromBody",
                $"'{nameof(CreateFromBody)}' method called using ID '{body.Id}'");
            return CreatedAtAction(nameof(CreateFromBody), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ioex)
        {
            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBody),
                "/dtroUsers/createFromBody",
                $"Bad Request using ID '{body.Id}'",
                ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBody),
                "/dtroUsers/createFromBody",
                "Unexpected Null value was found",
                anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBody),
                "/dtroUsers/createFromBody",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBody),
                "/dtroUsers/createFromBody",
                "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Unable to save record(s) to the database."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(nameof(CreateFromBody),"/dtroUsers/createFromBody", "", ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Update an existing Dtro User.
    /// </summary>
    /// <param name="body">Object containing the Dtro User details.</param>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>ID of the submitted DtroUser.</returns>
    [HttpPut]
    [Route("/dtroUsers/updateFromBody")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    [SwaggerResponse(statusCode: 404, description: "Not Found.")]
    [SwaggerResponse(statusCode: 400, description: "Bad Request.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<IActionResult> UpdateFromBody([FromBody] DtroUserRequest body)
    {
        try
        {
            GuidResponse response = await _dtroUserService.UpdateDtroUserAsync(body);
            _logger.LogInformation($"'{nameof(UpdateFromBody)}' method called using ID '{body.Id}'");
            _loggingExtension.LogInformation(
                nameof(UpdateFromBody),
                "/dtroUsers/updateFromBody",
                $"'{nameof(UpdateFromBody)}' method called using ID '{body.Id}'");
            return Ok(response);
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBody),
                "/dtroUsers/createFromBody",
                $"User not found with ID '{body.Id}'",
                nfex.Message);
            return NotFound(new ApiErrorResponse("Not Found", $"User not found with ID '{body.Id}'"));
        }
        catch (InvalidOperationException ioex)
        {
            _logger.LogError(ioex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBody),
                "/dtroUsers/createFromBody",
                $"Bad Request with ID '{body.Id}'",
                ioex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", ioex.Message));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBody),
                "/dtroUsers/createFromBody",
                "Unexpected Null value was found",
                anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBody),
                "/dtroUsers/createFromBody",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBody),
                "/dtroUsers/createFromBody",
               "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Unable to save record(s) to the database."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(
                nameof(CreateFromBody),
                "/dtroUsers/createFromBody",
               "",
                ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Retrieve a Dtro User
    /// </summary>
    /// <param name="dtroUserId">ID of the Dtro User.</param>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>A Dtro User</returns>
    [HttpGet("/dtroUsers/{dtroUserId:guid}")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(GuidResponse), description: "Ok")]
    [SwaggerResponse(statusCode: 404, description: "Not Found.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<ActionResult<DtroUserResponse>> GetDtroUser(Guid dtroUserId)
    {
        try
        {
            DtroUserResponse dtroUserResponses = await _dtroUserService.GetDtroUserAsync(dtroUserId) ?? new DtroUserResponse();
            _logger.LogInformation($"'{nameof(GetDtroUser)}' method called for User ID {dtroUserId}");
            _loggingExtension.LogInformation(
                nameof(GetDtroUser),
                $"/dtroUsers/{dtroUserId}",
                $"'{nameof(GetDtroUser)}' method called for User ID {dtroUserId}");
            return Ok(dtroUserResponses);
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(
                nameof(GetDtroUser),
                $"/dtroUsers/{dtroUserId}",
                $"User not found with ID '{dtroUserId}'",
                nfex.Message);
            return NotFound(new ApiErrorResponse("Not Found", $"User not found with ID '{dtroUserId}'"));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(
                nameof(GetDtroUser),
                $"/dtroUsers/{dtroUserId}",
                "Unexpected Null value was found",
                anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(GetDtroUser),
                $"/dtroUsers/{dtroUserId}",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(
                nameof(GetDtroUser),
                $"/dtroUsers/{dtroUserId}",
                "",
                ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Delete Dtro users by ids
    /// </summary>
    /// <param name="request">List of D-TRO User ID, comma separated</param>
    /// <response code="200">OK.</response>
    /// <response code="404">Not found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <returns>True if successful, False otherwise</returns>
    [HttpDelete("/dtroUsers/redundant")]
    [ValidateModelState]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Ok")]
    [SwaggerResponse(statusCode: 404, description: "Not Found.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]
    public async Task<ActionResult<bool>> DeleteDtroUsers([FromBody] DeleteDtroUsersRequest request)
    {
        try
        {
            if (request.Ids == null || !request.Ids.Any())
            {
                return BadRequest(new ApiErrorResponse("Bad Request", "No user IDs provided."));
            }
            bool state = await _dtroUserService.DeleteDtroUsersAsync(request.Ids);
            _logger.LogInformation($"Method '{nameof(DeleteDtroUsers)}' called");
            _loggingExtension.LogInformation(
                nameof(DeleteDtroUsers),
                "/dtroUsers/redundant",
                $"'{nameof(DeleteDtroUsers)}' method called for {request.Ids}");
            return Ok(state);
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            _loggingExtension.LogError(
                nameof(DeleteDtroUsers),
                "/dtroUsers/redundant",
                "User not found",
                nfex.Message);
            return NotFound(new ApiErrorResponse("Unknown User", "User not found"));
        }
        catch (ArgumentNullException anex)
        {
            _logger.LogError(anex.Message);
            _loggingExtension.LogError(
                nameof(DeleteDtroUsers),
                "/dtroUsers/redundant",
                "Unexpected Null value was found",
                anex.Message);
            return BadRequest(new ApiErrorResponse("Bad Request", anex.Message));
        }
        catch (OperationCanceledException ocex)
        {
            _logger.LogError(ocex.Message);
            _loggingExtension.LogError(
                nameof(DeleteDtroUsers),
                "/dtroUsers/redundant",
                "Operation to the database was unexpectedly canceled",
                ocex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Operation to the database was unexpectedly canceled."));
        }
        catch (DbUpdateException duex)
        {
            _logger.LogError(duex.Message);
            _loggingExtension.LogError(
                nameof(DeleteDtroUsers),
                "/dtroUsers/redundant",
                "Unable to save record(s) to the database",
                duex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occured: Unable to update record(s) to the database."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _loggingExtension.LogError(
                nameof(DeleteDtroUsers),
                "/dtroUsers/redundant",
                "",
                ex.Message);
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", $"An unexpected error occurred: {ex.Message}"));
        }
    }
}
