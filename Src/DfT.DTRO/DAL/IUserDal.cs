namespace DfT.DTRO.DAL;

public interface IUserDal
{
    PaginatedResult<User> GetUsers(Guid userId);
}