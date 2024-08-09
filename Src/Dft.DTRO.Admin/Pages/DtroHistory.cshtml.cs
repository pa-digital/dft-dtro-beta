namespace Dft.DTRO.Admin.Pages;

public class DtroHistoryModel : PageModel
{
    private readonly IDtroService _dtroService;

    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty(SupportsGet = true)]
    public List<DtroHistoryProvisionResponse> ProvisionHistory { get; set; }

    [BindProperty(SupportsGet = true)]
    public List<DtroHistorySourceResponse> SourceHistory { get; set; }


    public DtroHistoryModel(IDtroService dtroService)
    {
        _dtroService = dtroService;
    }

    public async Task OnGetAsync()
    {
        SourceHistory = await _dtroService.DtroSourceHistory(Id);
        ProvisionHistory = await _dtroService.DtroProvisionHistory(Id);
    }

}