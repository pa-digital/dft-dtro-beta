using System.Globalization;


namespace DfT.DTRO.Services;

public class DtroUserService : IDtroUserService
{
    private readonly IDtroUserDal _dtroUserDal;
    private readonly IMetricDal _metricsDal;

    public DtroUserService(IDtroUserDal dtroUserDal, IMetricDal metricsDal)
    {
        _dtroUserDal = dtroUserDal;
        _metricsDal = metricsDal;
    }

    private List<DtroUserResponse> FormatUserNameForUi(List<DtroUserResponse> responses)
    {
        var ret = new List<DtroUserResponse>();
        foreach (var response in responses)
        {
            ret.Add(FormatNameForUi(response));
        }

        return ret;
    }

    private DtroUserResponse FormatNameForUi(DtroUserResponse response)
    {
        var sb = new StringBuilder();
        string[] words = response.Name.Split(' ');

        bool isFirstWordInBrackets = false;
        if (words.Length > 0 && words[0].StartsWith("(") && words[0].EndsWith(")"))
        {
            isFirstWordInBrackets = true;
        }

        if (words.Length > 0)
        {
            if (!isFirstWordInBrackets && words[0].Length >= 4)
            {
                words[0] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[0].ToLower());
            }

            sb.Clear();
            sb.Append(words[0]);

            for (int i = 1; i < words.Length; i++)
            {
                sb.Append(' ');
                sb.Append(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[i].ToLower()));
            }

            response.Name = sb.ToString();
        }

        return response;
    }

    public async Task<List<DtroUserResponse>> SearchDtroUsers(string partialName)
    {
        var responses = await _dtroUserDal.SearchDtroUsersAsync(partialName);
        responses = FormatUserNameForUi(responses);
        return responses;
    }

    public async Task<List<DtroUserResponse>> GetAllDtroUsersAsync()
    {
        var responses = await _dtroUserDal.GetAllDtroUsersAsync();
        responses = FormatUserNameForUi(responses);
        return responses;
    }

    public async Task<DtroUserResponse> GetDtroUserAsync(Guid id)
    {
        var response = await _dtroUserDal.GetDtroUserResponseAsync(id);
        response = FormatNameForUi(response);
        return response;
    }

    public async Task<GuidResponse> SaveDtroUserAsync(DtroUserRequest dtroUserRequest)
    {

        var guid = await _dtroUserDal.SaveDtroUserAsync(dtroUserRequest);
        return guid;
    }

    public async Task<GuidResponse> UpdateDtroUserAsync(DtroUserRequest dtroUserRequest)
    {
        var exisitng = await _dtroUserDal.GetDtroUserByIdAsync(dtroUserRequest.Id);
        var guid = await _dtroUserDal.UpdateDtroUserAsync(dtroUserRequest);
        if (exisitng.UserGroup != dtroUserRequest.UserGroup)
        {
            await _metricsDal.UpdateUserGroupForMetricsAsync(dtroUserRequest.Id, dtroUserRequest.UserGroup);
        }

        return guid;
    }

    public async Task<bool> DeleteDtroUsersAsync(List<Guid> dtroUserIds)
    {
        List<DtroUser> users = (await _dtroUserDal.GetAllDtroUsersAsync())
            .Where(response => dtroUserIds.Contains(response.Id))
            .Select(it => new DtroUser { Id = it.Id })
            .ToList();
        if (!users.Any())
        {
            return false;
        }

        return await _dtroUserDal.DeleteDtroUsersAsync(users);
    }
}