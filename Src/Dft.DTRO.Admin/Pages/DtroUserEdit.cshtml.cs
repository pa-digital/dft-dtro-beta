public class DtroUserEditModel : PageModel
{
    private readonly IDtroUserService _traService;

    public DtroUserEditModel(IDtroUserService traService)
    {
        _traService = traService;
    }

    [BindProperty(SupportsGet = true)]
    public Guid? DtroUserId { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool IsEdit { get; set; }

    [BindProperty]
    public DtroUser DtroUser { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Search { get; set; }

    public async Task OnGetAsync()
    {
        if (IsEdit)
        {
            DtroUser = await _traService.GetDtroUserAsync(DtroUserId.Value);
        }
        else
        {
            DtroUser = new DtroUser();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var action = Request.Form["action"];
        if (action == "Cancel")
        {
            return RedirectToPage("DtroUserList", new { search = Search });
        }

        if (IsEdit)
        {
            DtroUser.Id = DtroUserId.Value;
            await _traService.UpdateDtroUserAsync(DtroUser);
        }
        else
        {
            await _traService.CreateDtroUserAsync(DtroUser);
            Search = DtroUser.Name;
        }

        return RedirectToPage("DtroUserList", new { search = Search });
    }
}
