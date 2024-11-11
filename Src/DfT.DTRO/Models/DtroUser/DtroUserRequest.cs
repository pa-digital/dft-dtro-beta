namespace DfT.DTRO.Models.SwaCode;

public class DtroUserRequest
{
    public Guid Id { get; set; }

    public int? TraId { get; set; }

    public string Name { get; set; }

    public string Prefix { get; set; }

    public UserGroup UserGroup { get; set; }

    public Guid xAppId { get; set; }
}