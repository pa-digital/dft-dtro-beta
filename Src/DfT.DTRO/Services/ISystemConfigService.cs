
using System.Threading.Tasks;
using DfT.DTRO.Models.SystemConfig;

namespace DfT.DTRO.Services;

public interface ISystemConfigService
{
    Task<SystemConfigResponse> GetSystemConfigAsync(Guid appId);
    Task<bool> UpdateSystemConfigAsync(SystemConfigRequest systemConfigRequest);
}