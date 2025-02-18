namespace DfT.DTRO.DAL

{
    public interface IApplicationDal
    {
        bool CheckApplicationNameDoesNotExist(string appName);
    }
}
