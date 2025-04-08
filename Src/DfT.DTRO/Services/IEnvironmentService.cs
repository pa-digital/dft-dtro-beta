namespace DfT.DTRO.Services;

public interface IEnvironmentService
{
    Task<bool> CanRequestProductionAccess(string email);
    Task RequestProductionAccess(string email);
}