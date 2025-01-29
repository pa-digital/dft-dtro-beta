using Cronos;

namespace Dft.DTRO.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<Worker> _logger;

        public Worker(IHttpClientFactory httpClientFactory, ILogger<Worker> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // TODO makes the run time come from an external config value to avoid redeployment of code
            const int MINUTE = 32;
            const int HOUR = 22;
            var cronExpression = $"{MINUTE} {HOUR} * * *";
            var cronSchedule = CronExpression.Parse(cronExpression);

            while (!stoppingToken.IsCancellationRequested)
            {
                var nextRun = cronSchedule.GetNextOccurrence(DateTime.UtcNow);
                if (nextRun == null)
                {
                    _logger.LogError("No valid next run time found for the cron expression.");
                    return;
                }

                var delay = nextRun.Value - DateTime.UtcNow;
                if (delay > TimeSpan.Zero)
                {
                    _logger.LogInformation($"Next run scheduled for {nextRun.Value} UTC. Waiting for {delay.TotalMinutes} minutes.");
                    await Task.Delay(delay, stoppingToken);
                }

                if (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker stopping as cancellation was requested.");
                    break;
                }

                try
                {
                    var client = _httpClientFactory.CreateClient("DtroApiClient");
                    client.DefaultRequestHeaders.Clear();
                    // TODO don't use string literals here - figure out where these headers can be accessed implicitly
                    client.DefaultRequestHeaders.Add("X-Correlation-ID", "41ae0471-d7de-4737-907f-cab2f0089796");
                    client.DefaultRequestHeaders.Add("x-app-id", "0cf72745-a8cb-4224-ad48-73eeabafc8ab");

                    var response = await client.GetAsync("/dtros/zip", stoppingToken);

                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation($"Successfully triggered D-TRO zip generation at {DateTime.UtcNow}.");
                    }
                    else
                    {
                        _logger.LogWarning($"Failed to trigger D-TRO zip generation. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while calling the D-TRO API.");
                }
            }
        }
    }
}
