public class SearchModel : PageModel
{
    public PaginatedResponse<DtroSearchResult> Dtros { get; set; }
    private readonly IDtroService _dtroService;
    private readonly IDtroUserService _dtroUserService;
    private readonly ISystemConfigService _systemConfigService;

    [BindProperty(SupportsGet = true)]
    public DtroUserSearch DtroUserSearch { get; set; } = new DtroUserSearch();

    [BindProperty(SupportsGet = true)]
    public bool AllowAddUpdate { get; set; } = false;

    public SearchModel(IDtroService dtroService, IDtroUserService dtroUserService, ISystemConfigService systemConfigService)
    {
        _dtroService = dtroService;
        _dtroUserService = dtroUserService;
        _systemConfigService = systemConfigService;
    }

    public async Task OnGetAsync()
    {
        int? useTraId = null;
        if(DtroUserSearch.DtroUserIdSelect != null && DtroUserSearch.DtroUserIdSelect != Guid.Empty)
        {
            var user = await _dtroUserService.GetDtroUserAsync(DtroUserSearch.DtroUserIdSelect.Value);
            useTraId = user.TraId;
        }
        Dtros = await _dtroService.SearchDtros(useTraId);
        DtroUserSearch.AlwaysButtonHidden = true;
        DtroUserSearch.UpdateButtonText = "Search";
        DtroUserSearch.DtroUsers = await _dtroUserService.GetDtroUsersAsync();
        DtroUserSearch.DtroUsers.RemoveAll(x => x.UserGroup == UserGroup.Consumer);
        DtroUserSearch.DtroUsers.Insert(0, new DtroUser { TraId = 0, Name = "[all]"});

       
        var users = await _dtroUserService.GetDtroUsersAsync();
        var myUser = users.FirstOrDefault(x => x.xAppId == Helper.MyXAppId());

        var systemConfig = await _systemConfigService.GetSystemConfig();

        if (myUser?.TraId != null && systemConfig.IsTest)
        {
            AllowAddUpdate = true;
        }
    }

    public IActionResult OnPostUpdate()
    {
        return RedirectToPage(new { DtroUserSearch.DtroUserIdSelect });
    }

    public IActionResult OnGetRefresh()
    {
        if (TempData.TryGetValue("DtroUserSelect", out object dtroUserSelect))
            DtroUserSearch.DtroUserIdSelect = (Guid)dtroUserSelect;

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
