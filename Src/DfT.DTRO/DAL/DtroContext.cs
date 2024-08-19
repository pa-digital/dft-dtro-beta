namespace DfT.DTRO.DAL;

/// <summary>
/// Represents a session with the D-TRO database.
/// </summary>
public class DtroContext : DbContext
{
    /// <summary>
    /// D-TRO table for create, read, update and delete operations.
    /// </summary>
    public virtual DbSet<Models.DataBase.DTRO> Dtros { get; set; }

    /// <summary>
    /// D-TRO history table for read operations.
    /// </summary>
    public virtual DbSet<DTROHistory> DtroHistories { get; set; }

    /// <summary>
    /// Schema Template table for create, read and update operations.
    /// </summary>
    public virtual DbSet<SchemaTemplate> SchemaTemplate { get; set; }

    /// <summary>
    /// Rule Template table for create, read and update operations.
    /// </summary>
    public virtual DbSet<RuleTemplate> RuleTemplate { get; set; }

    /// <summary>
    /// Metric table for read operations.
    /// </summary>
    public virtual DbSet<Metric> Metrics { get; set; }

    /// <summary>
    /// SWA Codes table for create, read and update operations.
    /// </summary>
    public virtual DbSet<DtroUser> DtroUsers { get; set; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="options">Base context options.</param>
    public virtual DbSet<SystemConfig> SystemConfig { get; set; }

    public DtroContext(DbContextOptions<DtroContext> options)
        : base(options)
    {
    }

    ///<inheritdoc />
    [SuppressMessage(
        "Usage",
        "EF1001:Internal EF Core API usage.",
        Justification = "Usage of this API is the easiest workaround for the time being.")]
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDbFunction(typeof(DatabaseMethods).GetMethod(nameof(DatabaseMethods.Overlaps), new[] { typeof(NpgsqlBox), typeof(NpgsqlBox) }))
            .HasTranslation(args =>
            {
                return new PostgresBinaryExpression(
                    PostgresExpressionType.Overlaps, args[0], args[1], args[0].Type, args[0].TypeMapping);
            });
    }

    ///<inheritdoc />
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<SchemaVersion>()
            .HaveConversion<SchemaVersionValueConverter>();

        configurationBuilder
            .Properties<ExpandoObject>()
            .HaveColumnType("jsonb")
            .HaveConversion<ExpandoObjectValueConverter>();

        configurationBuilder
            .Properties<BoundingBox>()
            .HaveConversion<BoundingBoxValueConverter>();
    }
}
