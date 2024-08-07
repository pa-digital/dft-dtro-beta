public class DtroDropEditModel : PageModel
{
    private readonly IDtroService _dtroService;

    public DtroDropEditModel(IDtroService dtroService, IConfiguration configuration)
    {
        _dtroService = dtroService;
        ApiBaseUrl = configuration["ExternalApi:BaseUrl"];
    }

    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }

    [BindProperty]
    public string ApiBaseUrl { get; set; }

    public async Task<IActionResult> OnPostAsync(IFormFile file, bool isEdit, string id)
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
}
