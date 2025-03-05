namespace DfT.DTRO.DAL;

public interface ISystemConfigDal
{
    Task<SystemConfigResponse> GetSystemConfigAsync();
    Task<bool> UpdateSystemConfigAsync(SystemConfigRequest systemConfigRequest);
}