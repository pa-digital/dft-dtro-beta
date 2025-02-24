namespace Dft.DTRO.Admin.Pages;

public class DtroUserListModel : PageModel
{
    private readonly IDtroUserService _dtroUserService;
    private readonly IErrHandlingService _errHandlingService;

    public DtroUserListModel(IDtroUserService dtroUserService, IErrHandlingService errHandlingService)
    {
        _dtroUserService = dtroUserService;
        _errHandlingService = errHandlingService;
    }

    [BindProperty(SupportsGet = true)]
    public DtroUserSearch DtroUserSearch { get; set; } = new DtroUserSearch();
    public List<DtroUser> FilteredDtroUsers { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
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
            return Page();
        }
        catch (Exception ex)
        {
            return _errHandlingService.HandleUiError(ex);
        }
    }

    public async Task<IActionResult> OnPostUpdate()
    {
        try
        {
            OnGetAsync().Wait();
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
        catch (Exception ex)
        {
            return _errHandlingService.HandleUiError(ex);
        }
    }

    public IActionResult OnGetRefresh()
    {
        return RedirectToPage(new { DtroUserSearch.DtroUserIdSelect });
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
