namespace Dft.DTRO.Admin.Pages;
public class DtroUserEditModel : PageModel
{
    private readonly IDtroUserService _dtroUserService;
    private readonly ISystemConfigService _systemConfigService;
    private readonly IErrHandlingService _errHandlingService;

    public DtroUserEditModel(IDtroUserService dtroUserService, ISystemConfigService systemConfigService, IErrHandlingService errHandlingService)
    {
        _systemConfigService = systemConfigService;
        _dtroUserService = dtroUserService;
        _errHandlingService = errHandlingService;
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

    public async Task<IActionResult> OnGetAsync()
    {
        try
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
            return Page();
        }
        catch (Exception ex)
        {
            return _errHandlingService.HandleUiError(ex);
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
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
        catch (Exception ex)
        {
            return _errHandlingService.HandleUiError(ex);
        }
    }
}
