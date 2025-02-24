namespace Dft.DTRO.Admin.Pages;
public class SchemaOverviewModel : PageModel
{
    private readonly ISchemaService _schemaService;
    private readonly IErrHandlingService _errHandlingService;
    public SchemaOverviewModel(ISchemaService schemaService, IErrHandlingService errHandlingService)
    {
        _schemaService = schemaService;
        _errHandlingService = errHandlingService;
    }

    public ViewSchemaOverview Schemas { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            List<SchemaTemplateOverview> data = await _schemaService.GetSchemaVersionsAsync();
            Schemas = new ViewSchemaOverview() { Items = data };
            return Page();
        }
        catch (Exception ex)
        {
            return _errHandlingService.HandleUiError(ex);
        }
    }

    public async Task<IActionResult> OnPostToggleActiveAsync(string version)
    {
        try
        {
            OnGetAsync().Wait();
            SchemaTemplateOverview? schema = Schemas.Items.Find(s => s.SchemaVersion == version);
            if (schema == null)
            {
                return NotFound();
            }

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
            return _errHandlingService.HandleUiError(ex);
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(string version)
    {
        try
        {
            OnGetAsync().Wait();
            var schema = Schemas.Items.Find(it => it.SchemaVersion == version);
            if (schema == null)
            {
                return NotFound();
            }

            if (schema.IsActive)
            {
                return _errHandlingService.HandleUiError(new InvalidOperationException($"Schema with version '{version}' is active. Make sure is deactivated before trying to delete it."));
            }

            await _schemaService.DeleteSchemaAsync(version);

            return RedirectToPage();
        }
        catch (Exception ex)
        {
            return _errHandlingService.HandleUiError(ex);
        }
    }
}
