using System.Net;
using System.Web;
using Dft.DTRO.Admin.Models.Errors;
using Dft.DTRO.Admin.Models.Views;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Dft.DTRO.Admin.Services;

public class ErrHandlingService : IErrHandlingService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITempDataDictionaryFactory _tempDataFactory;
 
    public ErrHandlingService(IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _tempDataFactory = tempDataFactory;
    }

    public IActionResult HandleUiError(Exception ex)
    {
        var errorView = new ErrorView
        {
            UiErrorResponse = new ApiErrorResponse() { Error = "Unexpected error", Message = ex.Message  }
        };

        switch (ex)
        {
            case HttpRequestException httpEx:
                errorView.ErrorType = "UI - A network error";
                break;

            default:
                errorView.ErrorType = "UI - Internal error";
                break;
        }
       
        string errorViewJson = JsonSerializer.Serialize(errorView);
        string errorViewEncoded = HttpUtility.UrlEncode(errorViewJson);

        var routeValues = new { errorView = errorViewEncoded };
        return new RedirectToPageResult("/Error", routeValues);
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
                case HttpStatusCode.NotFound:
                case HttpStatusCode.InternalServerError:
                    errorView.ApiErrorResponse = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent, jsonOptions);
                    break;

                case HttpStatusCode.BadRequest:

                    var dtroValidationException = JsonSerializer.Deserialize<DtroValidationExceptionResponse>(errorContent, jsonOptions);
                    // errorView.DtroValidationException = dtroValidationException;
                 

                    if (dtroValidationException == null)
                    {
                        errorView.ApiErrorResponse = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent, jsonOptions);
                    }
                    else
                    {
                        var tempData = _tempDataFactory.GetTempData(httpContext);
                        tempData["dtroValidationException"] = JsonSerializer.Serialize(dtroValidationException);
                        errorView.ApiErrorResponse = new ApiErrorResponse() { Error = "DTRO Validation error", Message = "" };
                    }
                    break;

                default:
                    errorView.ApiErrorResponse = new ApiErrorResponse() { Error  = "Unexpected error", Message = errorContent   };
                    break;
            }

            string errorViewJson = JsonSerializer.Serialize(errorView);
            string errorViewEncoded = HttpUtility.UrlEncode(errorViewJson);

            if (!httpContext.Response.HasStarted)
            {
               httpContext.Response.Redirect($"/Error?errorView={errorViewEncoded}", false);
               httpContext.Response.CompleteAsync().GetAwaiter().GetResult();
               // or - await httpContext.Response.CompleteAsync();
            }
            else
            {
                throw new InvalidOperationException("Cannot redirect as the response has already started.");
            }
        }
    }
}
