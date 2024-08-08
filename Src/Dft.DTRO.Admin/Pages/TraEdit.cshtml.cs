using Dft.DTRO.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

public class TraEditModel : PageModel
{
    private readonly ITraService _traService;

    public TraEditModel(ITraService traService)
    {
        _traService = traService;
    }

    [BindProperty(SupportsGet = true)]
    public int? TraId { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool IsEdit { get; set; }

    [BindProperty]
    public SwaCode SwaCode { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Search { get; set; }

    public async Task OnGetAsync()
    {
        if (IsEdit)
        {
            SwaCode = await _traService.GetSwaCode((int)TraId);
        }
        else
        {
            SwaCode = new SwaCode();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var action = Request.Form["action"];
        if (action == "Cancel")
        {
            return RedirectToPage("TraList", new { search = Search });
        }

        if (IsEdit)
        {
            await _traService.UpdateTraAsync(SwaCode);
        }
        else
        {
            await _traService.CreateTraAsync(SwaCode);
            Search = SwaCode.Name;
        }

        return RedirectToPage("TraList", new { search = Search });
    }

    public IActionResult OnPostCancel()
    {
        return RedirectToPage("TraList", new { search = Search });
    }
}
