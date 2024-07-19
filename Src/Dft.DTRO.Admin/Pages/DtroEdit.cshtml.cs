using Dft.DTRO.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class DtroEditModel : PageModel
{
    private readonly IDtroService _dtroService;

    public DtroEditModel(IDtroService dtroService, IConfiguration configuration)
    {
        _dtroService = dtroService;
        ApiBaseUrl = configuration["ExternalApi:BaseUrl"];
    }

    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }

    [BindProperty]
    public string ApiBaseUrl { get; set; }

    public async Task<IActionResult> OnPostAsync(Guid id, int assignToTraId)
    {
        await _dtroService.ReassignDtroAsync(id, assignToTraId);
        return RedirectToPage("Search");
    }
}
