public class TraListModel : PageModel
{
    private readonly ITraService _traService;

    public TraListModel(ITraService traService)
    {
        _traService = traService;
    }

    [BindProperty(SupportsGet = true)]
    public TraSearch TraSearch { get; set; } = new TraSearch();
    public List<SwaCode> FilteredSwaCodes { get; set; }

    public async Task OnGetAsync()
    {

        TraSearch.AlwaysButtonEnabled = true;
        if (TraSearch.Search != "" && TraSearch.PreviousSearch == "")
        {
            TraSearch.PreviousSearch = TraSearch.Search;
        }


        

        TraSearch.UpdateButtonText = "List";
        TraSearch.SwaCodes = await _traService.GetSwaCodes();

        if (TraSearch.TraSelect != null && TraSearch.TraSelect != 0)
        {
            FilteredSwaCodes = TraSearch.SwaCodes.Where(s => s.TraId == TraSearch.TraSelect).ToList();
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(TraSearch?.Search))
            {
                FilteredSwaCodes = await _traService.SearchSwaCodes(TraSearch.Search);
            }
            else
            {
                FilteredSwaCodes = new List<SwaCode>();
            }
        }
    }

    public async Task<IActionResult> OnPostUpdate()
    {
        OnGetAsync().Wait();
        GetParams();
        var action = Request.Form["action"];

        if (int.TryParse(action, out int traId))
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
            TraSearch.Search = TraSearch.PreviousSearch;
            TraSearch.TraSelect = TraSearch.PreviousTraSelect;
        }
        else
        {
            if (TraSearch.PreviousSearch != "" && TraSearch.Search == null)
            {
                TraSearch.Search = TraSearch.PreviousSearch;
            }
            else
            {
                TraSearch.PreviousSearch = TraSearch.Search;
            }
          
            TraSearch.PreviousTraSelect = TraSearch.TraSelect;
        }
        return RedirectToPage(new { TraSearch.Search, TraSearch.TraSelect, TraSearch.PreviousTraSelect, TraSearch.PreviousSearch });
    }

    public IActionResult OnGetRefresh()
    {
        GetParams();
        return RedirectToPage(new { TraSearch.TraSelect });
    }

    private void GetParams()
    {
        //if (Request.Form.TryGetValue("TraSearch.TraSelect", out StringValues traSelectObj))
        //{
        //    TraSearch.TraSelect = Convert.ToInt32(traSelectObj[0]);
        //}
        //if (Request.Form.TryGetValue("TraSearch.Search", out StringValues searchObj))
        //{
        //    TraSearch.Search = searchObj[0];
        //}

        //if (Request.Form.TryGetValue("TraSearch.PreviousTraSelect", out StringValues previousTraSelectObj))
        //{
        //    if (int.TryParse(previousTraSelectObj[0], out int previousTraSelect))
        //    {
        //        TraSearch.PreviousTraSelect = previousTraSelect;
        //    }

        //}
        //if (Request.Form.TryGetValue("TraSearch.PreviousSearch", out StringValues previousSearchObj))
        //{

        //    TraSearch.PreviousSearch = previousSearchObj[0];
        //}

        //if (TempData.TryGetValue("Search", out object search))
        //    TraSearch.Search = (string)search;
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
