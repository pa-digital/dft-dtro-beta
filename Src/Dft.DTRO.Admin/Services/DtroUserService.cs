using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Dft.DTRO.Admin.Services;
public class DtroUserService : IDtroUserService
{
    private readonly HttpClient _client;

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
    public DtroUserService(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("ExternalApi");
    }

    public async Task<List<DtroUser>> GetDtroUsersAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/dtroUsers");
        Helper.AddHeaders(ref request);

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();

       

        // Deserialize the JSON response into a List<DtroUserResponse>
        //var dtroUserResponseList = JsonSerializer.Deserialize<List<DtroUserResponse>>(jsonResponse, GetJsonOptions());
        //var dtroUserList = dtroUserResponseList.Select(dtroUserResponse => new DtroUser
        //{
        //    Id = dtroUserResponse.Id,
        //    xAppId = dtroUserResponse.xAppId,
        //    TraId = dtroUserResponse.TraId,
        //    Name = dtroUserResponse.Name,
        //    Prefix = dtroUserResponse.Prefix,
        //    UserGroup = dtroUserResponse.UserGroup
        //}).ToList();

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
        Helper.AddHeaders(ref request);

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

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
        Helper.AddHeaders(ref request);

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

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
        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeactivateDtroUserAsync(Guid dtroUserId)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/dtroUsers/deactivate/{dtroUserId}");
        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateDtroUserAsync(DtroUser dtroUser)
    {

        var content = JsonContent.Create(dtroUser);
        var request = new HttpRequestMessage(HttpMethod.Put, $"/dtroUsers/updateFromBody/")
        {
            Content = content
        };
        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateDtroUserAsync(DtroUser dtroUser)
    {
        var content = JsonContent.Create(dtroUser);
        var request = new HttpRequestMessage(HttpMethod.Post, $"/dtroUsers/createFromBody/")
        {
            Content = content
        };
        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}