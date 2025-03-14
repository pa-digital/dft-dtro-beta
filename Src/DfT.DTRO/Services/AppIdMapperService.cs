namespace DfT.DTRO.Services;

public class AppIdMapperService : IAppIdMapperService
{
    public async Task<Guid> GetAppId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(RequestHeaderNames.AppId, out var appId);
        
        Guid.TryParse(appId, out var appIdValue);

        if (appIdValue == Guid.Empty)
        {
            throw new Exception($"Middleware, access denied: {RequestHeaderNames.AppId} not in header");
        }
        
        return appIdValue;
    }
}