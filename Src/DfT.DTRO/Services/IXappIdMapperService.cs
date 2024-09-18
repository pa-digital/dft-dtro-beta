
namespace DfT.DTRO.Services;

public interface IXappIdMapperService
{
    Task<Guid> GetXappId(HttpContext context);
}