using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dft.DTRO.Admin.Services;
public class TraService : ITraService
{
    private readonly HttpClient _client;

    public TraService(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("ExternalApi");
    }


    public async Task<List<LookupResponse>> GetTraLookup()
    {
        List<LookupResponse> lookups = new();

        var look = new LookupResponse() { Id = 1, Name = "Peter Hopins" };
        lookups.Add(look);

        look = new LookupResponse() { Id = 21, Name = "John Peters" };
        lookups.Add(look);

        look = new LookupResponse() { Id = 3, Name = "Paul Hope" };
        lookups.Add(look);

        look = new LookupResponse() { Id = 41, Name = "Peter Bowles" };
        lookups.Add(look);

        look = new LookupResponse() { Id = 5, Name = "John Holly" };
        lookups.Add(look);

        look = new LookupResponse() { Id = 6, Name = "Paul Peters" };
        lookups.Add(look);

        return lookups;
    }
}