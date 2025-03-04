namespace DfT.DTRO.DAL

{
    public class ApplicationDal : IApplicationDal
    {
        private readonly DtroContext _context;

        public ApplicationDal(DtroContext context)
        {
            _context = context;
        }

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

        public PaginatedResult<ApplicationListDto> GetPendingApplications(PaginatedRequest request)
        {
            IQueryable<ApplicationListDto> query = _context
                .Applications
                .Include(application => application.User)
                .Include(application => application.TrafficRegulationAuthority)
                .Include(application => application.ApplicationType)
                .Where(application => application.Status == "pending")
                .Select(application => new ApplicationListDto
                {
                    Id = application.Id,
                    Name = application.Nickname,
                    Type = application.ApplicationType.Name,
                    Tra = application.TrafficRegulationAuthority.Name
                });

            IQueryable<ApplicationListDto> paginatedQuery = query
                .OrderBy(dto => dto.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);

            return new PaginatedResult<ApplicationListDto>(paginatedQuery.ToList(), paginatedQuery.Count());
        }
    }
}
