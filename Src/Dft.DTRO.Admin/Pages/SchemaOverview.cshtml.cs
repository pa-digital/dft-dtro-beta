using Dft.DTRO.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class SchemaOverviewModel : PageModel
{
    private readonly ISchemaService _schemaService;

    public SchemaOverviewModel(ISchemaService schemaService)
    {
        _schemaService = schemaService;
    }

    public ViewSchemaOverview Schemas { get; set; }

    public async Task OnGetAsync()
    {
        var data = await _schemaService.GetSchemaVersionsAsync();
        Schemas = new ViewSchemaOverview() { Items = data };
    }

    public async Task<IActionResult> OnPostToggleActiveAsync(string version)
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
}
