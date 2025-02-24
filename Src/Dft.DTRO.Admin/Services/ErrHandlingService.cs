using System.Net;
using Dft.DTRO.Admin.Models.Errors;
using Dft.DTRO.Admin.Models.Views;

namespace Dft.DTRO.Admin.Services;

public class ErrHandlingService : IErrHandlingService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ErrHandlingService> _logger;
    public ErrHandlingService(IHttpContextAccessor httpContextAccessor, ILogger<ErrHandlingService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public IActionResult HandleUiError(Exception ex)
    {
        _logger.LogError(ex, "UI Error occurred: {Message}, StackTrace: {StackTrace}, InnerException: {InnerException}",
                           ex.Message,
                           ex.StackTrace,
                           ex.InnerException != null ? ex.InnerException.Message : "None");

        var errorView = new ErrorView
        {
            UiErrorResponse = new ApiErrorResponse() { Error = "Unexpected error", Message = ex.Message }
        };

        switch (ex)
        {
            case HttpRequestException httpEx:
                errorView.ErrorType = "UI - A network error - " + httpEx.StatusCode.ToString() + " - " + httpEx.Message;
                break;

            default:
                errorView.ErrorType = "UI - Internal error";
                break;
        }
        var httpContext = _httpContextAccessor.HttpContext;

        httpContext?.Session.SetString("errorView", JsonSerializer.Serialize(errorView));
        return new RedirectToPageResult("/Error");
    }

    public async Task RedirectIfErrors(HttpResponseMessage response)
    {

        if (!response.IsSuccessStatusCode)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            string errorContent = await response.Content.ReadAsStringAsync();
            var errorView = new ErrorView();
            errorView.ErrorType = response.StatusCode.ToString();

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            switch (response.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    errorView.ApiErrorResponse = new ApiErrorResponse() { Error = "StatusCode - Forbidden", Message = "Access Denied" };
                    break;
                case HttpStatusCode.NotFound:
                    if (errorContent == "")
                    {
                        errorView.ApiErrorResponse = new ApiErrorResponse() { Error = "StatusCode - Not found", Message = "No payload" };
                    }
                    else
                    {
                        errorView.ApiErrorResponse = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent, jsonOptions);
                    }

                    break;
                case HttpStatusCode.InternalServerError:
                    errorView.ApiErrorResponse = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent, jsonOptions);
                    break;

                case HttpStatusCode.BadRequest:

                    var dtroValidationException = JsonSerializer.Deserialize<DtroValidationExceptionResponse>(errorContent, jsonOptions);

                    if (dtroValidationException == null)
                    {
                        errorView.ApiErrorResponse = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent, jsonOptions);
                    }
                    else
                    {
                        httpContext?.Session.SetString("dtroValidationException", JsonSerializer.Serialize(dtroValidationException));
                        errorView.ApiErrorResponse = new ApiErrorResponse() { Error = "DTRO Validation error", Message = errorContent };
                    }
                    break;

                default:
                    errorView.ApiErrorResponse = new ApiErrorResponse() { Error = "Unexpected error", Message = errorContent };
                    break;
            }

            httpContext?.Session.SetString("errorView", JsonSerializer.Serialize(errorView));
            httpContext?.Response.Redirect($"/Error", false);
            httpContext?.Response.CompleteAsync().GetAwaiter().GetResult();
            //await httpContext.Response.CompleteAsync();
            //await httpContext.Response.Body.FlushAsync();  
        }
    }
}
