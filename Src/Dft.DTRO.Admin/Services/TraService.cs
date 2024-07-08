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

        var look = new LookupResponse() { Id = 1, Name = "Alpha" };
        lookups.Add(look);

        look = new LookupResponse() { Id = 21, Name = "Beta" };
        lookups.Add(look);

        look = new LookupResponse() { Id = 3, Name = "Gamma" };
        lookups.Add(look);

        look = new LookupResponse() { Id = 41, Name = "Delta" };
        lookups.Add(look);

        look = new LookupResponse() { Id = 5, Name = "Zeta" };
        lookups.Add(look);

        look = new LookupResponse() { Id = 6, Name = "Eta" };
        lookups.Add(look);

        look = new LookupResponse() { Id = 7, Name = "Theta" };
        lookups.Add(look);
        return lookups;
    }
}