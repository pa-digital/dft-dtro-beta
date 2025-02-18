using Microsoft.AspNetCore.Mvc;
using DfT.DTRO.Services;

namespace DfT.DTRO.Controllers
{
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Tags("Applications")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        /// <summary>
        /// Validates if the Application Name is available.
        /// </summary>
        /// <param name="parameters"></param>
        /// <response code="200">Valid Application Name.</response>
        /// <response code="400">Invalid Application Name.</response>
        [HttpPost(RouteTemplates.ValidateApplicationName)]
        [FeatureGate(FeatureNames.ReadOnly)]
        public IActionResult ValidateApplicationName([FromBody] ApplicationNameQueryParameters parameters)
        {
            try
            {
                if (parameters == null || string.IsNullOrEmpty(parameters.Name))
                {
                    return BadRequest(new { message = "Application name is required" });
                }

                string appName = parameters.Name;
                var result = _applicationService.ValidateApplicationName(appName);
                return Ok(new { isValid = result, message = result ? "Application name available" : "Application name already in use" });

            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = "Invalid input parameters", error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = "An error occurred while validating the application name", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
            }
        }
    }
}
