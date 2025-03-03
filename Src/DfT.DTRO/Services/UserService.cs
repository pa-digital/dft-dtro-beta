namespace DfT.DTRO.Services;

/// <inheritdoc cref="IUserService"/>
public class UserService(IUserDal userDal, IUserStatusDal userStatusDal) : IUserService
{
    /// <inheritdoc cref="IUserService"/>
    public PaginatedResponse<UserDto> GetUsers(UserRequest request)
    {
        var users = userDal.GetUsers(request);

        var userStatuses = userStatusDal.GetStatuses();

        var dto = users.Results.Select(user => new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Created = user.Created.ToString("G"),
            LastUpdated = user.LastUpdated.HasValue ? user.LastUpdated.Value.ToString("G") : string.Empty,
            IsCentralServiceOperator = user.IsCentralServiceOperator,
            Username = $"{user.Forename} {user.Surname}",
            Status = userStatuses.FirstOrDefault(userStatus => userStatus.Id == user.UserStatus.Id)?.Status
        }).ToList();

        return new PaginatedResponse<UserDto>(dto, request.Page, request.PageSize);
    }
}