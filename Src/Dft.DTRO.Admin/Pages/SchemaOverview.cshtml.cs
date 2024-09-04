using Dft.DTRO.Admin.Helpers;

public class SchemaOverviewModel : PageModel
{
    private readonly ISchemaService _schemaService;

    public SchemaOverviewModel(ISchemaService schemaService)
    {
        _schemaService = schemaService;
    }

    public ViewSchemaOverview Schemas { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var data = await _schemaService.GetSchemaVersionsAsync();
            Schemas = new ViewSchemaOverview() { Items = data };
            return Page();
        }
        catch (Exception ex)
        {
            return HttpResponseHelper.HandleError(ex);
        }
    }

    public async Task<IActionResult> OnPostToggleActiveAsync(string version)
    {
        try
        {
            OnGetAsync().Wait();
            var schema = Schemas.Items.Find(s => s.SchemaVersion == version);
            if (schema == null) return NotFound();

            if (schema.IsActive)
            {
                await _schemaService.DeactivateSchemaAsync(version);
            }
            else
            {
                await _schemaService.ActivateSchemaAsync(version);
            }

            return RedirectToPage();
        }
        catch (Exception ex)
        {
            return HttpResponseHelper.HandleError(ex);
        }
    }
}
