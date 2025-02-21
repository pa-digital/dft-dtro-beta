using System.Threading;
using DfT.DTRO.Models.PortalUser;

namespace DfT.DTRO.Services;

public interface IPortalUserService
{
    Task<PortalUserResponse> CanUserPublish(string userId);
}