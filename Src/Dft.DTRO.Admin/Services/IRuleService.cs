
namespace Dft.DTRO.Admin.Services;
public interface IRuleService
{
    Task CreateRuleAsync(string version, IFormFile file);
    Task UpdateRuleAsync(string version, IFormFile file);
}