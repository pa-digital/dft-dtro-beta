using DfT.DTRO.Models.SystemConfig;

namespace DfT.DTRO.Services;

public class SystemConfigService : ISystemConfigService
{
    private readonly ISystemConfigDal _systemConfigDal;

    public SystemConfigService(ISystemConfigDal systemConfigDal)
    {
        _systemConfigDal = systemConfigDal;
    }

    public async Task<SystemConfigResponse> GetSystemConfigAsync()
    {
        var response = await _systemConfigDal.GetSystemConfigAsync();
        return response;
    }
    public async Task<bool> UpdateSystemConfigAsync(SystemConfigRequest systemConfigRequest)
    {
        return await _systemConfigDal.UpdateSystemConfigAsync(systemConfigRequest);
    }
    

}