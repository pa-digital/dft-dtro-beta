
using DfT.DTRO.Models.SystemConfig;

namespace Dft.DTRO.Admin.Services;
public interface ISystemConfigService
{
    Task<SystemConfig> GetSystemConfig();
    Task<bool> UpdateSystemConfig(SystemConfig systemConfig);

}