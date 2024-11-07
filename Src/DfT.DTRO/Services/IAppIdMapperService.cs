
namespace DfT.DTRO.Services;

public interface IAppIdMapperService
{
    Task<Guid> GetAppId(HttpContext context);
}