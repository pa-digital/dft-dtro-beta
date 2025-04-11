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
}