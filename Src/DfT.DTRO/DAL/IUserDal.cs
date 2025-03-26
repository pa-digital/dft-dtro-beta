namespace DfT.DTRO.DAL;

public interface IUserDal
{
    PaginatedResult<UserListDto> GetUsers(PaginatedRequest paginatedRequest);

    Task DeleteUser(Guid userId);
}