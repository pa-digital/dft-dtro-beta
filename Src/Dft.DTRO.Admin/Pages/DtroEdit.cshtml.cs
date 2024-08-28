public class DtroEditModel : PageModel
{
    private readonly IDtroUserService _dtroUserService;
    private readonly IDtroService _dtroService;
    [BindProperty(SupportsGet = true)]
    public DtroUserSearch DtroUserSearch { get; set; } = new DtroUserSearch();

    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }

    public DtroEditModel(IDtroService dtroService, IDtroUserService traService)
    {
        _dtroService = dtroService;
        _dtroUserService = traService;
    }

    public async Task OnGetAsync()
    {
        DtroUserSearch.UpdateButtonText = "Assign";
        DtroUserSearch.DtroUsers = await _dtroUserService.GetDtroUsersAsync();
    }


    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        if (TempData.TryGetValue("DtroUserSelect", out object dtroUserSelect))
        {
            DtroUserSearch.DtroUserIdSelect = (Guid)dtroUserSelect;
        }

        if (DtroUserSearch.DtroUserIdSelect != null)
        {
            await _dtroService.ReassignDtroAsync(id, DtroUserSearch.DtroUserIdSelect.Value);
        }

        return RedirectToPage("Search");
    }
}
