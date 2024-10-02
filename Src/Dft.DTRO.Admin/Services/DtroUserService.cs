using System.Text.Json.Serialization;
namespace Dft.DTRO.Admin.Services;
public class DtroUserService : IDtroUserService
{
    private readonly HttpClient _client;
    private readonly IXappIdService _xappIdService;
    private readonly IErrHandlingService _errHandlingService;

    private JsonSerializerOptions GetJsonOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };
        return options;
    }
    public DtroUserService(IHttpClientFactory clientFactory, IXappIdService xappIdService, IErrHandlingService errHandlingService)
    {
        _client = clientFactory.CreateClient("ExternalApi");
        _xappIdService = xappIdService;
        _errHandlingService = errHandlingService;
    }

    public async Task<List<DtroUser>> GetDtroUsersAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, ConfigHelper.Version + "/dtroUsers");
        await _xappIdService.AddXAppIdHeader(request);

        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);

        var jsonResponse = await response.Content.ReadAsStringAsync();

        var dtroUserList = JsonSerializer.Deserialize<List<DtroUser>>(jsonResponse, GetJsonOptions());

        if (dtroUserList == null)
        {
            dtroUserList = new List<DtroUser>();
        }
        return dtroUserList;
    }

    public async Task<DtroUser> GetDtroUserAsync(Guid dtroUserId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, ConfigHelper.Version + $"/dtroUsers/{dtroUserId}");
        await _xappIdService.AddXAppIdHeader(request);

        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var dtroUser = JsonSerializer.Deserialize<DtroUser>(jsonResponse, GetJsonOptions());
        if (dtroUser == null)
        {
            dtroUser = new DtroUser();
        }
        return dtroUser;
    }

    public async Task<DtroUser> GetDtroUserAsyncByXAppId(Guid xAppId)
    {
        List<DtroUser> users = await GetDtroUsersAsync();
        return users.Find(it => it.xAppId == xAppId) ?? new DtroUser();
    }

    public async Task<List<DtroUser>> SearchDtroUsersAsync(string partialName)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, ConfigHelper.Version + $"/dtroUsers/search/{partialName}");
        await _xappIdService.AddXAppIdHeader(request);

        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var dtroUserResponseList = JsonSerializer.Deserialize<List<DtroUser>>(jsonResponse, GetJsonOptions());
        if (dtroUserResponseList == null)
        {
            dtroUserResponseList = new List<DtroUser>();
        }
        return dtroUserResponseList;
    }

    public async Task ActivateDtroUserAsync(Guid dtroUserId)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, ConfigHelper.Version + $"/dtroUsers/activate/{dtroUserId}");
        await _xappIdService.AddXAppIdHeader(request);
        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);
    }

    public async Task DeactivateDtroUserAsync(Guid dtroUserId)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, ConfigHelper.Version + $"/dtroUsers/deactivate/{dtroUserId}");
        await _xappIdService.AddXAppIdHeader(request);
        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);
    }

    public async Task UpdateDtroUserAsync(DtroUser dtroUser)
    {

        var content = JsonContent.Create(dtroUser);
        var request = new HttpRequestMessage(HttpMethod.Put, ConfigHelper.Version + $"/dtroUsers/updateFromBody/")
        {
            Content = content
        };
        await _xappIdService.AddXAppIdHeader(request);
        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);
    }

    public async Task<bool> DeleteDtroUserAsync(List<Guid> dtroUserIds)
    {
        var content = new StringContent(JsonSerializer.Serialize(new { Ids = dtroUserIds }), Encoding.UTF8, "application/json");
        var request =
            new HttpRequestMessage(HttpMethod.Delete, ConfigHelper.Version + $"/dtroUsers/redundant")
            {
                Content = content
            };
        await _xappIdService.AddXAppIdHeader(request);

        var response = await _client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        await _errHandlingService.RedirectIfErrors(response);
        return false;
    }

    public async Task CreateDtroUserAsync(DtroUser dtroUser)
    {
        var content = JsonContent.Create(dtroUser);
        var request = new HttpRequestMessage(HttpMethod.Post, ConfigHelper.Version + $"/dtroUsers/createFromBody/")
        {
            Content = content
        };
        await _xappIdService.AddXAppIdHeader(request);
        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);
    }
}