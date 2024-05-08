using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DfT.DTRO.RequestCorrelation;

/// <summary>
/// A middleware function for logging correlation ID header values.
/// </summary>
public class RequestCorrelationMiddleware : IMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestCorrelationOptions _options;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="logger">An <see cref="ILogger"/> instance.</param>
    /// <param name="options">The request correlation configuration.</param>
    public RequestCorrelationMiddleware(ILogger<RequestCorrelationMiddleware> logger, IOptions<RequestCorrelationOptions> options)
    {
        _logger = logger;
        _options = options.Value;

        _logger.LogInformation("header {header}, prop {prop}", _options.HeaderName, _options.LogPropertyName);
    }

    /// <inheritdoc/>
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