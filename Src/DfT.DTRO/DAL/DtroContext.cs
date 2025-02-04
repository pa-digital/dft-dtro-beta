namespace DfT.DTRO.DAL;

/// <summary>
/// Represents a session with the D-TRO database.
/// </summary>
public class DtroContext : DbContext
{
    /// <summary>
    /// D-TROs table for create, read, update and delete operations.
    /// </summary>
    public virtual DbSet<DigitalTrafficRegulationOrder> DigitalTrafficRegulationOrders { get; set; }

    /// <summary>
    /// D-TRO Histories table for create, read, update and delete operations.
    /// </summary>
    public virtual DbSet<DigitalTrafficRegulationOrderHistory> DigitalTrafficRegulationOrderHistories { get; set; }

    /// <summary>
    /// Schema Templates table for create, read, update and delete operations.
    /// </summary>
    public virtual DbSet<SchemaTemplate> SchemaTemplate { get; set; }

    /// <summary>
    /// Rule Templates table for create, read, update and delete operations.
    /// </summary>
    public virtual DbSet<RuleTemplate> RuleTemplate { get; set; }

    /// <summary>
    /// Metrics table
    /// </summary>
    public virtual DbSet<Metric> Metrics { get; set; }

    /// <summary>
    /// D-TRO Users table for create, read, update and delete operations.
    /// </summary>
    public virtual DbSet<DtroUser> DtroUsers { get; set; }

    /// <summary>
    /// System Configuration table
    /// </summary>
    public virtual DbSet<SystemConfig> SystemConfig { get; set; }

    /// <summary>
    /// Traffic Regulation Authorities table for create, read, update and delete operations.
    /// </summary>
    public virtual DbSet<TrafficRegulationAuthority> TrafficRegulationAuthorities { get; set; }

    /// <summary>
    /// Digital Service Providers table for create, read, update and delete operations.
    /// </summary>
    public virtual DbSet<DigitalServiceProvider> DigitalServiceProviders { get; set; }

    /// <summary>
    /// Users table for create, read, update and delete operations.
    /// </summary>
    public virtual DbSet<User> Users { get; set; }

    /// <summary>
    /// User statuses table for create, read, update and delete operations.
    /// </summary>
    public virtual DbSet<UserStatus> UserStatuses { get; set; }

    /// <summary>
    /// Traffic Regulation Authority Digital Service Provider liaison table for create, read, update and delete operations.
    /// </summary>
    public virtual DbSet<TrafficRegulationAuthorityDigitalServiceProvider> TrafficRegulationAuthorityDigitalServiceProviders { get; set; }

    /// <summary>
    /// Traffic Regulation Authority Digital Service Provider Statuses table for create, read, update and delete operations.
    /// </summary>
    public virtual DbSet<TrafficRegulationAuthorityDigitalServiceProviderStatus> TrafficRegulationAuthorityDigitalServiceProviderStatuses { get; set; }

    /// <summary>
    /// Applications table for create, read, update and delete operations.
    /// </summary>
    public virtual DbSet<Application> Applications { get; set; }

    /// <summary>
    /// Application Purposes table for create, read, update and delete operations.
    /// </summary>
    public virtual DbSet<ApplicationPurpose> ApplicationPurposes { get; set; }

    /// <summary>
    /// Application Types table for create, read, update and delete operations.
    /// </summary>
    public virtual DbSet<ApplicationType> ApplicationTypes { get; set; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="options">Base context options.</param>

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
                new PostgresBinaryExpression(PostgresExpressionType.Overlaps, args[0], args[1], args[0].Type, args[0].TypeMapping));
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
