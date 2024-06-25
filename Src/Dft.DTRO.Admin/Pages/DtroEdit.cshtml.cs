using System;
using System.Threading.Tasks;
using Dft.DTRO.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

public class DtroEditModel : PageModel
{
    private readonly DtroService _dtroService;

    public DtroEditModel(DtroService dtroService, IConfiguration configuration)
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
        var res = await _dtroService.ReassignDtroAsync(id, assignToTraId);
        return res;
    }
}
