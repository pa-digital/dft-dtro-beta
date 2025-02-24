namespace Dft.DTRO.Admin.Pages;

public class MetricsModel : PageModel
{
    private readonly ILogger<MetricsModel> _logger;
    private readonly IMetricsService _metricsService;
    private readonly IDtroUserService _dtroUserService;
    private readonly IErrHandlingService _errHandlingService;
    public MetricsModel(ILogger<MetricsModel> logger, IMetricsService metricsService, IDtroUserService dtroUserService, IErrHandlingService errHandlingService)
    {
        _logger = logger;
        _metricsService = metricsService;
        _dtroUserService = dtroUserService;
        _errHandlingService = errHandlingService;
    }
    public MetricSummary Metrics { get; set; } = new MetricSummary();

    [BindProperty(SupportsGet = true)]
    public string UserGroup { get; set; } = "Admin";

    [BindProperty(SupportsGet = true)]
    public string PeriodOption { get; set; } = "months";

    [BindProperty(SupportsGet = true)]
    public int NumberSelect { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public DtroUserSearch DtroUserSearch { get; set; } = new DtroUserSearch();

    public async Task OnGetAsync()
    {
        await LoadMetricsDataAsync();
    }

    private async Task LoadMetricsDataAsync()
    {
        var metricRequest = await CreateRequestAsync(GetPeriodEnum(PeriodOption), NumberSelect, DtroUserSearch.DtroUserIdSelect, UserGroup);
        var metrics = await _metricsService.MetricsForDtroUser(metricRequest);
        Metrics = metrics ?? new MetricSummary();
        DtroUserSearch.UpdateButtonText = "Update";
        DtroUserSearch.AlwaysButtonHidden = true;
        DtroUserSearch.DtroUsers = await _dtroUserService.GetDtroUsersAsync();

        UserGroup selectedUserGroup;
        if (Enum.TryParse(UserGroup, out selectedUserGroup))
        {
            DtroUserSearch.DtroUsers.RemoveAll(x => x.UserGroup != selectedUserGroup);
        }
        DtroUserSearch.DtroUsers.Insert(0, new DtroUser { Id = Guid.Empty, Name = "[all]" });

    }

    private Period GetPeriodEnum(string periodOption)
    {
        if (Enum.TryParse(periodOption, true, out Period period))
        {
            return period;
        }
        return Period.Days;
    }

    private async Task<MetricRequest> CreateRequestAsync(Period period, int number, Guid? dtroUserId = null, string userGroupString = "Tra")
    {
        if (dtroUserId != null && dtroUserId != Guid.Empty)
        {
            var user = await _dtroUserService.GetDtroUserAsync((Guid)dtroUserId);
            userGroupString = user?.UserGroup.ToString() ?? "Tra";
        }

        UserGroup = userGroupString;

        UserGroup userGroup;
        Enum.TryParse(userGroupString, true, out userGroup);

        var metricRequest = new MetricRequest
        {
            DtroUserId = dtroUserId,
            UserGroup = userGroup // Include UserGroup in the request
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

    public async Task<IActionResult> OnPostAsync(bool? exportCsv = null)
    {
        try
        {
            if (exportCsv == true)
            {
                var metricRequest = await CreateRequestAsync(GetPeriodEnum(PeriodOption), NumberSelect, DtroUserSearch.DtroUserIdSelect, UserGroup);
                var fullMetrics = await _metricsService.FullMetricsForDtroUser(metricRequest);

                if (fullMetrics == null)
                {
                    return Page();
                }

                var csvContent = GenerateCsvContent(fullMetrics);
                var fileName = "MetricsData.csv";
                return File(Encoding.UTF8.GetBytes(csvContent), "text/csv", fileName);
            }

            await LoadMetricsDataAsync();
            return RedirectToPage(new { PeriodOption, NumberSelect, DtroUserSearch.DtroUserIdSelect, UserGroup });
        }
        catch (Exception ex)
        {
            return _errHandlingService.HandleUiError(ex);
        }

    }

    private string GenerateCsvContent(List<FullMetricSummary> metricList)
    {
        var sb = new StringBuilder();

        if (UserGroup == "Admin")
        {
            sb.AppendLine("User Name,For Date,Submission,Submission Failure,Deletion,Search ,Event,System Failure");
        }
        else if (UserGroup == "Tra")
        {
            sb.AppendLine("User Name,For Date,Submission,Submission Failure,Deletion,System Failure");
        }
        else if (UserGroup == "Consumer")
        {
            sb.AppendLine("User Name,For Date, Search ,Event,System Failure");
        }

        foreach (var metric in metricList)
        {
            if (UserGroup == "Admin")
            {
                sb.Append($"{metric.UserName}");
                sb.Append($",{metric.ForDate}");
                sb.Append($",{metric.SubmissionCount}");
                sb.Append($",{metric.SubmissionFailureCount}");
                sb.Append($",{metric.DeletionCount}");
                sb.Append($",{metric.SearchCount}");
                sb.Append($",{metric.EventCount}");
                sb.AppendLine($",{metric.SystemFailureCount}");
            }
            else if (UserGroup == "Tra")
            {
                sb.Append($"{metric.UserName}");
                sb.Append($",{metric.ForDate}");
                sb.Append($",{metric.SubmissionCount}");
                sb.Append($",{metric.SubmissionFailureCount}");
                sb.Append($",{metric.DeletionCount}");
                sb.AppendLine($",{metric.SystemFailureCount}");
            }
            else if (UserGroup == "Consumer")
            {
                sb.Append($"{metric.UserName}");
                sb.Append($",{metric.ForDate}");
                sb.Append($",{metric.SearchCount}");
                sb.Append($",{metric.EventCount}");
                sb.AppendLine($",{metric.SystemFailureCount}");
            }
        }
        return sb.ToString();
    }

    private enum Period
    {
        Days,
        Weeks,
        Months
    }
}