using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DfT.DTRO.Utilities;

/// <summary>
/// Middleware class to inject common security headers.
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Default constructor for DI.
    /// </summary>
    /// <param name="next">The next request processing delegate.</param>
    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes middelware function.
    /// </summary>
    /// <param name="httpContext">The current HTTP Context object.</param>
    /// <returns>A request decorated with common security headers.</returns>
    public Task Invoke(HttpContext httpContext)
    {
        SecurityHeadersHelper.ConfigureCommonSecurityHeaders(httpContext);
        return _next.Invoke(httpContext);
    }
}