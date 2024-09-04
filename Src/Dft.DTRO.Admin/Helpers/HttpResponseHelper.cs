using System.Net;
using Dft.DTRO.Admin.Models.Errors;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Dft.DTRO.Admin.Helpers;
public static class HttpResponseHelper
{
    public static Object Error = null;
    public static string ErrorType;
    public static IActionResult HandleError(Exception ex)
    {
        switch (ex)
        {
            case HttpRequestException httpEx:
                return new RedirectToPageResult("/Error", new { message = "UI- A network error occurred while processing your request.", error = httpEx });
            default:
                return new RedirectToPageResult("/Error", new { message = "UI - An unexpected error occurred.", error = ex });
        }
    }

    public static IActionResult HandleApiError()
    {
        if (Error != null)
        {
            return new RedirectToPageResult("/Error", new { message = ErrorType, error = Error });
        }
        return null;   
    }

    public static async Task SetError(HttpResponseMessage response)
    {
        Error = null;

        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();

            ErrorType = response.StatusCode.ToString();

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    if (response.Headers.TryGetValues("x-error-type", out var errorTypeHeaders) &&
                        errorTypeHeaders.Contains("DtroValidationException"))
                    {
                        Error = JsonSerializer.Deserialize<DtroValidationException>(errorContent, jsonOptions);
                    }
                    else
                    {
                        Error = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent, jsonOptions);
                    }
                    break;

                case HttpStatusCode.NotFound:
                    Error = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent, jsonOptions);
                    break;

                case HttpStatusCode.InternalServerError:
                    Error = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent, jsonOptions);
                    break;

                default:
                    Error = new Exception($"Unexpected error: {response.StatusCode}. Content: {errorContent}");
                    break;
            }
        }
    }

}