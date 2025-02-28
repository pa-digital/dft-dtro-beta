namespace DfT.DTRO.Models.Auth;

/// <summary>
/// Auth token input.
/// </summary>
[DataContract]
public class AuthTokenInput
{
    /// <summary>
    /// username.
    /// </summary>
    [DataMember(Name = "username")]
    public string Username { get; set; }
    
    /// <summary>
    /// password.
    /// </summary>
    [DataMember(Name = "password")]
    public string Password { get; set; }
}
