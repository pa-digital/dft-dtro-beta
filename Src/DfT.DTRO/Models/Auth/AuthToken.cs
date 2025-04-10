namespace DfT.DTRO.Models.Auth;

/// <summary>
/// Auth token.
/// </summary>
[DataContract]
public class AuthToken
{
    /// <summary>
    /// access token
    /// </summary>
    [DataMember(Name = "access_token")]
    public string AccessToken { get; set; }

    /// <summary>
    /// issued at
    /// </summary>
    [DataMember(Name = "issued_at")]
    public long IssuedAt { get; set; }

    /// <summary>
    /// expires in
    /// </summary>
    [DataMember(Name = "expires_in")]
    public int ExpiresIn { get; set; }
}