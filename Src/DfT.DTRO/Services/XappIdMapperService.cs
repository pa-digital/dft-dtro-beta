namespace DfT.DTRO.Services;

public class XappIdMapperService : IXappIdMapperService
{
    public async Task<Guid> GetXappId(HttpContext context)
    {
        context.Request.Headers.TryGetValue("x-app-id", out var appId);
        context.Request.Headers.TryGetValue("x-app-id-override", out var appIdOverride);


        Guid.TryParse(appId, out var xAppIdValue);
        Guid.TryParse(appIdOverride, out var appIdOverrideValue);

        if (xAppIdValue == Guid.Empty && appIdOverrideValue == Guid.Empty)
        {
            throw new Exception("Middleware, access denied: x-App-Id (or x-App-Id-Override) not in header");
        }
        if (appIdOverrideValue == Guid.Empty)
        {
            return xAppIdValue;
        }
        else
        {
            return appIdOverrideValue;
        }
    }
}
