namespace DfT.DTRO.Models.PortalUser;

public class UserAppDto
{
    public string User  { get; set; }
    
    public string UserId { get; set; }
    
    public List<ApplicationInfo> Apps { get; set; }
}