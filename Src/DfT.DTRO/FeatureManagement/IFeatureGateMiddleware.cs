
public interface IFeatureGateMiddleware
{
    Task InvokeAsync(HttpContext context);
}