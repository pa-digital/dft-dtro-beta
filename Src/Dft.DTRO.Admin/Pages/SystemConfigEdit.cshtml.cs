using DfT.DTRO.Models.SystemConfig;

public class SystemConfigEditModel : PageModel
{
    private readonly ISystemConfigService _systemConfigService;
    private readonly IErrHandlingService _errHandlingService;
    private readonly ILogger<SystemConfigEditModel> _logger;
    public SystemConfigEditModel(ISystemConfigService systemConfigService, ILogger<SystemConfigEditModel> logger, IErrHandlingService errHandlingService)
    {
        _systemConfigService = systemConfigService;
        _errHandlingService = errHandlingService;
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
        try
        {
            _logger.LogInformation($"Method {nameof(OnPostAsync)} called at {DateTime.UtcNow:G}");
            var action = Request.Form["action"];
            if (action == "Cancel")
            {
                return RedirectToPage("Index");
            }
            await _systemConfigService.UpdateSystemConfig(SystemConfig);
            _logger.LogInformation($"Method '{nameof(_systemConfigService.UpdateSystemConfig)}' called at {DateTime.UtcNow:G} returned {isUpdated}");
            _logger.LogInformation($"x-App-Id\t'{SystemConfig.xAppId}' called at {DateTime.UtcNow:G}");
            _logger.LogInformation($"Current User\t'{SystemConfig.CurrentUserName}' called at {DateTime.UtcNow:G}");
            _logger.LogInformation($"Is in test?\t'{SystemConfig.IsTest}' called at {DateTime.UtcNow:G}");
            _logger.LogInformation($"System name\t'{SystemConfig.SystemName}' called at {DateTime.UtcNow:G}");
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            return _errHandlingService.HandleUiError(ex);
        }
    }
}
