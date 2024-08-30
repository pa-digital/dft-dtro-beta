using DfT.DTRO.Models.SystemConfig;

namespace DfT.DTRO.Services;

public class SystemConfigService : ISystemConfigService
{
    private readonly ISystemConfigDal _systemConfigDal;
    private readonly IDtroUserDal _dtroUserDal;

    public SystemConfigService(ISystemConfigDal systemConfigDal, IDtroUserDal dtroUserDal)
    {
        _systemConfigDal = systemConfigDal;
        _dtroUserDal = dtroUserDal;
    }

    public async Task<SystemConfigResponse> GetSystemConfigAsync(Guid xAppId)
    {
        var response = await _systemConfigDal.GetSystemConfigAsync();

        var user = await _dtroUserDal.GetDtroUserOnAppIdAsync(xAppId);

        if (user != null)
        {
            response.CurrentUserName = user.Name;
        }
        return response;
    }
    public async Task<bool> UpdateSystemConfigAsync(SystemConfigRequest systemConfigRequest)
    {
        return await _systemConfigDal.UpdateSystemConfigAsync(systemConfigRequest);
    }
    

}