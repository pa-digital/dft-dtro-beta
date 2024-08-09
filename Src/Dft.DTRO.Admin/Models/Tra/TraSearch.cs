namespace Dft.DTRO.Admin.Models.Tra;

public class TraSearch
{
    public int? TraSelect { get; set; }

    public string Search { get; set; } = string.Empty;

    public bool AlwaysButtonEnabled { get; set; }

    public string UpdateButtonText { get; set; } = string.Empty;

    public List<SwaCode> SwaCodes { get; set; } = new List<SwaCode>();

    public int? PreviousTraSelect { get; set; }

    public string PreviousSearch { get; set; } = string.Empty;
}

