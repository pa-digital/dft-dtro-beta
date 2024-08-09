namespace Dft.DTRO.Admin.Models.DtroHistory;

public class DtroHistoryProvisionResponse
{
    public string ActionType { get; set; }

    public DateTime? Created { get; set; }

    public ExpandoObject Data { get; set; }

    public DateTime? LastUpdated { get; set; }

    public string Reference { get; set; }

    public string SchemaVersion { get; set; }
}
