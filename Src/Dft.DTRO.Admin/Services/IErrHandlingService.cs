
namespace Dft.DTRO.Admin.Services;

public interface IErrHandlingService
{
    IActionResult HandleUiError(Exception ex);

    Task RedirectIfErrors(HttpResponseMessage response);
}