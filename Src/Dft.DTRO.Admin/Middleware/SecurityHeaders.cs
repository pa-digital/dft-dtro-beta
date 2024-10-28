﻿public class SecurityHeaders
{
    private readonly RequestDelegate _next;

    public SecurityHeaders(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var scriptHash = context.Items["InlineScriptHash"]?.ToString() ?? "";
        var styleHash = context.Items["InlineStyleHash"]?.ToString() ?? "";

        context.Response.Headers.Add("Content-Security-Policy",
            $"default-src 'self'; " +
            $"form-action 'self'; " +
            $"object-src 'none'; " +
            $"img-src 'self' data:; " +
            $"script-src 'self' https://cdn.jsdelivr.net {scriptHash}; " +
            $"style-src 'self' {styleHash}; " +
            $"frame-ancestors 'none'; " +
            $"script-src-elem 'self' https://cdn.jsdelivr.net {scriptHash}; " +
            $"style-src-elem 'self' {styleHash}; " +
            $"unsafe-hashes");
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("X-Frame-Options", "DENY");
        context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
        context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");
        context.Response.Headers.Add("Referrer-Policy", "no-referrer");
        context.Response.Headers.Add("Permissions-Policy", "geolocation=(), midi=(), sync-xhr=(), microphone=(), camera=(), magnetometer=(), gyroscope=(), fullscreen=(), payment=()");

        await _next(context);
    }
}
