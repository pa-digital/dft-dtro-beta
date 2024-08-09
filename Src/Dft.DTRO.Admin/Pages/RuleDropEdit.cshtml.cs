public class RuleDropEditModel : PageModel
{
    private readonly IRuleService _ruleService;

    public RuleDropEditModel(IRuleService ruleService)
    {
        _ruleService = ruleService;
    }

    [BindProperty(SupportsGet = true)]
    public string Version { get; set; }

    [BindProperty]
    public string ApiBaseUrl { get; set; }

    public async Task<IActionResult> OnPostAsync(IFormFile file, bool IsEdit, string Version)
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
}
