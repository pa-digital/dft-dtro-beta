namespace DfT.DTRO.Services;

public interface IUserService
{

    PaginatedResponse<UserListDto> GetUsers(PaginatedRequest paginatedRequest);

    Task DeleteUser(Guid userId);
}