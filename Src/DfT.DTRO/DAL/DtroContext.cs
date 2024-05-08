using DfT.DTRO.Converters;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.DtroJson;
using DfT.DTRO.Models.SchemaTemplate;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using NpgsqlTypes;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;

namespace DfT.DTRO.DAL;

/// <summary>
/// Represents a session with the DTRO database.
/// </summary>
public partial class DtroContext : DbContext
{
    /// <summary>
    /// Used to query the DTRO table.
    /// </summary>
    public virtual DbSet<Models.DataBase.DTRO> Dtros { get; set; }

    /// <summary>
    /// Used to query the SchemaTemplate table.
    /// </summary>
    public virtual DbSet<SchemaTemplate> SchemaTemplate { get; set; }

    /// <summary>
    /// Used to query the RuleTemplate table.
    /// </summary>
    public virtual DbSet<RuleTemplate> RuleTemplate { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="DtroContext"/> using the specified options.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public DtroContext(DbContextOptions<DtroContext> options)
        : base(options)
    {
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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
