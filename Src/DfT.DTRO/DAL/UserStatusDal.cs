namespace DfT.DTRO.DAL;

/// <inheritdoc cref="IUserStatusDal"/>
public class UserStatusDal(DtroContext context) : IUserStatusDal
{
    /// <inheritdoc cref="IUserStatusDal"/>
    public List<UserStatus> GetStatuses() => context.UserStatuses.ToList();
}