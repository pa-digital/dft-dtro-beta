public class DtroEditModel : PageModel
{
    private readonly ITraService _traService;
    private readonly IDtroService _dtroService;
    [BindProperty(SupportsGet = true)]
    public TraSearch TraSearch { get; set; } = new TraSearch();

    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }

    public DtroEditModel(IDtroService dtroService, ITraService traService)
    {
        _dtroService = dtroService;
        _traService = traService;
    }

    public async Task OnGetAsync()
    {
        TraSearch.UpdateButtonText = "Assign";
        TraSearch.SwaCodes = await _traService.GetSwaCodes();
    }


    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        if (TempData.TryGetValue("TraSelect", out object traSelect))
        {
            TraSearch.TraSelect = (int)traSelect;
        }

        if (TraSearch.TraSelect != null)
        {
            await _dtroService.ReassignDtroAsync(id, (int)TraSearch.TraSelect);
        }

        return RedirectToPage("Search");
    }
}
