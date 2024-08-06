using System.Diagnostics.CodeAnalysis;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using NpgsqlTypes;

namespace DfT.DTRO.DAL;

public partial class DtroContext : DbContext
{
    public virtual DbSet<Models.DataBase.DTRO> Dtros { get; set; }

    public virtual DbSet<DTROHistory> DtroHistories { get; set; }

    public virtual DbSet<SchemaTemplate> SchemaTemplate { get; set; }

    public virtual DbSet<RuleTemplate> RuleTemplate { get; set; }

    public virtual DbSet<Metric> Metrics { get; set; }

    public virtual DbSet<SwaCode> SwaCodes { get; set; }

    public virtual DbSet<SystemConfig> SystemConfig { get; set; }

    public DtroContext(DbContextOptions<DtroContext> options)
        : base(options)
    {
    }

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
