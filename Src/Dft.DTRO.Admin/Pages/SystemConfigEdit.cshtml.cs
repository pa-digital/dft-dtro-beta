using DfT.DTRO.Models.SystemConfig;

public class SystemConfigEditModel : PageModel
{
    private readonly ISystemConfigService _systemConfigService;

    public SystemConfigEditModel(ISystemConfigService systemConfigService)
    {
        _systemConfigService = systemConfigService;
    }

    [BindProperty]
    public SystemConfig SystemConfig { get; set; }

    public async Task OnGetAsync()
    {
        SystemConfig = await _systemConfigService.GetSystemConfig();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var action = Request.Form["action"];
        if (action == "Cancel")
        {
            return RedirectToPage("Index");
        }
        await _systemConfigService.UpdateSystemConfig(SystemConfig);
        return RedirectToPage("Index");
    }
}
