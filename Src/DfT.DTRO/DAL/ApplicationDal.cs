namespace DfT.DTRO.DAL;

public class ApplicationDal(DtroContext context) : IApplicationDal
{
    private readonly DtroContext _context = context;


    public async Task<bool> CheckApplicationNameDoesNotExist(string appName)
    {
        return !await _context.Applications.AnyAsync(a => a.Nickname == appName);
    }

    public async Task<string> GetApplicationUser(Guid appId)
    {
        return await _context.Applications
            .Include(a => a.User)
            .Where(a => a.Id == appId)
            .Select(a => a.User.Email)
            .FirstOrDefaultAsync();
    }

    public async Task<ApplicationDetailsDto> GetApplicationDetails(Guid appId)
    {
        return await _context.Applications
            .Include(a => a.Purpose)
            .Where(a => a.Id == appId)
            .Select(a => new ApplicationDetailsDto
            {
                Name = a.Nickname,
                AppId = a.Id,
                Purpose = a.Purpose.Description
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<ApplicationListDto>> GetApplicationList(string email)
    {
        return await _context.Applications
            .Include(a => a.User)
            .Include(a => a.TrafficRegulationAuthority)
            .Include(a => a.ApplicationType)
            .Where(a => a.User.Email == email)
            .Select(a => new ApplicationListDto
            {
                Id = a.Id,
                Name = a.Nickname,
                Type = a.ApplicationType.Name,
                Tra = a.TrafficRegulationAuthority.Name
            })
            .ToListAsync();
    }

    public async Task<PaginatedResult<ApplicationPendingListDto>> GetPendingApplications(PaginatedRequest paginatedRequest)
    {
        IQueryable<ApplicationPendingListDto> query = _context.Applications
            .Include(a => a.User)
            .Include(a => a.TrafficRegulationAuthority)
            .Include(a => a.Status)
            .Include(a => a.ApplicationType)
            .Where(application => application.Status.Status == "pending")
            .Select(a => new ApplicationPendingListDto
            {
                TraName = a.TrafficRegulationAuthority.Name,
                Type = a.ApplicationType.Name,
                UserEmail = a.User.Email,
                UserName = $"{a.User.Forename} {a.User.Surname}",
            });
        IQueryable<ApplicationPendingListDto> paginatedQuery = query
            .Skip((paginatedRequest.Page - 1) * paginatedRequest.PageSize)
            .Take(paginatedRequest.PageSize);
        return new PaginatedResult<ApplicationPendingListDto>(paginatedQuery.ToList(), paginatedQuery.Count());
    }

    public async Task<bool> ActivateApplicationById(Guid appId)
    {
        try
        {
            Application application = await _context.Applications.FindAsync(appId);
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