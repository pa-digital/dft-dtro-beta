using DfT.DTRO.Models.SystemConfig;

namespace Dft.DTRO.Admin.Services;
public class SystemConfigService : ISystemConfigService
{
    private readonly HttpClient _client;
    private readonly IXappIdService _xappIdService;
    public SystemConfigService(IHttpClientFactory clientFactory, IXappIdService xappIdService)
    {
        _client = clientFactory.CreateClient("ExternalApi");
        _xappIdService = xappIdService;
    }

    public async Task<bool> UpdateSystemConfig(SystemConfig systemConfig)
    {
        var content = JsonContent.Create(systemConfig);
        var request = new HttpRequestMessage(HttpMethod.Put, $"/systemConfig/updateFromBody/")
        {
            Content = content
        };
        _xappIdService.AddXAppIdHeader(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        _xappIdService.ChangeXAppId(systemConfig.xAppId);
        return true;
    }
    

    public async Task<SystemConfig> GetSystemConfig()
    {

        var unknown  = new SystemConfig() { SystemName = "Unknown" , IsTest = false};
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/systemConfig");
            _xappIdService.AddXAppIdHeader(ref request);
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            if (jsonResponse == null)
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
            unknown.SystemName = "Not Found";
            unknown.CurrentUserName = "";
            return unknown;
        }   
    }
}