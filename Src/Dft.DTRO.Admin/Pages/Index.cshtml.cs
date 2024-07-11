using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Dft.DTRO.Admin.Services;

namespace Dft.DTRO.Admin.Pages
{
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

        [BindProperty(SupportsGet = true)]
        public string PeriodOption { get; set; } = "days"; // Default value

        [BindProperty(SupportsGet = true)]
        public int NumberSelect { get; set; } = 1; // Default value

        [BindProperty(SupportsGet = true)]
        public int? TraSelect { get; set; } // Nullable int for optional selection

        public List<SwaCodeResponse> swaCodes { get; set; } = new List<SwaCodeResponse>();

        public async Task OnGetAsync()
        {
            HealthApi = await _metricsService.HealthApi();
            HealthDatabase = await _metricsService.HealthDatabase();
            TraIdMatch = await _metricsService.TraIdMatch();

            MetricRequest metricRequest = CreateRequest(GetPeriodEnum(PeriodOption), NumberSelect, TraSelect);

            var metrics = await _metricsService.MetricsForTra(metricRequest);
            Metrics = metrics ?? new MetricSummary();

            swaCodes = await _traService.GetSwaCodes();
            swaCodes.Insert(0, new SwaCodeResponse { TraId = 0, Name = "[all]" });
        }

        private Period GetPeriodEnum(string periodOption)
        {
            if (Enum.TryParse(periodOption, true, out Period period))
            {
                return period;
            }
            return Period.Days; // default to days if parsing fails
        }

        private MetricRequest CreateRequest(Period period, int number, int? traId = null)
        {
            var metricRequest = new MetricRequest
            {
                TraId = traId == 0 ? null : traId
            };

            int deductDays = number;
            if (deductDays == 0)
            {
                deductDays = 1;
            }
            if (period == Period.Days)
            {
                deductDays -=1;
            }

            metricRequest.DateFrom = period switch
            {
                Period.Days => DateTime.Now.AddDays(-deductDays),
                Period.Weeks => DateTime.Now.AddDays(-number * 7),
                Period.Months => DateTime.Now.AddMonths(-number),
                _ => DateTime.Now // default case
            };

            metricRequest.DateTo = DateTime.Now;

            return metricRequest;
        }

        public IActionResult OnPostUpdate()
        {
            return RedirectToPage(new { PeriodOption, NumberSelect, TraSelect });
        }

        public IActionResult OnGetRefresh()
        {
            // Retrieve values from TempData to set them back into the page
            if (TempData.TryGetValue("PeriodOption", out object periodOption))
                PeriodOption = periodOption as string;

            if (TempData.TryGetValue("NumberSelect", out object numberSelect))
                NumberSelect = (int)numberSelect;

            if (TempData.TryGetValue("TraSelect", out object traSelect))
                TraSelect = (int)traSelect;

            return RedirectToPage(new { PeriodOption, NumberSelect, TraSelect });
        }

        private enum Period
        {
            Days,
            Weeks,
            Months
        }
    }
}
