public class ApplicationDetailsDto
{
    public string Name { get; set; }
    public Guid AppId { get; set; }
    public string Purpose { get; set; }
}

public class ApplicationListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Tra { get; set; }
}

public class ApplicationPendingListDto
{
    public string TraName { get; set; }
    public string Type { get; set; }
    public string UserEmail { get; set; }
    public string UserName { get; set; }
}