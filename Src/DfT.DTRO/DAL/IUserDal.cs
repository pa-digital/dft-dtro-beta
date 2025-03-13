namespace DfT.DTRO.DAL;

public interface IUserDal
{
    Task<PaginatedResult<UserListDto>> GetUsers(PaginatedRequest paginatedRequest);

    Task DeleteUser(Guid userId);
}