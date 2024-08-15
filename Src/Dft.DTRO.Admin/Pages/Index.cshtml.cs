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
        public bool HealthApi { get; set; } = true;
        public bool TraIdMatch { get; set; } = false;
        public bool HealthDatabase { get; set; } = true;

        public async Task OnGetAsync()
        {
            HealthApi = await _metricsService.HealthApi();
            HealthDatabase = await _metricsService.HealthDatabase();
            TraIdMatch = await _metricsService.TraIdMatch();

            var metricRequest = new MetricRequest
            {
                TraId = null,
                DateFrom = DateTime.Now.AddDays(-7),
                DateTo = DateTime.Now
            };
            var metrics = await _metricsService.MetricsForTra(metricRequest);
            Metrics = metrics ?? new MetricSummary();
        }
    }
}
