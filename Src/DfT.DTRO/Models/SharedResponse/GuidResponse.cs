namespace DfT.DTRO.Models.SharedResponse;

public class GuidResponse
{
    public Guid Id { get; set; }

    public GuidResponse()
    {
        Id = Guid.NewGuid();
    }
}