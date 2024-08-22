
using DfT.DTRO.Models.SystemConfig;

namespace DfT.DTRO.Services;

public interface ISystemConfigDal
{
    Task<SystemConfigResponse> GetSystemConfigAsync();
    Task<bool> UpdateSystemConfigAsync(SystemConfigRequest systemConfigRequest);
}