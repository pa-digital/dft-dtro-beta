
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

    public async Task<IActionResult> OnPostAsync(IFormFile file, bool IsEdit, string Version)
    {
        try
        {
            if (IsEdit)
            {
                await _schemaService.UpdateSchemaAsync(Version, file);
            }
            else
            {
                await _schemaService.CreateSchemaAsync(Version, file);
            }
            return RedirectToPage("SchemaOverview");
        }
        catch (Exception ex)
        {
            return _errHandlingService.HandleUiError(ex);
        }
    }
}
