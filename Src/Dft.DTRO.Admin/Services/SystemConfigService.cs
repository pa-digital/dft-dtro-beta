using System.Collections.Generic;
using DfT.DTRO.Models.SystemConfig;

namespace Dft.DTRO.Admin.Services;
public class SystemConfigService : ISystemConfigService
{
    private readonly HttpClient _client;

    public SystemConfigService(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("ExternalApi");
    }

    public async Task<bool> UpdateSystemConfig(SystemConfig systemConfig)
    {
        var content = JsonContent.Create(systemConfig);
        var request = new HttpRequestMessage(HttpMethod.Put, $"/systemConfig/updateFromBody/")
        {
            Content = content
        };
        Helper.AddXAppIdHeader(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return true;
    }
    

    public async Task<SystemConfig> GetSystemConfig()
    {

        var unknown  = new SystemConfig() { SystemName = "Unknown" , IsTest = true};
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/systemConfig");
            Helper.AddXAppIdHeader(ref request);
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
            return ret;
        }
        catch (Exception)
        {
            unknown.SystemName = "No System Config)";
            return unknown;
        }   
    }
}