using DfT.DTRO.Models.PortalUser;

namespace DfT.DTRO.DAL;

[ExcludeFromCodeCoverage]
public class PortalUserDal : IPortalUserDal
{
    private readonly DtroContext _dtroContext;
    
   
    public PortalUserDal(DtroContext dtroContext)
    {
        _dtroContext = dtroContext;
    }
    
    public async Task<PortalUserResponse> GetUserPublishPermission(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentNullException(nameof(userId), "userId cannot be empty");
        }

        var userResponse =  await _dtroContext.Users
            .Where(user => user.Email == userId)
            .Select(user => new PortalUserResponse
            {
                canPublish = user.CanPublishApp
            })
            .SingleOrDefaultAsync();

        if (userResponse == null)
        {
            throw new KeyNotFoundException($"User with email {userId} not found");
        }
        
        return userResponse;
    }
}