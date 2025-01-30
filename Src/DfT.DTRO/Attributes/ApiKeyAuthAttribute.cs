using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

public class ApiKeyAuthAttribute : ActionFilterAttribute
{
    private readonly string _apiKey;

    public ApiKeyAuthAttribute()
    {
        //TODO using a test API key, remove before pushing
        _apiKey = Environment.GetEnvironmentVariable("DTROZIPPER-KEY") ?? "d24b0298d286cdd2f61e9f8cc803f8f4";
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("X-DtroZipper-Key", out var extractedApiKey) ||
            extractedApiKey != _apiKey)
        {
            context.Result = new UnauthorizedResult();
        }
        base.OnActionExecuting(context);
    }
}