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
        var request = new HttpRequestMessage(HttpMethod.Get, "/dtroUsers");
        _xappIdService.AddXAppIdHeader(ref request);

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
        var request = new HttpRequestMessage(HttpMethod.Get, $"/dtroUsers/{dtroUserId}");
        _xappIdService.AddXAppIdHeader(ref request);

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

    public async Task<List<DtroUser>> SearchDtroUsersAsync(string partialName)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/dtroUsers/search/{partialName}");
        _xappIdService.AddXAppIdHeader(ref request);

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
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/dtroUsers/activate/{dtroUserId}");
        _xappIdService.AddXAppIdHeader(ref request);
        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);
    }

    public async Task DeactivateDtroUserAsync(Guid dtroUserId)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/dtroUsers/deactivate/{dtroUserId}");
        _xappIdService.AddXAppIdHeader(ref request);
        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);
    }

    public async Task UpdateDtroUserAsync(DtroUser dtroUser)
    {

        var content = JsonContent.Create(dtroUser);
        var request = new HttpRequestMessage(HttpMethod.Put, $"/dtroUsers/updateFromBody/")
        {
            Content = content
        };
        _xappIdService.AddXAppIdHeader(ref request);
        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);
    }

    public async Task CreateDtroUserAsync(DtroUser dtroUser)
    {
        var content = JsonContent.Create(dtroUser);
        var request = new HttpRequestMessage(HttpMethod.Post, $"/dtroUsers/createFromBody/")
        {
            Content = content
        };
        _xappIdService.AddXAppIdHeader(ref request);
        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);
    }
}