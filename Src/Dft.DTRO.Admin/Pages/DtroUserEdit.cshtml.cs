public class DtroUserEditModel : PageModel
{
    private readonly IDtroUserService _dtroUserService;
    private readonly ISystemConfigService _systemConfigService;
    public DtroUserEditModel(IDtroUserService dtroUserService, ISystemConfigService systemConfigService)
    {
        _systemConfigService = systemConfigService;
        _dtroUserService = dtroUserService;
    }

    [BindProperty(SupportsGet = true)]
    public Guid? DtroUserId { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool IsEdit { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool IsTestSystem { get; set; }

    [BindProperty]
    public DtroUser DtroUser { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Search { get; set; }

    public async Task OnGetAsync()
    {
        var systemConfig = await _systemConfigService.GetSystemConfig();
        IsTestSystem = systemConfig.IsTest;

        if (IsEdit)
        {
            DtroUser = await _dtroUserService.GetDtroUserAsync(DtroUserId.Value);
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
            await _dtroUserService.UpdateDtroUserAsync(DtroUser);
        }
        else
        {
            await _dtroUserService.CreateDtroUserAsync(DtroUser);
            Search = DtroUser.Name;
        }

        return RedirectToPage("DtroUserList", new { search = Search });
    }
}
