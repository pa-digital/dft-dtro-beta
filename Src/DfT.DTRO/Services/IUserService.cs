namespace DfT.DTRO.Services;

public interface IUserService
{

    Task<PaginatedResponse<UserListDto>> GetUsers(PaginatedRequest paginatedRequest);

    Task DeleteUser(string email, Guid userId);
}