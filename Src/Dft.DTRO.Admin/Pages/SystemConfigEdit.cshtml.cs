using DfT.DTRO.Models.SystemConfig;

public class SystemConfigEditModel : PageModel
{
    private readonly ISystemConfigService _systemConfigService;
    private readonly ILogger<SystemConfigEditModel> _logger;

    public SystemConfigEditModel(ISystemConfigService systemConfigService, ILogger<SystemConfigEditModel> logger)
    {
        _systemConfigService = systemConfigService;
        _logger = logger;
    }

    [BindProperty]
    public SystemConfig SystemConfig { get; set; }

    public async Task OnGetAsync()
    {
        _logger.LogInformation($"Method {nameof(OnGetAsync)} called at {DateTime.UtcNow:G}");
        SystemConfig = await _systemConfigService.GetSystemConfig();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        _logger.LogInformation($"Method {nameof(OnPostAsync)} called at {DateTime.UtcNow:G}");
        var action = Request.Form["action"];
        if (action == "Cancel")
        {
            _logger.LogError($"Action {string.Join(" | ", action)} called at {DateTime.UtcNow:G}");
            return RedirectToPage("Index");
        }
        var isUpdated = await _systemConfigService.UpdateSystemConfig(SystemConfig);
        _logger.LogInformation($"Method 'UpdateSystemConfig' called at {DateTime.UtcNow:G} returned {isUpdated}");
        return RedirectToPage("Index");
    }
}
