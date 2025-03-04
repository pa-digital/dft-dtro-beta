namespace DfT.DTRO.DAL;

[ExcludeFromCodeCoverage]
public class SystemConfigDal : ISystemConfigDal
{
    private readonly DtroContext _dtroContext;

    public SystemConfigDal(DtroContext dtroContext)
    {
        _dtroContext = dtroContext;
    }

    public async Task<SystemConfigResponse> GetSystemConfigAsync()
    {
        var config = await _dtroContext.SystemConfig.FirstOrDefaultAsync();
        if (config == null)
        {
            return new SystemConfigResponse() { SystemName = "Not Set (record not found in database)" };
        }

        return new SystemConfigResponse() { SystemName = config.SystemName, IsTest = config.IsTest };
    }

    public async Task<bool> UpdateSystemConfigAsync(SystemConfigRequest systemConfigRequest)
    {
        var existing = await _dtroContext.SystemConfig.FirstOrDefaultAsync();
        if (existing == null)
        {
            throw new NotFoundException();
        }
        existing.IsTest = systemConfigRequest.IsTest;
        existing.SystemName = systemConfigRequest.SystemName;
        await _dtroContext.SaveChangesAsync();
        return true;
    }
}