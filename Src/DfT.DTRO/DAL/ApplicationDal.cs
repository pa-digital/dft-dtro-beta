namespace DfT.DTRO.DAL

{
    public class ApplicationDal(DtroContext context) : IApplicationDal
    {
        private readonly DtroContext _context = context;

        public bool CheckApplicationNameDoesNotExist(string appName)
        {
            return !_context.Applications.Any(a => a.Nickname == appName);
        }

        public string GetApplicationUser(Guid appGuid)
        {
            return _context.Applications
                .Include(a => a.User)
                .Where(a => a.Id == appGuid)
                .Select(a => a.User.Email)
                .FirstOrDefault();
        }

        public ApplicationDetailsDto GetApplicationDetails(string appId)
        {
            if (!Guid.TryParse(appId, out Guid appGuid))
            {
                return null;
            }

            return _context.Applications
                .Include(a => a.Purpose)
                .Where(a => a.Id == appGuid)
                .Select(a => new ApplicationDetailsDto
                {
                    Name = a.Nickname,
                    AppId = a.Id,
                    Purpose = a.Purpose.Description
                })
                .FirstOrDefault();
        }

        public List<ApplicationListDto> GetApplicationList(string userId)
        {
            return _context.Applications
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
                .ToList();
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
}
