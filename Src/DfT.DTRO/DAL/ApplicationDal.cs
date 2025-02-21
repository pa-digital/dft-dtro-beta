namespace DfT.DTRO.DAL

{
    public class ApplicationDal(DtroContext context) : IApplicationDal
    {
        private readonly DtroContext _context = context;

        public bool CheckApplicationNameDoesNotExist(string appName)
        {
            return !_context.Applications.Any(a => a.Nickname == appName);
        }

        public Guid GetApplicationUser(Guid appGuid)
        {
            return _context.Applications
                .Where(a => a.Id == appGuid)
                .Select(a => a.UserId)
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
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return null;
            }

            return _context.Applications
                .Include(a => a.User)
                .Include(a => a.TrafficRegulationAuthority)
                .Include(a => a.ApplicationType)
                .Where(a => a.User.Id == userGuid)
                .Select(a => new ApplicationListDto
                {
                    Id = a.Id,
                    Name = a.Nickname,
                    Type = a.ApplicationType.Name,
                    Tra = a.TrafficRegulationAuthority.Name
                })
                .ToList();
        }

    }
}
