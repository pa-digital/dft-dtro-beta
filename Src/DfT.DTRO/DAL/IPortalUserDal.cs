using DfT.DTRO.Models.PortalUser;

namespace DfT.DTRO.DAL;

public interface IPortalUserDal
{
    Task<PortalUserResponse> GetUserPublishPermission(string userId);
    
    Task<UserAppDto> GetUserInfo(string userId);
}