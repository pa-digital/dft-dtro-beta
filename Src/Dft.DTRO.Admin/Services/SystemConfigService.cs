using DfT.DTRO.Models.SystemConfig;

namespace Dft.DTRO.Admin.Services;
public class SystemConfigService : ISystemConfigService
{
    private readonly HttpClient _client;
    private readonly IXappIdService _xappIdService;
    private readonly IErrHandlingService _errHandlingService;
    public SystemConfigService(IHttpClientFactory clientFactory, IXappIdService xappIdService, IErrHandlingService errHandlingService)
    {
        _client = clientFactory.CreateClient("ExternalApi");
        _xappIdService = xappIdService;
        _errHandlingService = errHandlingService;
    }

    public async Task<bool> UpdateSystemConfig(SystemConfig systemConfig)
    {
        var content = JsonContent.Create(systemConfig);
        var request = new HttpRequestMessage(HttpMethod.Put, ConfigHelper.Version + $"/systemConfig/updateFromBody/")
        {
            Content = content
        };
        await _xappIdService.AddXAppIdHeader(request);

        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);

        _xappIdService.ChangeXAppId(systemConfig.xAppId);
        return true;
    }


    public async Task<SystemConfig> GetSystemConfig()
    {
        var unknown = new SystemConfig() { SystemName = "Unknown", IsTest = false };
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, ConfigHelper.Version + "/systemConfig");
            await _xappIdService.AddXAppIdHeader(request);

            var response = await _client.SendAsync(request);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            if (jsonResponse == string.Empty)
            {
                return unknown;
            }
            var ret = JsonSerializer.Deserialize<SystemConfig>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (ret == null)
            {
                return unknown;
            }
            ret.xAppId = _xappIdService.MyXAppId();
            return ret;
        }
        catch (Exception)
        {
            unknown.SystemName = "Not Connected";
            unknown.CurrentUserName = "";
            return unknown;
        }
    }
}