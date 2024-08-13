public class FeatureGateMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<FeatureGateMiddleware> _logger;
    private readonly HashSet<string> _excludedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "/health",
        "/favicon.ico"// Add other paths you want to exclude
        // You can add other paths here if necessary
    };

    public FeatureGateMiddleware(RequestDelegate next, ILogger<FeatureGateMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    private async Task<bool> ApiIsAdmin(HttpContext context)
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

                foreach (var featureName in featureGateAttribute.Features)
                {
                    // Check if the feature flag is enabled
                    if (await featureManager.IsEnabledAsync(featureName))
                    {
                        if (featureName == nameof(FeatureNames.Admin))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private async Task<bool> ApiIsConsumer(HttpContext context)
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

                foreach (var featureName in featureGateAttribute.Features)
                {
                    // Check if the feature flag is enabled
                    if (await featureManager.IsEnabledAsync(featureName))
                    {
                        if (featureName == nameof(FeatureNames.Consumer))
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        try
        {
            // Skip middleware processing for excluded paths (e.g., health checks)
            if (_excludedPaths.Contains(context.Request.Path.Value))
            {
                await _next(context);
                return;
            }

            var isConsumer = await ApiIsConsumer(context);
            if (!isConsumer)
            {
                var trafficAuthority = new SwaCode();

                // Retrieve the 'ta' value from the request headers
                if (context.Request.Headers.TryGetValue("TA", out var taHeaderValue))
                {
                    int? traId = int.TryParse(taHeaderValue, out var taValue) ? taValue : (int?)null;

                    if (traId.HasValue)
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            var swaCodeDal = scope.ServiceProvider.GetRequiredService<ISwaCodeDal>();

                            trafficAuthority = await swaCodeDal.GetTraAsync(traId.Value);
                            if (trafficAuthority == null)
                            {
                                throw new Exception($"Middleware exception: Traffic authority ({traId}) not found");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Middleware exception: Traffic authority(id) is not a valid number");
                    }
                }
                else
                {
                    throw new Exception("Middleware exception: Traffic authority(id) not in header");
                }

                if (!trafficAuthority.IsActive)
                {
                    throw new Exception($"Middleware exception: Traffic authority ({trafficAuthority.TraId}) has been deactivated");
                }

                var isAdminCall = await ApiIsAdmin(context);
                if (isAdminCall)
                {
                    if (!trafficAuthority.IsAdmin)
                    {
                        throw new Exception($"Middleware exception: Traffic authority ({trafficAuthority.TraId}) is not an admin user");
                    }
                }
            }

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
public static class FeatureGateMiddlewareExtensions
{
    public static IApplicationBuilder UseFeatureGateMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<FeatureGateMiddleware>();
    }
}