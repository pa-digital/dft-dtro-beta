using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace DfT.DTRO.RequestCorrelation;

/// <summary>
/// The default implementation of <see cref="IRequestCorrelationProvider"/>.
/// Provides correlation information from the HTTP header.
/// </summary>
public class RequestCorrelationProvider : IRequestCorrelationProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly RequestCorrelationOptions _options;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="contextAccessor">An <see cref="IHttpContextAccessor"/> instance.</param>
    /// <param name="options">The request correlation configuration.</param>
    public RequestCorrelationProvider(IHttpContextAccessor contextAccessor, IOptions<RequestCorrelationOptions> options)
    {
        _contextAccessor = contextAccessor;
        _options = options.Value;
    }

    /// <inheritdoc/>
    public string CorrelationId =>
        _contextAccessor.HttpContext?.Request?.Headers?.TryGetValue(_options.HeaderName, out StringValues correlationIds) ?? false
            ? correlationIds.FirstOrDefault(value => !string.IsNullOrEmpty(value))?.ToString()
            : null;
}
