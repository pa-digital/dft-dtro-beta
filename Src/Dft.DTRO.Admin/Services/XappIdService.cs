using System.Net.Http.Headers;

namespace Dft.DTRO.Admin.Services;
public class XappIdService : IXappIdService
{
    private Guid _xAppIdOverride = Guid.Empty;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _tokenEndpoint;
    private TokenResponse _token;

    public XappIdService(IConfiguration configuration)
    {
        _clientId = ConfigHelper.ClientId;
        _clientSecret = ConfigHelper.ClientSecret;
        _tokenEndpoint = ConfigHelper.TokenEndpoint;

        bool environmentVariableExists = !string.IsNullOrEmpty(_clientId);

        if (!environmentVariableExists)
        {
            _clientId = ConfigHelper.ClientId;
            _clientSecret = ConfigHelper.ClientSecret;
            _tokenEndpoint = ConfigHelper.TokenEndpoint;
        }


        var xAppIdConfigValue = ConfigHelper.XAppIdOverride;

        if (!string.IsNullOrEmpty(xAppIdConfigValue) && Guid.TryParse(xAppIdConfigValue, out Guid configXAppId))
        {
            _xAppIdOverride = configXAppId;
        }
    }

    public async Task<bool> AddXAppIdHeader(HttpRequestMessage httpRequestMessage)
    {
        if (!string.IsNullOrEmpty(_tokenEndpoint))
        {
            var accessToken = await GetAccessTokenAsync();
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        if (_xAppIdOverride != Guid.Empty)
        {
            httpRequestMessage.Headers.Add("x-app-id-override", _xAppIdOverride.ToString());
        }
        if (string.IsNullOrEmpty(_tokenEndpoint))
        {
            httpRequestMessage.Headers.Add("x-app-id", _xAppIdOverride.ToString());
        }

        httpRequestMessage.Headers.Add("X-Correlation-ID", Guid.NewGuid().ToString());

        return true;
    }

    public Guid MyXAppId()
    {
        return _xAppIdOverride;
    }

    public void ChangeXAppId(Guid guid)
    {
        _xAppIdOverride = guid;
    }

    private async Task<string> GetAccessTokenAsync()
    {
        if (_token == null || IsTokenExpired())
        {
            _token = await RequestNewTokenAsync();
        }
        return _token.access_token;
    }

    private bool IsTokenExpired()
    {
        if (_token == null)
            return true;
        var issuedAt = ConvertUnixTimestampToDateTime(Convert.ToInt64(_token.issued_at));
        var expirationTime = issuedAt.AddSeconds(Convert.ToInt64(_token.expires_in));
        return expirationTime <= DateTime.UtcNow;
    }

    private async Task<TokenResponse> RequestNewTokenAsync()
    {
        using (var httpClient = new HttpClient())
        {
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, _tokenEndpoint);
            var requestBody = new StringContent(
                $"grant_type=client_credentials&client_id={_clientId}&client_secret={_clientSecret}",
                Encoding.UTF8,
                "application/x-www-form-urlencoded");

            tokenRequest.Content = requestBody;

            var response = await httpClient.SendAsync(tokenRequest);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to retrieve access token.");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);

            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.access_token))
            {
                throw new Exception("Invalid oAuth token response.");
            }
            if (_xAppIdOverride == Guid.Empty)
            {
                _xAppIdOverride = new Guid(tokenResponse.application_name);
            }
            return tokenResponse;
        }
    }
    public DateTime ConvertUnixTimestampToDateTime(long unixTimeMilliseconds)
    {
        // Unix epoch starts from 1970-01-01T00:00:00Z
        var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMilliseconds);
        return dateTimeOffset.UtcDateTime;
    }

    private class TokenResponse
    {
        public string organization_name { get; set; }
        public string token_type { get; set; }
        public string issued_at { get; set; }
        public string client_id { get; set; }
        public string access_token { get; set; }
        public string application_name { get; set; }
        public string scope { get; set; }
        public string expires_in { get; set; }
        public string refresh_count { get; set; }
        public string status { get; set; }
    }
}
