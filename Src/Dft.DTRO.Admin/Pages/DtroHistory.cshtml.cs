using Dft.DTRO.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dft.DTRO.Admin.Pages;

public class DtroHistoryModel : PageModel
{
    private readonly DtroService _dtroService;

    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty(SupportsGet = true)]
    public List<DtroHistoryProvisionResponse> ProvisionHistory { get; set; }

    [BindProperty(SupportsGet = true)]
    public List<DtroHistorySourceResponse> SourceHistory { get; set; }


    public DtroHistoryModel(DtroService dtroService, IConfiguration configuration)
    {
        _dtroService = dtroService;
    }

    public async Task OnGetAsync()
    {
        SourceHistory = await _dtroService.DtroSourceHistory(Id);
        ProvisionHistory = await _dtroService.DtroProvisionHistory(Id);
    }

}