namespace Dft.DTRO.Admin.Pages;
public class SchemaDropEditModel : PageModel
{
    private readonly ISchemaService _schemaService;
    private readonly IErrHandlingService _errHandlingService;
    public SchemaDropEditModel(ISchemaService schemaService, IConfiguration configuration, IErrHandlingService errHandlingService)
    {
        _schemaService = schemaService;
        _errHandlingService = errHandlingService;
    }

    [BindProperty(SupportsGet = true)]
    public string Version { get; set; }

    public async Task<IActionResult> OnPostAsync(IFormFile file, bool isEdit, string version)
    {
        try
        {
            if (isEdit)
            {
                await _schemaService.UpdateSchemaAsync(version, file);
            }
            else
            {
                await _schemaService.CreateSchemaAsync(version, file);
            }
            return RedirectToPage("SchemaOverview");
        }
        catch (Exception ex)
        {
            return _errHandlingService.HandleUiError(ex);
        }
    }
}
