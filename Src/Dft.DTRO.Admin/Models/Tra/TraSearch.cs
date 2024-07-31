
public class TraSearch
{
    public int? TraSelect { get; set; }

    public string Search { get; set; } = string.Empty;

    public  bool AlwaysButtonEnabled { get; set; }

    public string UpdateButtonText { get; set; }

    public List<SwaCodeResponse> SwaCodes { get; set; } = new List<SwaCodeResponse>();
}

