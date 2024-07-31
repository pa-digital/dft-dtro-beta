using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using Dft.DTRO.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class TraListModel : PageModel
{
    private readonly ITraService _traService;

    public TraListModel(ITraService traService)
    {
        _traService = traService;
    }

    [BindProperty(SupportsGet = true)]
    public TraSearch TraSearch { get; set; } = new TraSearch();
    public List<SwaCodeResponse> FilteredSwaCodes { get; set; }
    
    public async Task OnGetAsync()
    {
  
        TraSearch.AlwaysButtonEnabled = true;
        TraSearch.UpdateButtonText = "List";
        TraSearch.SwaCodes = await _traService.GetSwaCodes();

        if (TraSearch.TraSelect != null && TraSearch.TraSelect != 0)
        {
            FilteredSwaCodes = TraSearch.SwaCodes.Where(s => s.TraId == TraSearch.TraSelect).ToList();
        }
        else
        {
            if (TraSearch.Search.Trim() != string.Empty)
            {
                FilteredSwaCodes = await _traService.SearchSwaCodes(TraSearch.Search);
            }
            else
            {
                FilteredSwaCodes = new List<SwaCodeResponse>();
            }
        }
    }

    public async Task<IActionResult> OnPostUpdate()
    {
        GetParams();
        var action = Request.Form["action"];

        if (int.TryParse(action,out int traId))
        {
            if (traId != 0)
            {
                TraSearch.SwaCodes = await _traService.GetSwaCodes();
                var tra = TraSearch.SwaCodes.Find(s => s.TraId == traId);
                if (tra == null) return NotFound();

                if (tra.IsActive)
                {
                    await _traService.DeactivateTraAsync(traId);
                }
                else
                {
                    await _traService.ActivateTraAsync(traId);
                }
            }
        }
  
        return RedirectToPage(new { TraSearch.Search,TraSearch.TraSelect });
    }

    public IActionResult OnGetRefresh()
    {
        GetParams();
        return RedirectToPage(new { TraSearch.TraSelect });
    }

    private void GetParams()
    {
        if (TempData.TryGetValue("TraSelect", out object traSelect))
        {
            TraSearch.TraSelect = (int)traSelect;
        }
        if (TempData.TryGetValue("Search", out object search))
        {
            TraSearch.Search = (string)search;
        }
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
