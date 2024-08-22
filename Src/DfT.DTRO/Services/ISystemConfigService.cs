
using DfT.DTRO.Models.SystemConfig;
using System.Threading.Tasks;

namespace DfT.DTRO.Services;

public interface ISystemConfigService
{
    Task<SystemConfigResponse> GetSystemConfigAsync();
    Task<bool> UpdateSystemConfigAsync(SystemConfigRequest systemConfigRequest);
}