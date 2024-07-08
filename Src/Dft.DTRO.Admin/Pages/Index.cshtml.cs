using System.Runtime.Serialization;
using Dft.DTRO.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dft.DTRO.Admin.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IMetricsService _metricsService;
    private readonly ITraService _traService;
    public IndexModel(ILogger<IndexModel> logger, IMetricsService metricsService, ITraService traService)
    {
        _logger = logger;
        _metricsService = metricsService;
        _traService = traService;
    }

    public MetricSummary Metrics { get; set; } = new MetricSummary();
    public bool HealthApi { get; set; } = true;

    public bool TraIdMatch { get; set; } = false;

    public bool HealthDatabase { get; set; } = true;


    [BindProperty]
    public string PeriodOption { get; set; }

    [BindProperty]
    public int NumberSelect { get; set; }

    [BindProperty]
    public int TraSelect { get; set; }

    public List<LookupResponse> LookupResponses { get; set; } = new List<LookupResponse>();


    public async Task OnGetAsync()
    {
        HealthApi = await _metricsService.HealthApi();
        HealthDatabase = await _metricsService.HealthDatabase();
        TraIdMatch = await _metricsService.TraIdMatch();

        var metricRequest = new MetricRequest();
        metricRequest.TraId = null;
        metricRequest.DateFrom = DateTime.Now.AddDays(-200);
        metricRequest.DateTo = DateTime.Now.AddDays(1);


        var metrics = await _metricsService.MetricsForTra(metricRequest);
        if (metrics == null)
        {
            metrics = new MetricSummary();
        }
        else
        {
            Metrics = metrics;
        }
        PopulateLookupResponses();
    }

    private async void PopulateLookupResponses()
    {
        LookupResponses = await _traService.GetTraLookup();
        LookupResponses.Insert(0,new LookupResponse { Id = 0, Name = "(all)" });
      
        // Add more options as needed
    }

    public IActionResult OnPostUpdate()
    {
        // Access the selected values here
        // Example: Logging the values to console
        Console.WriteLine($"PeriodOption: {PeriodOption}");
        Console.WriteLine($"NumberSelect: {NumberSelect}");
        Console.WriteLine($"TraSelect: {TraSelect}");

        // Example: Redirecting to another page after form submission
        return RedirectToPage("/Index"); // Redirect to Index page
    }
}

