﻿public class UserGroupMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserGroupMiddleware> _logger;
    private readonly HashSet<string> _excludedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "/favicon.ico"// Add other paths you want to exclude
        // You can add other paths here if necessary
    };

    public UserGroupMiddleware(RequestDelegate next, ILogger<UserGroupMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    private async Task<bool> ApiIsFeatureAsync(HttpContext context, string featureName)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>() is ControllerActionDescriptor actionDescriptor)
        {
            // Get the method information
            var methodInfo = actionDescriptor.MethodInfo;

            // Get the FeatureGate attribute applied to the method (if any)
            var featureGateAttribute = methodInfo.GetCustomAttribute<FeatureGateAttribute>();

            if (featureGateAttribute != null)
            {
                // Get the feature manager service
                var featureManager = context.RequestServices.GetRequiredService<IFeatureManager>();

                foreach (var checkFeatureName in featureGateAttribute.Features)
                {
                    if (await featureManager.IsEnabledAsync(checkFeatureName) && checkFeatureName == featureName)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool ApiHasFeatureGate(HttpContext context)
    {
        // Check if the request has a FeatureGate attribute
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>() is ControllerActionDescriptor actionDescriptor)
        {
            var methodInfo = actionDescriptor.MethodInfo;
            var featureGateAttribute = methodInfo.GetCustomAttribute<FeatureGateAttribute>();

            if (featureGateAttribute == null)
            {
                // No FeatureGate attribute, skip further processing
                return false;
            }
        }
        else
        {
            // No endpoint metadata, skip further processing
            return false;
        }
        return true;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        try
        {
            var hasFeatureGate = ApiHasFeatureGate(context);
            if (!hasFeatureGate)
            {
                await _next(context);
                return;
            }

            //var isConsumerApi = await ApiIsFeatureAsync(context, FeatureNames.Consumer);

            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync($"An error occurred: {ex.Message}");
            _logger.LogError(ex.Message);
        }
    }
}

public static class UserGroupMiddlewareExtensions
{
    public static IApplicationBuilder UserGroupMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserGroupMiddleware>();
    }
}