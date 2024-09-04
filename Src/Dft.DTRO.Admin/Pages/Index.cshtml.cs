using Dft.DTRO.Admin.Helpers;

namespace Dft.DTRO.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMetricsService _metricsService;
        public IndexModel(ILogger<IndexModel> logger, IMetricsService metricsService)
        {
            _logger = logger;
            _metricsService = metricsService;
        }

        public MetricSummary Metrics { get; set; } = new MetricSummary();
        public bool HealthApi { get; set; } = false;
        public bool HealthDatabase { get; set; } = false;

        public async Task<IActionResult> OnGetAsync()
        {

            try
            {
                HealthApi = await _metricsService.HealthApi();
                HealthDatabase = await _metricsService.HealthDatabase();

                var metricRequest = new MetricRequest
                {
                    DtroUserId = null,
                    DateFrom = DateTime.Now.AddDays(-7),
                    DateTo = DateTime.Now
                };
                var metrics = await _metricsService.MetricsForDtroUser(metricRequest);
                Metrics = metrics ?? new MetricSummary();

                return Page();
            }
            catch (Exception ex)
            {
                return HttpResponseHelper.HandleError(ex);
            }
        }
    }
}
