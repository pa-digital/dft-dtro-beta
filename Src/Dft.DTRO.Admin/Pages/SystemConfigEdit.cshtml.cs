namespace Dft.DTRO.Admin.Pages;

using DfT.DTRO.Models.SystemConfig;

public class SystemConfigEditModel : PageModel
{
    private readonly ISystemConfigService _systemConfigService;
    private readonly IErrHandlingService _errHandlingService;

    public SystemConfigEditModel(ISystemConfigService systemConfigService, IErrHandlingService errHandlingService)
    {
        _systemConfigService = systemConfigService;
        _errHandlingService = errHandlingService;
    }

    [BindProperty]
    public SystemConfig SystemConfig { get; set; }

    public async Task OnGetAsync()
    {
        SystemConfig = await _systemConfigService.GetSystemConfig();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var action = Request.Form["action"];
            if (action == "Cancel")
            {
                return RedirectToPage("Index");
            }
            var isUpdated = await _systemConfigService.UpdateSystemConfig(SystemConfig);
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            return _errHandlingService.HandleUiError(ex);
        }
    }
}
