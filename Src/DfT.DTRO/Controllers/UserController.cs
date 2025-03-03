namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller capturing users
/// </summary>
/// <param name="userService">Service passed in</param>
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Tags("Users")]
[TokenValidation]
public class UserController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Retrieve user list for user
    /// </summary>
    /// <response code="200">Valid user ID</response>
    /// <response code="400">Invalid or empty parameters, or no matching user</response>
    /// <response code="500">Invalid operation or other exception.</response>
    [HttpPost(RouteTemplates.GetUsers)]
    [FeatureGate(FeatureNames.Admin)]
    [SwaggerResponse(statusCode: 200, type: typeof(PaginatedResponse<ApplicationListDto>), description: "Ok")]
    [SwaggerResponse(statusCode: 400, type: typeof(BadRequestObjectResult), description: "No users found for this user ID")]
    [SwaggerResponse(statusCode: 404, type: typeof(ArgumentNullException), description: "Could not found any users")]
    [SwaggerResponse(statusCode: 500, type: typeof(InvalidOperationException), description: "An error occurred while fetching users")]
    [SwaggerResponse(statusCode: 500, type: typeof(Exception), description: "An unexpected error occurred")]
    public ActionResult<PaginatedResponse<UserDto>> GetUsers([FromBody] UserRequest request)
    {
        try
        {
            request.UserId = HttpContext.Items["UserId"] as string;
            var result = userService.GetUsers(request);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest(new { message = "No users found for this user ID" });

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
}

