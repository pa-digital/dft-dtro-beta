
namespace DfT.DTRO.Services;

public interface ISystemConfigDal
{
    Task<SystemConfig> GetSystemConfigAsync();
}