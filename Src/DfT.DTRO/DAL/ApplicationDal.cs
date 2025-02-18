namespace DfT.DTRO.DAL

{
    public class ApplicationDal(DtroContext context) : IApplicationDal
    {
        private readonly DtroContext _context = context;

        public bool CheckApplicationNameDoesNotExist(string appName)
        {
            return !_context.Applications.Any(a => a.Nickname == appName);
        }
    }
}
