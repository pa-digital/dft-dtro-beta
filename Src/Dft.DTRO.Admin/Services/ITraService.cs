
namespace Dft.DTRO.Admin.Services;

public interface ITraService
{
    Task<List<SwaCodeResponse>> GetSwaCodes();
}