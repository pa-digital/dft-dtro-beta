namespace Dft.DTRO.Admin.Pages
{
    public class MetricsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMetricsService _metricsService;
        private readonly IDtroUserService _dtroUserService;

        public MetricsModel(ILogger<IndexModel> logger, IMetricsService metricsService, IDtroUserService dtroUserService)
        {
            _logger = logger;
            _metricsService = metricsService;
            _dtroUserService = dtroUserService;
        }

        public MetricSummary Metrics { get; set; } = new MetricSummary();

        [BindProperty(SupportsGet = true)]
        public string PeriodOption { get; set; } = "months";

        [BindProperty(SupportsGet = true)]
        public int NumberSelect { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public DtroUserSearch DtroUserSearch  { get; set; } = new DtroUserSearch();

        public async Task OnGetAsync()
        {
            var metricRequest = CreateRequest(GetPeriodEnum(PeriodOption), NumberSelect, DtroUserSearch.DtroUserIdSelect);
            var metrics = await _metricsService.MetricsForDtroUser(metricRequest);
            Metrics = metrics ?? new MetricSummary();
            DtroUserSearch.UpdateButtonText = "Update";
            DtroUserSearch.DtroUsers = await _dtroUserService.GetDtroUsersAsync();
            DtroUserSearch.DtroUsers.Insert(0, new DtroUser {Id = Guid.Empty, Name = "[all]" });
        }

        private Period GetPeriodEnum(string periodOption)
        {
            if (Enum.TryParse(periodOption, true, out Period period))
            {
                return period;
            }
            return Period.Days;
        }

        private MetricRequest CreateRequest(Period period, int number, Guid? dtroUserId = null)
        {
            var metricRequest = new MetricRequest
            {
                DtroUserId = dtroUserId == null ? null : dtroUserId
            };

            int deductDays = number;
            if (deductDays == 0)
            {
                deductDays = 1;
            }
            if (period == Period.Days)
            {
                deductDays -= 1;
            }

            metricRequest.DateFrom = period switch
            {
                Period.Days => DateTime.Now.AddDays(-deductDays),
                Period.Weeks => DateTime.Now.AddDays(-number * 7),
                Period.Months => DateTime.Now.AddMonths(-number),
                _ => DateTime.Now
            };

            metricRequest.DateTo = DateTime.Now;

            return metricRequest;
        }

        public IActionResult OnPostUpdate()
        {
            return RedirectToPage(new { PeriodOption, NumberSelect, DtroUserSearch.DtroUserIdSelect });
        }

        public IActionResult OnGetRefresh()
        {
            if (TempData.TryGetValue("PeriodOption", out object periodOption))
                PeriodOption = periodOption as string;

            if (TempData.TryGetValue("NumberSelect", out object numberSelect))
                NumberSelect = (int)numberSelect;

            if (TempData.TryGetValue("DtroUserSelect", out object dtroUserSelect))
                DtroUserSearch.DtroUserIdSelect = (Guid)dtroUserSelect;

            return RedirectToPage(new { PeriodOption, NumberSelect, DtroUserSearch.DtroUserIdSelect });
        }

        private enum Period
        {
            Days,
            Weeks,
            Months
        }
    }
}
