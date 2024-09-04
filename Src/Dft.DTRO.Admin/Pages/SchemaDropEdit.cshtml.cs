using Dft.DTRO.Admin.Helpers;

public class SchemaDropEditModel : PageModel
{
    private readonly ISchemaService _schemaService;

    public SchemaDropEditModel(ISchemaService schemaService, IConfiguration configuration)
    {
        _schemaService = schemaService;
        ApiBaseUrl = configuration["ExternalApi:BaseUrl"];
    }

    [BindProperty(SupportsGet = true)]
    public string Version { get; set; }

    [BindProperty]
    public string ApiBaseUrl { get; set; }

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
            return HttpResponseHelper.HandleError(ex);
        }
    }
}
