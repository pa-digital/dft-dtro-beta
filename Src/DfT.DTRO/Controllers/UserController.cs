namespace DfT.DTRO.Controllers;

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
    [HttpGet(RouteTemplates.GetUsers)]
    [FeatureGate(FeatureNames.Admin)]
    public ActionResult<PaginatedResponse<UserDto>> GetUsers(PaginatedRequest request)
    {
        try
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = userService.GetUsers(userId);
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

