namespace DfT.DTRO.DAL;

/// <summary>
/// Implementation of the <see cref="IDtroUserDal"/> service.
/// </summary>
[ExcludeFromCodeCoverage]
public class DtroUserDal : IDtroUserDal
{
    private readonly DtroContext _dtroContext;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="dtroContext"><see cref="DtroContext"/> database context.</param>
    public DtroUserDal(DtroContext dtroContext)
    {
        _dtroContext = dtroContext;
    }

    ///<inheritdoc cref="IDtroUserDal"/>
    public async Task<DtroUserResponse> GetDtroUserByIdAsync(Guid userId)
    {
        var user = await _dtroContext.DtroUsers
            .Where(dtroUser => dtroUser.Id == userId)
            .Select(dtroUser => new DtroUserResponse
            {
                Id = dtroUser.Id,
                TraId = dtroUser.TraId,
                Name = dtroUser.Name,
                Prefix = dtroUser.Prefix,
                UserGroup = (UserGroup)dtroUser.UserGroup,
                xAppId = dtroUser.xAppId
            })
            .FirstOrDefaultAsync();

        return user;
    }

    ///<inheritdoc cref="IDtroUserDal"/>
    public async Task<List<DtroUserResponse>> GetAllDtroUsersAsync(List<Guid> ids) =>
        await _dtroContext
            .DtroUsers
            .Where(user => ids.Contains(user.Id))
            .Select(user => new DtroUserResponse
            {
                Id = user.Id,
                Name = user.Name,
                TraId = user.TraId,
                xAppId = user.xAppId
            }).ToListAsync();

    ///<inheritdoc cref="IDtroUserDal"/>
    public async Task<List<DtroUserResponse>> GetAllDtroUsersAsync()
    {
        var admins = await _dtroContext.DtroUsers
              .OrderBy(dtroUser => dtroUser.Name)
              .Select(dtroUser => new DtroUserResponse
              {
                  Id = dtroUser.Id,
                  TraId = dtroUser.TraId,
                  Name = dtroUser.Name,
                  Prefix = dtroUser.Prefix,
                  UserGroup = (UserGroup)dtroUser.UserGroup,
                  xAppId = dtroUser.xAppId
              }).Where(x => x.UserGroup == UserGroup.Admin)
              .ToListAsync();

        var nonAdmins = await _dtroContext.DtroUsers
            .OrderBy(dtroUser => dtroUser.Name)
            .Select(dtroUser => new DtroUserResponse
            {
                Id = dtroUser.Id,
                TraId = dtroUser.TraId,
                Name = dtroUser.Name,
                Prefix = dtroUser.Prefix,
                UserGroup = (UserGroup)dtroUser.UserGroup,
                xAppId = dtroUser.xAppId
            }).Where(x => x.UserGroup != UserGroup.Admin)
            .ToListAsync();
        admins.AddRange(nonAdmins);
        return admins;
    }
    ///<inheritdoc cref="IDtroUserDal"/>
    public async Task<DtroUserResponse> GetDtroUserResponseAsync(Guid id)
    {
        var dtroUser = await _dtroContext.DtroUsers
            .Where(dtroUser => dtroUser.Id == id)
            .Select(dtroUser => new DtroUserResponse
            {
                Id = dtroUser.Id,
                TraId = dtroUser.TraId,
                Name = dtroUser.Name,
                Prefix = dtroUser.Prefix,
                UserGroup = (UserGroup)dtroUser.UserGroup,
                xAppId = dtroUser.xAppId
            })
            .FirstOrDefaultAsync();

        if (dtroUser == null)
        {
            throw new NotFoundException($"There is no DtroUser with Id {id}");
        }

        return dtroUser;
    }

    ///<inheritdoc cref="IDtroUserDal"/>
    public async Task<List<DtroUserResponse>> SearchDtroUsersAsync(string partialName) =>
     await _dtroContext.DtroUsers
         .Where(dtroUser => EF.Functions.Like(dtroUser.Name.ToLower(), $"%{partialName.ToLower()}%"))
         .OrderBy(dtroUser => dtroUser.Name)
         .Select(dtroUser => new DtroUserResponse
         {
             Id = dtroUser.Id,
             TraId = dtroUser.TraId,
             Name = dtroUser.Name,
             Prefix = dtroUser.Prefix,
             UserGroup = (UserGroup)dtroUser.UserGroup,
             xAppId = dtroUser.xAppId
         })
         .ToListAsync();

    ///<inheritdoc cref="IDtroUserDal"/>
    public async Task<bool> TraExistsAsync(int traId) =>
        await _dtroContext.DtroUsers.AnyAsync(dtroUser => dtroUser.TraId == traId);

    public async Task<bool> AnyAdminUserExistsAsync()
    {
        var exists = await _dtroContext.DtroUsers.AnyAsync(it => it.xAppId != Guid.Empty && it.UserGroup == (int)UserGroup.Admin);
        return exists;
    }

    ///<inheritdoc cref="IDtroUserDal"/>
    public async Task<bool> DtroUserExistsAsync(Guid guid)
    {
        var exists = await _dtroContext.DtroUsers.AnyAsync(it => it.Id == guid);
        return exists;
    }

    ///<inheritdoc cref="IDtroUserDal"/>
    public async Task<DtroUser> GetDtroUserAsync(Guid id)
    {
        var ret = await _dtroContext.DtroUsers.FirstOrDefaultAsync(b => b.Id == id);
        return ret;
    }

    ///<inheritdoc cref="IDtroUserDal"/>
    public async Task<DtroUser> GetDtroUserByTraIdAsync(int traId)
    {
        var ret = await _dtroContext.DtroUsers.FirstOrDefaultAsync(b => b.TraId == traId);
        return ret;
    }

    ///<inheritdoc cref="IDtroUserDal"/>
    public async Task<DtroUser> GetDtroUserOnAppIdAsync(Guid appid)
    {
        var ret = await _dtroContext.DtroUsers.FirstOrDefaultAsync(b => b.xAppId == appid);
        return ret;
    }

    ///<inheritdoc cref="IDtroUserDal"/>
    public async Task<GuidResponse> SaveDtroUserAsync(DtroUserRequest dtroUserRequest)
    {
        var dtroUser = new DtroUser();
        var response = new GuidResponse();

        if (dtroUserRequest.Id == Guid.Empty)
        {
            throw new InvalidOperationException("System ID cannot be zero value");
        }

        if (string.IsNullOrEmpty(dtroUserRequest.Name))
        {
            throw new InvalidOperationException("Name cannot be null");
        }

        if (dtroUserRequest.xAppId == Guid.Empty)
        {
            throw new InvalidOperationException("App Id cannot be zero value");
        }

        dtroUser.Id = response.Id;
        dtroUser.TraId = dtroUserRequest.TraId;
        dtroUser.Name = dtroUserRequest.Name;
        dtroUser.Prefix = dtroUserRequest.Prefix ?? "";
        dtroUser.UserGroup = (int)dtroUserRequest.UserGroup;
        dtroUser.xAppId = dtroUserRequest.xAppId;

        await _dtroContext.DtroUsers.AddAsync(dtroUser);

        await _dtroContext.SaveChangesAsync();
        return response;
    }

    ///<inheritdoc cref="IDtroUserDal"/>
    public async Task<GuidResponse> UpdateDtroUserAsync(DtroUserRequest dtroUserRequest)
    {
        if (!await DtroUserExistsAsync(dtroUserRequest.Id))
        {
            throw new InvalidOperationException($"There is no DtroUser with Id {dtroUserRequest.Id}");
        }

        var existing = await GetDtroUserAsync(dtroUserRequest.Id);

        existing.TraId = dtroUserRequest.TraId;
        existing.Name = dtroUserRequest.Name;
        existing.Prefix = dtroUserRequest.Prefix ?? "";
        existing.UserGroup = (int)dtroUserRequest.UserGroup;
        existing.xAppId = dtroUserRequest.xAppId;

        await _dtroContext.SaveChangesAsync();
        return new GuidResponse() { Id = existing.Id };
    }

    ///<inheritdoc cref="IDtroUserDal"/>
    public async Task<bool> DeleteDtroUsersAsync(List<DtroUser> users)
    {
        try
        {
            _dtroContext.DtroUsers.RemoveRange(users);
            await _dtroContext.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }
}