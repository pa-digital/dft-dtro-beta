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

    public async Task<UserAppDto> GetUserInfo(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentNullException(nameof(userId), "userId cannot be empty");
        }
        
        
        var userResponse = _dtroContext.Users
            .Where(user => user.Email == userId)
            .Include(user => user.Applications)
            .ThenInclude(app => app.ApplicationType)
            .Include(user => user.Applications) 
            .ThenInclude(app => app.TrafficRegulationAuthority)
            .Include(user => user.Applications)
            .ThenInclude(app => app.Purpose)
            .Select(user => new UserAppDto
            {
                UserId = user.Email,
                User = $"{user.Forename} {user.Surname}",
                Apps = user.Applications.Select(app => new ApplicationInfo
                {
                    Name = app.Nickname,
                    Created = app.Created,
                    Purpose = app.Purpose.Description,
                    Type = app.ApplicationType.Name,
                    Id = app.Id,
                    Tra = app.TrafficRegulationAuthority.Name
                }).ToList()
            })
            .FirstOrDefault();


        if (userResponse == null)
        {
            throw new KeyNotFoundException($"User with email {userId} not found");
        }

        return userResponse;
    }
}