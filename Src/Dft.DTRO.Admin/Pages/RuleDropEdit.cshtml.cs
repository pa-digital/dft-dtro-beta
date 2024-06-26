using Dft.DTRO.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class RuleDropEditModel : PageModel
{
    private readonly RuleService _ruleService;

    public RuleDropEditModel(RuleService ruleService, IConfiguration configuration)
    {
        _ruleService = ruleService;
        ApiBaseUrl = configuration["ExternalApi:BaseUrl"];
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
