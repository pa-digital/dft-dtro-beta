namespace DfT.DTRO.DAL;

public class ApplicationDal(DtroContext context) : IApplicationDal
{
    private readonly DtroContext _context = context;


    public async Task<bool> CheckApplicationNameDoesNotExist(string appName)
    {
        return !await _context.Applications.AnyAsync(a => a.Nickname == appName);
    }

    public async Task<string> GetApplicationUser(Guid appGuid)
    {
        return await _context.Applications
            .Include(a => a.User)
            .Where(a => a.Id == appGuid)
            .Select(a => a.User.Email)
            .FirstOrDefaultAsync();
    }

    public async Task<ApplicationDetailsDto> GetApplicationDetails(string appId)
    {
        if (!Guid.TryParse(appId, out Guid appGuid))
        {
            return null;
        }

        return await _context.Applications
            .Include(a => a.Purpose)
            .Where(a => a.Id == appGuid)
            .Select(a => new ApplicationDetailsDto
            {
                Name = a.Nickname,
                AppId = a.Id,
                Purpose = a.Purpose.Description
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<ApplicationListDto>> GetApplicationList(string userId)
    {
        return await _context.Applications
            .Include(a => a.User)
            .Include(a => a.TrafficRegulationAuthority)
            .Include(a => a.ApplicationType)
            .Where(a => a.User.Email == userId)
            .Select(a => new ApplicationListDto
            {
                Id = a.Id,
                Name = a.Nickname,
                Type = a.ApplicationType.Name,
                Tra = a.TrafficRegulationAuthority.Name
            })
            .ToListAsync();
    }

    public async Task<bool> ActivateApplicationById(Guid appGuid)
    {
        try
        {
            Application application = await _context.Applications.FindAsync(appGuid);
            if (application == null)
            {
                return false;
            }

            ApplicationStatus activeStatus = await _context.ApplicationStatus.SingleOrDefaultAsync(a => a.Status == "Active");
            if (activeStatus == null)
            {
                return false;
            }

            if (application.Status != activeStatus)
            {
                application.Status = activeStatus;
                await _context.SaveChangesAsync();
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}