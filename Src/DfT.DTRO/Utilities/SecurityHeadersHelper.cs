#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace DfT.DTRO.Utilities;

public static class SecurityHeadersHelper
{
    public static void ConfigureCommonSecurityHeaders(HttpContext ctx)
    {
        ctx.Response.Headers.Append("Cross-Origin-Embedder-Policy", "unsafe-none");
        ctx.Response.Headers.Append("Cross-Origin-Opener-Policy", "same-origin");
        ctx.Response.Headers.Append("Cross-Origin-Resource-Policy", "cross-origin");
        ctx.Response.Headers.Append("permissions-policy", "geolocation=(), midi=(), sync-xhr=(), microphone=(), camera=(), magnetometer=(), gyroscope=(), fullscreen=(), payment=()");
        ctx.Response.Headers.Append("Referrer-Policy", "no-referrer");
        ctx.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        ctx.Response.Headers.Append(HeaderNames.XFrameOptions, "DENY");
        ctx.Response.Headers.Append("X-Permitted-Cross-Domain-Policies", "none");
        ctx.Response.Headers.Append("X-Xss-Protection", "1; mode=block");
        ctx.Response.Headers.Append("X-UA-Compatible", "IE=Edge");

        ctx.Response.Headers.Append(HeaderNames.StrictTransportSecurity, "max-age=31536000");

        ctx.Response.Headers.Append(HeaderNames.CacheControl, "no-store,no-cache,must-revalidate");
        ctx.Response.Headers.Append(HeaderNames.Pragma, "no-cache");
    }
}