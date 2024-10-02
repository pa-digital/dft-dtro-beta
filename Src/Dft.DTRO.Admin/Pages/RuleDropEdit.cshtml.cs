namespace Dft.DTRO.Admin.Pages;
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

    public async Task<IActionResult> OnPostAsync(IFormFile file, bool isEdit, string version)
    {
        try
        {
            if (isEdit)
            {
                await _ruleService.UpdateRuleAsync(version, file);
            }
            else
            {
                await _ruleService.CreateRuleAsync(version, file);
            }
            return RedirectToPage("SchemaOverview");
        }
        catch (Exception ex)
        {
            return _errHandlingService.HandleUiError(ex);
        }
    }
}
