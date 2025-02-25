using System.Net.Http;
using System.Net.Http.Headers;
using DfT.DTRO.Models.App;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;

namespace DfT.DTRO.Apis.Clients;

public class ApigeeClient : IApigeeClient
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly SecretManagerClient _secretManagerClient;
    
    private string json = "{\n  \"type\": \"service_account\",\n  \"project_id\": \"dft-dtro-dev-01\",\n  \"private_key_id\": \"f6c105424c9176a4a8bd05488c79c5d819af38b7\",\n  \"private_key\": \"-----BEGIN PRIVATE KEY-----\\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCqhSCq1gmf2Kju\\nBRNQw3YaUTkwUu0l3bRp0bpsXwAFjoufEZIu3cmoHK06ECD2hrvtWKk5I03zzVgI\\nZZJ1lj2lmgQJB2rva1ZOubOMO4rGNQuFubDCaLE3SiTjp4ENTtf77OQOGRoSs0qx\\nDjeBHJdOUttcH8s+T26MQekeYK1WEAZjFeOjykKKI2jSoHEpU2azVpUfl2AjGKq+\\nhYU0trjLTVy/cY9zB0lqk8RM424HWpPbxQc9aB1HUsMfLMyovIQ5DEVaUj9nuTjZ\\ns90tbKoR3PbY1IF9Fc8wzJvaK7FUOzK5PYDk2LUehraj83YPHIDAMjRNbikoEH7A\\n5tVtOVYJAgMBAAECggEATD1AaVFQNUkj4mOfirURmVumY544DH2PG97a6HgjJNji\\nQQkUoGKmNkU4CCVM75w26rwA0RyhTjeJvBtZu7wkLOXrFvRbkdceXA14IuM+PTdb\\nQvtOdeVkEpHSY3yxXPda5va30jvT5feGlhoCo/XaCnlOPWazstTF2uzfIydbaIX/\\nw9yMNeOqgIRnu8OsDqU6HjuNuZwtGR+LZVCxGY4cMh/zcZ3cT+rV0iA2dNRJ4VFR\\nfjBFW3eIcJZ7q1p8CtdEdAvuVyz8yfscW92A8ZxDEYJDAKnNrCYQTsrOEQrIym2J\\nCBj3rEghztxEfgOw2wmcKk3VKtJ1Eocm3Yj14WP2bwKBgQDXKDV20VmkJbPVSUUA\\nDoUMnUXJo0eEEZB08BWYYCZHDomUylwxjXV7Ga7VhQ29s/9weX5ntuXHNWMQj8BC\\nI4epexLZiU1cKB5g4vj8pDyqjB0trEJGwSYKJlG0tupPwjtyBQIOAKUbwKlVoZYU\\nbClQ4Pn1EuaVg/yFWJuDyaG+JwKBgQDK47vJcN+QNyC6spP5e5ApUEksax+NZDKv\\nmRTCAUXSTFxb8bwQjnwTVjFcc+TrYph1crqvp+AxDSdTl9loBTpP9vlROsdsyGAQ\\naqIKSJGTzxvaiOGOZLGU9IGUOQ8sJuWNND4/Hp4A085QTnS36PQYBpZpdxHhZe55\\nAG7mayQYTwKBgQDCuvagoluicCkyCg10Pq/2ucU2+Ru6EXeQDtdMwQED4MlurDQC\\n+Ufx8U3GnJTSND4l2yAn2GZhBRWzIJfehG+9WdG2p0kn9nuALw0xA6iQpx2lf7nA\\ndgHqv8HFcKPjKiVJTfUNHVJSKu5XvVarBzXhqK0yPTgK7Jk5Svf9sBk+kwKBgQCq\\ncUaAo+IsNSO5s+u72qHxExFlz1hiX5p1ChT2JjuRv7SXSSzEe+6gD/jHwlS9qcaK\\nZCeRCWa9zl8LSrXJPRsPslsgclC2gl6xa+NU4EHr+kFcBUH3bYABsqZo7sZgQQz2\\n4ryoKgBykXzw2fgTyA/HX0FcuDq9L/R2knaX+8oG4QKBgAvyYkkNjIf5XEQRkcuR\\njpGjbCRlrE18LAPAnKktTQmdnsGt+PZ82NJnuLLPH6uwK3gI+v1Wf2Ksv9uUqqyX\\nnuhVBIIX/HClwHAXdEhYHAF0gwRLVw+QKAfsEBOogw2zFJ3j61vwQc34EzhKV7En\\nkwkQ6EWW5OInSairrcabQqnD\\n-----END PRIVATE KEY-----\\n\",\n  \"client_email\": \"sa-deployment@dft-dtro-dev-01.iam.gserviceaccount.com\",\n  \"client_id\": \"111156452995261606422\",\n  \"auth_uri\": \"https://accounts.google.com/o/oauth2/auth\",\n  \"token_uri\": \"https://oauth2.googleapis.com/token\",\n  \"auth_provider_x509_cert_url\": \"https://www.googleapis.com/oauth2/v1/certs\",\n  \"client_x509_cert_url\": \"https://www.googleapis.com/robot/v1/metadata/x509/sa-deployment%40dft-dtro-dev-01.iam.gserviceaccount.com\",\n  \"universe_domain\": \"googleapis.com\"\n}\n";

    public ApigeeClient(IConfiguration configuration, HttpClient httpClient, SecretManagerClient secretManagerClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _secretManagerClient = secretManagerClient;
    }
    
    public async Task<HttpResponseMessage> CreateApp(AppInput appInput)
    {
        var requestUri = $"developers/{appInput.Username}/apps";
        return await SendRequest(HttpMethod.Post, requestUri, appInput);
    }
    
    private async Task<HttpResponseMessage> SendRequest(HttpMethod method, string requestUri, object requestMessageContent)
    {
        string secret = _secretManagerClient.GetSecret("d-tro-dev-db-access");
        GoogleCredential credential = GoogleCredential.FromJson(secret).CreateScoped("https://www.googleapis.com/auth/cloud-platform");
        var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
        var apiUrl = _configuration.GetValue<string>("ApiSettings:ApigeeApiUrl");
        var requestMessage = new HttpRequestMessage(method, $"{apiUrl}{requestUri}");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var content = JsonConvert.SerializeObject(requestMessageContent);
        requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
        return await _httpClient.SendAsync(requestMessage);
    }
}