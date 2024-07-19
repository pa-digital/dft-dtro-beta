using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace DfT.DTRO.RequestCorrelation;

public class RequestCorrelationProvider : IRequestCorrelationProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly RequestCorrelationOptions _options;

    public RequestCorrelationProvider(IHttpContextAccessor contextAccessor, IOptions<RequestCorrelationOptions> options)
    {
        _contextAccessor = contextAccessor;
        _options = options.Value;
    }

    public string CorrelationId =>
        _contextAccessor.HttpContext?.Request?.Headers?.TryGetValue(_options.HeaderName, out StringValues correlationIds) ?? false
            ? correlationIds.FirstOrDefault(value => !string.IsNullOrEmpty(value))?.ToString()
            : null;
}
