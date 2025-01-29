namespace DfT.DTRO.Attributes;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

public class IpWhitelistAttribute : ActionFilterAttribute
{
    private readonly string[] _allowedIps;

    public IpWhitelistAttribute(params string[] allowedIps)
    {
        _allowedIps = allowedIps;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var remoteIp = context.HttpContext.Connection.RemoteIpAddress?.ToString();

        if (!_allowedIps.Contains(remoteIp))
        {
            context.Result = new ContentResult
            {
                StatusCode = 403,
                Content = "Access forbidden: Your IP is not whitelisted."
            };
        }

        base.OnActionExecuting(context);
    }
}