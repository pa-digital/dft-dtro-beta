using DfT.DTRO.Models.SystemConfig;

namespace Dft.DTRO.Admin.Services;
public class SystemConfigService : ISystemConfigService
{
    private readonly HttpClient _client;
    private readonly IXappIdService _xappIdService;
    private readonly IErrHandlingService _errHandlingService;
    private readonly ILogger<ISystemConfigService> _logger;
    public SystemConfigService(IHttpClientFactory clientFactory, IXappIdService xappIdService, ILogger<ISystemConfigService> logger, IErrHandlingService errHandlingService)
    {
        _client = clientFactory.CreateClient("ExternalApi");
        _xappIdService = xappIdService;
        _errHandlingService = errHandlingService;
    }

    public async Task<bool> UpdateSystemConfig(SystemConfig systemConfig)
    {
        _logger.LogInformation($"Method {nameof(UpdateSystemConfig)} called at {DateTime.UtcNow:G}");
        var content = JsonContent.Create(systemConfig);
        var request = new HttpRequestMessage(HttpMethod.Put, $"/systemConfig/updateFromBody/")
        {
            Content = content
        };
        _logger.LogInformation($"Request headers: {request.Headers} generated at {DateTime.UtcNow:G}");
        _logger.LogInformation($"Request uri: {request.RequestUri} generated at {DateTime.UtcNow:G}");
        
        _xappIdService.AddXAppIdHeader(ref request);
        _logger.LogInformation($"Client Base URL: {_client.BaseAddress} generated at {DateTime.UtcNow:G}");

        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);

        _logger.LogInformation($"Response content: {response.Content} generated at {DateTime.UtcNow:G}");
        _logger.LogInformation($"Response request message: {response.RequestMessage} generated at {DateTime.UtcNow:G}");


        _xappIdService.ChangeXAppId(systemConfig.xAppId);
        _logger.LogInformation($"Client Base URL: {_client.BaseAddress} generated at {DateTime.UtcNow:G}");
        return true;
    }


    public async Task<SystemConfig> GetSystemConfig()
    {
        _logger.LogInformation($"Method {nameof(GetSystemConfig)} called at {DateTime.UtcNow:G}");

        var unknown = new SystemConfig() { SystemName = "Unknown", IsTest = false };
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/systemConfig");
            _xappIdService.AddXAppIdHeader(ref request);
            _logger.LogInformation($"Client Base URL: {_client.BaseAddress} generated at {DateTime.UtcNow:G}");
            _logger.LogInformation($"Request headers: {request.Headers} generated at {DateTime.UtcNow:G}");
            _logger.LogInformation($"Request uri: {request.RequestUri} generated at {DateTime.UtcNow:G}");

            var response = await _client.SendAsync(request);
            await _errHandlingService.RedirectIfErrors(response);
            _logger.LogInformation($"Response content: {response.Content} generated at {DateTime.UtcNow:G}");
            _logger.LogInformation($"Response request message: {response.RequestMessage} generated at {DateTime.UtcNow:G}");

            var jsonResponse = await response.Content.ReadAsStringAsync();
            if (jsonResponse == null)
            {
                _logger.LogError($"Response content {jsonResponse} generated at {DateTime.UtcNow:G}");
                return unknown;
            }
            var ret = JsonSerializer.Deserialize<SystemConfig>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (ret == null)
            {
                _logger.LogError($"Response content {jsonResponse} generated at {DateTime.UtcNow:G}");
                return unknown;
            }
            _logger.LogInformation($"xAppId from response {ret.xAppId} generated at {DateTime.UtcNow:G}");

            ret.xAppId = _xappIdService.MyXAppId();
            _logger.LogInformation($"xAppId from service {_xappIdService.MyXAppId()} generated at {DateTime.UtcNow:G}");
            return ret;
        }
        catch (Exception)
        {
            unknown.SystemName = "Not Found";
            unknown.CurrentUserName = "";
            return unknown;
        }
    }
}