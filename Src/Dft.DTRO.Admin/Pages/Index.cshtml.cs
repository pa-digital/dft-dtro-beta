namespace Dft.DTRO.Admin.Pages //TODO: Main page, update default user to DfT, Remove Privacy
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMetricsService _metricsService;
        private readonly IErrHandlingService _errHandlingService;
        public IndexModel(ILogger<IndexModel> logger, IMetricsService metricsService, IErrHandlingService errHandlingService)
        {
            _logger = logger;
            _metricsService = metricsService;
            _errHandlingService = errHandlingService;
        }

        public MetricSummary Metrics { get; set; } = new MetricSummary();
        public bool HealthApi { get; set; } = false;
        public bool HealthDatabase { get; set; } = false;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                HealthApi = await _metricsService.HealthApi();
            }
            catch (Exception)
            {
            }

            try
            {
                HealthDatabase = await _metricsService.HealthDatabase();
            }
            catch (Exception)
            {
            }

            try
            {
                var metricRequest = new MetricRequest
                {
                    DtroUserId = null,
                    DateFrom = DateTime.Now.AddDays(-7),
                    DateTo = DateTime.Now
                };
                var metrics = await _metricsService.MetricsForDtroUser(metricRequest);
                Metrics = metrics ?? new MetricSummary(); 
            }
            catch (Exception)
            {
            }
            return Page();
        }
    }
}
