using DfT.DTRO.Models.Applications;

namespace DfT.DTRO.Controllers;

[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Tags("Users")]
public class ErrorReportController : ControllerBase
{
    private readonly IErrorReportService _errorReportService;
    private readonly IGoogleCloudStorageService _storageService;

    public ErrorReportController(IErrorReportService errorReportService, IGoogleCloudStorageService storageService)
    {
        _errorReportService = errorReportService;
        _storageService = storageService;
    }

    /// <summary>
    /// Submits an error report
    /// </summary>
    /// <param name="parameters"></param>
    /// <response code="201">Error report successfully submitted</response>
    /// <response code="400">Invalid or empty parameters</response>
    /// <response code="500">Invalid operation or other exception</response>
    [HttpPost(RouteTemplates.ErrorReportBase)]
    [FeatureGate(FeatureNames.ReadOnly)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ErrorReportSubmit([FromForm] ErrorReportRequest request)
    {
        try
        {
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "ErrorReportFiles");
            Directory.CreateDirectory(uploadPath);
            List<string> savedFileNames = new List<string>();
            foreach (var file in request.Files)
            {
                if (file.Length > 0)
                {
                    var tempFilePath = Path.Combine(Path.GetTempPath(), file.FileName);
                    using (var stream = new FileStream(tempFilePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    await _storageService.UploadFileAsync(tempFilePath);
                    savedFileNames.Add(tempFilePath);
                    System.IO.File.Delete(tempFilePath);
                }
            }

            string username = "user@test.com"; // TODO: get this from JWT
            await _errorReportService.CreateErrorReport(username, savedFileNames, request);
            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = "Invalid input parameters", error = ex.Message });
        }
        catch (FormatException)
        {
            return BadRequest(new { message = "Invalid TRO ID format" });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { message = "An error occurred while submitting the error report", error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }
}