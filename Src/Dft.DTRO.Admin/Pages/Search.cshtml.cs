using System.Globalization;
using System.Text.RegularExpressions;
using Dft.DTRO.Admin.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class SearchModel : PageModel
{
    public PaginatedResponse<DtroSearchResult> Dtros { get; set; }
    private readonly IDtroService _dtroService;

    public SearchModel(IDtroService dtroService)
    {
        _dtroService = dtroService;
    }

    public async Task OnGetAsync()
    {
        Dtros = await _dtroService.SearchDtros();
    }

    public string FormatOrderReportingPoint(IEnumerable<string> orderReportingPoints)
    {
        var formattedPoints = new List<string>();
        foreach (var point in orderReportingPoints)
        {
            string formattedPoint = Regex.Replace(point, "(?<!^)([A-Z])", " $1");
            formattedPoint = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(formattedPoint.ToLower());
            formattedPoints.Add(formattedPoint);
        }
        return string.Join(", ", formattedPoints);
    }
}
