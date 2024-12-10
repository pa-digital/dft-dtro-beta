namespace DfT.DTRO.Services;

public class AppIdMapperService : IAppIdMapperService
{
    public async Task<Guid> GetAppId(HttpContext context)
    {
        context.Request.Headers.TryGetValue("x-app-id", out var appId);
        context.Request.Headers.TryGetValue("x-app-id-override", out var appIdOverride);


        Guid.TryParse(appId, out var xAppIdValue);
        Guid.TryParse(appIdOverride, out var xAppIdOverrideValue);

        if (xAppIdValue == Guid.Empty && xAppIdOverrideValue == Guid.Empty)
        {
            throw new Exception("Middleware, access denied: x-App-Id (or x-App-Id-Override) not in header");
        }

        return xAppIdOverrideValue == Guid.Empty ? xAppIdValue : xAppIdOverrideValue;
    }
}
