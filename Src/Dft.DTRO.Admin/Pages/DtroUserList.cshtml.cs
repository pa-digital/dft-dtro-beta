public class DtroUserListModel : PageModel
{
    private readonly IDtroUserService _dtroUserService;

    public DtroUserListModel(IDtroUserService dtroUserService)
    {
        _dtroUserService = dtroUserService;
    }

    [BindProperty(SupportsGet = true)]
    public DtroUserSearch DtroUserSearch { get; set; } = new DtroUserSearch();
    public List<DtroUser> FilteredDtroUsers { get; set; }

    public async Task OnGetAsync()
    {

        DtroUserSearch.AlwaysButtonEnabled = true;
        if (DtroUserSearch.Search != "" && DtroUserSearch.PreviousSearch == "")
        {
            DtroUserSearch.PreviousSearch = DtroUserSearch.Search;
        }

        DtroUserSearch.UpdateButtonText = "List";
        DtroUserSearch.DtroUsers = await _dtroUserService.GetDtroUsersAsync();

        if (DtroUserSearch.DtroUserIdSelect != null && DtroUserSearch.DtroUserIdSelect != Guid.Empty)
        {
            FilteredDtroUsers = DtroUserSearch.DtroUsers.Where(s => s.Id == DtroUserSearch.DtroUserIdSelect).ToList();
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(DtroUserSearch?.Search))
            {
                FilteredDtroUsers = await _dtroUserService.SearchDtroUsersAsync(DtroUserSearch.Search);
            }
            else
            {
                FilteredDtroUsers = new List<DtroUser>();
            }
        }
    }

    public async Task<IActionResult> OnPostUpdate()
    {
        OnGetAsync().Wait();
        GetParams();
        var action = Request.Form["action"];

        if (Guid.TryParse(action, out Guid dtroUserId))
        {
            if (dtroUserId != Guid.Empty)
            {
                DtroUserSearch.DtroUsers = await _dtroUserService.GetDtroUsersAsync();
                var dtroUser = DtroUserSearch.DtroUsers.Find(s => s.Id == dtroUserId);
                if (dtroUser == null) return NotFound();

                //To 
                await _dtroUserService.DeactivateDtroUserAsync(dtroUserId);
                await _dtroUserService.ActivateDtroUserAsync(dtroUserId); 
            }
            DtroUserSearch.Search = DtroUserSearch.PreviousSearch;
            DtroUserSearch.DtroUserIdSelect = DtroUserSearch.PreviousDtroUserIdSelect;
        }
        else
        {
            if (DtroUserSearch.PreviousSearch != "" && DtroUserSearch.Search == null)
            {
                DtroUserSearch.Search = DtroUserSearch.PreviousSearch;
            }
            else
            {
                DtroUserSearch.PreviousSearch = DtroUserSearch.Search;
            }
          
            DtroUserSearch.PreviousDtroUserIdSelect = DtroUserSearch.DtroUserIdSelect;
        }
        return RedirectToPage(new { DtroUserSearch.Search, DtroUserSearch.DtroUserIdSelect, DtroUserSearch.PreviousDtroUserIdSelect, DtroUserSearch.PreviousSearch });
    }

    public IActionResult OnGetRefresh()
    {
        GetParams();
        return RedirectToPage(new { DtroUserSearch.DtroUserIdSelect });
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
