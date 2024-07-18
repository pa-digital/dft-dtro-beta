using Serilog.Context;

namespace DfT.DTRO.RequestCorrelation;

public class RequestCorrelationEnricherMiddleware : IMiddleware
{
    private readonly IRequestCorrelationProvider _requestCorrelationProvider;
    private readonly RequestCorrelationOptions _options;

    public RequestCorrelationEnricherMiddleware(
        IRequestCorrelationProvider requestCorrelationProvider,
        IOptions<RequestCorrelationOptions> options)
    {
        _requestCorrelationProvider = requestCorrelationProvider;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (_requestCorrelationProvider.CorrelationId is string correlationId)
        {
            using (LogContext.PushProperty(_options.LogPropertyName, correlationId))
            {
                await next.Invoke(context);
            }
        }
        else
        {
            await next.Invoke(context);
        }
    }
}