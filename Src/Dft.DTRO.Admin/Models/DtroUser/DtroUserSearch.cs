namespace Dft.DTRO.Admin.Models.DtroUser;

public class DtroUserSearch
{
    public Guid? DtroUserIdSelect { get; set; }

    public string Search { get; set; } = string.Empty;

    public bool AlwaysButtonEnabled { get; set; }

    public bool AlwaysButtonHidden { get; set; } = false;

    public string UpdateButtonText { get; set; } = string.Empty;

    public List<DtroUser> DtroUsers { get; set; } = new List<DtroUser>();

    public Guid? PreviousDtroUserIdSelect { get; set; }

    public string PreviousSearch { get; set; } = string.Empty;
    
    public string SearchTerm { get; set; }
    public string SelectedRegulation { get; set; }
    public string VehicleType { get; set; }
    
}

