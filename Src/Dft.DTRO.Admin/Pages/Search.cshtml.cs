using System.Globalization;
using System.Text.RegularExpressions;
using Dft.DTRO.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
public class SearchModel : PageModel
{
    public PaginatedResponse<DtroSearchResult> Dtros { get; set; }
    private readonly IDtroService _dtroService;
    private readonly ITraService _traService;

    [BindProperty(SupportsGet = true)]
    public TraSearch TraSearch { get; set; } = new TraSearch();

    public SearchModel(IDtroService dtroService, ITraService traService)
    {
        _dtroService = dtroService;
        _traService = traService;
    }

    public async Task OnGetAsync()
    {
        Dtros = await _dtroService.SearchDtros(TraSearch.TraSelect);

        TraSearch.UpdateButtonText = "Search";
        TraSearch.SwaCodes = await _traService.GetSwaCodes();
        TraSearch.SwaCodes.Insert(0, new SwaCodeResponse { TraId = 0, Name = "[all]" , IsActive = true});
    }

    public IActionResult OnPostUpdate()
    {
        return RedirectToPage(new {TraSearch.TraSelect});
    }

    public IActionResult OnGetRefresh()
    {
        if (TempData.TryGetValue("TraSelect", out object traSelect))
            TraSearch.TraSelect = (int)traSelect;

        return RedirectToPage(new {TraSearch.TraSelect });
    }

    public string FormatListToSingle(IEnumerable<string> items)
    {
        var formattedItems = new List<string>();
        foreach (var item in items)
        {
            string formattedItem = Regex.Replace(item, "(?<!^)([A-Z])", " $1");
            formattedItem = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(formattedItem.ToLower());
            formattedItems.Add(formattedItem);
        }
        return string.Join(", ", formattedItems);
    }
}
