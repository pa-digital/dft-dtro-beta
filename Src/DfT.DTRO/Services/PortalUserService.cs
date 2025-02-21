using DfT.DTRO.Models.PortalUser;

namespace DfT.DTRO.Services;

public class PortalUserService : IPortalUserService
{
    private readonly IPortalUserDal _portalUserDal;

    public PortalUserService(IPortalUserDal portalUserDal)
    {
        _portalUserDal = portalUserDal;
    }
    
    public async Task<PortalUserResponse> CanUserPublish(string userId) => 
        await _portalUserDal.GetUserPublishPermission(userId);
}