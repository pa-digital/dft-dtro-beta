namespace DfT.DTRO.Controllers;

public abstract class BaseController : ControllerBase
{
    private readonly ILogger<BaseController> _logger;
    private readonly LoggingExtension _loggingExtension;
    
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="logger">An <see cref="ILogger{BaseController}"/> instance.</param>
    /// <param name="loggingExtension">An <see cref="LoggingExtension"/> instance.</param>
    protected BaseController(ILogger<BaseController> logger, LoggingExtension loggingExtension)
    {
        _logger = logger;
        _loggingExtension = loggingExtension;
    }

    protected void LogInformation(string methodName, string endpoint, string message)
    {
        _logger.LogInformation(message);
        _loggingExtension.LogInformation(methodName, endpoint, message);
    }
    
    protected void LogError(string methodName, string route, string errorMessage, string exceptionMessage = "")
    {
        _logger.LogError(errorMessage);
        _loggingExtension.LogError(methodName, route, errorMessage, exceptionMessage);
    }
}