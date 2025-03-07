using DfT.DTRO.Models.Applications;

namespace DfT.DTRO.Controllers;

[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Tags("Users")]
[TokenValidation]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param name="parameters"></param>
    /// <response code="204">User successfully deleted</response>
    /// <response code="400">Invalid or empty parameters</response>
    /// <response code="500">Invalid operation or other exception</response>
    [HttpDelete(RouteTemplates.UserDelete)]
    [FeatureGate(FeatureNames.ReadOnly)]
    public async Task<IActionResult> DeleteUser([FromBody] UserDeleteRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrEmpty(request.Id))
            {
                return BadRequest(new { message = "User ID is required" });
            }

            string userId = request.Id;
            await _userService.DeleteUser(userId);
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