
namespace DfT.DTRO.Services;

public interface IAppIdMapperService
{
    Guid GetAppId(HttpContext context);
}