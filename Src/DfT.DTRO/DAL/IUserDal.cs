namespace DfT.DTRO.DAL;

public interface IUserDal
{
    Task DeleteUser(Guid userId);
}