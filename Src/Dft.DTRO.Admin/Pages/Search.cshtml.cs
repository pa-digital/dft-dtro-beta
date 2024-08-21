public class SearchModel : PageModel
{
    public PaginatedResponse<DtroSearchResult> Dtros { get; set; }
    private readonly IDtroService _dtroService;
    private readonly IDtroUserService _dtroUserService;

    [BindProperty(SupportsGet = true)]
    public DtroUserSearch DtroUserSearch { get; set; } = new DtroUserSearch();

    public SearchModel(IDtroService dtroService, IDtroUserService dtroUserService)
    {
        _dtroService = dtroService;
        _dtroUserService = dtroUserService;
    }

    public async Task OnGetAsync()
    {
        int? useTraId = null;
        if(DtroUserSearch.DtroUserIdSelect != null)
        {
            var user = await _dtroUserService.GetDtroUserAsync(DtroUserSearch.DtroUserIdSelect.Value);
            useTraId = user.TraId;
        }
        Dtros = await _dtroService.SearchDtros(useTraId);

        DtroUserSearch.UpdateButtonText = "Search";
        DtroUserSearch.DtroUsers = await _dtroUserService.GetDtroUsersAsync();
        DtroUserSearch.DtroUsers.Insert(0, new DtroUser { TraId = 0, Name = "[all]"});
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
