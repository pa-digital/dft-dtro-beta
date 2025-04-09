using DfT.DTRO.Models.Applications;

namespace DfT.DTRO.Controllers;

[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Tags("Users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    /// <summary>
    /// Retrieve user list for user
    /// </summary>
    /// <response code="200">Valid user ID</response>
    /// <response code="400">Invalid or empty parameters, or no matching user</response>
    /// <response code="500">Invalid operation or other exception.</response>
    [HttpGet(RouteTemplates.UsersFindAll)]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(PaginatedResponse<ApplicationListDto>), description: "Ok")]
    [SwaggerResponse(statusCode: 400, type: typeof(BadRequestObjectResult), description: "No users found for this user ID")]
    [SwaggerResponse(statusCode: 404, type: typeof(ArgumentNullException), description: "Could not found any users")]
    [SwaggerResponse(statusCode: 500, type: typeof(InvalidOperationException), description: "An error occurred while fetching users")]
    [SwaggerResponse(statusCode: 500, type: typeof(Exception), description: "An unexpected error occurred")]
    public async Task<ActionResult<PaginatedResponse<UserListDto>>> findUsers([FromQuery] PaginatedRequest request)
    {
        try
        {
            var result = await _userService.GetUsers(request);
            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = "Invalid input parameters", error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching users", error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieve user details
    /// </summary>
    /// <response code="200">Valid user ID</response>
    /// <response code="400">Invalid or empty parameters, or no matching user</response>
    /// <response code="500">Invalid operation or other exception.</response>
    [HttpGet(RouteTemplates.UserDetails)]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(PaginatedResponse<ApplicationListDto>), description: "Ok")]
    [SwaggerResponse(statusCode: 400, type: typeof(BadRequestObjectResult), description: "No users found for this user ID")]
    [SwaggerResponse(statusCode: 404, type: typeof(ArgumentNullException), description: "Could not found any users")]
    [SwaggerResponse(statusCode: 500, type: typeof(InvalidOperationException), description: "An error occurred while fetching users")]
    [SwaggerResponse(statusCode: 500, type: typeof(Exception), description: "An unexpected error occurred")]
    public async Task<ActionResult<UserDto>> FindUserDetails([FromRoute] string userId)
    {
        // TODO: validate user is CSO
        try
        {
            Guid userGuid = Guid.Parse(userId);
            var result = await _userService.GetUserDetails(userGuid);
            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = "Invalid input parameters", error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching user details", error = ex.Message });
        }
        catch (FormatException ex)
        {
            return BadRequest(new { message = "Invalid user ID", error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param name="parameters"></param>
    /// <response code="204">User successfully deleted</response>
    /// <response code="400">Invalid or empty parameters</response>
    /// <response code="500">Invalid operation or other exception</response>
    [HttpDelete(RouteTemplates.UsersDelete)]
    [FeatureGate(FeatureNames.ReadOnly)]
    public async Task<IActionResult> DeleteUser([FromHeader(Name = RequestHeaderNames.Email)][Required] string email, [FromRoute] Guid userId)
    {
        try
        {
            await _userService.DeleteUser(email, userId);
            return NoContent();
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = "Invalid input parameters", error = ex.Message });
        }
        catch (FormatException)
        {
            return BadRequest(new { message = "Invalid user ID format" });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the user", error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }
}