public class RuleDropEditModel : PageModel
{
    private readonly IRuleService _ruleService;
    private readonly IErrHandlingService _errHandlingService;
    public RuleDropEditModel(IRuleService ruleService, IErrHandlingService errHandlingService)
    {
        _ruleService = ruleService;
        _errHandlingService = errHandlingService;
    }

    [BindProperty(SupportsGet = true)]
    public string Version { get; set; }

    [BindProperty]
    public string ApiBaseUrl { get; set; }

    public async Task<IActionResult> OnPostAsync(IFormFile file, bool IsEdit, string Version)
    {
        try
        {
            if (IsEdit)
            {
                await _ruleService.UpdateRuleAsync(Version, file);
            }
            else
            {
                await _ruleService.CreateRuleAsync(Version, file);
            }
            return RedirectToPage("SchemaOverview");
        }
        catch (Exception ex)
        {
            return _errHandlingService.HandleUiError(ex);
        }
    }
}
