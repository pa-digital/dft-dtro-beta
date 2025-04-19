public class ErrorReport
{
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid? TroId { get; set; }
    public DTRO? Dtro { get; set; }

    public List<string> Tras { get; set; }
    public List<string> RegulationTypes { get; set; }
    public List<string> TroTypes { get; set; }
    public string Type { get; set; }
    public string? OtherType { get; set; }
    public string MoreInformation { get; set; }
    
    public string? FilePaths { get; set; }
}
