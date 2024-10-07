using Dft.DTRO.Admin.Models.Errors;
using Dft.DTRO.Admin.Models.Views;

namespace Dft.DTRO.Admin.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public ErrorView ErrorView { get; set; }

    public void OnGet()
    {


        var errorView = HttpContext.Session.GetString("errorView");
        if (!string.IsNullOrEmpty(errorView))
        {
            ErrorView = JsonSerializer.Deserialize<ErrorView>(errorView);

            if (ErrorView == null)
            {
                ErrorView = new ErrorView
                {
                    ErrorType = "UI - Unable to process error details"
                };
            }

            if (HttpContext.Session.GetString("dtroValidationException") != null)
            {
                ErrorView.DtroValidationException = JsonSerializer.Deserialize<DtroValidationExceptionResponse>((string)HttpContext.Session.GetString("dtroValidationException"));

                if (ErrorView.DtroValidationException.RequestComparedToSchema != null)
                {
                    var unPackedList = unPackList(ErrorView.DtroValidationException.RequestComparedToSchema);
                    ErrorView.DtroValidationException.RequestComparedToSchema = unPackedList.Distinct(new DtroJsonValidationErrorResponseComparer()).ToList();
                }
            }

        }
        else
        {
            ErrorView = new ErrorView
            {
                ErrorType = "UI - No error details provided"
            };
        }
    }

    private List<DtroJsonValidationErrorResponse> unPackList(List<DtroJsonValidationErrorResponse> list)
    {
        var ret = new List<DtroJsonValidationErrorResponse>();

        foreach (var item in list)
        {
            ret.Add(item);
            if (item.ChildErrors != null && item.ChildErrors.Count > 0)
            {
                var kids = unPackList((List<DtroJsonValidationErrorResponse>)item.ChildErrors);
                ret.AddRange(kids);
            }
            item.ChildErrors?.Clear();
        }

        return ret.Distinct(new DtroJsonValidationErrorResponseComparer()).ToList();
    }
}

public class DtroJsonValidationErrorResponseComparer : IEqualityComparer<DtroJsonValidationErrorResponse>
{
    public bool Equals(DtroJsonValidationErrorResponse x, DtroJsonValidationErrorResponse y)
    {
        if (x == null || y == null)
            return false;

        return x.Message.Trim() == y.Message.Trim() &&
               x.LineNumber == y.LineNumber &&
               x.LinePosition == y.LinePosition &&
               x.Path == y.Path;
    }

    public int GetHashCode(DtroJsonValidationErrorResponse obj)
    {
        return HashCode.Combine(obj.Message, obj.LineNumber, obj.LinePosition, obj.Path);
    }
}