using System.Net.Http;
using Dft.DTRO.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class DtroDropEditModel : PageModel
{
    private readonly DtroService _dtroService;

    public DtroDropEditModel(DtroService dtroService, IConfiguration configuration)
    {
        _dtroService = dtroService;
        ApiBaseUrl = configuration["ExternalApi:BaseUrl"];
    }

    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }

    [BindProperty]
    public string ApiBaseUrl { get; set; }

    public async Task<IActionResult> OnPostAsync(IFormFile file, bool isEdit, string id)
    {

        if (isEdit)
        {
            await _dtroService.UpdateDtroAsync(Guid.Parse(id), file);
        }
        else
        {
            await _dtroService.CreateDtroAsync(file);
        }
        return RedirectToPage("Search");
    }

    public async Task<IActionResult> OnPostReassign(Guid id, int assignToTraId)
    {
        var res = await _dtroService.ReassignDtroAsync(id, assignToTraId);
        return res;
    }

    public class ReassignRequest
    {
        public Guid Id { get; set; }
        public int AssignToTraId { get; set; }
    }
}
