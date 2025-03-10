
using System.Threading.Tasks;
using DfT.DTRO.Models.SystemConfig;

namespace DfT.DTRO.Services;

public interface ISystemConfigService
{
    Task<SystemConfigResponse> GetSystemConfigAsync(Guid xAppId);
    Task<bool> UpdateSystemConfigAsync(SystemConfigRequest systemConfigRequest);
}