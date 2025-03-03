namespace DfT.DTRO.DTO;

public class UserDto
{
    public Guid Id { get; set; }
    public string Created { get; set; }
    public string LastUpdated { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public bool IsCSO { get; set; }
    public string Status { get; set; }
}