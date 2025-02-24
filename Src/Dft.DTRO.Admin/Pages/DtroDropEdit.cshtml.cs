namespace Dft.DTRO.Admin.Pages;
public class DtroDropEditModel : PageModel
{
    private readonly IDtroService _dtroService;
    private readonly IErrHandlingService _errHandlingService;
    public DtroDropEditModel(IDtroService dtroService, IConfiguration configuration, IErrHandlingService errHandlingService)
    {
        _dtroService = dtroService;
        _errHandlingService = errHandlingService;
    }

    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }

    public async Task<IActionResult> OnPostAsync(IFormFile file, bool isEdit, string id)
    {
        try
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
        catch (Exception ex)
        {
            return _errHandlingService.HandleUiError(ex);
        }
    }
}
