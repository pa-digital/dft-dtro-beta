using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog.Context;
using System.Threading.Tasks;

namespace DfT.DTRO.RequestCorrelation;

/// <summary>
/// Serilog provider extension to log correlation IDs from request headers.
/// </summary>
public class RequestCorrelationEnricherMiddleware : IMiddleware
{
    private readonly IRequestCorrelationProvider _requestCorrelationProvider;
    private readonly RequestCorrelationOptions _options;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="requestCorrelationProvider">An <see cref="IRequestCorrelationProvider"/> instance.</param>
    /// <param name="options">The configuration of the request correlation.</param>
    public RequestCorrelationEnricherMiddleware(
        IRequestCorrelationProvider requestCorrelationProvider,
        IOptions<RequestCorrelationOptions> options)
    {
        _requestCorrelationProvider = requestCorrelationProvider;
        _options = options.Value;
    }

    /// <inheritdoc/>
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