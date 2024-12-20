#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace DfT.DTRO.Utilities;

public static class SecurityHeadersHelper
{
    public static void ConfigureCommonSecurityHeaders(HttpContext ctx)
    {
        ctx.Response.Headers.Add("Cross-Origin-Embedder-Policy", "unsafe-none");
        ctx.Response.Headers.Add("Cross-Origin-Opener-Policy", "same-origin");
        ctx.Response.Headers.Add("Cross-Origin-Resource-Policy", "cross-origin");
        ctx.Response.Headers.Add("permissions-policy", "geolocation=(), midi=(), sync-xhr=(), microphone=(), camera=(), magnetometer=(), gyroscope=(), fullscreen=(), payment=()");
        ctx.Response.Headers.Add("Referrer-Policy", "no-referrer");
        ctx.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        ctx.Response.Headers.Add(HeaderNames.XFrameOptions, "DENY");
        ctx.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
        ctx.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
        ctx.Response.Headers.Add("X-UA-Compatible", "IE=Edge");

        ctx.Response.Headers.Add(HeaderNames.StrictTransportSecurity, "max-age=31536000");

        ctx.Response.Headers.Add(HeaderNames.CacheControl, "no-store,no-cache,must-revalidate");
        ctx.Response.Headers.Add(HeaderNames.Pragma, "no-cache");
    }
}
