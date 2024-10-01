namespace Dft.DTRO.Admin.Pages;
public class DtroEditModel : PageModel
{
    private readonly IDtroUserService _dtroUserService;
    private readonly IDtroService _dtroService;
    private readonly IErrHandlingService _errHandlingService;
    [BindProperty(SupportsGet = true)]
    public DtroUserSearch DtroUserSearch { get; set; } = new DtroUserSearch();

    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }

    public DtroEditModel(IDtroService dtroService, IDtroUserService traService, IErrHandlingService errHandlingService)
    {
        _dtroService = dtroService;
        _dtroUserService = traService;
        _errHandlingService = errHandlingService;
    }

    public async Task OnGetAsync()
    {
        DtroUserSearch.AlwaysButtonHidden = true;
        DtroUserSearch.DtroUsers = await _dtroUserService.GetDtroUsersAsync();
        DtroUserSearch.DtroUsers.RemoveAll(x => x.UserGroup != UserGroup.Tra);
    }


    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        try
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
        catch (Exception ex)
        {
            return _errHandlingService.HandleUiError(ex);
        }
    }
}
