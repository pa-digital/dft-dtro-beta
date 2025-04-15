namespace DfT.DTRO.DAL;

public interface IAuthDal
{
    Task<Auth> GetAuthUser(User user);
}