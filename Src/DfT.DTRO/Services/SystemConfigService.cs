namespace DfT.DTRO.Services;

public class SystemConfigService : ISystemConfigService
{
    private readonly ISystemConfigDal _systemConfigDal;

    public SystemConfigService(ISystemConfigDal systemConfigDal)
    {
        _systemConfigDal = systemConfigDal;
    }

    public async Task<string> GetSystemNameAsync()
    {
        var response = await _systemConfigDal.GetSystemConfigAsync();
        return response.SystemName;
    }
}