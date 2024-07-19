namespace DfT.DTRO.Utilities;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        SecurityHeadersHelper.ConfigureCommonSecurityHeaders(httpContext);
        return _next.Invoke(httpContext);
    }
}