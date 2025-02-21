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
    /// System configuration table
    /// </summary>
    public virtual DbSet<SystemConfig> SystemConfig { get; set; }


    /// <summary>
    /// Users table
    /// </summary>
    public virtual DbSet<User> Users { get; set; }

    /// <summary>
    /// User Statuses table
    /// </summary>
    public virtual DbSet<UserStatus> UserStatuses { get; set; }

    /// <summary>
    /// Digital Service Provider table
    /// </summary>
    public virtual DbSet<DigitalServiceProvider> DigitalServiceProviders { get; set; }

    /// <summary>
    /// Application Type table
    /// </summary>
    public virtual DbSet<ApplicationType> ApplicationTypes { get; set; }

    /// <summary>
    /// Application Purpose table
    /// </summary>
    public virtual DbSet<ApplicationPurpose> ApplicationPurposes { get; set; }

    /// <summary>
    /// Application table
    /// </summary>
    public virtual DbSet<Application> Applications { get; set; }

    /// <summary>
    /// Traffic Regulation Authority table
    /// </summary>
    public virtual DbSet<TrafficRegulationAuthority> TrafficRegulationAuthorities { get; set; }

    /// <summary>
    /// Digital Service Provider table
    /// </summary>
    public virtual DbSet<DigitalServiceProvider> ServiceProviders { get; set; }

    /// <summary>
    /// Traffic Regulation Authority Digital Service Provider table
    /// </summary>
    public virtual DbSet<TrafficRegulationAuthorityDigitalServiceProvider> TrafficRegulationAuthorityDigitalServiceProviders { get; set; }

    /// <summary>
    /// Traffic Regulation Authority Digital Service Provider Status table
    /// </summary>
    public virtual DbSet<TrafficRegulationAuthorityDigitalServiceProviderStatus> TrafficRegulationAuthorityDigitalServiceProviderStatuses { get; set; }

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
        modelBuilder.HasDbFunction(typeof(DatabaseMethods)
                .GetMethod(nameof(DatabaseMethods.Overlaps), new[] { typeof(NpgsqlBox), typeof(NpgsqlBox) }))
            .HasTranslation(args =>
            {
                var sqlExpressionFactory = (SqlExpressionFactory)typeof(RelationalSqlTranslatingExpressionVisitor)
                    .GetProperty("SqlExpressionFactory", BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .GetValue(args[0].TypeMapping) as SqlExpressionFactory;

                return sqlExpressionFactory.MakeBinary(
                    ExpressionType.AndAlso, // Equivalent to "OVERLAPS"
                    args[0],
                    args[1],
                    args[0].TypeMapping);
            });
        
        modelBuilder.Entity<Application>()
            .HasOne(a => a.Purpose)
            .WithOne(p => p.Application)
            .HasForeignKey<Application>(a => a.PurposeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Application>()
            .HasOne(a => a.User)
            .WithMany(u => u.Applications)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Application>()
            .HasOne(a => a.TrafficRegulationAuthority)
            .WithMany(tra => tra.Applications)
            .HasForeignKey(a => a.TrafficRegulationAuthorityId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Application>()
            .HasOne(a => a.ApplicationType)
            .WithMany()
            .HasForeignKey(a => a.ApplicationTypeId)
            .OnDelete(DeleteBehavior.Restrict);
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
