namespace DfT.DTRO.DAL;

public interface IUserDal
{
    Task<PaginatedResult<UserListDto>> GetUsers(PaginatedRequest paginatedRequest);

    Task DeleteUser(Guid userId);

    Task<User> GetUserFromEmail(string email);
    Task<bool> HasUserRequestedProductionAccess(Guid userId);
    Task RequestProductionAccess(Guid userId);
}