namespace DfT.DTRO.RequestCorrelation;

public class RequestCorrelationMiddleware : IMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestCorrelationOptions _options;

    public RequestCorrelationMiddleware(ILogger<RequestCorrelationMiddleware> logger, IOptions<RequestCorrelationOptions> options)
    {
        _logger = logger;
        _options = options.Value;

        _logger.LogInformation("header {header}, prop {prop}", _options.HeaderName, _options.LogPropertyName);
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string correlationId = null;

        if (context.Request.Headers.TryGetValue(_options.HeaderName, out StringValues correlationIds))
        {
            correlationId = correlationIds.First(value => !string.IsNullOrEmpty(value)).ToString();
            _logger.LogDebug("Correlation ID from Request Header: {CorrelationId}", correlationId);
        }
        else
        {
            correlationId = Guid.NewGuid().ToString();
            context.Request.Headers.Add(_options.HeaderName, correlationId);
            _logger.LogDebug("Generated correlation Id: {CorrelationId}", correlationId);
        }

        context.Response.OnStarting(() =>
        {
            if (!context.Response.Headers.TryGetValue(_options.HeaderName, out correlationIds))
            {
                context.Response.Headers.Add(_options.HeaderName, correlationId);
            }

            return Task.CompletedTask;
        });

        await next.Invoke(context);
    }
}