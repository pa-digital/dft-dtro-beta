using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class TokenValidationAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authorizationHeader = context.HttpContext.Request.Headers.Authorization.FirstOrDefault();

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedObjectResult("Missing or invalid Authorization header.");
            return;
        }

        var token = authorizationHeader.Split(" ").Last();
        string? userId = ValidateToken(token);
        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new UnauthorizedResult();
        }

        context.HttpContext.Items["UserId"] = userId;
    }

    private static String? ValidateToken(string? token)
    {
        // TODO: validate token with Apigee
        return "user@test.com";
    }
}
